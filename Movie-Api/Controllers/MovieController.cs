using Microsoft.AspNetCore.Mvc;
using Movie_Api.Models.Filters;
using Movie_Api.Models.Request;
using Movie_Api.Services;

namespace Movie_Api.Controllers
{
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost("/movies")]
        public async Task<IActionResult> AddMovie([FromBody] AddMovieReq req)
        {
            var res = await _movieService.AddMovie(req);
            return res is not null ? Ok(res) : Conflict();
        }

        [HttpGet("/movies")]
        public async Task<JsonResult> GetMovies([FromQuery] MovieFilter filter)
        {
            return new JsonResult(await _movieService.GetMovies(filter));
        }

        [HttpPost("/reviews")]
        public async Task<IActionResult> AddReview([FromBody] AddReviewReq req)
        {
            var res = await _movieService.AddReview(req);
            return res is not null ? Ok(res) : Conflict();
        }

        [HttpGet("/reviews")]
        public async Task<IActionResult> GetReviews([FromQuery] ReviewFilter filter)
        {
            return new JsonResult(await _movieService.GetReviews(filter));
        }
    }
}
