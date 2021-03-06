﻿using System;
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
            catch { return false; }
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
            catch { return false; }
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
            catch { return null; }
        }

        public DataTable Query(string cmd)
        {
            return Query(new NpgsqlCommand(cmd));
        }

        public List<MovieModel> LoadMovies()
        {
            List<MovieModel> ret = new List<MovieModel>();

            DataTable tmp = Query("SELECT id,thumbnail,description,m_name,publishing_year,price_per_day_borrow,duration FROM yc_movie");

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
                    Price = double.Parse(r[5].ToString()),
                    Duration = DateTime.Parse(r[6].ToString())
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

            DataTable tmp = Query("SELECT BID,start_time,end_time,fk_customerid,fk_movieid FROM yc_borrow");

            if (tmp == null)
                throw new Exception("Datenbank hat NULL zurückgegeben bei yc_borrow");

            foreach (DataRow r in tmp.Rows)
            {
                ret.Add(new BorrowModel()
                {
                    ID = r[0].ToString(),
                    LendDate = DateTime.Parse(r[1].ToString()),
                    BringBackDate = DateTime.Parse(r[2].ToString()),
                    Customer = r[3].ToString(),
                    Movie = r[4].ToString()
                });
            }

            return ret;
        }

        public List<BorrowLogModel> LoadBorrowLog()
        {
            List<BorrowLogModel> ret = new List<BorrowLogModel>();

            DataTable tmp = Query("SELECT logdate,fk_customerid,fk_movieid FROM yc_borrow_log");

            if (tmp == null)
                throw new Exception("Datenbank hat NULL zurückgegeben bei yc_borrow_log");

            foreach (DataRow r in tmp.Rows)
            {
                ret.Add(new BorrowLogModel()
                {
                    Date = DateTime.Parse(r[0].ToString()),
                    Customer = r[1].ToString(),
                    Movie = r[2].ToString()
                });
            }

            return ret;
        }

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

        public MovieModel CreateMovie(string name, string description, DateTime year, double price, System.Drawing.Image photo, DateTime duration)
        {
            string pid = Config.MediaConnection.UploadImage(photo);
            if (pid == null)
                throw new Exception("Fehler beim Hochladen des Bildes");

            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO yc_movie (m_name,description,publishing_year,price_per_day_borrow, thumbnail,duration) VALUES (@Name, @Desc, @Year, @Price, @Photo,@Lenght) RETURNING id");
            cmd.Parameters.Add(new NpgsqlParameter("Name", name));
            cmd.Parameters.Add(new NpgsqlParameter("Desc", description));
            cmd.Parameters.Add(new NpgsqlParameter("Year", year));
            cmd.Parameters.Add(new NpgsqlParameter("Price", price));
            cmd.Parameters.Add(new NpgsqlParameter("Photo", pid));
            cmd.Parameters.Add(new NpgsqlParameter("Lenght", duration));

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
                Image = pid,
                Duration = duration
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

        public ProjectionModel CreateProjection(DateTime date, double price, string movieID, string auditID)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO yc_demonstration (ticket_prices, demonstration_date, fk_movieid, fk_roomid) VALUES (@Price, @Date, @Movie, @Room) RETURNING demonstrationid");
            cmd.Parameters.Add(new NpgsqlParameter("Price", price));
            cmd.Parameters.Add(new NpgsqlParameter("Date", date));
            cmd.Parameters.Add(new NpgsqlParameter("Movie", movieID));
            cmd.Parameters.Add(new NpgsqlParameter("Room", auditID));

            DataTable tmp = Query(cmd);
            if (tmp == null)
                throw new Exception("Die Datenbank hat NULL zurückgegeben bei yc_demonstration!");

            return new ProjectionModel()
            {
                ID = tmp.Rows[0][0].ToString(),
                 Date = date,
                 Price = price,
                 Movie = movieID,
                 Auditorium = auditID
            };
        }

        public bool DeleteProjection(string ID)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM yc_demonstration WHERE demonstrationid=@ID");
            cmd.Parameters.Add(new NpgsqlParameter("ID", ID));
            return Execute(cmd);
        }

        public ReservationModel CreateReservation(string customerID, string projectionID, int col, int row)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO yc_reserved (fk_customerid, fk_demonstrationid, pos) VALUES (@Customer, @Projection, @Position) RETURNING ticketid");
            cmd.Parameters.Add(new NpgsqlParameter("Customer", customerID));
            cmd.Parameters.Add(new NpgsqlParameter("Projection", projectionID));
            cmd.Parameters.Add(new NpgsqlParameter("Position", new int[] { col, row }));

            DataTable tmp = Query(cmd);
            if (tmp == null)
                throw new Exception("Die Datenbank hat NULL zurückgegeben bei yc_reserved!");

            return new ReservationModel()
            {
                ID = tmp.Rows[0][0].ToString(),
                Customer = customerID,
                Projection = projectionID,
                Column = col,
                Row = row
            };
        }

        public bool DeleteReservation(string ID)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM yc_reserved WHERE ticketid=@ID");
            cmd.Parameters.Add(new NpgsqlParameter("ID", ID));
            return Execute(cmd);
        }

        public bool ClearProjections()
        {
            // Lösche alle Vorführungen und dessen reservierungen die schon abgespielt wurden
            NpgsqlCommand cmd = new NpgsqlCommand(
                "DELETE FROM yc_reserved" +
                "WHERE fk_demonstrationid" +
                "IN" +
                "(" +
                "    SELECT demonstrationid" +
                "    FROM yc_demonstration d, yc_movie m" +
                "    WHERE m.id = d.fk_movieid" +
                "    AND d.demonstration_date < now() - m.duration" +
                ");" +

                "DELETE FROM yc_demonstration d" +
                "USING yc_movie m" +
                "WHERE m.id = d.fk_movieid" +
                "AND d.demonstration_date < now() - m.duration;");
            return Execute(cmd);
        }

        public bool RemoveBorrowedMovie(string ID)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM yc_borrow WHERE id=@ID");
            cmd.Parameters.Add(new NpgsqlParameter("ID", ID));
            return Execute(cmd);
        }

        public BorrowModel AddBorrowedMovie(DateTime endtime, string name, string movie)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO yc_borrow (start_time, end_time, fk_customerid, fk_movieid) VALUES (now(), @End, @Customer, @Movie) RETURNING BID");
            cmd.Parameters.Add(new NpgsqlParameter("End", endtime));
            cmd.Parameters.Add(new NpgsqlParameter("Customer", name));
            cmd.Parameters.Add(new NpgsqlParameter("Movie", movie));

            DataTable tmp = Query(cmd);
            if (tmp == null)
                throw new Exception("Die Datenbank hat NULL zurückgegeben bei yc_borrow!");

            return new BorrowModel()
            {
                ID = tmp.Rows[0][0].ToString(),
                LendDate = DateTime.Now,
                BringBackDate = endtime,
                Customer = name,
                Movie = movie
            };
        }

        public bool UpdateCustomer(string ID, string email, string name, string lname, double credit)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("UPDATE yc_customer SET email=@Mail, firstname=@Name, lastname=@LName, credit=@Credit WHERE id=@ID");
            cmd.Parameters.Add(new NpgsqlParameter("ID", ID));
            cmd.Parameters.Add(new NpgsqlParameter("Mail", email));
            cmd.Parameters.Add(new NpgsqlParameter("Name", name));
            cmd.Parameters.Add(new NpgsqlParameter("LName", lname));
            cmd.Parameters.Add(new NpgsqlParameter("Credit", credit));
            return Execute(cmd);
        }
    }
}