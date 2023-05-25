namespace Movie_Api.Models.Response
{
    public class ReviewRes
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string ReviewText { get; set; }
    }
}
