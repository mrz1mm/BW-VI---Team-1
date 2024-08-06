﻿using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;

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
                Owner = model.Owner,
                ImageUrl = imageUrl
            };
            _context.Animals.Add(newAnimal);
            await _context.SaveChangesAsync();
            return newAnimal;
        }

        public async Task<Animal> UpdateAnimalAsync(Animal model)
        {
            var animal = await _context.Animals.FindAsync(model.Id);
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
            animal.Owner = model.Owner;
            animal.ImageUrl = model.ImageUrl;

            _context.Animals.Update(animal);
            await _context.SaveChangesAsync();
            return animal;
        }

        public async Task<bool> DeleteAnimalAsync(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return false;
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Visit>> GetVisitsByAnimalIdAsync(int animalId)
        {
            return await _context.Visits.Where(v => v.Animal.Id == animalId).OrderByDescending(v => v.Date).ToListAsync();
        }
    }
}
