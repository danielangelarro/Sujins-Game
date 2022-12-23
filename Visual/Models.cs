using SujinsCards;
using SujinsLogic;

namespace Sujins.Visual;

public partial class SujinsGame
{
    static void PrintMenu()
    {
        string banner = @"
███████╗██╗   ██╗     ██╗██╗███╗   ██╗███████╗     ██████╗  █████╗ ███╗   ███╗███████╗
██╔════╝██║   ██║     ██║██║████╗  ██║██╔════╝    ██╔════╝ ██╔══██╗████╗ ████║██╔════╝
███████╗██║   ██║     ██║██║██╔██╗ ██║███████╗    ██║  ███╗███████║██╔████╔██║█████╗  
╚════██║██║   ██║██   ██║██║██║╚██╗██║╚════██║    ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝  
███████║╚██████╔╝╚█████╔╝██║██║ ╚████║███████║    ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗
╚══════╝ ╚═════╝  ╚════╝ ╚═╝╚═╝  ╚═══╝╚══════╝     ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝
                                                                                      
        ";

        Console.WriteLine(banner); 
        Console.WriteLine("[J] Jugar\n[P] PVP\n[T] Tienda\n[S] Salir");
    }

    static void PrintMarket()
    {
        string banner = @"
████████╗██╗███████╗███╗   ██╗██████╗  █████╗     ██╗   ██╗██╗██████╗ ████████╗██╗   ██╗ █████╗ ██╗     
╚══██╔══╝██║██╔════╝████╗  ██║██╔══██╗██╔══██╗    ██║   ██║██║██╔══██╗╚══██╔══╝██║   ██║██╔══██╗██║     
   ██║   ██║█████╗  ██╔██╗ ██║██║  ██║███████║    ██║   ██║██║██████╔╝   ██║   ██║   ██║███████║██║     
   ██║   ██║██╔══╝  ██║╚██╗██║██║  ██║██╔══██║    ╚██╗ ██╔╝██║██╔══██╗   ██║   ██║   ██║██╔══██║██║     
   ██║   ██║███████╗██║ ╚████║██████╔╝██║  ██║     ╚████╔╝ ██║██║  ██║   ██║   ╚██████╔╝██║  ██║███████╗
   ╚═╝   ╚═╝╚══════╝╚═╝  ╚═══╝╚═════╝ ╚═╝  ╚═╝      ╚═══╝  ╚═╝╚═╝  ╚═╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚══════╝
                                                                                                                                                                                                
        ";

        Console.WriteLine(banner); 
        Console.WriteLine("[C] Comprar una carta monstruo\n[V] Vender una carta monstruo\n[S] Salir");
    }

    #region Game Combat

