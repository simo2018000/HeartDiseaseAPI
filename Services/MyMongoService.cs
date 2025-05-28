// In Services/MyMongoService.cs
using HeartDiseaseAPI.Models; // Assuming Patient and MongoDbSettings are in this namespace or referenced
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MyMongoService
{
    private readonly IMongoCollection<Patient> _patientsCollection;

    public MyMongoService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        // These names come from your appsettings.json via MongoDbSettings
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _patientsCollection = mongoDatabase.GetCollection<Patient>(mongoDbSettings.Value.CollectionName);
    }

    // Get all patients
    public async Task<List<Patient>> GetAsync() =>
        await _patientsCollection.Find(_ => true).ToListAsync();

    // Get a single patient by ID (string, as we changed Patient.Id to string for ObjectId)
    public async Task<Patient?> GetAsync(string id) =>
        await _patientsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    // Get a single patient by Email (useful for checking during registration)
    public async Task<Patient?> GetByEmailAsync(string email) =>
        await _patientsCollection.Find(x => x.Email == email).FirstOrDefaultAsync();

    // Create a new patient
    public async Task CreateAsync(Patient newPatient) =>
        await _patientsCollection.InsertOneAsync(newPatient); // MongoDB will generate the Id if it's null

    // Update an existing patient
    public async Task UpdateAsync(string id, Patient updatedPatient) =>
        await _patientsCollection.ReplaceOneAsync(x => x.Id == id, updatedPatient);

    // Delete a patient
    public async Task<bool> RemoveAsync(string id)
    {
        var result = await _patientsCollection.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}