using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using HeartDiseaseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeartDiseaseAPI.Services
{
    public class OnnxBinaryPredictionService
    {
        private readonly InferenceSession _session;

        public OnnxBinaryPredictionService()
        {
            _session = new InferenceSession("xgboost_model.onnx");
        }

        public bool Predict(Patient patient)
        {
            var inputData = new float[]
            {
                patient.Age,
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

            var inputTensor = new DenseTensor<float>(inputData, new[] { 1, 11 });

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("input", inputTensor)
            };

            using var results = _session.Run(inputs);
            var prediction = results.First().AsEnumerable<float>().ToArray();

            return prediction[1] > 0.5f; // Class 1 probability
        }

        private float NormalizeSex(string sex)
        {
            sex = sex.Trim().ToUpper();
            return (sex == "M" || sex == "H") ? 1f :
                   (sex == "F") ? 0f :
                   throw new ArgumentException("Invalid gender value");
        }
    }
}
