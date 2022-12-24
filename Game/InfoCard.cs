using System;
using System.Collections.Generic;

using SujinsCards;

namespace SujinsLogic
{
    /// <summary>
    /// Tablero que muestra la informacion de una carta
    /// </summary>
    public class InfoCard
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string HealtPoints { get; private set; }
        public string ManaPoints { get; private set; }
        public string Attack { get; private set; }
        public string Deffense { get; private set; }
        public string TypeCard { get; private set; }

        /// <summary>
        /// Actualiza el tablero con la infoprmacion de una carta
        /// </summary>
        public void UpdateInfo(ICard card, string type)
        {
            ClearInfo();

            if (type == "monster")
                UpdateInfoMonster((MonsterCard)card);

            else if (type == "magic")
                UpdateInfoMagic((MagicCard)card);
        }

        /// <summary>
        /// Vacia la informacion que contiene el tablero
        /// </summary>
        private void ClearInfo()
        {
            this.Name = "-";
            this.Description = "-";
            this.HealtPoints = "-";
            this.ManaPoints = "-";
            this.Attack = "-";
            this.Deffense = "-";
            this.TypeCard = "-";
        }

        /// <summary>
        /// Actualiza el tablero con la infoprmacion de una carta de tipo monstruo
        /// </summary>
        private void UpdateInfoMonster(MonsterCard card)
        {
            List<string> info = card.GetInfo;

            this.Name = info[1];
            this.Description = info[2];
            this.HealtPoints = info[3];
            this.ManaPoints = info[4];
            this.Attack = info[5];
            this.Deffense = info[6];
            this.TypeCard = info[7];
        }

        /// <summary>
        /// Actualiza el tablero con la infoprmacion de una carta magica
        /// </summary>
        private void UpdateInfoMagic(MagicCard card)
        {
            List<string> info = card.GetInfo;

            this.Name = info[1];
            this.Description = info[2];
        }
    }
}