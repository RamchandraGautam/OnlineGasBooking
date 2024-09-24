using OnlineGasBooking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGasBooking.Models
{
    public class NavbarService : INavbarService
    {
        private readonly OnlineGasDBContext _context;

        public NavbarService(OnlineGasDBContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, IEnumerable<string>>> GetNavbarDataAsync()
        {
            var navbar = new Dictionary<string, IEnumerable<string>>();

            var categories = await _context.Categories.ToListAsync();

            foreach (var cat in categories)
            {
                var subCategories = await _context.SubCategories
                    .Where(x => x.CategoryID == cat.CategoryID)
                    .Select(x => x.Name)
                    .ToListAsync();

                navbar.Add(cat.Name, subCategories);
            }

            return navbar;
        }
    }

}
