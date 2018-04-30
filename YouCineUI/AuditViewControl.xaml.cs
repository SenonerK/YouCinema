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
                lbl_places.Text = (value.Columns * value.Rows).ToString();
                /// TODO - anzeigen welcher movie gard läuft
                _auditorium = value;
            }
        }

        public AuditViewControl()
        {
            InitializeComponent();
        }
    }
}
