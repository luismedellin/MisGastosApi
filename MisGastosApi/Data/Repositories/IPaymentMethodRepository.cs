using MisGastosApi.Data.Models;

namespace MisGastosApi.Data.Repositories
{
    public interface IPaymentMethodRepository
    {
        Task<List<PaymentMethod>> GetPaymentsByUser(string userId);
        Task Save(PaymentMethod paymentMethod);
        Task Delete(int paymentMethodId);
    }
}
