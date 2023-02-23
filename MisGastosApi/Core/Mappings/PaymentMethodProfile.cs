using AutoMapper;
using MisGastosApi.Core.DTOs;
using MisGastosApi.Data.Models;

namespace MisGastosApi.Core.Mappings
{
    public class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            CreateMap<PaymentMethodDto, PaymentMethod>()
                .ReverseMap();
        }
    }
}
