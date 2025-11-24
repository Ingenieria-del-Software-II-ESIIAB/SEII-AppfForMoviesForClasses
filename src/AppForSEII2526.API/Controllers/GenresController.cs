using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers {
    public class GenresController : Controller {
        private readonly ApplicationDbContext _context;
        private ILogger _logger;

        public GenresController(ApplicationDbContext context, ILogger<MoviesController> logger) {
            _context = context;
            _logger = logger;
        }

        // GET: api/Movies/GetMoviesForPurchase
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetGenres(string? genreName) {

            IList<string> genres = await _context.Genre
                .Where(genre => (genreName == null || genre.Name.Contains(genreName))) // where clause             
                .OrderBy(genre => genre.Name)
                .Select(genre => genre.Name)
                .ToListAsync();

            return Ok(genres);
        }
    }
}
