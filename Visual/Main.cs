using Internal;
using System;
using SujinsLogic;

namespace Sujins.Visual;

public partial class SujinsGame
{
    static Game game = new Game("none");
    static Market market = new Market();

    public static void Main()
    {
        string error = Boveda.Load();
        Console.WriteLine(error);
        
        Console.WriteLine("Presione una tecla para continuar...");
        Console.ReadKey();

        if (error != "")
            return;

        while (true)
        {
            Console.Clear();
            PrintMenu();

            ConsoleKey key = Console.ReadKey(true).Key;

            switch(key)
            {
                case ConsoleKey.J:

                    NewGame();
                    break;
                
                case ConsoleKey.P:

                    NewGamePVP();
                    break;
                
                case ConsoleKey.T:

                    MainMarket();
                    break;
                
                case ConsoleKey.S:
                    return;
            }
        } 
    }

    public static void MainMarket()
    {
        while (true)
        {
            Console.Clear();
            PrintMarket();

            ConsoleKey key = Console.ReadKey(true).Key;

            switch(key)
            {
                case ConsoleKey.C:

                    ActionVirtualMarket("buy");
                    break;
                
                case ConsoleKey.V:

                    ActionVirtualMarket("sell");
                    break;
                
                case ConsoleKey.S:
                    return;
            }
        } 
    }

    static void SelectCard(int player)
    {
        while (true)
        {

            if ((player == 1 && game.Status.MonstersP1.Count == 3) || (player == 2 && game.Status.MonstersP2.Count == 3))
                break;

            Console.Clear();
            PrintSelectCard(player);

            ConsoleKey key = Console.ReadKey(true).Key;

            switch(key)
            {
                case ConsoleKey.A:

                    game.PrevCard(player);
                    break;
                
                case ConsoleKey.S:

                    game.SelectCard(player);
                    break;
                
                case ConsoleKey.P:

                    game.NextCard(player);
                    break;
                
                case ConsoleKey.Q:
                    
                    return;
                
                case ConsoleKey.X:

                    try
                    {
                        game.SelectCardOfHand(player, 0);
                        game.RemoveCard(player);
                    }
                    catch (System.Exception) {  }
                    break;
                
                case ConsoleKey.Y:

                    try
                    {
                        game.SelectCardOfHand(player, 1);
                        game.RemoveCard(player);
                    }
                    catch (System.Exception) {  }
                    break;
                
                case ConsoleKey.Z:

                    try
                    {
                        game.SelectCardOfHand(player, 2);
                        game.RemoveCard(player);
                    }
                    catch (System.Exception) {  }
                    break;
            }
        }
    }

    static void NewGame()
    {
        Console.Clear();

        game = new Game("AI");
        SelectCard(1);

        game.BotLoadMonsterCards();

        GameCamp();
    }

    static void NewGamePVP()
    {
        Console.Clear();

        game = new Game("pvp");

        SelectCard(1);
        SelectCard(2);

        GameCamp();
    }

    static void GameCamp()
    {
        while (!game.Status.IsLosser(1) && !game.Status.IsLosser(2))
        {
            Console.Clear();

            if (game.Status.Turn == 1 && game.Mode == "AI")
            {
                PrintGameCamp(game.Status.Turn);

                Console.WriteLine("El jugador virtual esta pensando su jugada...");
                Thread.Sleep(2000);

                game.BotPlay();

                continue;
            }

            PrintGameCamp(game.Status.Turn + 1);
            ConsoleKey key = Console.ReadKey(true).Key;

            switch(key)
            {
                case ConsoleKey.C:
                    
                    ActionMoveToCamp(game.Status.Turn + 1);
                    break;
                
                case ConsoleKey.A:
                    
                    ActionAttack(game.Status.Turn + 1);
                    break;

                case ConsoleKey.U:
                    
                    ActionUsemagicCard(game.Status.Turn + 1);
                    break;

                case ConsoleKey.P:
                    
                    game.Actions(TypeAction.None, game.Status.Turn + 1, -1);
                    break;

                case ConsoleKey.S:
                    
                    return;
            }
        }

        if (game.Status.IsLosser(1))
            PlayerWins(2);
        else if (game.Status.IsLosser(2))
            PlayerWins(1);
    }
}
