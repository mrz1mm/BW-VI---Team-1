using BW_VI___Team_1.Interfaces;
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
            return await _context.Animals.FindAsync(id);
        }

        public async Task<Animal> AddAnimalAsync(AnimalDTO model)
        {
            var newAnimal = new Animal
            {
                // Aggiungere cose (es. Name = model.Name)
                // ProductImageUrl = model.ProductImageFile != null ? await _imageSvc.SaveImageAsync(model.ProductImageFile) : null,

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

            // Aggiungere cose (es. animal.Name = model.Name)
            /*
             *             if (model.ProductImageFile != null)
            {
                if (!string.IsNullOrEmpty(product.ProductImageUrl))
                {
                    await _imageSvc.DeleteImageAsync(product.ProductImageUrl);
                }
                product.ProductImageUrl = await _imageSvc.SaveImageAsync(model.ProductImageFile);
            }
             * 
             */

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
    }
}
