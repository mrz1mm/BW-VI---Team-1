using BW_VI___Team_1.Models;

namespace BW_VI___Team_1.Services
{
    public class ImageSvc
    {
        private readonly LifePetDBContext _context;
        public ImageSvc(LifePetDBContext context)
        {
            _context = context;
        }
    }
}
