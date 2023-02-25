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

    public async Task Delete(PaymentMethodDto paymentMethodDto)
    {
        var (isValid, errorMessage) = await ValidateDeletePaymentMethod(paymentMethodDto);
        if(!isValid) throw new Exception(errorMessage);

        await unitOfWork.PaymentMethods.Delete(paymentMethodDto.PaymentMethodId.Value);
    }

    private async Task<(bool, string)> ValidateDeletePaymentMethod(PaymentMethodDto paymentMethodDto)
    {
        var userPaymentMethods = await GetPaymentsByUser(paymentMethodDto.UserId);
        if (!userPaymentMethods.Any(up => up.PaymentMethodId == paymentMethodDto.PaymentMethodId))
        {
            return (false, "El usuario no puede borrar el actual método de pago");
        }

        return (true, string.Empty);
    }

}
