using HeartDiseaseAPI.Models;
using HeartDiseaseAPI.Dtos;
using HeartDiseaseAPI.Extentions;
using System.Security.Cryptography;
using BCrypt.Net;

namespace HeartDiseaseAPI.Services
{
    public class PatientServices
    {

        private List<Patient> _patients = new List<Patient>();
        private static int _nextId = 1;

        public Patient Register(PatientCreateDtos dto)
        {
            if(_patients.Any(p => p.Email == dto.Email))
            {
                throw new Exception("Email deja existant.");
  
            }

            var patient = dto.ToPatient();
            patient.ID = _nextId++;
            patient.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            _patients.Add(patient);
            return patient;
        }
        public Patient Login(LoginDto dto)
        {
            var patient = _patients.FirstOrDefault(p => p.Email == dto.Email);
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, patient.Password))
            {
                throw new Exception("Email ou mot de passe incorrect.");
            }
            return patient;
        }

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
            int index = FindIndexByID(id);
            if (index == -1)
            {
                return false; // Patient not found
            }

            updatedPatient.ID = id; // Ensure the ID remains the same
            _patients[index] = updatedPatient; // Update the patient in the list
            return true;
        }

        public bool Delete(int id)
        {
            var patient = _patients.FirstOrDefault(p => p.ID == id);
            if (patient == null) return false;
            _patients.Remove(patient);
            return true;
        }

        private int FindIndexByID(int id)
        {
            return _patients.FindIndex(p => p.ID == id);
        }

    }
}
