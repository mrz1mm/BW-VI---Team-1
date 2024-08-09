using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BW_VI___Team_1.Models;
using Microsoft.AspNetCore.Authorization;

namespace BW_VI___Team_1.Controllers
{

    
    public class AnimalController : Controller
    {
        private readonly LifePetDBContext _context;
        private readonly IAnimalSvc _animalSvc;
        private readonly IImageSvc _imageSvc;

        public AnimalController(IAnimalSvc animalSvc, IImageSvc imageSvc, LifePetDBContext context)
        {
            _animalSvc = animalSvc;
            _imageSvc = imageSvc;
            _context = context;
        }
        [Authorize(Policy = Policies.Veterinarian)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var animals = await _animalSvc.GetAllAnimalsAsync();
            return View(animals);
        }
        [Authorize(Policy = Policies.Veterinarian)]
        [HttpGet]
        public async Task<IActionResult> AddAnimal()
        {
            var owners = await _context.Owners.ToListAsync();
            var ownerSelectList = owners.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = $"{o.FirstName} {o.LastName}"
            }).ToList();
            ViewBag.OwnerSelectList = ownerSelectList;

            return View();
        }
        [Authorize(Policy = Policies.Veterinarian)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAnimal([Bind("Name,Species,Breed,Color,BirthDate,RegisterDate,Microchip,MicrochipNumber,OwnerId,ImageFile")] AnimalDTO model)
        {
            try
            {
                var owner = await _context.Owners.FindAsync(model.OwnerId);
                if (owner == null)
                {
                    ModelState.AddModelError("", "Proprietario non trovato.");
                    return View(model);
                }

                var newAnimal = new Animal
                {
                    Name = model.Name,
                    Species = model.Species,
                    Breed = model.Breed,
                    Color = model.Color,
                    BirthDate = model.BirthDate,
                    RegisterDate = model.RegisterDate,
                    Microchip = model.Microchip,
                    MicrochipNumber = model.MicrochipNumber,
                    Owner = owner,
                    ImageUrl = model.ImageFile != null ? await _imageSvc.SaveImageAsync(model.ImageFile) : null
                };

                _context.Animals.Add(newAnimal);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Animale aggiunto con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta dell'animale";
                return View(model);
            }
        }
        [Authorize(Policy = Policies.Veterinarian)]
        [HttpGet]
        public async Task<IActionResult> UpdateAnimal(int id)
        {
            var animal = await _animalSvc.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            var owners = await _context.Owners.ToListAsync();
            var ownerSelectList = owners.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = $"{o.FirstName} {o.LastName}",
                Selected = o.Id == animal.OwnerId
            }).ToList();
            ViewBag.OwnerSelectList = ownerSelectList;

            var model = new AnimalDTO
            {
                Name = animal.Name,
                Species = animal.Species,
                Breed = animal.Breed,
                Color = animal.Color,
                BirthDate = animal.BirthDate,
                RegisterDate = animal.RegisterDate,
                Microchip = animal.Microchip,
                MicrochipNumber = animal.MicrochipNumber,
                OwnerId = animal.OwnerId
            };

            ViewBag.AnimalId = id;
            return View(model);
        }
        [Authorize(Policy = Policies.Veterinarian)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAnimal(int id, [Bind("Name,Species,Breed,Color,BirthDate,RegisterDate,Microchip,MicrochipNumber,OwnerId,ImageFile")] AnimalDTO model)
        {
            try
            {
                await _animalSvc.UpdateAnimalAsync(id, model);
                TempData["Success"] = "Animale modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica dell'animale";
                return View(model);
            }
        }
        [Authorize(Policy = Policies.Veterinarian)]
        [HttpPost]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            try
            {
                await _animalSvc.DeleteAnimalAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Policy = Policies.Veterinarian)]
        [HttpGet]
        public async Task<IActionResult> AnimalDetails(int id)
        {
            var animal = await _animalSvc.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            var visits = await _animalSvc.GetVisitsByAnimalIdAsync(id);
            var model = new VisitHistoryDTO
            {
                Animal = animal,
                Visits = visits
            };

            return View(model);
        }

        public IActionResult SearchBy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string microchipNumber)
        {
            if (string.IsNullOrEmpty(microchipNumber))
            {
                return PartialView("_SearchResults", null);
            }

            var animals = await _context.Animals
                .Include(a => a.Owner)
                .Where(a => a.MicrochipNumber.Contains(microchipNumber))
                .Select(a => new BW_VI___Team_1.Models.DTO.AnimalSearchResultDTO
                {
                    Name = a.Name,
                    MicrochipNumber = a.MicrochipNumber,
                    ImageUrl = a.ImageUrl,
                    IsInRecovery = _context.Recoverys.Any(r => r.AnimalId == a.Id)
                })
                .ToListAsync();

            return PartialView("_SearchResults", animals);
        }
    }
}
