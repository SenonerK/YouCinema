using System;

namespace YouCineLibrary.Models
{
    public class BorrowLogModel
    {
        public DateTime Date { get; set; }
        public string Movie { get; set; }
        public string Customer { get; set; }
    }
}
