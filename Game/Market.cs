using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SujinsCards;
using SujinsInterpreter;

namespace SujinsLogic
{
    /// <summary>
    /// Clase que representa la tienda virtual del juego
    /// </summary>
    public class Market
    {
        public string Error { get; private set; }
        private int BuyCardID, SellCardID;
        private int CantOfCards = Boveda.MarketMonsterDeck.Count;
        private int CantCardsPublics = Boveda.PublicMonsterDeck.Count;

        /// <summary>
        /// Si ingresa a la tienda con la opcion de vender retorna las
        /// cartas a las que puede acceder el player, que son las que se pueden
        /// vender y si entra con la opcion de comprar entonces devuleve las cartas
        /// de la tienda
        /// </summary>
        /// <param value="operation">
        /// Tipo de operacion a realizar (comprar o vender).
        /// </param>
        public string ShowMonster(string operation)
        {
            if (operation == "buy")
                return Boveda.MarketMonsterDeck[BuyCardID].ToString();
            
            return Boveda.PublicMonsterDeck[SellCardID].ToString();
        }

        /// <summary>
        /// Retorna el precio de compra o venta asociado a la carta seleccionada.
        /// </summary>
        /// <param value="operation">
        /// Tipo de operacion a realizar (comprar o vender).
        /// </param>
        public string GetPrice(string operation)
        {
            if (operation == "buy")
                return Boveda.MarketMonsterDeck[BuyCardID].Price.ToString();
            
            int price = Boveda.PublicMonsterDeck[SellCardID].Price;
            int sell = (int)(price * 0.7);

            return sell.ToString();
        }

        /// <summary>
        /// Retorna la direccion donde se encuentra almacenada la imagen asociada ala carta actual
        /// </summary>
        /// <param value="operation">
        /// Tipo de operacion a realizar (comprar o vender).
        /// </param>
        public string ShowImageMonster(string operation)
        {
            string dir = Config.MediaBase + "image_card/";

            if (operation == "buy")
                return dir + Boveda.MarketMonsterDeck[BuyCardID].Image;
            
            return dir + Boveda.PublicMonsterDeck[SellCardID].Image;
        }

        /// <summary>
        /// Pasa a la siguiente carta. Si llega a la ultima vuelve a empezar desde
        /// la primera
        /// </summary>
        /// <param value="operation">
        /// Tipo de operacion a realizar (comprar o vender).
        /// </param>
        public void NextCard(string operation)
        {
            if (operation == "buy")
                BuyCardID = (BuyCardID + 1) % CantOfCards;

            else 
                SellCardID = (SellCardID + 1) % CantCardsPublics;
        }

        /// <summary>
        /// Pasa a la carta anterior. Si llega a la primera vuelve a empezar
        /// desde la ultima
        /// </summary>
        /// <param value="operation">
        /// Tipo de operacion a realizar (comprar o vender).
        /// </param>
        public void PrevCard(string operation)
        {
            if (operation == "buy")
                BuyCardID = (BuyCardID + CantOfCards - 1) % CantOfCards;
            
            else 
                SellCardID = (SellCardID + CantOfCards - 1) % CantOfCards;
        }

        /// <summary>
        /// Ejecuta la accion de comprar una carta. Primero revisa que el dinero
        /// actual es mayor que el precio de la carta, en caso de ser asi entonces
        /// descuenta el precio de esta del dinero y cambia esta carta para las que
        /// puede acceder el player 
        /// </summary>
        public void BuyCard()
        {
            if (Boveda.Coins >= Boveda.MarketMonsterDeck[BuyCardID].Price)
            {
                Boveda.SetCoins(-Boveda.MarketMonsterDeck[BuyCardID].Price);
                Boveda.PublicMonsterDeck.Add(Boveda.MarketMonsterDeck[BuyCardID]);
                Boveda.MarketMonsterDeck.RemoveAt(BuyCardID);

                NextCard("buy");
            }
        }
        
        /// <summary>
        /// Permite al player vender una carta, aplicando un descuento de 70%
        /// al precio de la carta
        /// </summary>
        public void SellCard()
        {
            int price = (int)(Boveda.PublicMonsterDeck[SellCardID].Price * 0.7);
            Boveda.SetCoins(price);
            Boveda.MarketMonsterDeck.Add(Boveda.PublicMonsterDeck[SellCardID]);
            Boveda.PublicMonsterDeck.RemoveAt(SellCardID);

            NextCard("sell");
        }

        /// <summary>
        /// Permite crear una carta monstruo. Primero verifica que los parametros
        /// pasados son correctos(que el ataque se amyor que 0 etc...) y despues que
        /// la carta no exista ya en el juego. Si nada de esto ocurre se crea la carta
        /// y se adiciona a las cartas de la tienda
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
        /// <param value="attack">
        /// Poder de ataque del monstruo.
        /// </param>
        /// <param value="defense">
        /// Poder defensivo del monstruo.
        /// </param>
        /// <param value="hp">
        /// Cantidad de puntos de vida que posee el monstruo. Inicialmente
        /// se inicializan en 100 a menos que decidan ser cambiados.
        /// </param>
        public bool CreateMonsterCard(string name, string description, int price, string image, int attack,
            int defense, int hp)
        {
            if (name.Trim() == "" || description.Trim() == "" || image.Trim() == "")
                return false;
            
            if (price <= 0 || attack <= 0 || defense <= 0 || hp <= 0)
                return false;
            
            if (Boveda.PublicMonsterDeck.Exists(m => m.Name == name) || Boveda.MarketMonsterDeck.Exists(m => m.Name == name))
                return false;
            
            MonsterCard card = new MonsterCard(
                name, description, price, image, attack, defense, TypeMonsterElement.Fire, hp
            );

            Boveda.MarketMonsterDeck.Add(card);

            return true;
        }

        /// <summary>
        /// Permite crear una carta magica. Primero se le pasa el codigo del
        /// paramtro al interprete el cual se encarga de verificar si su sintaxis
        /// es la correcta, si es asi se crea la carta y se adiciona a las demas y el
        /// metodo devuelve true, sino no se crea y devuelve false
        /// </summary>
        /// <param value="code">
        /// Codigo de accion asociado a la carta magica.
        /// </param>
        public bool CreateMagicCard(string code)
        {
            if (code.Trim() == "")
                return false;
            
            Lexer lexer = new Lexer(code, "run");
            Parser parser = new Parser(lexer);

            try
            {
                parser.Parse();
            }
            catch (System.Exception e)
            {
                Error = e.Message;
                return false;
            }

            Interpreter interprete = new Interpreter(parser);

            foreach (var card in interprete.GetCards())
            {
                Boveda.PublicMagicDeck.Add(card);
            }

            return true;
        }
    }
}
