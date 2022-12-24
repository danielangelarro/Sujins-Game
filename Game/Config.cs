using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SujinsLogic
{
    /// <summary>
    /// En esta clase se almacenan variables que tiene que ver con el estado del
    /// juego y el nombre de los documentos con las cartas.
    /// </summary>
    public class Config
    {
        public static string DataBase = "./Assest/data/";
        public static string MediaBase = "./Assest/img/";
        public static int CantCardsPublics { get; set; } = 6;

        public int CantMonsterForPlayer { get; set; } = 3;
        public int CantMagicsForPlayer { get; set; } = 3;
        public int CantMagicsForPlayerAtHand { get; set; } = 3;
        public string Status;

        // Si el ataque puede revirarse contra el atacante en caso de que el defensor tenga
        // mayor poder de defensivo.
        public bool ReverseAttack { get; set; }

        public Config(string status="Debug")
        {
            this.Status = status;
        }
    }
}
