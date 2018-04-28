using System;

namespace YouCineLibrary.Models
{
    public class ProjectionModel
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public float Price { get; set; }
        public MovieModel Movie { get; set; }
        public AuditoriumModel Auditorium { get; set; }
    }
}
