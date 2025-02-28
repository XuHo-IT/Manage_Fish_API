using Fish_Manage.Models;
using Fish_Manage.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Fish_Manage.Repository
{
    public class CouponModelRepository : Repository<CouponModel>, ICouponModelRepository
    {
        private readonly FishManageContext _context;

        public CouponModelRepository(FishManageContext context) : base(context)
        {
            _context = context;
        }
        async Task<CouponModel> ICouponModelRepository.UpdateAsync(CouponModel entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
