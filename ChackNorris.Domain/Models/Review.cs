using System;
using System.Collections.Generic;
using System.Text;

namespace ChackNorris.Domain.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int JokeId { get; set; }
        public double Score { get; set; }
    }
}
