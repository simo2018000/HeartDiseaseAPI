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

        public async Task<Patient> Register(PatientCreateDtos dto) //
        {
            var existingPatient = await _myMongoService.GetByEmailAsync(dto.Email);
            if (existingPatient != null)
            {
                throw new Exception("Email deja existant.");
            }

            var patient = dto.ToPatient(); // Uses your existing extension method
                                           // Ensure ToPatient maps LastName, FirstName, Email correctly
                                           // If your ToPatient() doesn't map LastName, FirstName, Email, do it here:
                                           // patient.LastName = dto.LastName;
                                           // patient.FirstName = dto.FirstName;
                                           // patient.Email = dto.Email;

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


        public async Task<bool> UpdateAsync(string id, Patient updatedPatient) // Parameter changed to string
        {
            var patient = await _myMongoService.GetAsync(id);
            if (patient == null)
            {
                return false; // Patient not found
            }

            updatedPatient.Id = id; 

             if (!string.IsNullOrEmpty(updatedPatient.Password) && !updatedPatient.Password.StartsWith("$2a$")) // Basic check if it's not already hashed
             {
             updatedPatient.Password = BCrypt.Net.BCrypt.HashPassword(updatedPatient.Password);
            }


            await _myMongoService.UpdateAsync(id, updatedPatient);
            return true;
        }

        public async Task<bool> DeleteAsync(string id) // Parameter changed to string
        {
            return await _myMongoService.RemoveAsync(id);
        }

        // The FindIndexByID method is no longer needed as we're not managing an in-memory list.
        // private int FindIndexByID(int id) //
        // {
        // return _patients.FindIndex(p => p.ID == id);
        // }
    }
}