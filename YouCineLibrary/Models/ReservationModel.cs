namespace YouCineLibrary.Models
{
    public class ReservationModel
    {
        public int ID { get; set; }
        public CustomerModel Customer { get; set; }
        public ProjectionModel Projection { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
    }
}
