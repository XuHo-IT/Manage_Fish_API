﻿using Fish_Manage.Models;
using Fish_Manage.Models.Momo;
using Fish_Manage.Repository.IRepository;

namespace Fish_Manage.Service.IService
{
    public interface IMomoService : IRepository<Order>
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfoModel model);
        Task<MomoExecuteResponseModel> PaymentExecuteAsync(IQueryCollection collection);
    }
}
