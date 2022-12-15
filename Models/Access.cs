using System;

namespace OBS_Zutrittskontrollen_Simulation.Models
{
    public class Access
    {
        public int Transponder_ID { get; set; }
        public int Terminal_ID { get; set; }
        public DateTime AccessFrom { get; set; }
        public DateTime AccessUntil { get; set; }

        public Access(int transponderID, int terminalID, DateTime accessFrom, DateTime acessUntil)
        {
            Transponder_ID = transponderID;
            Terminal_ID = terminalID;
            AccessFrom = accessFrom;
            AccessUntil = acessUntil;
        }
    }
}
