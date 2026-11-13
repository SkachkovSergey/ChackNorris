using ChackNorris.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChackNorris.Infrastructure.Repositories
{
    public class JokeDbRepository
    {
        private readonly string _connectionString;
        public JokeDbRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task CreateAsync(Joke joke)
        {
            const string sql = @"
                            INSERT INTO Jokes (Content, Score)
                            VALUES (@Content, @Score);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //int rows = await connection.ExecuteAsync(
                //    sql, 
                //    new { Content = joke.Content, Score = joke.Score }
                //    );

                joke.Id = await connection.ExecuteScalarAsync<int>(sql, joke);
            }
            
        }
        public async Task <IEnumerable<Joke>>GetAll()
        {                       
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT Id, Content, Score FROM Jokes;";
                var jokes = await connection.QueryAsync<Joke>(sql);

                return jokes;
            }
        }
        public async Task<Joke?> GetByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT Id, Content FROM Jokes WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync(sql, new { Id = id });
            }
        }
        public async Task UpdateScoreAsync(int id, double score)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                const string sql = @"Update Jokes SET Score = @Score = WHERE Id = @Id";

                await connection.ExecuteAsync(sql, new { Score = score, Id = id });
            }
            
        }
    }
}
