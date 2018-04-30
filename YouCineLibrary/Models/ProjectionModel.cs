using System;

namespace YouCineLibrary.Models
{
    public class ProjectionModel
    {
        public string ID { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string Movie { get; set; }
        public string Auditorium { get; set; }
    }
}
