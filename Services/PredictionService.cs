using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using HeartDiseaseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeartDiseaseAPI.Services
{
    public class PredictionService
    {
        private readonly InferenceSession _session;

        public PredictionService(IConfiguration configuration)
        {
            var modelPath = configuration.GetValue<string>("ModelSettings:ModelPath");
            _session = new InferenceSession(modelPath);

            Console.WriteLine("== MODEL INPUTS ==");
            foreach (var input in _session.InputMetadata)
            {
                Console.WriteLine($"Input Name: {input.Key}, Type: {input.Value.ElementType}, Dimensions: {string.Join(",", input.Value.Dimensions)}");
            }

            Console.WriteLine("== MODEL OUTPUTS ==");
            foreach (var output in _session.OutputMetadata)
            {
                Console.WriteLine($"Output Name: {output.Key}, Type: {output.Value.ElementType}, Dimensions: {string.Join(",", output.Value.Dimensions)}");
            }
        }

        public PredictionResult Predict(Patient patient)
        {
            // Prepare input tensor
            var inputData = new DenseTensor<float>(new[] { 1, 11 });
            var inputValues = new float[]
            {
                patient.Age,
                NormalizeSex(patient.Sex),
                patient.Height,
                patient.Weight,
                patient.BloodPressureLow,
                patient.BloodPressureHigh,
                patient.Cholesterol,
                patient.Glucose,
                patient.IsSmoker ? 1f : 0f,
                patient.IsAlcoholic ? 1f : 0f,
                patient.IsActive ? 1f : 0f
            };

            for (int i = 0; i < inputValues.Length; i++)
            {
                inputData[0, i] = inputValues[i];
            }

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("input", inputData)
            };

            using var results = _session.Run(inputs);
            var predictionLabel = results.FirstOrDefault(r => r.Name == "label")?.AsEnumerable<long>().First();
            var predictionProbs = results.FirstOrDefault(r => r.Name == "probabilities")?.AsEnumerable<float>().ToArray();

            return new PredictionResult
            {
                HasHeartDisease = predictionProbs[1] > 0.5f,
                Confidence = predictionProbs[1]*100
            };
        }

        private float NormalizeSex(string sex)
        {
            sex = sex.Trim().ToUpper();
            return sex switch
            {
                "M" or "H" => 1f,
                "F" => 0f,
                _ => throw new ArgumentException("Invalid sex value.")
            };
        }
    }
}
