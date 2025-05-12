namespace HeartDiseaseAPI.Dtos
{
    public class PatientDtos
    {
        public class PatientCreateDto
        {
            public int Age { get; set; }
            public string Sex { get; set; } = string.Empty;
            public int Height { get; set; }
            public int Weight { get; set; }
            public float BloodPressureLow { get; set; }
            public float BloodPressureHigh { get; set; }
            public float Cholesterol { get; set; }
            public float Glucose { get; set; }
            public bool IsSmoker { get; set; }
            public bool IsAlcoholic { get; set; }
            public bool IsActive { get; set; }
        }
        public class PatientReadDto : PatientCreateDto
        {
            public int ID { get; set; }
        }


    }
}
