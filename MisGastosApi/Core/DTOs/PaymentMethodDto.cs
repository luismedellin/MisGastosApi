namespace MisGastosApi.Core.DTOs
{
    public class PaymentMethodDto
    {
        public int? PaymentMethodId { get; set; }
        public string? UserId { get; set; }
        public string? Description { get; set; }
        public DateTime? DeadLine { get; set; }
    }
}
