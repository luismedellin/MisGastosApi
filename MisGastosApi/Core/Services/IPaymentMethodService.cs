using MisGastosApi.Core.DTOs;
using MisGastosApi.Data.Models;

namespace MisGastosApi.Core.Services
{
    public interface IPaymentMethodService
    {
        Task<List<PaymentMethod>> GetPaymentsByUser(string userId);
        Task<PaymentMethod> Save(PaymentMethodDto paymentMethod);
    }
}
