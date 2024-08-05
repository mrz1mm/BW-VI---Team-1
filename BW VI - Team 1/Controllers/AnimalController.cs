using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BW_VI___Team_1.Controllers
{
    public class AnimalController : Controller
    {
        private readonly LifePetDBContext _context;
        private readonly IAnimalSvc _animalSvc;
        public AnimalController(LifePetDBContext context, IAnimalSvc animalSvc)
        {
            _context = context;
            _animalSvc = animalSvc;
        }

        // VISTE
        [HttpGet]
        public IActionResult Index()
        {
            var animals = _animalSvc.GetAllAnimalsAsync();
            return View(animals);
        }

        [HttpGet]
        public IActionResult AddAnimal()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateAnimal(int id)
        {
            var animal = _animalSvc.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            var model = new Animal
            {
                // aggiungere cose (es. Name = animal.Name)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteAnimal(int id)
        {
            var animal = _animalSvc.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            var model = new Animal
            {
                // aggiungere cose (es. Name = animal.Name)
            };

            return View();
        }


        // METODI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAnimal(AnimalDTO model) // aggiungere il Binding
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAnimal(Animal model) // aggiungere il Binding
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
    }
}
