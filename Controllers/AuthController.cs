using Microsoft.AspNetCore.Mvc;
using HeartDiseaseAPI.Dtos;
using HeartDiseaseAPI.Services;
using System.Threading.Tasks;
using HeartDiseaseAPI.Models; // For Patient model if returned
using AutoMapper; // If you want to return PatientReadDto

namespace HeartDiseaseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This would make the route /api/auth
    public class AuthController : ControllerBase
    {
        private readonly PatientServices _patientServices;
        private readonly IMapper _mapper; // Optional: if you map Patient to PatientReadDto

        public AuthController(PatientServices patientServices, IMapper mapper)
        {
            _patientServices = patientServices;
            _mapper = mapper;
        }

        [HttpPost("login")] // Route: POST /api/auth/login
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var patient = await _patientServices.Login(loginDto);
                if (patient != null)
                {
                    // Login successful
                    // TODO: Implement JWT token generation here in a real application

                    // For now, let's return some patient info (excluding password)
                    var patientReadDto = _mapper.Map<PatientReadDto>(patient);
                    return Ok(new { message = "Login successful", user = patientReadDto });
                }
                else
                {
                    // Should not happen if Login service throws an exception on failure,
                    // but as a fallback:
                    return Unauthorized(new { message = "Login failed. Please check your credentials." });
                }
            }
            catch (System.Exception ex)
            {
                // Catch the "Email ou mot de passe incorrect." exception from PatientServices
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")] // Optional: if you want a register endpoint here too
                               // You already have POST /api/patient, so this might be redundant
                               // or could be specifically for user self-registration.
        public async Task<IActionResult> Register([FromBody] PatientCreateDto patientCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var registeredPatient = await _patientServices.Register(patientCreateDto);
                var patientReadDto = _mapper.Map<PatientReadDto>(registeredPatient);

                // Using CreatedAtRoute to the GetPatientById route in PatientController
                // This assumes PatientController has a GetByIdAsync method with Name = "GetPatientById"
                // and that you've handled the "No route matches" error for CreatedAtAction/Route.
                // If that Get route is not named, this specific CreatedAtRoute will fail.
                // For simplicity, you could just return Ok(patientReadDto);
                return CreatedAtRoute("GetPatientById", new { id = patientReadDto.Id }, patientReadDto);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}