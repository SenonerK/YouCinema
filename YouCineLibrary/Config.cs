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
        public static MediaServer MediaConnection { get; private set; }
        public static Models.CinemaModel Cinema { get; set; }

        public static void InitializeMediaConnection(string mediaserver)
        {
            MediaConnection = new MediaServer(mediaserver);
        }

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
            MediaConnection = null;
        }

        public static bool LoadCinema()
        {
            Cinema = new CinemaModel();

            try
            {
                // Alles von der DB laden. Bitte Reihenfolge nicht ändern
                Cinema.Movies = Connection.LoadMovies();
                Cinema.Auditoriums = Connection.LoadAuditoriums();
                Cinema.Actors = Connection.LoadActors();
                Cinema.Customers = Connection.LoadCustomers();
                Cinema.Projections = Connection.LoadProjections();
                Cinema.Reservations = Connection.LoadReservations();
                Cinema.Borrows = Connection.LoadBorrows();
                Cinema.MovieParticipations = Connection.LoadMovieParticipations();

                LoadBorrowLog();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void LoadBorrowLog()
        {
            Cinema.BorrowLog = Connection.LoadBorrowLog();
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

        public static CustomerModel GetCustomerById(string ID)
        {
            foreach (CustomerModel m in Cinema.Customers)
                if (m.ID == ID)
                    return m;

            return null;
        }

        public static ProjectionModel GetProjectionById(string ID)
        {
            foreach (ProjectionModel m in Cinema.Projections)
                if (m.ID == ID)
                    return m;

            return null;
        }

        public static List<MovieParticipationModel> GetParticipationsByMovie(string movieID)
        {
            List<MovieParticipationModel> ret = new List<MovieParticipationModel>();

            foreach(MovieParticipationModel m in Cinema.MovieParticipations)
            {
                if (m.Movie == movieID)
                    ret.Add(m);
            }

            return ret;
        }

        public static List<ProjectionModel> SearchProjectionByDate(DateTime from, DateTime to)
        {
            List<ProjectionModel> ret = new List<ProjectionModel>();

            foreach(ProjectionModel m in Cinema.Projections)
            {
                if (m.Date >= from && m.Date <= to)
                    ret.Add(m);
            }
            return ret;
        }

        public static List<ReservationModel> SearchReservationByDate(DateTime from, DateTime to)
        {
            List<ReservationModel> ret = new List<ReservationModel>();

            foreach (ReservationModel m in Cinema.Reservations)
            {
                ProjectionModel tmp = GetProjectionById(m.Projection);
                if (tmp.Date >= from && tmp.Date <= to)
                    ret.Add(m);
            }
            return ret;
        }

        public static bool ReservationPositionExists(string audit, int col, int row)
        {
            AuditoriumModel tmp = Config.GetAuditById(audit);
            if (col == 0 || row == 0)
                return false;

            return (row <= tmp.Rows && col <= tmp.Columns);
        }

        public static bool ReservationPositionIsTaken(string proj, int row, int col)
        {
            foreach (ReservationModel m in Cinema.Reservations)
            {
                if (m.Projection == proj && m.Row == row && m.Column == col)
                    return true;
            }

            return false;
        }

        public static MovieModel GetRunningMovieByAudit(string auditID)
        {
            foreach (ProjectionModel m in Cinema.Projections)
            {
                if (m.Auditorium == auditID && m.Date < DateTime.Now && DateTime.Now < (AddTime(m.Date, GetMovieById(m.Movie).Duration)))
                    return GetMovieById(m.Movie);
            }

            return null;
        }

        public static DateTime AddTime(DateTime date, DateTime duration)
        {
            return date.AddSeconds((duration.Hour*60*60)+(duration.Minute*60)+duration.Second);
        }

        public static bool isAuditOccupied(string auditID, DateTime from, DateTime to)
        {
            foreach (ProjectionModel m in Cinema.Projections)
            {
                DateTime tmpto = AddTime(m.Date, GetMovieById(m.Movie).Duration);

                if (m.Auditorium==auditID && ((from >= m.Date && from <= tmpto) || (to>=m.Date && to<=tmpto) || (m.Date>=from && m.Date<=to) || (tmpto>=from && tmpto<=to)))
                    return true;
            }

            return false;
        }

        public static List<BorrowLogModel> SearchBorrowLogByDate(DateTime from, DateTime to)
        {
            List<BorrowLogModel> ret = new List<BorrowLogModel>();

            foreach (BorrowLogModel m in Cinema.BorrowLog)
            {
                if (m.Date >= from && m.Date <= to)
                    ret.Add(m);
            }
            return ret;
        }
    }
}
