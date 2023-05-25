namespace Movie_Api.Models.Response
{
    public class MovieRes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public short Year { get; set; }
        public string Genre { get; set; }
        public string Actors { get; set; }
        public string Poster { get; set; }
        public float ImdbRating { get; set; }
    }
}
