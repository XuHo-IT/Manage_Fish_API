using Fish_Manage.Models;
using Microsoft.EntityFrameworkCore;

namespace Fish_Manage.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly FishManageContext _context;

        public ProductRepository(FishManageContext context) : base(context)
        {
            _context = context;
        }

        async Task<Product> IProductRepository.UpdateAsync(Product entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
