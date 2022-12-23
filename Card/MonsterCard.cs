using System;
using System.Collections.Generic;

namespace SujinsCards
{
    public class MonsterCard : ICard
    {
        // Propiedades particulares de las cartas de monstruos.
        public int  MaxHealtPoint { private set; get; }
        public int HealtPoints { private set; get; }
        public int Defense { private set; get; }
        public int Attack { private set; get; }
        public TypeMonsterElement Type { private set; get; }

        public MonsterCard() { }

        public MonsterCard(string name, string description, int price, string image, int atack,
            int defense, TypeMonsterElement type, int hp = 100)
        {
            this.Name = name;
            this.Description = description;
            this.Price = price;
            this.Image = image;
            this.Attack = atack;
            this.Defense = defense;
            this.Type = type;
            this.HealtPoints = hp;
            this.MaxHealtPoint = hp;
        }
        
        public string ImageDir => "./Assest/img/image_card/" + this.Image;

        public bool IsDead()
        {
            return this.HealtPoints <= 0;
        }

        public List<string> GetInfo
        {
            get {
                return new List<string>()
                    {
                        this.Image,
                        this.Name,
                        this.Description,
                        this.HealtPoints.ToString(),
                        this.MaxHealtPoint.ToString(),
                        this.Attack.ToString(),
                        this.Defense.ToString(),
                        this.Type.ToString(),
                        this.Price.ToString()
                    };
            }
        }

        public void UpdateHealtPoints(int value)
        {
            this.HealtPoints += value;

            if (HealtPoints < 0)
                HealtPoints = 0;
        }

        public void UpdateAttack(int value)
        {
            this.Attack += value;
        }

        public void UpdateDeffense(int value)
        {
            this.Defense += value;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj != null && this.GetType().Equals(obj.GetType()))
                
                return this.Name == ((MonsterCard)obj).Name;
            
            return false;
        }


        public MonsterCard Clone()
        {
            MonsterCard clone = new MonsterCard();

            clone.Name = this.Name;
            clone.Description = this.Description;
            clone.Price = this.Price;
            clone.Image = this.Image;
            clone.Attack = this.Attack;
            clone.Defense = this.Defense;
            clone.Type = this.Type;
            clone.HealtPoints = this.HealtPoints;
            clone.MaxHealtPoint = this.MaxHealtPoint;

            return clone;
        }

        public override string ToString()
        {
            string text = "";

            text +=  "+----------------------------+\n";
            text +=  "|         MONSTER CARD       |\n";
            text +=  "+----------------------------+\n";
            text += $" Nombre: {this.Name}\n";
            text += $" HP: {HealtPoints}\n";
            text += $" ATK: {this.Attack}\t";
            text += $" DEF: {this.Defense}\t";
            text += $" Type: {this.Type.ToString()}\n";

            return text;
        }

        public string InfoToString()
        {
            string text = this.ToString();

            text +=  "------------------------------\n";
            text += $"| \n";
            text +=  "------------------------------\n";
            text += $"|          Descripcion:      |\n";
            text +=  "------------------------------\n";
            text += $"{this.Description}\n";
            text +=  "------------------------------\n";

            return text;
        }
    }
}