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
        }

        public PredictionResult Predict(Patient patient)
        {
            var inputData = new DenseTensor<float>(new[]
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
            }, new[] { 1, 11 });

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", inputData)
            };

            using var results = _session.Run(inputs);
            var output = results.First().Value as DenseTensor<float>;
            var probabilities = output?.ToArray();

            if (probabilities == null || probabilities.Length < 2)
                throw new InvalidOperationException("Model did not return valid prediction probabilities.");


            return new PredictionResult
            {
                HasHeartDisease = probabilities[1] > 0.5f,
                Confidence = probabilities[1]
            };
        }

        private float NormalizeSex(string sex)
        {
            sex = sex.Trim().ToUpper();
            switch (sex)
            {
                case "M":
                case "H":
                    return 1f;
                case "F":
                    return 0f;
                default:
                    throw new ArgumentException("Invalid sex value.");
            }
        }


    }
}