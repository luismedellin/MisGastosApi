using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MisGastosApi.Core.DTOs;
using MisGastosApi.Core.Services;
using MisGastosApi.Data.Models;
using System.Security.Claims;

namespace MisGastosApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/payment-methods")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentsByUser()
        {
            var userName = GetUserName();
            try
            {
                var paymentMethods = await _paymentMethodService.GetPaymentsByUser(userName);
                return Ok(paymentMethods);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePaymentMethod([FromBody] PaymentMethodDto paymentMethodDto)
        {
            paymentMethodDto.UserId = GetUserName();
            try
            {
                var paymentMethod = await _paymentMethodService.Save(paymentMethodDto);
                return Ok(paymentMethod);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{paymentMethodId}")]
        public async Task<IActionResult> SavePaymentMethod(int paymentMethodId)
        {
            var paymentMethod = new PaymentMethodDto
            {
                UserId = GetUserName(),
                PaymentMethodId = paymentMethodId
            };

            try
            {
                await _paymentMethodService.Delete(paymentMethod);
                return Ok(paymentMethod);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private string GetUserName()
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
        }
    }
}
