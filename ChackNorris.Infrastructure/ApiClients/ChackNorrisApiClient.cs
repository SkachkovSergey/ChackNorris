using ChackNorris.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ChackNorris.Infrastructure.ApiClients
{
    public class ChuckNorrisApiClient
    {
        private readonly HttpClient _client;
        public ChuckNorrisApiClient()
        {
            _client = new HttpClient(); 
        }
        public async Task<ChuckNorrisApiResponse> GetRandomJokeAsync()
        {
            try
            {
                var responce = await _client.GetFromJsonAsync<ChuckNorrisApiResponse>("https://api.chucknorris.io/jokes/random");

                return responce!;
            } 
            catch(HttpRequestException)
            {
                throw new InvalidOperationException("Geen internet");
            }
            catch (NotSupportedException)
            {
                throw new InvalidOperationException("De inhoud kon niet worden gelezen als JSON.");

            }
            catch(JsonException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        public async Task<ChuckNorrisApiResponse> GetRandomJokeByCategoryAsync(string category)
        {
            throw new Exception("");
        }
        public Task<IEnumerable<string>> GetCategoriesAsync()
        {
            throw new NotFiniteNumberException();
        }
    }
}
