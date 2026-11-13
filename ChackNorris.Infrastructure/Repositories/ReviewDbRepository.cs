using ChackNorris.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChackNorris.Infrastructure.Repositories
{
    public class ReviewDbRepository
    {
        private readonly string _connectionString;
        public ReviewDbRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreateAsync(Review review)
        {        
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO Reviews (JokeId, Score) 
                           VALUES (@JokeId, @Score);
                            SELECT CAST(SCOPE_IDENTITY() AS INT)";

                review.Id = await connection.ExecuteScalarAsync<int>(sql, review);
            }
        }
        //public async Task<IEnumerable<Review>>GetByIdAsync(Review review)
        //{
        //    using(SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        const string sql = """
        //            SELECT * FROM Reviews
        //            WHERE JokeId = @JokeId;
        //            """;
        //        return await connection.QueryAsync<Review>(sql, new { JokeId = review.JokeId });
        //    }
        //}



        //public async Task AddReviewAsync(Review review)
        //{
        //    await _reviewRepository.CreateAsync(review);

        //    string sql = @"SELECT AVG(Score) FROM Reviews WHERE JokeId = @JokeId;";

        //    IEnumerable<Review> reviews = await _reviewRepository.GetByJokeIdAsync(review.JokeId);

        //    double avg = reviews.Average(r => r.Score);

        //    await _repository.CreateAsync(avg);
        //}
    }
}
