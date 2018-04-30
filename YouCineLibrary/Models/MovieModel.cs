using System;

namespace YouCineLibrary.Models
{
    public class MovieModel
    {
        public int ID { get; set; }
        public string MovieDescription { get; set; }
        public string MovieName { get; set; }
        public DateTime Published { get; set; }
        public decimal Price { get; set; }
        public byte[] Image { get; set; }
    }
}
