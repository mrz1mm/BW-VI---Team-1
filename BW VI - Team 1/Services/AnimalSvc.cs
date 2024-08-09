using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BW_VI___Team_1.Services
{
    public class AnimalSvc : IAnimalSvc
    {
        private readonly LifePetDBContext _context;
        private readonly IImageSvc _imageSvc;

        public AnimalSvc(LifePetDBContext context, IImageSvc imageSvc)
        {
            _context = context;
            _imageSvc = imageSvc;
        }

        public async Task<List<Animal>> GetAllAnimalsAsync()
        {
            return await _context.Animals.ToListAsync();
        }

        public async Task<Animal> GetAnimalByIdAsync(int id)
        {
            return await _context.Animals.Include(a => a.Owner).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Animal> AddAnimalAsync(AnimalDTO model)
        {
            var imageUrl = await _imageSvc.SaveImageAsync(model.ImageFile);

            var existingOwner = await _context.Owners
                .FirstOrDefaultAsync(o => o.FiscalCode == model.Owner.FiscalCode);

            Owner owner;
            if (existingOwner == null)
            {
                owner = new Owner
                {
                    FirstName = model.Owner.FirstName,
                    LastName = model.Owner.LastName,
                    FiscalCode = model.Owner.FiscalCode
                };
                _context.Owners.Add(owner);
                await _context.SaveChangesAsync();
            }
            else
            {
                owner = existingOwner;
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
                ImageUrl = imageUrl
            };
            _context.Animals.Add(newAnimal);
            await _context.SaveChangesAsync();
            return newAnimal;
        }

        public async Task<Animal> UpdateAnimalAsync(int id, AnimalDTO model)
        {
            var animal = await _context.Animals.Include(a => a.Owner).FirstOrDefaultAsync(a => a.Id == id);
            if (animal == null)
            {
                return null;
            }
            animal.Name = model.Name;
            animal.Species = model.Species;
            animal.Breed = model.Breed;
            animal.Color = model.Color;
            animal.BirthDate = model.BirthDate;
            animal.RegisterDate = model.RegisterDate;
            animal.Microchip = model.Microchip;
            animal.MicrochipNumber = model.MicrochipNumber;

            var owner = await _context.Owners.FindAsync(model.OwnerId);
            if (owner == null)
            {
                throw new InvalidOperationException("Il proprietario specificato non esiste.");
            }
            animal.Owner = owner;
            if (model.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(animal.ImageUrl))
                {
                    await _imageSvc.DeleteImageAsync(animal.ImageUrl);
                }
                animal.ImageUrl = await _imageSvc.SaveImageAsync(model.ImageFile);
            }
            _context.Animals.Update(animal);
            await _context.SaveChangesAsync();
            return animal;
        }

        public async Task DeleteAnimalAsync(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Visit>> GetVisitsByAnimalIdAsync(int animalId)
        {
            return await _context.Visits.Where(v => v.Animal.Id == animalId).OrderByDescending(v => v.Date).ToListAsync();
        }

        public async Task<List<Animal>> GetAnimalsByOwnerIdsAsync(List<int?> ownerIds)
        {
            if (ownerIds == null || !ownerIds.Any())
            {
                return new List<Animal>();
            }
            var validOwnerIds = ownerIds
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();
            return await _context.Animals
                .Where(a => validOwnerIds.Contains(a.OwnerId ?? -1))
                .ToListAsync();
        }

        public async Task<List<Animal>> GetAnimalsByPartialMicrochipNumberAsync(string partialMicrochipNumber)
        {
            return await _context.Animals
                .Where(a => EF.Functions.Like(a.MicrochipNumber, $"%{partialMicrochipNumber}%"))
                .Include(a => a.Owner)
                .ToListAsync();
        }
    }
}
