using HeartDiseaseAPI.Dtos;
using HeartDiseaseAPI.Models;
using HeartDiseaseAPI.Services;
using Microsoft.AspNetCore.Mvc;
using HeartDiseaseAPI.Extentions;

namespace HeartDiseaseAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly PatientServices _patientServices;

    public AuthController(PatientServices patientServices) =>
        _patientServices = patientServices;
    [HttpPost("register")]
    public ActionResult<Patient> Register(PatientCreateDtos dto)
    {
        try
        {
            var patient = _patientServices.Register(dto);
            return CreatedAtAction(nameof(Register), new { id = patient.ID }, patient);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }


    }
    [HttpPost("login")]


    public ActionResult<Patient> Login(LoginDto dto)
    {
        var patient = _patientServices.Login(dto);
        if (patient == null)

            return Unauthorized("Invalid Credentials ");

        return Ok(patient);


    }
} 
    public class PatientController : ControllerBase
    {
        private readonly PatientServices _patientServices;

        public PatientController(PatientServices patientServices) =>
            _patientServices = patientServices;

        [HttpGet]
        public async Task<List<Patient>> GetAll() =>
            await Task.FromResult(_patientServices.GetAll());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Patient>> GetById(int id)
        {
            var patient = await Task.FromResult(_patientServices.GetById(id));
            return patient == null ? NotFound() : Ok(patient);
        }

        [HttpPost]
        public ActionResult<Patient> Create(PatientCreateDtos dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = dto.ToPatient();
            var createdPatient = _patientServices.Add(patient);

            return CreatedAtAction(nameof(GetById), new { id = createdPatient.ID }, createdPatient);
        }

        [HttpPut("{id:int}")]
        public ActionResult Update(int id, PatientCreateDtos dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPatient = _patientServices.GetById(id);
            if (existingPatient == null)
            {
                return NotFound();
            }

            var updatedPatient = dto.ToPatient();
            updatedPatient.ID = id;

            var result = _patientServices.Update(id, updatedPatient);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var success = _patientServices.Delete(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
