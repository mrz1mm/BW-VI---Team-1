using BW_VI___Team_1.Interfaces;

namespace BW_VI___Team_1.Services
{
    public class ImageSvc : IImageSvc
    {
        private readonly string _imagePath;

        public ImageSvc(string imagePath)
        {
            _imagePath = imagePath;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("File immagine non valido");
            }

            var filePath = Path.Combine(_imagePath, imageFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/images/{imageFile.FileName}";
        }

        public Task DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new ArgumentException("URL immagine non valido");
            }

            var fileName = Path.GetFileName(imageUrl);
            var filePath = Path.Combine(_imagePath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }
    }
}
