using Fish_Manage.Models;

namespace Fish_Manage.Repository.IRepository
{
    public interface ICouponModelRepository : IRepository<CouponModel>
    {
        Task<CouponModel> UpdateAsync(CouponModel entity);
    }
}
