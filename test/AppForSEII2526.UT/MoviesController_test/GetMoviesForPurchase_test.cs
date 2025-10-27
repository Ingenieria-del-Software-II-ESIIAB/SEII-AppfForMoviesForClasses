using AppForMovies.UT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.MoviesController_test {
    public class GetMoviesForPurchase_test : AppForMovies4SqliteUT {
        public GetMoviesForPurchase_test() {

            var genres = new List<Genre>() {
                    new Genre("Sci - Fi"),
                    new Genre("Drama"),
            };

            var movies = new List<Movie>(){
                    new Movie("The lord of the rings", genres[0],new DateTime(2011, 10, 20),10.0m, 5,1.0m,1),
                    new Movie("The mechanic orange", genres[1],new DateTime(1988, 02, 23),15.0m, 10,2.0m,2),
             //this movie has quantityforpurchase=0 and quantityforrenting=0 so it shouldn't be returned when 
                    new Movie("The flying castle", genres[1],new DateTime(2007, 04, 04),20.0m, 0,3.0m,10),
            };
            _context.AddRange(genres);
            _context.AddRange(movies);
            _context.SaveChanges();

        }
    }
}
