using Movie_Api.Base;
using System.Text.Json.Serialization;

namespace Movie_Api.Entities
{
    public class Movie : IBaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public short Year { get; set; }
        public string Genre { get; set; }
        public string Actors { get; set; }
        public string Poster { get; set; }
        public float ImdbRating { get; set; }

        [JsonIgnore]
        public string ImdbId { get; set; }
    }
}
