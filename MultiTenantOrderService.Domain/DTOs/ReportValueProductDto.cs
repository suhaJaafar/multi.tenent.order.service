namespace MultiTenantOrderService.Domain.DTOs;
public class ReportValueProductDto
{
    public string Product { get; set; }
    public int NumberOfTransactions { get; set; }
    public decimal Amount { get; set; }
    public decimal Liters { get; set; }
}