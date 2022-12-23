using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SujinsCards;
using SujinsInterpreter;

namespace SujinsLogic
{
    public static class Boveda
    {
        public static List<MonsterCard> MarketMonsterDeck = new List<MonsterCard>();
        public static List<MonsterCard> PublicMonsterDeck = new List<MonsterCard>();

        public static Deck<MagicCard> PublicMagicDeck = new Deck<MagicCard>();

        public static int Coins { private set; get; }
        
        public static string Load()
        {
            string error = "", msg;

            msg = LoadMonsterCards();
            
            if (msg == string.Empty)
                error += "\n" + msg;

            msg = LoadMagicCards();

            if (msg == string.Empty)
                error += "\n" + msg;

            return error.Trim();
        }

        private static string LoadMonsterCards()
        {
            string path = Config.DataBase + "Monstruos.data";

            if (!File.Exists(path))
            {
                return "No se ha podido encontrar el archivo \"Monstruos.data\".";
            }

            string[] monsterData = File.ReadAllText(path).Split("\n\r".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

            Random r = new Random();

            for (int i = 0; i < monsterData.Length; i++)
            {
                string[] propiedades = monsterData[i].Split("-".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

                MonsterCard monster = new MonsterCard(
                    propiedades[0].Trim(),      // Name
                    propiedades[1].Trim(),      // Description
                    r.Next(1, 8),               // Prize
                    propiedades[2].Trim(),      // Image
                    r.Next(50, 60),             // Attack
                    r.Next(25, 40),             // Deffense
                    TypeMonsterElement.Dark     // Type
                );

                if (i < Config.CantCardsPublics)
                    PublicMonsterDeck.Add(monster);

                else 
                    MarketMonsterDeck.Add(monster);
            }

            return string.Empty;
        }

        private static string LoadMagicCards()
        {
            string path = Config.DataBase + "Magics.txt";

            if (!File.Exists(path))
            {
                return "No se ha podido encontrar el archivo \"Magics.txt\".";
            }

            string magicData = File.ReadAllText(path);

            Lexer lexer = new Lexer(magicData, "debug");
            Parser parser = new Parser(lexer);
            Interpreter interpreter = new Interpreter(parser);

            try
            {
                parser.GetCards();
            }
            catch (System.Exception e)
            {
                return e.Message;
            }

            lexer = new Lexer(magicData, "debug");
            parser = new Parser(lexer);
            interpreter = new Interpreter(parser);

            foreach (MagicCard card in interpreter.GetCards())
            {
                PublicMagicDeck.Add(card);
            }


            return String.Empty;
        }

        public static void SetCoins(int coins)
        {
            Coins += coins;
        }
    }
}
