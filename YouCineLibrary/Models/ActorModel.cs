using System;

namespace YouCineLibrary.Models
{
    public class ActorModel
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public double Rating { get; set; }
    }
}
