using System;

namespace YouCineLibrary.Models
{
    public class BorrowModel
    {
        public string ID { get; set; }
        public DateTime LendDate { get; set; }
        public DateTime BringBackDate { get; set; }
        public string Customer { get; set; }
        public string Movie { get; set; }

        public bool Remove()
        {
            return Config.Connection.RemoveBorrowedMovie(ID);
        }
    }
}
