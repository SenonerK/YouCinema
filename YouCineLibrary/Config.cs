using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouCineLibrary.DataAccess;
using YouCineLibrary.Models;

namespace YouCineLibrary
{
    public static class Config
    {
        public static IDataConnection Connection { get; private set; }
        public static Models.CinemaModel Cinema { get; set; }

        public static void InitializeConnection(ConnectionType type, string CnnString)
        {
            switch (type)
            {
                case ConnectionType.PostgreSQL:
                    Connection = new PSqlConnector(CnnString);
                    break;
                case ConnectionType.MySQL:
                    // ...
                    break;
            }
        }

        public static void RemoveConnection()
        {
            Connection = null;
        }

        public static void LoadCinema()
        {
            Cinema = new CinemaModel();

            // Alles von der DB laden. Bitte Reihenfolge nicht ändern
            Cinema.Movies = Connection.LoadMovies();
            Cinema.Auditoriums = Connection.LoadAuditoriums();
            Cinema.Actors = Connection.LoadActors();
            Cinema.Customers = Connection.LoadCustomers();
            Cinema.Projections = Connection.LoadProjections();
            Cinema.Reservations = Connection.LoadReservations();
            Cinema.Borrows = Connection.LoadBorrows();
            Cinema.MovieParticipations = Connection.LoadMovieParticipations();
        }

        public static AuditoriumModel GetAuditById(string v)
        {
            foreach (AuditoriumModel a in Cinema.Auditoriums)
                if (a.ID == v)
                    return a;

            return null;
        }

        public static MovieModel GetMovieById(string v)
        {
            foreach (MovieModel m in Cinema.Movies)
                if (m.ID == v)
                    return m;

            return null;
        }

        public static ActorModel GetActorById(string ID)
        {
            foreach (ActorModel m in Cinema.Actors)
                if (m.ID == ID)
                    return m;

            return null;
        }
    }
}
