using System;
using System.Collections.Generic;

namespace YouCineLibrary.Models
{
    public class CinemaModel
    {
        public List<ActorModel> Actors { get; set; }
        public List<CustomerModel> Customers { get; set; }
        public List<MovieModel> Movies { get; set; }
        public List<AuditoriumModel> Auditoriums { get; set; }
        public List<BorrowModel> Borrows { get; set; }
        public List<MovieParticipationModel> MovieParticipations { get; set; }
        public List<ProjectionModel> Projections { get; set; }
        public List<ReservationModel> Reservations { get; set; }
    }
}
