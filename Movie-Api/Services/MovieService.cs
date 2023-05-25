using Microsoft.EntityFrameworkCore;
using Movie_Api.DbContexts;
using Movie_Api.Entities;
using Movie_Api.Models.Filters;
using Movie_Api.Models.FromJson;
using Movie_Api.Models.Request;
using Movie_Api.Models.Response;
using System.Globalization;
using System.Text.Json;

namespace Movie_Api.Services
{
    public interface IMovieService
    {
        Task<Movie?> AddMovie(AddMovieReq req);
        Task<List<MovieRes>> GetMovies(MovieFilter filter);
        Task<Review?> AddReview(AddReviewReq req);
        Task<List<ReviewRes>> GetReviews(ReviewFilter filter);
    }
    public class MovieService : IMovieService
    {
        private readonly DataContext _dataContext;

        public MovieService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Movie?> AddMovie(AddMovieReq req)
        {
            var movieDetails = await GetMovieDetailsFromOmdbApi(req.Title);

            if (movieDetails is null || movieDetails.Title is null)
            {
                return null;
            }

            var movieIsExist = await _dataContext.Movie.AnyAsync(x => x.ImdbId == movieDetails.imdbID);

            if (movieIsExist)
            {
                return null;
            }

            try
            {
                var provider = new NumberFormatInfo
                {
                    NumberDecimalSeparator = "."
                };

                var movie = new Movie
                {
                    Year = short.Parse(movieDetails.Year),
                    Genre = movieDetails.Genre,
                    Actors = movieDetails.Actors,
                    Title = movieDetails.Title,
                    Poster = movieDetails.Poster,
                    ImdbRating = float.Parse(movieDetails.imdbRating, provider),
                    ImdbId = movieDetails.imdbID,
                };

                await _dataContext.AddAsync(movie);
                var res = await _dataContext.SaveChangesAsync() > 0;

                if (res) return movie;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        public async Task<List<MovieRes>> GetMovies(MovieFilter filter)
        {
            var query = GetFilteredMovieQuery(filter);

            var res = await query
                                .Skip(filter.Skip)
                                .Take(filter.Take)
                                .Select(x => new MovieRes
                                {
                                    Id = x.Id,
                                    Title = x.Title,
                                    Actors = x.Actors,
                                    Genre = x.Genre,
                                    ImdbRating = x.ImdbRating,
                                    Poster = x.Poster,
                                    Year = x.Year,
                                }).ToListAsync();

            return res;
        }

        public async Task<Review?> AddReview(AddReviewReq req)
        {
            var movie = await _dataContext.Movie.FindAsync(req.MovieId);

            if (movie is null)
            {
                return null;
            }

            try
            {
                var review = new Review
                {
                    Movie = movie,
                    ReviewText = req.ReviewText                    
                };

                await _dataContext.AddAsync(review);
                var res = await _dataContext.SaveChangesAsync() > 0;

                if (res) return review;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        public async Task<List<ReviewRes>> GetReviews(ReviewFilter filter)
        {
            var query = GetFilteredReviewQuery(filter);

            var res = await query
                                .Skip(filter.Skip)
                                .Take(filter.Take)
                                .Select(x => new ReviewRes
                                {
                                    Id = x.Id,
                                    MovieId = x.MovieId,
                                    ReviewText = x.ReviewText,
                                }).ToListAsync();

            return res;
        }

        private IQueryable<Movie> GetFilteredMovieQuery(MovieFilter filter)
        {
            var query = _dataContext.Movie.AsQueryable();

            if (filter.YearStart.HasValue)
            {
                query = query.Where(x => x.Year >= filter.YearStart);
            }

            if (filter.YearEnd.HasValue)
            {
                query = query.Where(x => x.Year <= filter.YearEnd);
            }

            if (filter.MinimumImdbRating.HasValue)
            {
                query = query.Where(x => x.ImdbRating >= filter.MinimumImdbRating);
            }

            if (filter.MaximumImdbRating.HasValue)
            {
                query = query.Where(x => x.ImdbRating <= filter.MaximumImdbRating);
            }

            if (filter.Ascending)
            {
                query = query.OrderBy(MovieFilter.Mapper.GetValueOrDefault(filter.OrderBy, p => p.Id));
            }
            else
            {
                query = query.OrderByDescending(MovieFilter.Mapper.GetValueOrDefault(filter.OrderBy, p => p.Id));
            }

            return query;
        }

        private IQueryable<Review> GetFilteredReviewQuery(ReviewFilter filter)
        {
            var query = _dataContext.Review.AsQueryable();

            if (filter.MovieId.HasValue)
            {
                query = query.Where(x => x.MovieId == filter.MovieId);
            }

            if (filter.MovieYearStart.HasValue)
            {
                query = query.Where(x => x.Movie.Year >= filter.MovieYearStart);
            }

            if (filter.MovieYearEnd.HasValue)
            {
                query = query.Where(x => x.Movie.Year <= filter.MovieYearEnd);
            }

            if (filter.Ascending)
            {
                query = query.OrderBy(ReviewFilter.Mapper.GetValueOrDefault(filter.OrderBy, p => p.Id));
            }
            else
            {
                query = query.OrderByDescending(ReviewFilter.Mapper.GetValueOrDefault(filter.OrderBy, p => p.Id));
            }

            return query;
        }

        private async Task<MovieFromOmdbApi?> GetMovieDetailsFromOmdbApi(string title)
        {
            using HttpClient client = new();

            try
            {
                title = Uri.EscapeDataString(title);
                var response = await client.GetAsync($"http://www.omdbapi.com/?t={title}&apikey=3f6c0764");
                var json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<MovieFromOmdbApi>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
    }
}