    static void PrintSelectCard(int player)
    {
        string banner = @"
 __   ___       ___  __   __     __           __   ___           __        __  ___  __        __   __  
/__` |__  |    |__  /  ` /  ` | /  \ |\ |    |  \ |__      |\/| /  \ |\ | /__`  |  |__) |  | /  \ /__` 
.__/ |___ |___ |___ \__, \__, | \__/ | \|    |__/ |___     |  | \__/ | \| .__/  |  |  \ \__/ \__/ .__/ 
                                                                                                       
        ";

        Console.WriteLine($"{banner} JUGADOR: {player})***\n");
        Console.WriteLine(game.GetMonsterSelect(player).ToString());
        Console.WriteLine("\n[A] Anterior carta");
        Console.WriteLine("[S] Seleccionar carta");
        Console.WriteLine("[P] Proxima carta\n");
        Console.WriteLine("[Q] Quitar");

        int rest = game.Status.MonstersP1.Count;

        Console.WriteLine("\n🗑 Quitar cartas ({})\n");
        char[] codes = {'X', 'Y', 'Z'};

        if (player == 1)
            for (int i = 0; i < game.Status.MonstersP1.Count; i++)
                Console.WriteLine($"[{codes[i]}] 🎴 {game.Status.MonstersP1[i].Name}");
        
        else
            for (int i = 0; i < game.Status.MonstersP2.Count; i++)
                Console.WriteLine($"[{codes[i]}] 🎴 {game.Status.MonstersP2[i].Name}");
    }

    static void PrintCards(int player, bool isCamp, string color)
    {
        List<MonsterCard> hand = player == 1 ? game.Status.MonstersP1 : game.Status.MonstersP2;

        string[] name = new string[3];
        string[] hp = new string[3];
        string[] atk = new string[3];
        string[] def = new string[3];

        for (int i = 0; i < 3; i++)
        {
            if (!hand[i].IsDead() && hand[i].IsActive == isCamp)
            {
                name[i] = hand[i].Name;
                hp[i] = $"({hand[i].HealtPoints}/{hand[i].MaxHealtPoint})";
                atk[i] = hand[i].Attack.ToString();
                def[i] = hand[i].Defense.ToString();
            }
            else
            {
                name[i] = "";
                hp[i] = "";
                atk[i] = "";
                def[i] = "";
            }

            name[i] = name[i].PadRight(26);
            hp[i] = hp[i].PadRight(22);
            atk[i] = atk[i].PadRight(21);
            def[i] = def[i].PadRight(21);
        }

        string header = "CARTAS EN MI MANO";
        
        if (game.Status.Turn + 1 != player)
            header = "CARTAS EN CAMPO ENEMIGO";
        
        else if (isCamp)
            header = "CARTAS EN MI CAMPO";

        string str = @$"
                                           {header}
    +----------------------------+    +----------------------------+    +----------------------------+
    |       (A) Monstruo         |    |        (B) Monstruo        |    |        (C) Monstruo        | 
    |____________________________|    |____________________________|    |____________________________|
    | {name[0]} |    | {name[1]} |    | {name[2]} |
    | HP: {hp[0]} |    | HP: {hp[1]} |    | HP: {hp[2]} |
    | ATK: {atk[0]} |    | ATK: {atk[1]} |    | ATK: {atk[2]} |
    | DEF: {def[0]} |    | DEF: {def[1]} |    | DEF: {def[2]} |
    +----------------------------+    +----------------------------+    +----------------------------+
";

        if (color == "yellow")
            Console.ForegroundColor = ConsoleColor.Yellow;
        else if (color == "green")
            Console.ForegroundColor = ConsoleColor.Green;
        else if (color == "green")
            Console.ForegroundColor = ConsoleColor.Green;
        
        Console.WriteLine(str);
        Console.ForegroundColor = ConsoleColor.White;
    }

    static void PrintGameCamp(int player)
    {
        if (player == 1)
        {
            PrintCards(2, true, "green");
            PrintCards(1, true, "green");
            PrintCards(1, false, "yellow");
        }
            
        else
        {
            PrintCards(1, true, "green");
            PrintCards(2, true, "green");
            PrintCards(2, false, "yellow");
        }
   
        Console.WriteLine("\n***Cartas mágicas en mi mano***");
        
        if (player == 1)
            foreach (var card in game.Status.MagicsP1)
            {
                if (!card.IsActive)
                    Console.WriteLine(card.ToString());
            }
            
        else
            foreach (var card in game.Status.MagicsP2)
            {
                if (!card.IsActive)
                    Console.WriteLine(card.ToString());
            }
        
        Console.WriteLine($"***Seleccione la acción a realizar (Jugador {player})***\n");
        
        Console.WriteLine("[C] Colocar monstruo en el campo.        [P] Pasar turno.");
        Console.WriteLine("[U] Usar carta mágica.                   [S] Salir al menu principal.");
        Console.WriteLine("[A] Atacar");
    }

    static void PlayerWins(int player)
    {
        string banner1 = @"
     ██╗██╗   ██╗ ██████╗  █████╗ ██████╗  ██████╗ ██████╗      ██╗    ██╗  ██╗ █████╗      ██████╗  █████╗ ███╗   ██╗ █████╗ ██████╗  ██████╗ ██╗██╗██╗
     ██║██║   ██║██╔════╝ ██╔══██╗██╔══██╗██╔═══██╗██╔══██╗    ███║    ██║  ██║██╔══██╗    ██╔════╝ ██╔══██╗████╗  ██║██╔══██╗██╔══██╗██╔═══██╗██║██║██║
     ██║██║   ██║██║  ███╗███████║██║  ██║██║   ██║██████╔╝    ╚██║    ███████║███████║    ██║  ███╗███████║██╔██╗ ██║███████║██║  ██║██║   ██║██║██║██║
██   ██║██║   ██║██║   ██║██╔══██║██║  ██║██║   ██║██╔══██╗     ██║    ██╔══██║██╔══██║    ██║   ██║██╔══██║██║╚██╗██║██╔══██║██║  ██║██║   ██║╚═╝╚═╝╚═╝
╚█████╔╝╚██████╔╝╚██████╔╝██║  ██║██████╔╝╚██████╔╝██║  ██║     ██║    ██║  ██║██║  ██║    ╚██████╔╝██║  ██║██║ ╚████║██║  ██║██████╔╝╚██████╔╝██╗██╗██╗
 ╚════╝  ╚═════╝  ╚═════╝ ╚═╝  ╚═╝╚═════╝  ╚═════╝ ╚═╝  ╚═╝     ╚═╝    ╚═╝  ╚═╝╚═╝  ╚═╝     ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝  ╚═╝╚═════╝  ╚═════╝ ╚═╝╚═╝╚═╝
                                                                                                                                                        
        ";

        string banner2 = @"
     ██╗██╗   ██╗ ██████╗  █████╗ ██████╗  ██████╗ ██████╗     ██████╗     ██╗  ██╗ █████╗      ██████╗  █████╗ ███╗   ██╗ █████╗ ██████╗  ██████╗ ██╗██╗██╗
     ██║██║   ██║██╔════╝ ██╔══██╗██╔══██╗██╔═══██╗██╔══██╗    ╚════██╗    ██║  ██║██╔══██╗    ██╔════╝ ██╔══██╗████╗  ██║██╔══██╗██╔══██╗██╔═══██╗██║██║██║
     ██║██║   ██║██║  ███╗███████║██║  ██║██║   ██║██████╔╝     █████╔╝    ███████║███████║    ██║  ███╗███████║██╔██╗ ██║███████║██║  ██║██║   ██║██║██║██║
██   ██║██║   ██║██║   ██║██╔══██║██║  ██║██║   ██║██╔══██╗    ██╔═══╝     ██╔══██║██╔══██║    ██║   ██║██╔══██║██║╚██╗██║██╔══██║██║  ██║██║   ██║╚═╝╚═╝╚═╝
╚█████╔╝╚██████╔╝╚██████╔╝██║  ██║██████╔╝╚██████╔╝██║  ██║    ███████╗    ██║  ██║██║  ██║    ╚██████╔╝██║  ██║██║ ╚████║██║  ██║██████╔╝╚██████╔╝██╗██╗██╗
 ╚════╝  ╚═════╝  ╚═════╝ ╚═╝  ╚═╝╚═════╝  ╚═════╝ ╚═╝  ╚═╝    ╚══════╝    ╚═╝  ╚═╝╚═╝  ╚═╝     ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝  ╚═╝╚═════╝  ╚═════╝ ╚═╝╚═╝╚═╝
                                                                                                                                                            
        ";

        Console.Clear();
        Console.WriteLine(player == 1 ? banner1 : banner2);
        Console.WriteLine("\nPresione una tecla para continuar...");
        Console.ReadKey();
    }

    #endregion

    #region Virtual Market

    static void PrintMarketBuy()
    {
        string banner = @"
 ██████╗ ██████╗ ███╗   ███╗██████╗ ██████╗  █████╗     ██████╗ ███████╗    ███╗   ███╗ ██████╗ ███╗   ██╗███████╗████████╗██████╗ ██╗   ██╗ ██████╗ ███████╗
██╔════╝██╔═══██╗████╗ ████║██╔══██╗██╔══██╗██╔══██╗    ██╔══██╗██╔════╝    ████╗ ████║██╔═══██╗████╗  ██║██╔════╝╚══██╔══╝██╔══██╗██║   ██║██╔═══██╗██╔════╝
██║     ██║   ██║██╔████╔██║██████╔╝██████╔╝███████║    ██║  ██║█████╗      ██╔████╔██║██║   ██║██╔██╗ ██║███████╗   ██║   ██████╔╝██║   ██║██║   ██║███████╗
██║     ██║   ██║██║╚██╔╝██║██╔═══╝ ██╔══██╗██╔══██║    ██║  ██║██╔══╝      ██║╚██╔╝██║██║   ██║██║╚██╗██║╚════██║   ██║   ██╔══██╗██║   ██║██║   ██║╚════██║
╚██████╗╚██████╔╝██║ ╚═╝ ██║██║     ██║  ██║██║  ██║    ██████╔╝███████╗    ██║ ╚═╝ ██║╚██████╔╝██║ ╚████║███████║   ██║   ██║  ██║╚██████╔╝╚██████╔╝███████║
 ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝    ╚═════╝ ╚══════╝    ╚═╝     ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚══════╝
                                                                                                                                                             
        ";

        Console.WriteLine(banner);
        Console.WriteLine($"Dinero actual: ${Boveda.Coins}\n");

        Console.WriteLine(market.ShowMonster("buy"));
        Console.WriteLine("\n[A] Anterior carta");
        Console.WriteLine("[P] Proxima carta\n");
        Console.WriteLine("[Q] Quitar");

        Console.WriteLine($"\n[C] Comprar por ${market.GetPrice("buy")}");
    }

    static void PrintMarketSell()
    {
        string banner = @"
██╗   ██╗███████╗███╗   ██╗████████╗ █████╗     ██████╗ ███████╗    ███╗   ███╗ ██████╗ ███╗   ██╗███████╗████████╗██████╗ ██╗   ██╗ ██████╗ ███████╗
██║   ██║██╔════╝████╗  ██║╚══██╔══╝██╔══██╗    ██╔══██╗██╔════╝    ████╗ ████║██╔═══██╗████╗  ██║██╔════╝╚══██╔══╝██╔══██╗██║   ██║██╔═══██╗██╔════╝
██║   ██║█████╗  ██╔██╗ ██║   ██║   ███████║    ██║  ██║█████╗      ██╔████╔██║██║   ██║██╔██╗ ██║███████╗   ██║   ██████╔╝██║   ██║██║   ██║███████╗
╚██╗ ██╔╝██╔══╝  ██║╚██╗██║   ██║   ██╔══██║    ██║  ██║██╔══╝      ██║╚██╔╝██║██║   ██║██║╚██╗██║╚════██║   ██║   ██╔══██╗██║   ██║██║   ██║╚════██║
 ╚████╔╝ ███████╗██║ ╚████║   ██║   ██║  ██║    ██████╔╝███████╗    ██║ ╚═╝ ██║╚██████╔╝██║ ╚████║███████║   ██║   ██║  ██║╚██████╔╝╚██████╔╝███████║
  ╚═══╝  ╚══════╝╚═╝  ╚═══╝   ╚═╝   ╚═╝  ╚═╝    ╚═════╝ ╚══════╝    ╚═╝     ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚══════╝
                                                                                                                                                     
        ";

        Console.WriteLine(banner);
        Console.WriteLine($"Dinero actual: ${Boveda.Coins}\n");

        Console.WriteLine(market.ShowMonster("sell"));
        Console.WriteLine("\n[A] Anterior carta");
        Console.WriteLine("[P] Proxima carta\n");
        Console.WriteLine("[Q] Quitar");

        Console.WriteLine($"\n[V] Vender por ${market.GetPrice("sell")}");
    }
    #endregion
}