using System.Collections.Generic;

namespace SujinsCards
{

    public class MagicCard : ICard
    {
        // Propiedades globales de las cartas.
        public string Name { private set; get; }
        public string Description { private set; get; }
        public int Price { private set; get; }
        public string Image { private set; get; }

        // Propiedades privadas de las clases privadas
        public int Position { private set; get; }
        public string Action { private set; get; }

        public MagicCard() { }

        public MagicCard(string name, string description, int price, string image, string action, int position)
        {
            this.Name = name;
            this.Description = description;
            this.Price = price;
            this.Image = image;
            this.Position = position;
            this.Action = action;
        }

        public string ImageDir => "./Assest/img/image_card/" + this.Image;

        
        public MagicCard Clone()
        {
            return new MagicCard(
                this.Name,
                this.Description,
                this.Price,
                this.Image,
                this.Action,    
                this.Position
            );
        }

        public List<string> GetInfo
        {
            get {
                return new List<string>()
                {
                    this.Image,
                    this.Name,
                    this.Description,
                    this.Action
                };
            }
        }

        public override string ToString()
        {
            string text = "";

            text +=  "------------------------------\n";
            text +=  "|          MAGIC CARD        |\n";
            text +=  "------------------------------\n";
            text += $"| Nombre: {this.Name}\n";
            text +=  "------------------------------\n";
            text += $"|          Descripcion:      |\n";
            text +=  "------------------------------\n";
            text += $"{this.Description}\n";
            text +=  "------------------------------\n";

            return text;
        }
    }
}