using System.ComponentModel.DataAnnotations;

namespace Exam2.ViewModels.AuthVMs
{
    public class RegisterVM
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
