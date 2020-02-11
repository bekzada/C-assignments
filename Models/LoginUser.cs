using System.ComponentModel.DataAnnotations;

namespace DemoLoginRegistration.Models
{
    public class LoginUser
    {
       [Required(ErrorMessage="Email is required.")]
       [EmailAddress]
       [Display(Name="Email:")]
       public string LoginEmail  { get; set;}
       [Required (ErrorMessage="Password is required.")]
       [DataType(DataType.Password)]
       public string LoginPassword { get; set;}

    }
}