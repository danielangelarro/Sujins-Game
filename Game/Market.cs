using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SujinsCards;
using SujinsInterpreter;

namespace SujinsLogic
{
    public class Market
    {
        public string Error { get; private set; }
        private int BuyCardID, SellCardID;
        private int CantOfCards = Boveda.MarketMonsterDeck.Count;
        private int CantCardsPublics = Boveda.PublicMonsterDeck.Count;

        public string ShowMonster(string operation)
        {
            if (operation == "buy")
                return Boveda.MarketMonsterDeck[BuyCardID].ToString();
            
            return Boveda.PublicMonsterDeck[SellCardID].ToString();
        }

        public string GetPrice(string operation)
        {
            if (operation == "buy")
                return Boveda.MarketMonsterDeck[BuyCardID].Price.ToString();
            
            int price = Boveda.PublicMonsterDeck[SellCardID].Price;
            int sell = (int)(price * 0.7);

            return sell.ToString();
        }

        public string ShowImageMonster(string operation)
        {
            string dir = Config.MediaBase + "image_card/";

            if (operation == "buy")
                return dir + Boveda.MarketMonsterDeck[BuyCardID].Image;
            
            return dir + Boveda.PublicMonsterDeck[SellCardID].Image;
        }

        public void NextCard(string operation)
        {
            if (operation == "buy")
                BuyCardID = (BuyCardID + 1) % CantOfCards;

            else 
                SellCardID = (SellCardID + 1) % CantCardsPublics;
        }

        public void PrevCard(string operation)
        {
            if (operation == "buy")
                BuyCardID = (BuyCardID + CantOfCards - 1) % CantOfCards;
            
            else 
                SellCardID = (SellCardID + CantOfCards - 1) % CantOfCards;
        }

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
        
        public void SellCard()
        {
            int price = (int)(Boveda.PublicMonsterDeck[SellCardID].Price * 0.7);
            Boveda.SetCoins(price);
            Boveda.MarketMonsterDeck.Add(Boveda.PublicMonsterDeck[SellCardID]);
            Boveda.PublicMonsterDeck.RemoveAt(SellCardID);

            NextCard("sell");
        }

        public bool CreateMonsterCard(string name, string description, int price, string image, int attack,
            int defense, int hp, int mp)
        {
            if (name.Trim() == "" || description.Trim() == "" || image.Trim() == "")
                return false;
            
            if (price <= 0 || attack <= 0 || defense <= 0 || hp <= 0 || mp <= 0)
                return false;
            
            if (Boveda.PublicMonsterDeck.Exists(m => m.Name == name) || Boveda.MarketMonsterDeck.Exists(m => m.Name == name))
                return false;
            
            MonsterCard card = new MonsterCard(
                name, description, price, image, attack, defense, TypeMonsterElement.Fire, hp
            );

            Boveda.MarketMonsterDeck.Add(card);

            return true;
        }

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
