using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using YouCineLibrary.Models;
using System.Text;

namespace YouCineLibrary.DataAccess
{
    public class PSqlConnector : IDataConnection
    {
        public string ConnectionString { get; set; }

        public PSqlConnector(string CnnString)
        {
            ConnectionString = CnnString;
        }

        private bool hasConnectionString()
        {
            if (!string.IsNullOrEmpty(ConnectionString) && !string.IsNullOrWhiteSpace(ConnectionString))
                return true;

            return false;
        }

        public bool TestConnection()
        {
            try
            {
                if (!hasConnectionString())
                    return false;

                using (NpgsqlConnection cnn = new NpgsqlConnection(ConnectionString))
                {
                    cnn.Open();
                    cnn.Close();
                    return true;
                }
            }
            catch (Exception ex) { return false; }
        }

        public bool Execute(NpgsqlCommand cmd)
        {
            try
            {
                if (!hasConnectionString())
                    return false;

                using (NpgsqlConnection cnn = new NpgsqlConnection(ConnectionString))
                {
                    cnn.Open();
                    cmd.Connection = cnn;
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { return false; }
        }

        public bool Execute(string cmd)
        {
            return Execute(new NpgsqlCommand(cmd));
        }

        public DataTable Query(NpgsqlCommand cmd)
        {
            try
            {
                if (!hasConnectionString())
                    return null;

                using (NpgsqlConnection cnn = new NpgsqlConnection(ConnectionString))
                {
                    cnn.Open();
                    cmd.Connection = cnn;
                    DataTable data = new DataTable();
                    new NpgsqlDataAdapter(cmd).Fill(data);
                    return data;
                }
            }
            catch (Exception ex) { return null; }
        }

        public DataTable Query(string cmd)
        {
            return Query(new NpgsqlCommand(cmd));
        }

        public List<MovieModel> LoadMovies()
        {
            List<MovieModel> ret = new List<MovieModel>();

            DataTable tmp = Query("SELECT id,thumbnail,description,m_name,publishing_year,price_per_day_borrow FROM yc_movie");

            if (tmp == null)
                throw new Exception("Die Datenbank hat NULL zurückgegeben bei yc_movie!");

            foreach (DataRow r in tmp.Rows)
            {
                ret.Add(new MovieModel()
                {
                    ID = r[0].ToString(),
                    Image = r[1].ToString(),
                    MovieDescription = r[2].ToString(),
                    MovieName = r[3].ToString(),
                    Published = DateTime.Parse(r[4].ToString()),
                    Price = double.Parse(r[5].ToString())
                });
            }

            return ret;
        }

        public List<AuditoriumModel> LoadAuditoriums()
        {
            List<AuditoriumModel> ret = new List<AuditoriumModel>();

            DataTable tmp = Query("SELECT roomid,room_name,max_col,max_row FROM yc_rooms");

            if (tmp == null)
                throw new Exception("Datenbank gibt null zurück bei yc_rooms");

            foreach (DataRow r in tmp.Rows)
            {
                ret.Add(new AuditoriumModel()
                {
                    ID = r[0].ToString(),
                    Room = r[1].ToString(),
                    Columns = int.Parse(r[2].ToString()),
                    Rows = int.Parse(r[3].ToString())
                });
            }

            return ret;
        }

        public List<ActorModel> LoadActors()
        {
            List<ActorModel> ret = new List<ActorModel>();

            DataTable tmp = Query("SELECT id,firstname,lastname,date_of_birth,imdb_rating FROM yc_cast");

            if (tmp == null)
                throw new Exception("Datenbank hat NULL zurückgegeben bei yc_cast");

            foreach (DataRow r in tmp.Rows)
            {
                ret.Add(new ActorModel()
                {
                    ID = r[0].ToString(),
                    FirstName = r[1].ToString(),
                    LastName = r[2].ToString(),
                    BirthDate = DateTime.Parse(r[3].ToString()),
                    Rating = double.Parse(r[4].ToString())
                });
            }

            return ret;
        }

        public List<CustomerModel> LoadCustomers()
        {
            List<CustomerModel> ret = new List<CustomerModel>();

            DataTable tmp = Query("SELECT id,firstname,lastname,email,credit FROM yc_customer WHERE isdisabled=false");

            if (tmp == null)
                throw new Exception("Datenbank hat NULL zurückgegeben bei yc_customer");

            foreach (DataRow r in tmp.Rows)
            {
                ret.Add(new CustomerModel()
                {
                    ID = r[0].ToString(),
                    FirstName = r[1].ToString(),
                    LastName = r[2].ToString(),
                    Email = r[3].ToString(),
                    Credit = double.Parse(r[4].ToString())
                });
            }

            return ret;
        }

        public List<ProjectionModel> LoadProjections()
        {
            List<ProjectionModel> ret = new List<ProjectionModel>();

            DataTable tmp = Query("SELECT demonstrationid,ticket_prices,demonstration_date,fk_movieid,fk_roomid FROM yc_demonstration");

            if (tmp == null)
                throw new Exception("Datenbank hat NULL zurückgegeben bei yc_demonstration");

            foreach (DataRow r in tmp.Rows)
            {
                ret.Add(new ProjectionModel()
                {
                    ID = r[0].ToString(),
                    Price = double.Parse(r[1].ToString()),
                    Date = DateTime.Parse(r[2].ToString()),
                    Movie = r[3].ToString(),
                    Auditorium = r[4].ToString()
                });
            }

            return ret;

        }

        public List<ReservationModel> LoadReservations()
        {
            List<ReservationModel> ret = new List<ReservationModel>();

            DataTable tmp = Query("SELECT ticketid,pos,fk_customerid,fk_demonstrationid FROM yc_reserved");

            if (tmp == null)
                throw new Exception("Datenbank hat NULL zurückgegeben bei yc_reserved");

            foreach (DataRow r in tmp.Rows)
            {
                ret.Add(new ReservationModel()
                {
                    ID = r[0].ToString(),
                    Column = ((int[])r[1])[0],
                    Row = ((int[])r[1])[1],
                    Customer = r[2].ToString(),
                    Projection = r[3].ToString()
                });
            }

            return ret;

        }

        public List<BorrowModel> LoadBorrows()
        {
            List<BorrowModel> ret = new List<BorrowModel>();

            DataTable tmp = Query("SELECT BID,CAST(start_time AS DATE),CAST(end_time AS DATE),fk_customerid,fk_movieid, lastname, firstname, m_name FROM yc_borrow JOIN yc_customer ON yc_borrow.fk_customerid = yc_customer.id JOIN yc_movie ON yc_borrow.fk_movieid = yc_movie.id");

            if (tmp == null)
                throw new Exception("Datenbank hat NULL zurückgegeben bei yc_borrow");

            foreach (DataRow r in tmp.Rows)
            {
                ret.Add(new BorrowModel()
                {
                    ID = r[0].ToString(),
                    LendDate = DateTime.Parse(r[1].ToString()),
                    BringBackDate = DateTime.Parse(r[2].ToString()),
                    Cutomer = r[3].ToString(),
                    Movie = r[4].ToString(),
                    CLN = r[5].ToString(),
                    CFN = r[6].ToString(),
                    MN = r[7].ToString()
                });
            }

            return ret;
        }

         
         /// TODO borrow_log search fertig machen
        /*
        public List<BorrowModel> searchBorrows(DateTime DTFS, DateTime DTLS)
        {
            List<BorrowModel> res = new List<BorrowModel>();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT CAST(logdate AS DATE),fk_movieid, fk_customerid, lastname, firstname, m_name FROM yc_borrow_log JOIN yc_customer ON yc_borrow_log.fk_customerid = yc_customer.id JOIN yc_movie ON yc_borrow_log.fk_movieid = yc_movie.id WHERE yc_borrow_log.logdate BETWEEN @dtfs and @dtls");
            cmd.Parameters.Add("dtfs", DTFS);
            cmd.Parameters.Add("dtls", DTLS);

            DataTable tmp = Query(cmd);

            if (tmp == null)
            {
                throw new Exception("Datenbank hat NULL zurückgegeben bei yc_borrow_log");
            }

            foreach (DataRow r in tmp.Rows)
            {
                ret.Add(new BorrowModel()
                {
                    ID = r[0].ToString
                });
            }
        }
        */
        public List<MovieParticipationModel> LoadMovieParticipations()
        {
            List<MovieParticipationModel> ret = new List<MovieParticipationModel>();

            DataTable tmp = Query("SELECT fk_actor,fk_movie,movie_role FROM yc_participations");

            if (tmp == null)
                throw new Exception("Datenbank hat NULL zurückgegeben bei yc_borrow");

            foreach (DataRow r in tmp.Rows)
            {
                ret.Add(new MovieParticipationModel()
                {
                    Actor = r[0].ToString(),
                    Movie = r[1].ToString(),
                    Role = r[2].ToString()
                });
            }

            return ret;
        }

        public bool DeleteCustomer(string ID)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("UPDATE yc_customer SET isdisabled=true WHERE id=@CID");
            cmd.Parameters.Add(new NpgsqlParameter("CID", ID));
            return Execute(cmd);
        }

        public bool RemoveBorrowedMovie(string ID)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM yc_borrow WHERE bid = @ID");
            cmd.Parameters.Add(new NpgsqlParameter("ID", ID));
            return Execute(cmd);
        }

