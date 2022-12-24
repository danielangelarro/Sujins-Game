using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.IO;

using SujinsCards;

namespace SujinsLogic
{
    /// <summary>
    /// Guarda el estado actual del juego.
    /// </summary>
    public class GameState
    {
        // Monstruos de cada player
        public List<MonsterCard> MonstersP1;
        public List<MonsterCard> MonstersP2;
        
        // Cartas magicas de cada player
        public List<MagicCard> MagicsP1;
        public List<MagicCard> MagicsP2;

        // Tablero donde se m uestra la informacion asociadas a una carta.
        public InfoCard TableOfInfo;

        // Chat interno donde se muestran las jugadas realizadas
        public string ChatGame;

        // Turno que indica a que jugador le corresponde jugar.
        public int Turn;

        /// <summary>
        /// Inicializa los valores de las listas.
        /// </summary>
        public GameState()
        {
            MonstersP1 = new List<MonsterCard>();
            MonstersP2 = new List<MonsterCard>();
            MagicsP1 = new List<MagicCard>();
            MagicsP2 = new List<MagicCard>();
        }

        /// <summary>
        /// Devuelve una nueva instancia del estado del juego en caso de que 
        /// se quieran hacer cambios sin afectar la partida actual
        /// </summary>
        public GameState Clone()
        {
            GameState newGame = new GameState();

            newGame.Turn = Turn;
            newGame.ChatGame = ChatGame;

            this.MonstersP1.ForEach(card => newGame.MonstersP1.Add(card.Clone()));
            this.MonstersP2.ForEach(card => newGame.MonstersP2.Add(card.Clone()));
            this.MagicsP1.ForEach(card => newGame.MagicsP1.Add(card.Clone()));
            this.MagicsP2.ForEach(card => newGame.MagicsP2.Add(card.Clone()));
            
            return newGame;
        }

        /// <summary>
        /// Verifica si la cantidad de monstruos vivos de un player es 0
        /// para saber si alguno perdio la partida.
        /// </summary>
        public bool IsLosser(int player)
        {
            if (player == 1)
                return !MonstersP1.Exists(card => !card.IsDead());
            
            return !MonstersP2.Exists(card => !card.IsDead());
        }

        /// <summary>
        /// Calcula que tanto beneficia el estado del juego actual a un determinado player.
        /// </summary>
        /// <remarks>
        /// Hace que todos los monstruos de un player ataquen a todos los monstruos del oponente.
        /// Una vez realizado esto en los 2 bandos se suman los valores de los puntos de vida con lo0s que se quedaron
        /// los monstruos que aparecen sobre el campo.
        /// </remarks>
        /// <return>
        /// La diferencia entre la puntiuacion del player actual y la del enemigo
        /// </return>
        public int GetStatusOfMonsters(int player)
        {
            int monsters1 = 0;
            int monsters2 = 0;

            for (int i = 0; i < MonstersP1.Count; i++)
            {
                for (int j = 0; j < MonstersP2.Count; j++)
                {
                    if (MonstersP1[i].IsActive && MonstersP2[j].IsActive)
                    {
                        monsters1 += MonstersP1[i].HealtPoints - (MonstersP2[j].Attack - MonstersP1[i].Defense);
                        monsters2 += MonstersP2[j].HealtPoints - (MonstersP1[i].Attack - MonstersP2[j].Defense);
                    }
                }
            }

            if (player == 1)
                return monsters1 - monsters2;

            return monsters2 - monsters1;
        }
    }
}
