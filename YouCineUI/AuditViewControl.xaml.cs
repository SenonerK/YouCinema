using System.Windows.Controls;
using YouCineLibrary.Models;

namespace YouCineUI
{
    public partial class AuditViewControl : UserControl
    {
        private AuditoriumModel _auditorium;
        public AuditoriumModel Auditorium
        {
            get
            {
                return _auditorium;
            }
            set
            {
                lbl_name.Text = value.Room;
                lbl_places.Text = "Plätze: " + (value.Columns * value.Rows).ToString();
                MovieModel tmp = YouCineLibrary.Config.GetRunningMovieByAudit(value.ID);
                lbl_mov_name.Text = tmp==null ? "Film: /" : "Film: " + tmp.MovieName;
                _auditorium = value;
            }
        }

        public AuditViewControl()
        {
            InitializeComponent();
        }
    }
}
