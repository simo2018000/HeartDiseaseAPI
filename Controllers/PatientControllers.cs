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
    public async Task<ActionResult<Patient>> CreateAsync(PatientCreateDtos dto) // Make async Task, rename to CreateAsync
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
    public async Task<ActionResult> UpdateAsync(string id, PatientCreateDtos dto) // Change id to string, make async Task, rename
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
    public async Task<ActionResult<bool>> PredictAsync(PatientCreateDtos dto) // Consider making async if service is async
    {
        // 1. Map DTO to the input model your PredictionService expects.
        // Using PatientCreateDtos directly might send too much/irrelevant data.
        // We planned a HeartDiseasePrediction model specifically for ONNX input.
        // For now, if your PredictionService.Predict takes the full Patient model:
        var patientDataForPrediction = dto.ToPatient();

        // This depends on your PredictionService.Predict method signature.
        // If it's synchronous:
        // bool result = _predictionService.Predict(patientDataForPrediction);
        // If it becomes asynchronous (recommended if it involves I/O or is potentially long):
        bool result = await _predictionService.PredictAsync(patientDataForPrediction);
        // ^ Assuming an async version

        // Returning a simple bool might be okay for a start.
        // We previously discussed a PatientResultDto for more detailed output (e.g., probability).
        return Ok(result);
    }
}