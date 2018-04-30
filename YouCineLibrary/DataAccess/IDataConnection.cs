using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouCineLibrary.Models;

namespace YouCineLibrary.DataAccess
{
    public interface IDataConnection
    {
        bool TestConnection();

        List<MovieModel> LoadMovies();

        List<AuditoriumModel> LoadAuditoriums();

        List<ActorModel> LoadActors();

        List<CustomerModel> LoadCustomers();

        List<ProjectionModel> LoadProjections();

        List<ReservationModel> LoadReservations();

        List<BorrowModel> LoadBorrows();

        List<MovieParticipationModel> LoadMovieParticipations();

        bool DeleteCustomer(string ID);

        CustomerModel CreateCustomer(string firstname, string lastname, string email);
    }
}
