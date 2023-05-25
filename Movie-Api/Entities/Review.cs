using Movie_Api.Base;

namespace Movie_Api.Entities
{
    public class Review : IBaseEntity
    {
        public int Id { get; set; }
        public string ReviewText { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
