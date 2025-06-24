namespace Backend.Models
{
    public record PaymentDto(
        int Id,
        int ClientId,
        decimal AmountT,
        DateTime Timestamp,
        ClientDto Client
    );
}
