using Movie_Api.Base;
using Movie_Api.Entities;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Movie_Api.Models.Filters
{
    public class ReviewFilter : BaseFilter
    {
        public int? MovieId { get; set; }
        public short? MovieYearStart { get; set; }
        public short? MovieYearEnd { get; set; }

        public static Dictionary<string, Expression<Func<Review, object>>> Mapper
        {
            get =>
                new()
                {
                    { "movieId", x => x.MovieId },
                    { "movieTitle", x => x.Movie.Title },
                    { "movieRating", x => x.Movie.ImdbRating },
                };
        }
    }
}