        public CustomerModel CreateCustomer(string firstname, string lastname, string email)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO yc_customer (firstname,lastname,email,credit,isdisabled) VALUES (@FName, @LName, @Email, 0, false) RETURNING id");
            cmd.Parameters.Add(new NpgsqlParameter("FName", firstname));
            cmd.Parameters.Add(new NpgsqlParameter("LName", lastname));
            cmd.Parameters.Add(new NpgsqlParameter("Email", email));

            return new CustomerModel()
            {
                ID = Query(cmd).Rows[0][0].ToString(),
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                Credit = 0,
            };
        }

        public ActorModel CreateActor(string firstname, string lastname, DateTime birthday, double rating)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO yc_cast (firstname,lastname,date_of_birth,imdb_rating) VALUES (@FName, @LName, @Birth, @Rating) RETURNING id");
            cmd.Parameters.Add(new NpgsqlParameter("FName", firstname));
            cmd.Parameters.Add(new NpgsqlParameter("LName", lastname));
            cmd.Parameters.Add(new NpgsqlParameter("Birth", birthday));
            cmd.Parameters.Add(new NpgsqlParameter("Rating", rating));

            return new ActorModel()
            {
                ID = Query(cmd).Rows[0][0].ToString(),
                FirstName = firstname,
                LastName = lastname,
                BirthDate = birthday,
                Rating = rating
            };
        }

        public MovieModel CreateMovie(string name, string description, DateTime year, double price, System.Drawing.Image photo)
        {
            string pid = Config.MediaConnection.UploadImage(photo);

            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO yc_movie (m_name,description,publishing_year,price_per_day_borrow, thumbnail) VALUES (@Name, @Desc, @Year, @Price, @Photo) RETURNING id");
            cmd.Parameters.Add(new NpgsqlParameter("Name", name));
            cmd.Parameters.Add(new NpgsqlParameter("Desc", description));
            cmd.Parameters.Add(new NpgsqlParameter("Year", year));
            cmd.Parameters.Add(new NpgsqlParameter("Price", price));
            cmd.Parameters.Add(new NpgsqlParameter("Photo", pid));

            DataTable tmp = Query(cmd);
            if(tmp==null)
                throw new Exception("Die Datenbank hat NULL zurückgegeben bei yc_participations!");

            return new MovieModel()
            {
                ID = tmp.Rows[0][0].ToString(),
                MovieName = name,
                MovieDescription = description,
                Published = year,
                Price = price,
                Image = pid
            };
        }

        public MovieParticipationModel CreateMovieParticipation(string movieID, string ActorID, string Role)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO yc_participations (fk_actor,fk_movie,movie_role) VALUES (@Actor, @Movie, @Role)");
            cmd.Parameters.Add(new NpgsqlParameter("Actor", ActorID));
            cmd.Parameters.Add(new NpgsqlParameter("Movie", movieID));
            cmd.Parameters.Add(new NpgsqlParameter("Role", Role));

            if (!Execute(cmd))
                throw new Exception("Die Daten konnten nicht in die Datenbank geschrieben werden");

            return new MovieParticipationModel()
            {
                Actor = ActorID,
                Movie = movieID,
                Role = Role
            };
        }

        public AuditoriumModel CreateAuditorium(string name, int cols, int rows)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO yc_rooms (room_name,max_col,max_row) VALUES (@Name, @Col, @Row) RETURNING roomid");
            cmd.Parameters.Add(new NpgsqlParameter("Name", name));
            cmd.Parameters.Add(new NpgsqlParameter("Col", cols));
            cmd.Parameters.Add(new NpgsqlParameter("Row", rows));

            DataTable tmp = Query(cmd);
            if (tmp == null)
                throw new Exception("Die Datenbank hat NULL zurückgegeben bei yc_rooms!");

            return new AuditoriumModel()
            {
                ID = tmp.Rows[0][0].ToString(),
                Room = name,
                Columns = cols,
                Rows = rows
            };
        }

        public BorrowModel AddBorrowedMovie(DateTime date, string name, string movie)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO yc_borrow (end_time, fk_customerid, fk_movieid) VALUES (@Date, @Name, @Movie) RETURNING bid");
            cmd.Parameters.Add(new NpgsqlParameter("Date", date));
            cmd.Parameters.Add(new NpgsqlParameter("Name", name));
            cmd.Parameters.Add(new NpgsqlParameter("Movie", movie));

            DataTable tmp = Query(cmd);
            if(tmp == null)
                throw new Exception("Die Datenbank hat NULL zurückgegeben bei yc_borrow");

            return new BorrowModel()
            {
                ID = tmp.Rows[0][0].ToString(),
                BringBackDate = date,
                Cutomer = name,
                Movie = movie
            };
        }
    }
}
/// TODO - patchen lol min 30 min warten
/// TODO - Version
/// TODO - Bildnr.