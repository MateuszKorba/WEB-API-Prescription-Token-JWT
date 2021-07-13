using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppi5.Services;

namespace WebAppi5.Controllers
{
    [ApiController]
    [Route("api/prescription")]
    [Authorize(Roles = "user")]
    public class PrescriptionController : ControllerBase
    {
        private IDataBaseService _dataBaseService;

        public PrescriptionController(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        [HttpGet("{IdPrescription}")]
        public async Task<IActionResult> GetPrescription(int IdPrescription)
        {
            var prescriptioExists = await _dataBaseService.IsPrescriptionExists(IdPrescription);
            if (prescriptioExists.StatusCode == 404)
            {
                return NotFound(prescriptioExists.StatusDescription);
            }
            else
            {
                var resultOperation = await _dataBaseService.GetPrescription(IdPrescription);
                return Ok(resultOperation);
            }
            
        }
    }
}
