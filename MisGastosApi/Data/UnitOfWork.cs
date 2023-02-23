using MisGastosApi.Data.Repositories;

namespace MisGastosApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IPaymentMethodRepository PaymentMethods { get; }

        public UnitOfWork(
            IPaymentMethodRepository paymentMethodRepository
            )
        {
            PaymentMethods = paymentMethodRepository;
        }
    }
}
