using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BW_VI___Team_1.Controllers
{
    public class AnimalController : Controller
    {
        private readonly IAnimalSvc _animalSvc;

        public AnimalController(IAnimalSvc animalSvc)
        {
            _animalSvc = animalSvc;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var animals = await _animalSvc.GetAllAnimalsAsync();
            return View(animals);
        }

        [HttpGet]
        public IActionResult AddAnimal()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAnimal(AnimalDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _animalSvc.AddAnimalAsync(model);
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

        [HttpGet]
        public async Task<IActionResult> UpdateAnimal(int id)
        {
            var animal = await _animalSvc.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            var model = new AnimalDTO
            {
                Id = animal.Id,
                Name = animal.Name,
                Species = animal.Species,
                Breed = animal.Breed,
                Color = animal.Color,
                BirthDate = animal.BirthDate,
                RegisterDate = animal.RegisterDate,
                Microchip = animal.Microchip,
                MicrochipNumber = animal.MicrochipNumber,
                Owner = animal.Owner,
                ImageUrl = animal.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAnimal(AnimalDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _animalSvc.UpdateAnimalAsync(model);
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

        [HttpGet]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _animalSvc.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteAnimal(int id)
        {
            try
            {
                await _animalSvc.DeleteAnimalAsync(id);
                TempData["Success"] = "Animale eliminato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore nell'eliminazione dell'animale";
                return RedirectToAction(nameof(Index));
            }
        }

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
    }
}
