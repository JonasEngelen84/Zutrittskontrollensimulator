using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OBS_Zutrittskontrolle.Models
{
    public class Terminal
    {
        public int Terminal_ID { get; set; }
        public int Terminal_Type { get; set; }
        public string Terminal_Place { get; set; }
        public CheckBox Checkbox { get; set; }
        public BitmapImage Image { get; set; }

        public Terminal(int terminalID, int termnaltype, string terminalPlace)
        {
            Checkbox = new();
            Terminal_ID = terminalID;
            Terminal_Type = termnaltype;
            Terminal_Place = terminalPlace;
        }
    }
}
