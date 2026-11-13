using System;
using System.Collections.Generic;
using System.Text;

namespace ChackNorris.Domain.Models
{
    public class Joke
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public double Score { get; set; }
    }
}
