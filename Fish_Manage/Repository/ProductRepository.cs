using Fish_Manage.Models;
using Fish_Manage.Repository.IRepository;
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

        public async Task<List<Product>> GetProductAsc(decimal minRange, decimal maxRange)
        {
            return await _context.Products.Where(p => p.Price >= minRange && p.Price <= maxRange).OrderBy(p => p.Price).ToListAsync();
        }


        public async Task<List<Product>> GetProductDesc(decimal minRange, decimal maxRange)
        {
            return await _context.Products.Where(p => p.Price >= minRange && p.Price <= maxRange).OrderByDescending(p => p.Price).ToListAsync();
        }

        public async Task<List<Product>> GetProductInRange(decimal minRange, decimal maxRange)
        {
            return await _context.Products.Where(p => p.Price >= minRange && p.Price <= maxRange).ToListAsync();
        }

        public async Task<List<Product>> GetProductNewest(decimal minRange, decimal maxRange)
        {
            return await _context.Products.Where(p => p.Price >= minRange && p.Price <= maxRange).OrderBy(p => p.ProductId).ToListAsync();

        }

        public async Task<List<Product>> GetProductOldest(decimal minRange, decimal maxRange)
        {
            return await _context.Products.Where(p => p.Price >= minRange && p.Price <= maxRange).OrderByDescending(p => p.ProductId).ToListAsync();
        }

        async Task<Product> IProductRepository.UpdateAsync(Product entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
