namespace MisGastosApi.Data.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public DateTime? DeadLine { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
