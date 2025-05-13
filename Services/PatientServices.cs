using HeartDiseaseAPI.Models;
namespace HeartDiseaseAPI.Services

{
    public class PatientServices
    {
        private List<Patient> _patients = new List<Patient>();
        private static int _nextId = 1;
        public List<Patient> GetAll()
        {
            return _patients;
        }
        public Patient? GetById(int id)
        {
            return _patients.FirstOrDefault(p => p.ID == id);
        }
        public Patient Add(Patient patient)
        {
            patient.ID = _nextId++;
            _patients.Add(patient);
            return patient;
        }
        public bool Update(int id, Patient updatedPatient)
        {
            var patient = _patients.FirstOrDefault(p => p.ID == id);
            if (patient == null) return false;
            patient.ID = id; // Ensure the ID remains the same
            patient.Age = updatedPatient.Age;
            patient.Sex = updatedPatient.Sex;
            patient.Height = updatedPatient.Height;
            patient.Weight = updatedPatient.Weight;
            patient.BloodPressureLow = updatedPatient.BloodPressureLow;
            patient.BloodPressureHigh = updatedPatient.BloodPressureHigh;
            patient.Cholesterol = updatedPatient.Cholesterol;
            patient.Glucose = updatedPatient.Glucose;

            return true;
        }
    
    public bool Delete(int id)
        {
            var patient = _patients.FirstOrDefault(p => p.ID == id);
            if (patient == null) return false;
            _patients.Remove(patient);
            return true;
        }
    }
}
