namespace Backend.Models
{
    public record CreatePaymentDto(
      int ClientId,
      decimal AmountT,
      DateTime Timestamp
    );
}
