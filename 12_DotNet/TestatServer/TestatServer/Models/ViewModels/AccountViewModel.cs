namespace TestatServer.Models.ViewModels
{
    public class AccountViewModel<T> where T : UserViewModel
    {
        public string AccountNr { get; set; }
        public T Owner { get; set; }
    }

    public class AccountViewModel : AccountViewModel<UserViewModel> { }

    public class AccountOwnerViewModel : AccountViewModel<OwnerViewModel>
    {
        public double Amount { get; set; }
        public string OwnerId { get; set; }
    }
}
