using AppForSEII2526.API.DTOs.MovieDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> GetMoviesForRenting(string? movieTitle) {
            IList<MovieForRentalDTO> moviesDTOS = await _context.Movies
                .Include(movie=>movie.Genre)
                .Where(movie=>movie.Title.Contains(movieTitle) 
                        || (movieTitle == null))
                .OrderBy(movie=>movie.Title)
                .Select(movie=>new MovieForRentalDTO(movie.Id, movie.Title, movie.Genre.Name))
                .ToListAsync();
            return Ok(moviesDTOS);
        }
    }
}
