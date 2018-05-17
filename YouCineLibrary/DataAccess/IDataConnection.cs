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

        bool RemoveBorrowedMovie(string ID);

        CustomerModel CreateCustomer(string firstname, string lastname, string email);

        ActorModel CreateActor(string firstname, string lastname, DateTime birthday, double rating);

        MovieModel CreateMovie(string name, string description, DateTime year, double price, System.Drawing.Image photo);

        MovieParticipationModel CreateMovieParticipation(string movieID, string ActorID, string Role);

        AuditoriumModel CreateAuditorium(string name, int cols, int rows);

        BorrowModel AddBorrowedMovie(DateTime endtime, string name, string movie);
    }
}
