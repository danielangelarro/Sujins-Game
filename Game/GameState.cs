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
    public class GameState
    {
        public List<MonsterCard> MonstersP1;
        public List<MonsterCard> MonstersP2;
        
        public List<MagicCard> MagicsP1;
        public List<MagicCard> MagicsP2;

        public InfoCard TableOfInfo;
        public string ChatGame;

        public int Turn;

        public GameState()
        {
            MonstersP1 = new List<MonsterCard>();
            MonstersP2 = new List<MonsterCard>();
            MagicsP1 = new List<MagicCard>();
            MagicsP2 = new List<MagicCard>();
        }

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

        public bool IsLosser(int player)
        {
            if (player == 1)
                return !MonstersP1.Exists(card => !card.IsDead());
            
            return !MonstersP2.Exists(card => !card.IsDead());
        }

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
