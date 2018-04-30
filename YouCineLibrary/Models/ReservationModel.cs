namespace YouCineLibrary.Models
{
    public class ReservationModel
    {
        public string ID { get; set; }
        public string Customer { get; set; }
        public string Projection { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
    }
}
