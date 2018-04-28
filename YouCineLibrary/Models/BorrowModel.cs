using System;

namespace YouCineLibrary.Models
{
    public class BorrowModel
    {
        public int ID { get; set; }
        public DateTime LendDate { get; set; }
        public DateTime BringBackDate { get; set; }
        public CustomerModel Cutomer { get; set; }
        public MovieModel Movie { get; set; }
    }
}
