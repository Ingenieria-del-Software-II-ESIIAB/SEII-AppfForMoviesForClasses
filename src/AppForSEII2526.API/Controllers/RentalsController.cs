using AppForSEII2526.API.DTOs.RentalDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AppForSEII2526.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase {
        private ApplicationDbContext _context;
        private ILogger<RentalsController> _logger;

        public RentalsController(ApplicationDbContext context, ILogger<RentalsController> logger) {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalForDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetRental(int id) {

            //perhaps it does not exist a Rental with the id provided
            RentalForDetailDTO? rental = await _context.Rentals
                .Where(r => r.Id == id)

                .Include(r => r.RentalItems) //join table RentalItems
                    .ThenInclude(ri => ri.Movie) //then join table Movies
                        .ThenInclude(movie => movie.Genre) //then join table Genre

                .Include(r=>r.Customer) //join table ApplicationUser

                .Select(r => new RentalForDetailDTO(r.Id, r.CostofRental, r.RentalDate, 
                        r.DeliveryAddress,r.NameCustomer,r.SurnameCustomer,
                        r.RentalDateFrom,r.RentalDateTo,r.PaymentMethod,

                        //we add the null forgiving operator, ! to the right-hand side to avoid the warning "maybe-null"
                        r.Customer.UserName!, 
                        r.RentalItems
                            .Select(ri => new RentalItemDTO(ri.Movie.Id,
                                    ri.Movie.Title, ri.Movie.Genre.Name,
                                    ri.Movie.PriceForRenting, ri.Description)).ToList())
                )
             //it obtains just the first Rental that satisfies the where clause
             .FirstOrDefaultAsync();



            if (rental == null) {
                _logger.LogError($"Error: Rental with id {id} does not exist");
                return NotFound();
            }


            return Ok(rental);
        }


        [HttpPost]
        [Route("[action]")]

        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]

        [ProducesResponseType(typeof(RentalForDetailDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateRental(RentalForCreateDTO rentalForCreate) {
            //any validation defined in PurchaseForCreate is checked before running the method so they don't have to be checked again
            if (rentalForCreate.RentalDateFrom <= DateTime.Today)
                ModelState.AddModelError("RentalDateFrom", "Error! Your rental date must start later than today");

            if (rentalForCreate.RentalDateFrom >= rentalForCreate.RentalDateTo)
                ModelState.AddModelError("RentalDateFrom&RentalDateTo", "Error! Your rental must end later than it starts");

            if (rentalForCreate.RentalItems.Count == 0)
                ModelState.AddModelError("RentalItems", "Error! You must include at least one movie to be rented");

            //we must relate the Rental to the User
            var user = await _context.Users.FirstOrDefaultAsync(au => au.UserName == rentalForCreate.UserNameCustomer);
            if (user == null)
                ModelState.AddModelError("RentalApplicationUser", "Error! UserName is not registered");


            //we must check that all the movies to be rented exist in the database 

            var movieTitles = rentalForCreate.RentalItems.Select(ri => ri.Title).ToList<string>();

            var movies = _context.Movies.Include(m => m.RentalItems)
                .ThenInclude(ri => ri.Rental)

                //we must check that all the movies to be rented exist in the database 
                .Where(m => movieTitles.Contains(m.Title))

                //we use an anonymous type https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/anonymous-types
                .Select(m => new {
                    m.Id,
                    m.Title,
                    m.QuantityForRental,
                    m.PriceForRenting,

                    //we count the number of rentalItems that are within the rental period 
                    NumberOfRentedItems = m.RentalItems.Count(ri => ri.Rental.RentalDateFrom <= rentalForCreate.RentalDateTo
                            && ri.Rental.RentalDateTo >= rentalForCreate.RentalDateFrom)
                })
                .ToList();



            //we must provide rental with the info to be saved in the database
            Rental rental = new Rental(rentalForCreate.DeliveryAddress, rentalForCreate.NameCustomer,
                rentalForCreate.SurnameCustomer, user, rentalForCreate.RentalDateFrom, rentalForCreate.RentalDateTo,
                rentalForCreate.PaymentMethod, new List<RentalItem>());


            rental.CostofRental = 0;
            var numDays = (rental.RentalDateTo - rental.RentalDateFrom).TotalDays;


            foreach (var item in rentalForCreate.RentalItems) {
                var movie = movies.FirstOrDefault(m => m.Title == item.Title);
                //we must check that there is enough quantity to be rented in the database
                if ((movie == null) || (movie.NumberOfRentedItems >= movie.QuantityForRental)) {
                    ModelState.AddModelError("RentalItems", $"Error! Movie titled '{item.Title}' is not available for being rented from {rentalForCreate.RentalDateFrom.ToShortDateString()} to {rentalForCreate.RentalDateTo.ToShortDateString()}");
                }
                else {
                    // rental does not exist in the database yet and does not have a valid Id, so we must relate rentalitem to the object rental
                    rental.RentalItems.Add(new RentalItem(movie.Id, rental, movie.PriceForRenting, item.Description));
                    item.PriceForRenting = movie.PriceForRenting;
                }
            }
         //   rental.CostofRental = rental.RentalItems.Sum(ri =>  ri.Price * numDays);

            //if there is any problem because of the available quantity of movies or because the movie does not exist
            if (ModelState.ErrorCount > 0) {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(rental);

            try {
                //we store in the database both rental and its rentalitems
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Rental", $"Error! There was an error while saving your rental, plese, try again later");
                return Conflict("Error" + ex.Message);

            }

            var rentalDetail = new RentalForDetailDTO(rental.Id,rental.CostofRental,
                rental.RentalDate,rental.DeliveryAddress,rental.NameCustomer,rental.SurnameCustomer,
                rental.RentalDateFrom, rental.RentalDateTo,rental.PaymentMethod,

                rental.Customer.UserName!,
                new List<RentalItemDTO>());

            return CreatedAtAction("GetRental", new { id = rental.Id }, rentalDetail);

        }
    }
}

