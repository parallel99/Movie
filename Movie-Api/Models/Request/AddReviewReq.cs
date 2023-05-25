namespace Movie_Api.Models.Request
{
    public class AddReviewReq
    {
        public int MovieId { get; set; }
        public string ReviewText { get; set; }
    }
}
