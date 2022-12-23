using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SujinsLogic
{
    public class Config
    {
        public static string DataBase = "./Assest/data/";
        public static string MediaBase = "./Assest/img/";
        public static int CantCardsPublics { get; set; } = 6;

        public int CantMonsterForPlayer { get; set; } = 3;
        public int CantMagicsForPlayer { get; set; } = 3;
        public int CantMagicsForPlayerAtHand { get; set; } = 3;
        public string Status;

        public bool ReverseAttack { get; set; }

        public Config(string status="Debug")
        {
            this.Status = status;
        }
    }
}
