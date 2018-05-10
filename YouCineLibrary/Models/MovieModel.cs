using System;

namespace YouCineLibrary.Models
{
    public class MovieModel
    {
        public string ID { get; set; }
        public string MovieDescription { get; set; }
        public string MovieName { get; set; }
        public DateTime Published { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
    }
}
