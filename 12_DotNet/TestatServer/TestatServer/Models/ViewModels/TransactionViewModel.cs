namespace TestatServer.Models.ViewModels
{
    public class TransactionViewModel
    {
        public string Target { get; set; }
        public double Amount { get; set; }
    }

    public class TransactionResultViewModel : Transaction
    {
        public double Total { get; set; }
    }
}
