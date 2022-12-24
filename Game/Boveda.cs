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
    /// <summary>
    /// Clase estatica que se va a encargar de almacenar variables globales del juego como
    /// las cartas y su distribucion en la tienda o de uso publico.
    /// </summary>
    public static class Boveda
    {
        // Cartas que van a estar en la tienda.
        public static List<MonsterCard> MarketMonsterDeck = new List<MonsterCard>();
        
        // Cartas con las que el player tiene acceso inicialmente.
        public static List<MonsterCard> PublicMonsterDeck = new List<MonsterCard>();

        // Cartas magicas que aparecen en el juego.
        public static Deck<MagicCard> PublicMagicDeck = new Deck<MagicCard>();

        // Dinero de la seccion del juego con el cual se puede realizar transacciones en la tienda.
        public static int Coins { private set; get; }
        
        /// <summary>
        /// Carga las cartas una vez iniciado el juego
        /// </summary>
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

        /// <summary>
        /// Carga las cartas de tipo monstruos que estan en el arhivo Monstruos.data y 
        /// las carga una vez inicado el juego
        /// </summary>
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

                // Las propiedades como el ataque, la defensa
                // y el precio se crean con numeros aleatorios
                // por lo que cada vez que inicie el juego estas cartas nunca
                // van a tener los mismos valores en estas propiedades
                MonsterCard monster = new MonsterCard(
                    propiedades[0].Trim(),      // Name
                    propiedades[1].Trim(),      // Description
                    r.Next(1, 8),               // Prize
                    propiedades[2].Trim(),      // Image
                    r.Next(50, 60),             // Attack
                    r.Next(25, 40),             // Deffense
                    TypeMonsterElement.Dark     // Type
                );

                // Cartas a las que puede acceder el player
                if (i < Config.CantCardsPublics)
                    PublicMonsterDeck.Add(monster);

                // Cartas que van dirigidas a la tienda.
                else 
                    MarketMonsterDeck.Add(monster);
            }

            return string.Empty;
        }

        /// <summary>
        /// Lee el archivo Magics.txt y carga las cartas magicas que este
        /// almacena, si el codigo de alguna carta es incorrecto lanza un mensaje
        /// de error
        /// </summary>
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

            // Verifica que el codigo del efecto sea correcto
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

            // Si el codigo fue correcto entonces se adiciona a la
            // lista de cartas magicas
            foreach (MagicCard card in interpreter.GetCards())
            {
                PublicMagicDeck.Add(card);
            }


            return String.Empty;
        }

        /// <summary>
        /// Actualiza el valor del dinero depues que este se haya usado
        /// o se haya culminado una partida en single players
        /// </summary>
        public static void SetCoins(int coins)
        {
            Coins += coins;
        }
    }
}
