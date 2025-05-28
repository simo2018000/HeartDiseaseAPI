// Services/PredictionService.cs
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using HeartDiseaseAPI.Models;
using System.Linq;

namespace HeartDiseaseAPI.Services
{
    public class PredictionService
    {
        private readonly InferenceSession _session;

        public PredictionService()
        {
            _session = new InferenceSession("xgboost_model.onnx");
        }

        public bool Predict(Patient patient)
        {
            var input = new DenseTensor<float>(new[] {
                (float)patient.Age,
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
            }, new[] { 1, 11 });

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("input", input)
            };

            using var results = _session.Run(inputs);
            var prediction = results.First().AsEnumerable<float>().ToArray();

            return prediction[1] > 0.5f; // class 1 probability > 50% → heart disease
        }

        private float NormalizeSex(string sex)
        {
            sex = sex.Trim().ToUpper();
            return (sex == "H" || sex == "M") ? 1f :
                   (sex == "F") ? 0f : throw new ArgumentException("Invalid sex value");
        }
    }
}
