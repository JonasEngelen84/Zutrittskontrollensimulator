using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OBS_Zutrittskontrolle.Models
{
    public class Transponder
    {
        public int Transponder_ID { get; set; }
        public string Surname { get; set; } = null!;
        public string Forename { get; set; } = null!;
        public int Employee_ID { get; set; }
        public CheckBox Checkbox { get; set; }
        public BitmapImage Image { get; set; }

        public Transponder(int transponder, string forename, string surname, int employeeID)
        {
            Checkbox = new();
            Transponder_ID = transponder;
            Forename = forename;
            Surname = surname;
            Employee_ID = employeeID;
        }
    }
}
