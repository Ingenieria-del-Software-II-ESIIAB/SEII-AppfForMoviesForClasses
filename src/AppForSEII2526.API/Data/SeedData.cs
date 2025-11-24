using AppForSEII2526.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Data {
    public static class SeedData {

        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger) {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the roles in the Database.");
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the Users in the Database.");
            }
            try {
                //it initializes the database with genres and movies
                SeedGenresAndMovies(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the Movies and Genres in the Database.");
            }
            try {
                var user = dbContext.Users.OfType<ApplicationUser>().FirstOrDefault(u => u.UserName == "elena@uclm.es");

                //it initializes the database with a Rental
                SeedRental(dbContext, user);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding a Rental in the Database.");
            }

        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles) {

            foreach (string roleName in roles) {
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result) {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles) {
            //first, it checks the user does not already exist in the DB
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null) {
                ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es", "Avda. España 2, Albacete");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    //administrator role
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }

            if (userManager.FindByNameAsync("gregorio@uclm.es").Result == null) {
                ApplicationUser user = new ApplicationUser("2", "Gregorio", "Diaz Descalzo", "gregorio@uclm.es", "Avda. España 25, Ciudad Real");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "APassword1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    //employee role
                    userManager.AddToRoleAsync(user, roles[1]).Wait();
                }
            }

            if (userManager.FindByNameAsync("peter@uclm.es").Result == null) {
                //A customer class has been defined because it has different attributes (purchase, rental, etc.)
                ApplicationUser user = new ApplicationUser("3", "Peter", "Jackson", "peter@uclm.es", "Avda. España 75, London");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "OtherPass12$");

                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    //customer role
                    userManager.AddToRoleAsync(user, roles[2]).Wait();

                }
            }

        }

        public static void SeedGenresAndMovies(ApplicationDbContext dbcontext) {
            string[] genresnames = ["Sci - Fi", "Drama", "Comedy", "Soap opera"];
            List<Genre> genres = [];
            Movie movie;
            foreach (string genrename in genresnames) {
                var genre = dbcontext.Genre.FirstOrDefault(g => g.Name == genrename);
                if (genre == null)
                    genres.Add(new Genre(genrename));
                else
                    genres.Add(genre);
            }
            if (dbcontext.Movies.FirstOrDefault(m => m.Title == "The last of us") == null) {
                movie = new Movie("The lord of the rings", genres[0], new DateTime(2011, 10, 20), 10.0m, 5, 1.0m, 2);
                dbcontext.Movies.Add(movie);

            }

            if (dbcontext.Movies.FirstOrDefault(m => m.Title == "The man in the high castle") == null) {
                movie = new Movie("The mechanic orange", genres[1], new DateTime(1988, 02, 23), 15.0m, 10, 2.0m, 10);
                dbcontext.Movies.Add(movie);
            }

            //it saves the modification of dbcontext to the database
            dbcontext.SaveChanges();

            //alternatively you may have used a raw SQL
            //dbcontext.Database.ExecuteSqlRaw("INSERT INTO [Movies] ([Id], [Title], [GenreId], [ReleaseDate], [PriceForPurchase], [QuantityForPurchase], [PriceForRenting], [QuantityForRenting]) VALUES (1, N'The lord of the rings', 1, N'2011-10-20 00:00:00', 10, 1000, 1, 100)");
            //dbcontext.Database.ExecuteSqlRaw("INSERT INTO [Movies] ([Id], [Title], [GenreId], [ReleaseDate], [PriceForPurchase], [QuantityForPurchase], [PriceForRenting], [QuantityForRenting]) VALUES (2, N'The flying castle', 2, N'2007-04-04 00:00:00', 20, 1000, 3, 10)");


            //Since EFCORE7, you can perform bulk updates with linq.
            dbcontext.Movies.ExecuteUpdate(s => s.SetProperty(m => m.QuantityForPurchase, 10));

            //other example using existing information: add 100 to the QuantityForPurchase of each Movie
            //dbcontext.Movies.ExecuteUpdate(s => s.SetProperty(m => m.QuantityForPurchase, m=>m.QuantityForPurchase+100));

            //You can alternatively use raw SQL to perform the operation where performance is sensitive:
            //dbcontext.Database.ExecuteSqlRaw("UPDATE [Movies] SET [QuantityForPurchase] = 100");

            dbcontext.SaveChanges();


        }

        public static void SeedRental(ApplicationDbContext dbcontext, ApplicationUser user) {

            if (dbcontext.Rentals.FirstOrDefault(p => p.Id == 1) == null) {
                var movie = dbcontext.Movies.First();
                var rental = new Rental("Avda. España s/n, Albacete 02071", "Elena", "Navarro Martínez", user,
                    DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), PaymentMethodType.CreditCard, new List<RentalItem>());

                rental.RentalItems.Add(new RentalItem(movie.Id, rental, 1.0m, "My favourite movie"));

                decimal numDays = (decimal)(rental.RentalDateTo - rental.RentalDateFrom).TotalDays;
                rental.CostofRental = rental.RentalItems.Sum(ri => ri.Price * numDays);

                dbcontext.Rentals.Add(rental);
            }


            dbcontext.SaveChanges();

        }
    }
}
