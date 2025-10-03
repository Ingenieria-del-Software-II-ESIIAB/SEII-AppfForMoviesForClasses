using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {

    [Required]
    public string Name {get;set;}

    [Required]
    public string Surname {get;set;}

    [Required]
    public string Address { get; set; }

    public IList<Rental> Rentals { get; set; }

}