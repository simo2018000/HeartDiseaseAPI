// In Services/PatientServices.cs
using HeartDiseaseAPI.Models;
using HeartDiseaseAPI.Dtos;
using HeartDiseaseAPI.Extentions; 
using System.Security.Cryptography;
using BCrypt.Net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HeartDiseaseAPI.Services
{
    public class PatientServices
    {
        private readonly MyMongoService _myMongoService; // Inject MyMongoService

        // Remove: private List<Patient> _patients = new List<Patient>();
        // Remove: private static int _nextId = 1;

        public PatientServices(MyMongoService myMongoService) // Constructor injection
        {
            _myMongoService = myMongoService;
        }

        public async Task<Patient> Register(PatientCreateDto dto) //
        {
            var existingPatient = await _myMongoService.GetByEmailAsync(dto.Email);
            if (existingPatient != null)
            {
                throw new Exception("Email deja existant.");
            }

            var patient = dto.ToPatient();

            patient.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            // MongoDB will generate the patient.Id if it's null (when Patient.Id is string? and [BsonId][BsonRepresentation(BsonType.ObjectId)])

            await _myMongoService.CreateAsync(patient);
            return patient;
        }

        public async Task<Patient?> Login(LoginDto dto) //
        {
            var patient = await _myMongoService.GetByEmailAsync(dto.Email);
            if (patient == null || !BCrypt.Net.BCrypt.Verify(dto.Password, patient.Password))
            {


                throw new Exception("Email ou mot de passe incorrect.");
            }


            return patient;
        }

        public async Task<List<Patient>> GetAllAsync() // Renamed to Async convention
        {
            return await _myMongoService.GetAsync();
        }

        public async Task<Patient?> GetByIdAsync(string id) // Parameter changed to string for ObjectId
        {
            return await _myMongoService.GetAsync(id);
        }

        // The original 'Add' method is effectively replaced by 'Register' or a direct call to _myMongoService.CreateAsync
        // If you need a separate Add that doesn't involve hashing/email checks (like the one used in Program.cs POST /patients):
        public async Task<Patient> AddPatientAsync(Patient patient)
        {
            // Assuming patient.Id might be null and MongoDB will generate it
            // If ID needs to be set or validated before this, add logic
            await _myMongoService.CreateAsync(patient);
            return patient;
        }


        // In Services/PatientServices.cs
        // ...
        public async Task<bool> UpdateAsync(string id, UpdatedPatientDto dto) // <<-- CHANGE HERE to UpdatedPatientDto
        {
            var patientToUpdate = await _myMongoService.GetAsync(id);
            if (patientToUpdate == null)
            {
                return false; // Patient not found
            }

            // Map fields from DTO to the existing patient entity
            patientToUpdate.Age = dto.Age;
            patientToUpdate.Sex = dto.Sex;
            patientToUpdate.Height = dto.Height;
            patientToUpdate.Weight = dto.Weight;
            patientToUpdate.BloodPressureLow = dto.BloodPressureLow;
            patientToUpdate.BloodPressureHigh = dto.BloodPressureHigh;
            patientToUpdate.Cholesterol = dto.Cholesterol;
            patientToUpdate.Glucose = dto.Glucose;
            patientToUpdate.IsSmoker = dto.IsSmoker;
            patientToUpdate.IsAlcoholic = dto.IsAlcoholic;
            patientToUpdate.IsActive = dto.IsActive;

            // Removed personal data updates like LastName, FirstName, Email, Password from here
            // as UpdatedPatientDto only contains health data.
            // If you need to update personal data, you'd need a separate DTO/method.

            await _myMongoService.UpdateAsync(id, patientToUpdate);
            return true;
        }
        // ...

        public async Task<bool> DeleteAsync(string id) // Parameter changed to string
        {
            return await _myMongoService.RemoveAsync(id);
        }
    }
}
    