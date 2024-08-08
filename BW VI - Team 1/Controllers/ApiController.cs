using BW_VI___Team_1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BW_VI___Team_1.Models;

namespace BW_VI___Team_1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IOwnerSvc _ownerSvc;
        private readonly LifePetDBContext _context;

        public ApiController(IOwnerSvc ownerSvc, LifePetDBContext context)
        {
            _ownerSvc = ownerSvc;
            _context = context;
        }

        [HttpGet("SearchByPartialFiscalCode")]
        public async Task<IActionResult> GetOwnersByPartialFiscalCode(string partialFiscalCode)
        {
            if (string.IsNullOrEmpty(partialFiscalCode))
            {
                return BadRequest("Il codice fiscale parziale non può essere vuoto.");
            }

            var owners = await _ownerSvc.GetOwnersByPartialFiscalCodeAsync(partialFiscalCode);

            if (owners == null || owners.Count == 0)
            {
                return NotFound("Nessun proprietario trovato con il codice fiscale specificato.");
            }
            foreach (var owner in owners)
            {
                owner.Animals = await _context.Animals.Where(a => a.OwnerId == owner.Id).ToListAsync();
            }

            return Ok(owners);
        }
    }
}
