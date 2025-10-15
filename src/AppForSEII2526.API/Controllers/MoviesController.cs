using AppForSEII2526.API.DTOs.MovieDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<MoviesController> _logger;

        public MoviesController(ApplicationDbContext context, ILogger<MoviesController> logger)
        {
            _context = context;
            _logger = logger;
        }
        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        //public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        //{

        //    if (op2 == 0)
        //    {
        //        string error = "Op2 cannot be 0 to compute a division";
        //        _logger.LogError(DateTime.Now + " Error:" + error);
        //        return BadRequest(error);
        //    }
        //    decimal result = op1 / op2;
        //    return Ok(result);
        //}
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<MovieForRentalDTO>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelError), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetMoviesForRenting(string? movieTitle, string? genreName,
            DateTime? fromDate, DateTime? toDate) {

            if (fromDate != null && toDate != null && fromDate > toDate) {
                //return BadRequest( Problem("fromDate must be earlier than toDate", 
                //    $"fromDate ({fromDate}) toDate({toDate})", 400,"Bad Request", 
                //    "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"));
                ModelState.AddModelError("fromDate&toDate", "fromDate must be earlier than toDate");
                _logger.LogError($"{DateTime.Now} Error: fromDate must be earlier than toDate");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            //if not renting dates are provided a value by default is assigned
            fromDate = fromDate == null ? DateTime.Today.AddDays(1) : fromDate;
            toDate = toDate == null ? DateTime.Today.AddDays(2) : toDate;

            IList<MovieForRentalDTO> moviesDTOS = await _context.Movies
                .Include(movie=>movie.Genre)

                //to show when the movie was rented last time
                .Include(movie => movie.RentalItems)
                    .ThenInclude(rentailItem => rentailItem.Rental)
                
                .Where(movie=>(movie.Title.Contains(movieTitle)|| (movieTitle == null)
                    && (movie.Genre.Name.Contains(genreName)|| (genreName == null)))

                    //it is checkted that there are still movies available for the selected dates 
                    && (movie.RentalItems.Where(ri => ri.Rental.RentalDateFrom <= toDate
                                            && ri.Rental.RentalDateTo >= fromDate).Count() < movie.QuantityForRental)

                    )



                .OrderBy(movie=>movie.Title)

                //to show when the movie was rented last time
                .Select(movie=>new MovieForRentalDTO(movie.Id, movie.Title, movie.Genre.Name, 
                            movie.ReleaseDate, movie.PriceForRenting, 
                            movie.RentalItems.Max(ri=>ri.Rental.RentalDate)))
                .ToListAsync();
            return Ok(moviesDTOS);
        }
    }
}
