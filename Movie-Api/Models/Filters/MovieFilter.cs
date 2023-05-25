using Movie_Api.Base;
using Movie_Api.Entities;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Movie_Api.Models.Filters
{
    public class MovieFilter : BaseFilter
    {
        public float? MinimumImdbRating { get; set; }
        public float? MaximumImdbRating { get; set; }
        public short? YearStart { get; set; }
        public short? YearEnd { get; set; }

        public static Dictionary<string, Expression<Func<Movie, object>>> Mapper
        {
            get =>
                new()
                {
                    { "title", x => x.Title },
                    { "year", x => x.Year },
                    { "imdbRating", x => x.ImdbRating }
                };
        }
    }
}
