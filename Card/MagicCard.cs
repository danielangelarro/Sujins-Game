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

        /// <summary>
        /// Crea una nueva carta magica.
        /// </summary>
        /// <param value="name">
        /// Nombre de la carta magica.
        /// </param>
        /// <param value="description">
        /// Breve descripcion de las acciones que realiza la carta magica.
        /// </param>
        /// <param value="price">
        /// Precio en la tienda de la carta magica.
        /// </param>
        /// <param value="image">
        /// Direccion donde se guarda la imagen asociada a la carta magica.
        /// </param>
        /// <param value="action">
        /// Codigo que repersenta la accion que ejecuta una carta magica al setr invocada.
        /// </param>
        /// <param value="position">
        /// Posicion del monstruo sobre el que tiene incidencia, en caso de ser -1
        /// puede incidir sobre todos los monstruos que se encuentren en el campo.
        /// </param>
        public MagicCard(string name, string description, int price, string image, string action, int position)
        {
            this.Name = name;
            this.Description = description;
            this.Price = price;
            this.Image = image;
            this.Position = position;
            this.Action = action;
        }

        /// <summary>
        /// Direccion donde se encuentran guardadas las imagenes de las cartas.
        /// </summary>
        public string ImageDir => "./Assest/img/image_card/" + this.Image;

        /// <summary>
        /// Devuelve una copia del objeto para no hacer cambios al original
        /// </summary>
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

        /// <summary>
        /// Retorna una lista con las propiedades de las cartas 
        /// </summary>
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

        /// <summary>
        /// Devuelve un string con la forma en la que se va a ver en consola
        /// una carta magica
        /// </summary>
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