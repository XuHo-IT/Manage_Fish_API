using Fish_Manage.Models;
using Fish_Manage.Repository.IRepository;

namespace Fish_Manage.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> UpdateAsync(Product entity);
    }
}
