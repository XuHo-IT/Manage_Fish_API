using Fish_Manage.Models;

namespace Fish_Manage.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        Double GetMoneyPerTerm(int term);
        Task<Order> UpdateAsync(Order entity);
        ApplicationUser UserBuyMost(int term);
    }
}
