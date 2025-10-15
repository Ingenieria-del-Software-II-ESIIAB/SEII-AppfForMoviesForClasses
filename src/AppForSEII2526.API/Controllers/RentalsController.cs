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
    }
}
