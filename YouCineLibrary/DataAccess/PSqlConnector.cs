﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using YouCineLibrary.Models;

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

            DataTable tmp = Query("SELECT id,thumbnail,description,m_name,publishing_year,price_per_day_borrow FROM yc_movie");

            if (tmp == null)
                throw new Exception("Die Datenbank hat NULL zurückgegeben bei yc_movie!");

            foreach (DataRow r in tmp.Rows)
            {
                ret.Add(new MovieModel()
                {
                    ID = r[0].ToString(),
                    Image = (byte[])r[1],
                    MovieDescription = r[2].ToString(),
                    MovieName = r[3].ToString(),
                    Published = DateTime.Parse(r[4].ToString()),
                    Price = decimal.Parse(r[5].ToString())
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

            DataTable tmp = Query("SELECT id,firstname,lastname,email,credit FROM yc_customer");

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

            DataTable tmp = Query("SELECT id,start_time,end_time,fk_customerid,fk_movieid FROM yc_borrow");

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
                    Movie = r[4].ToString()
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
    }
}
