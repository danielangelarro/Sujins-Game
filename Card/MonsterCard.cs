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

        /// <summary>
        /// Crea una nueva carta monstruo.
        /// </summary>
        /// <param value="name">
        /// Nombre de la carta.
        /// </param>
        /// <param value="description">
        /// Breve descripcion de las acciones que realiza la carta.
        /// </param>
        /// <param value="price">
        /// Precio en la tienda de la carta.
        /// </param>
        /// <param value="image">
        /// Direccion donde se guarda la imagen asociada a la carta.
        /// </param>
        /// <param value="atack">
        /// Poder de ataque del monstruo.
        /// </param>
        /// <param value="defense">
        /// Poder defensivo del monstruo.
        /// </param>
        /// <param value="type">
        /// Tipo de elemento que representa el monstruo, puede ser de agua, aire, etc.
        /// </param>
        /// <param value="hp">
        /// Cantidad de puntos de vida que posee el monstruo. Inicialmente
        /// se inicializan en 100 a menos que decidan ser cambiados.
        /// </param>
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
        
        /// <summary>
        /// Direccion donde se encuentra almacenada la imagen asociada
        /// a la carta de monstruo actual.
        /// </summary>
        public string ImageDir => "./Assest/img/image_card/" + this.Image;

        /// <summary>
        /// Verifica si la vida del monstruo es menor q 0,
        /// para saber si debe ser eliminado de la partida 
        /// </summary>
        public bool IsDead()
        {
            return this.HealtPoints <= 0;
        }

        /// <summary>
        /// Retorna una lista con las propiedades del monstruo
        /// </summary>
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

        /// <summary>
        /// Aumenta o disminuye la vida del montruo.
        /// </summary>
        /// <param value="value">
        /// Valor por el cual se va a modificar el parametro.
        /// </param>
        public void UpdateHealtPoints(int value)
        {
            this.HealtPoints += value;

            if (HealtPoints < 0)
                HealtPoints = 0;
        }

        /// <summary>
        /// Aumenta o disminuye el ataque del montruo
        /// </summary>
        /// <param value="value">
        /// Valor por el cual se va a modificar el parametro.
        /// </param>
        public void UpdateAttack(int value)
        {
            this.Attack += value;
        }

        /// <summary>
        /// Aumenta o disminuye la defensa del montruo
        /// </summary>
        /// <param value="value">
        /// Valor por el cual se va a modificar el parametro.
        /// </param>
        public void UpdateDeffense(int value)
        {
            this.Defense += value;
        }

        /// <summary>
        /// Modifica el valor propio caracteristico de la carta actual
        /// basandose en el hashing representado por su nombre.
        /// </summary>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        /// <summary>
        /// Verifica si dos monstruos son iguales
        /// </summary>
        /// <param value="obj">
        /// Objeto por el cual se va a realizar la comparacion.
        /// </param>
        public override bool Equals(object obj)
        {
            if (obj != null && this.GetType().Equals(obj.GetType()))
                
                return this.Name == ((MonsterCard)obj).Name;
            
            return false;
        }

        /// <summary>
        /// Devuele un nuevo monstruo con las mismas propiedades que este
        /// </summary>
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

        /// <summary>
        /// Muestra las propiedades de una carta y la forma en la
        /// que se va a ver en consola la misma
        /// </summary>
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

        /// <summary>
        /// Muestra como se va a ver en la consola la descripcion de la carta
        /// </summary>
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