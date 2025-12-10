namespace Domain.Entities;

public class DashBoard
{
    public int TotalBooks { get; set; }
    public int AvailableBooks { get; set; }
    public int ActiveLoans { get; set; }
    public int PendingReturns { get; set; }
}