using AutoMapper; // Added for IMapper
using HeartDiseaseAPI.Dtos;
using HeartDiseaseAPI.Models;
using HeartDiseaseAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic; // Added for List
using System.Threading.Tasks; // Added for Task

[ApiController]
[Route("api/[controller]")] // Standard route for controllers
public class PatientController : ControllerBase
{
    private readonly PatientServices _patientServices;
    private readonly PredictionService _predictionService;
    private readonly IMapper _mapper; // Added for AutoMapper

    public PatientController(PatientServices patientServices, PredictionService predictionService, IMapper mapper) // Added IMapper
    {
        _patientServices = patientServices;
        _predictionService = predictionService;
        _mapper = mapper; // Added
    }

    [HttpGet]
    public async Task<ActionResult<List<PatientReadDto>>> GetAllAsync() // Return List<PatientReadDto>
    {
        var patients = await _patientServices.GetAllAsync();
        var patientDto = _mapper.Map<List<PatientReadDto>>(patients); // Map to DTO
        return Ok(patientDto);
    }

    [HttpGet("{id}", Name = "GetPatientById")]
    public async Task<ActionResult<PatientReadDto>> GetByIdAsync(string id) // Return PatientReadDto
    {
        var patient = await _patientServices.GetByIdAsync(id);
        if (patient == null)
        {
            return NotFound();
        }
        var patientDto = _mapper.Map<PatientReadDto>(patient); // Map to DTO
        return Ok(patientDto);
    }

    [HttpPost]
    // Changed to use PatientCreateDto. Register method in service will handle mapping and password.
    public async Task<ActionResult<PatientReadDto>> CreateAsync([FromBody] PatientCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // The Register service method now takes PatientCreateDto, maps it to Patient,
            // hashes the password, and saves it.
            var registeredPatient = await _patientServices.Register(dto);
            var patientReadDto = _mapper.Map<PatientReadDto>(registeredPatient);

            return CreatedAtRoute("GetPatientById", new { id = patientReadDto.Id }, patientReadDto);
        }
        catch (Exception ex)
        {
            // Log the exception ex
            return BadRequest(ex.Message); // Or a more generic error message
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(string id, [FromBody] UpdatedPatientDto dto) // Changed to PatientCreateDto
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // The service's UpdateAsync now takes PatientCreateDto and handles finding the existing patient
        // and mapping updated fields, including password if provided.
        var success = await _patientServices.UpdateAsync(id, dto);
        if (!success)
        {
            return NotFound(); // Or some other error if appropriate (e.g., concurrency issue)
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        var success = await _patientServices.DeleteAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }

    // In Controllers/PatientController.cs
    // ...
    [HttpPost("predict")]
    public ActionResult<PredictionResult> Predict([FromBody] PatientInputDto dto) // Changed DTO here
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Invalid input data for prediction.", errors = ModelState });
        }

        var patientDataForPrediction = new Patient
        {
            Age = dto.Age,
            Sex = dto.Sex,
            Height = dto.Height,
            Weight = dto.Weight,
            BloodPressureHigh = dto.BloodPressureHigh,
            BloodPressureLow = dto.BloodPressureLow,
            Cholesterol = dto.Cholesterol,
            Glucose = dto.Glucose,
            IsSmoker = dto.IsSmoker,
            IsAlcoholic = dto.IsAlcoholic,
            IsActive = dto.IsActive
        };

        PredictionResult resultFromService = _predictionService.Predict(patientDataForPrediction);

        return Ok(resultFromService);
    }
    // ...
}
