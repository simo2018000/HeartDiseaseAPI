// This class should not be here if AuthController is in the same file and namespace.
// It needs to be in its own scope or a different file if you intend two separate controller classes.
// Assuming PatientController is a separate class:
// [ApiController] // Add this if it's a separate controller class
// [Route("api/[controller]")] // Add this if it's a separate controller class
using HeartDiseaseAPI.Dtos;
using HeartDiseaseAPI.Models;
using HeartDiseaseAPI.Services;
using Microsoft.AspNetCore.Mvc;
using HeartDiseaseAPI.Extentions; // Assuming this contains the ToPatient() extension method

public class PatientController : ControllerBase
{
    private readonly PatientServices _patientServices;
    private readonly PredictionService _predictionService; // Assuming this is the intended service

    // Use one constructor that injects all necessary services
    public PatientController(PatientServices patientServices, PredictionService predictionService)
    {
        _patientServices = patientServices;
        _predictionService = predictionService; // Initialize it
    }

    [HttpGet]
    public async Task<List<Patient>> GetAllAsync() // Signature is good
    {
        // Correct way to call an async service method
        return await _patientServices.GetAllAsync();
    }

    // If Patient.Id is now string for MongoDB, change route and parameter type
    [HttpGet("{id}")] // Remove :int constraint if ID is string
    public async Task<ActionResult<Patient>> GetByIdAsync(string id) // Change id to string, rename to GetByIdAsync
    {
        // Correct way to call, pass string id
        var patient = await _patientServices.GetByIdAsync(id);
        return patient == null ? NotFound() : Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> CreateAsync(PatientCreateDto dto) // Make async Task, rename to CreateAsync
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var patientModel = dto.ToPatient(); // Assuming ToPatient() is not async
                                            // Ensure ToPatient correctly maps LastName, FirstName, Email
                                            // from PatientCreateDtos to Patient model.

        // AddPatientAsync is async and returns Task<Patient>
        var createdPatient = await _patientServices.AddPatientAsync(patientModel);

        // If Patient.Id is string, this is fine
        return CreatedAtAction(nameof(GetByIdAsync), new { id = createdPatient.Id }, createdPatient);
    }

    // If Patient.Id is now string for MongoDB, change route and parameter type
    [HttpPut("{id}")] // Remove :int constraint
    public async Task<ActionResult> UpdateAsync(string id, PatientCreateDto dto) // Change id to string, make async Task, rename
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // GetByIdAsync takes string id and is async
        var existingPatient = await _patientServices.GetByIdAsync(id);
        if (existingPatient == null)
        {
            return NotFound();
        }

        var updatedPatientModel = dto.ToPatient();
        updatedPatientModel.Id = id; // Set the string Id

        // UpdateAsync takes string id and is async
        var success = await _patientServices.UpdateAsync(id, updatedPatientModel);
        if (!success) // If update failed (e.g., another check inside UpdateAsync returned false)
        {
            // This case might be rare if GetByIdAsync already confirmed existence.
            // Typically UpdateAsync might throw an exception for concurrency issues,
            // or simply perform the update.
            return NotFound(); // Or some other error if appropriate
        }

        return NoContent();
    }

    // If Patient.Id is now string for MongoDB, change route and parameter type
    [HttpDelete("{id}")] // Remove :int constraint
    public async Task<ActionResult> DeleteAsync(string id) // Change id to string, make async Task, rename
    {
        // DeleteAsync takes string id and is async, returns Task<bool>
        var success = await _patientServices.DeleteAsync(id);
        if (!success) // If not successful (e.g., patient not found to delete)
        {
            return NotFound();
        }
        return NoContent(); // Success
    }

    // Predict Endpoint
    [HttpPost("predict")]
    public ActionResult<bool> Predict(PatientCreateDto dto)
    {
        var patient = dto.ToPatient();
        bool result = _predictionService.Predict(patient); // ✅ Not PredictAsync
        return Ok(result);
    }

}