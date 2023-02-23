using MisGastosApi.Data.Repositories;

namespace MisGastosApi.Data
{
    public interface IUnitOfWork
    {
        IPaymentMethodRepository PaymentMethods { get; }
    }
}
