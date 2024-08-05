using BW_VI___Team_1.Models;
using BW_VI___Team_1.Interfaces;

namespace BW_VI___Team_1.Services
{
    public class ImageSvc : IImageSvc
    {
        private readonly LifePetDBContext _context;
        public ImageSvc(LifePetDBContext context)
        {
            _context = context;
        }
    }
}
