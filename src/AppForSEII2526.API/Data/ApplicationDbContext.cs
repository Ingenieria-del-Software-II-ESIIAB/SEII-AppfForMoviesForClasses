using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<RentalItem> RentalItems { get; set; }
    
    [Precision(10,2)]
    public decimal RentalPrice { get; set; }

}
