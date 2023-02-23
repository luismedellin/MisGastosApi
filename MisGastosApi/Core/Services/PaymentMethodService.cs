using AutoMapper;
using MisGastosApi.Core.DTOs;
using MisGastosApi.Data;
using MisGastosApi.Data.Models;

namespace MisGastosApi.Core.Services;

public class PaymentMethodService: IPaymentMethodService
{
    private readonly IUnitOfWork unitOfWork;
    public readonly IMapper mapper;

    public PaymentMethodService(IUnitOfWork unitOfWork,
            IMapper mapper)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.mapper = mapper;
    }

    public Task<List<PaymentMethod>> GetPaymentsByUser(string userId)
    {
        return unitOfWork.PaymentMethods.GetPaymentsByUser(userId);
    }

    public async Task<PaymentMethod> Save(PaymentMethodDto paymentMethodDto)
    {
        var paymentMethod = mapper.Map<PaymentMethod>(paymentMethodDto);
        paymentMethod.CreatedDate = DateTime.Now;
        await unitOfWork.PaymentMethods.Save(paymentMethod);
        return paymentMethod;
    }
}
