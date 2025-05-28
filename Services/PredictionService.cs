using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using HeartDiseaseAPI.Models;
using System.Linq;
using Microsoft.Extensions.Options; // Added for IOptions
using System; // Added for ArgumentException, List
using System.Collections.Generic; // Added for List

namespace HeartDiseaseAPI.Services
{
    public class PredictionService
    {
        private readonly InferenceSession _session;
        private readonly string _modelPath; // To store the model path from configuration

        public PredictionService(IOptions<ModelSettings> modelSettings) // Inject IOptions<ModelSettings>
        {
            _modelPath = modelSettings.Value.ModelPath;
            if (string.IsNullOrEmpty(_modelPath))
            {
                throw new ArgumentException("ONNX ModelPath is not configured in appsettings.");
            }
            // Consider adding File.Exists(_modelPath) check if it's a local path
            _session = new InferenceSession(_modelPath);
        }

        public bool Predict(Patient patient) // Patient model should contain all necessary features
        {
            var inputData = new float[]
            {
                patient.Age, // Ensure Age is in the correct unit (years, as standardized)
                NormalizeSex(patient.Sex),
                patient.Height,
                patient.Weight,
                patient.BloodPressureHigh,
                patient.BloodPressureLow,
                patient.Cholesterol,
                patient.Glucose,
                patient.IsSmoker ? 1f : 0f,
                patient.IsAlcoholic ? 1f : 0f,
                patient.IsActive ? 1f : 0f
            };

            var inputTensor = new DenseTensor<float>(inputData, new[] { 1, 11 }); // Assuming 11 features

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("input", inputTensor) // Ensure "input" matches your ONNX model's input node name
            };

            using var results = _session.Run(inputs);
            // Assuming the model outputs probabilities for two classes [prob_class_0, prob_class_1]
            // And class 1 means has heart disease.
            var prediction = results.FirstOrDefault()?.AsEnumerable<float>().ToArray();

            if (prediction != null && prediction.Length >= 2)
            {
                return prediction[1] > 0.5f; // Probability of class 1 (heart disease) > 0.5
            }
            // Fallback or error handling if prediction format is unexpected
            // For simplicity, throwing an exception or returning false.
            // Consider logging this scenario.
            // throw new InvalidOperationException("Prediction output was not in the expected format.");
            return false; // Default to no heart disease if prediction is malformed
        }

        private float NormalizeSex(string sex)
        {
            sex = sex.Trim().ToUpper();
            // Updated to match the DTO validation and common expectations
            // "H" for Homme (Man), "M" for Male are treated as male. "F" for Femme (Female) as female.
            return (sex == "M" || sex == "H") ? 1f :
                   (sex == "F") ? 0f :
                   throw new ArgumentException($"Invalid gender value: '{sex}'. Expected 'M', 'H', or 'F'.");
        }

        // If you want to return more detailed prediction (like probability)
        public HeartDiseasePrediction PredictWithDetails(Patient patient)
        {
            var inputData = new float[]
            {
                patient.Age, NormalizeSex(patient.Sex), patient.Height, patient.Weight,
                patient.BloodPressureHigh, patient.BloodPressureLow, patient.Cholesterol, patient.Glucose,
                patient.IsSmoker ? 1f : 0f, patient.IsAlcoholic ? 1f : 0f, patient.IsActive ? 1f : 0f
            };
            var inputTensor = new DenseTensor<float>(inputData, new[] { 1, 11 });
            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor("input", inputTensor) };

            using var results = _session.Run(inputs);
            var output = results.FirstOrDefault()?.AsEnumerable<float>().ToArray();

            if (output != null && output.Length >= 2)
            {
                float probabilityClass1 = output[1]; // Probability of having heart disease
                return new HeartDiseasePrediction
                {
                    PredictedLabel = probabilityClass1 > 0.5f,
                    Probability = probabilityClass1,
                    // Score might be the same as probability for binary classification,
                    // or it could be a raw logit value depending on the model.
                    // For now, using probability as score.
                    Score = probabilityClass1
                };
            }
            // Fallback for unexpected output format
            return new HeartDiseasePrediction
            {
                PredictedLabel = false,
                Probability = 0f,
                Score = 0f
            };
        }
    }
}