using System;

namespace OBS_Zutrittskontrolle.Models
{
    public class Result
    {
        public int Transponder_ID { get; set; }
        public int Terminal_ID { get; set; }
        public string Terminal_Place { get; set; } = null!;
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public bool Access { get; set; }
        public string AccessIssue { get; set; }
        public int Employee_ID { get; set; }
        public string Surname { get; set; } = null!;
        public string Forename { get; set; } = null!;

        public Result(int transponderID, int TerminalID, string terminalPlace, DateTime date,
            DateTime time, bool access, int employeeID, string forename, string surname)
        {
            Transponder_ID = transponderID;
            this.Terminal_ID = TerminalID;
            Terminal_Place = terminalPlace;
            Time = time;
            Date = date;
            Access = access;
            Employee_ID = employeeID;
            Forename = forename;
            Surname = surname;
            AccessIssue = Access ? "Erlaubt" : "Verweigert";
        }
    }
}
