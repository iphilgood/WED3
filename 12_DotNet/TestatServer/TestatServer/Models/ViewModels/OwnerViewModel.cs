namespace TestatServer.Models.ViewModels
{
    public class UserViewModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }


    public class OwnerViewModel : UserViewModel
    {
        public string AccountNr { get; set; }
        public string Login { get; set; }
    }
}