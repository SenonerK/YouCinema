using System;

namespace YouCineLibrary.Models
{
    public class ActorModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Rating { get; set; }
    }
}
