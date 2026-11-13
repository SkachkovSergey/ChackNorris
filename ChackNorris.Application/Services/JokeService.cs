using ChackNorris.Domain.Models;
using ChackNorris.Infrastructure.ApiClients;
using ChackNorris.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChackNorris.Application.Services
{
   
    public class JokeService
    {
        private readonly ChuckNorrisApiClient _client;
        private readonly JokeDbRepository _jokeRepository;
        private readonly ReviewDbRepository _reviewRepository;
        public JokeService(string connectionString)
        {
            _client = new ChuckNorrisApiClient();
            _jokeRepository = new JokeDbRepository(connectionString);
            _reviewRepository = new ReviewDbRepository(connectionString);
        }
        public async Task<Joke> GetRandomJokeAsync()
        {
            var responce = await _client.GetRandomJokeAsync();
            Joke joke = new Joke();
            joke.Content = responce.Value;
            joke.Score = 0;
            return joke; 
        }
        public async Task<IEnumerable<Joke>> GetAllJokesAsync() => 
            await _jokeRepository.GetAll();

        public async Task AddJokeAsync(Joke joke) => 
            await _jokeRepository.CreateAsync(joke);

        public async Task<Joke> GetRandomJokeByCategoryAsync(string category)
        {
            return null!;
        }
        public async Task AddReviewAsync(Review review) => 
            await _reviewRepository.CreateAsync(review);
        
        //public async Task<Review> GetReviewByIdAsync(Review review) => 
        //    await _reviewRepository.GetByIdAsync(review);
        
        
        
    }
}
