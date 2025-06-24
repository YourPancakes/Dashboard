namespace Backend.Models
{
    public record ClientWithPaymentsDto(
        int Id,
        string Name,
        string Email,
        decimal BalanceT,
        List<PaymentIdDto> Payments
    );
}
