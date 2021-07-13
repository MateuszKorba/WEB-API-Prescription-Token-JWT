using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAppi5.Models.DTO.Requests;
using WebAppi5.Services;

namespace WebAppi5.Controllers
{
    [ApiController]
    [Route("api/doctor")]
    [Authorize(Roles = "user")]
    public class DoctorsController : ControllerBase
    {
        private IDataBaseService _dataBaseService;

        public DoctorsController(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        [HttpGet("{IdDoctor}")]
        public async Task<IActionResult> GetDoctor([FromRoute] int IdDoctor)
        {
            var doctorExist = await _dataBaseService.IsDoctorExists(IdDoctor);
            if(doctorExist.StatusCode == 404)
            {
                return NotFound(doctorExist.StatusDescription);
            }
            else
            {
                var resultOperation = await _dataBaseService.GetDoctor(IdDoctor);
                return Ok(resultOperation);
            }
        }

        [HttpPut]
        public async Task<IActionResult> AddNewDoctor([FromBody] DoctorRequestDto doctor)
        {
            var resultOperation = await _dataBaseService.AddDoctor(doctor);
            if (resultOperation.StatusCode == 200)
            {
                return Ok(resultOperation.StatusDescription);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("{IdDoctor}")]
        public async Task<IActionResult> UpdateDoctorData([FromBody] DoctorRequestDto doctor, [FromRoute] int IdDoctor)
        {
            var resultOperation = await _dataBaseService.UpdateDoctor(doctor,IdDoctor);
            if (resultOperation.StatusCode == 404)
            {
                return NotFound(resultOperation.StatusDescription);
            }
            else
            {
                return Ok(resultOperation.StatusDescription);
            }
        }

        [HttpDelete("{IdDoctor}")]
        public async Task<IActionResult> DeleteDoctor([FromRoute] int IdDoctor)
        {
            var resultOperation = await _dataBaseService.DeleteDoctor(IdDoctor);
            if (resultOperation.StatusCode == 404)
            {
                return NotFound(resultOperation.StatusDescription);
            }
            else
            {
                return Ok(resultOperation.StatusDescription);
            }
        }

    }
}

