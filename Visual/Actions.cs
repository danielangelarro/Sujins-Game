using System.Data;
using System.Security.Cryptography;
using System.ComponentModel;
using System;

using SujinsLogic;

namespace Sujins.Visual;

public partial class SujinsGame
{
    #region Virtual Market

    static void ActionVirtualMarket(string operation)
    {
        while(true)
        {
            Console.Clear();

            if (operation == "buy")
                PrintMarketBuy();
            else
                PrintMarketSell();

            ConsoleKey key = Console.ReadKey(true).Key;

            switch(key)
            {
                case ConsoleKey.A:
                    
                    market.PrevCard(operation);
                    break;
                
                case ConsoleKey.C:
                    
                    if (operation == "buy")
                        market.BuyCard();

                    break;
                
                case ConsoleKey.V:

                    if (operation == "sell")
                        market.SellCard();
                    
                    break;

                case ConsoleKey.P:
                    
                    market.NextCard(operation);
                    break;

                case ConsoleKey.Q:
                    return;
            }
        }
    }

    #endregion

    #region Game Combat

    static void ActionMoveToCamp(int player)
    {
        Console.Clear();

        Console.Write($"\nSeleccione el monstruo a colocar [A - B - C]: ");
        ConsoleKey key = Console.ReadKey(true).Key;

        switch(key)
        {
            case ConsoleKey.A:
                
                game.Actions(TypeAction.MoveMonsterToCamp, player, 0);
                break;
            
            case ConsoleKey.B:
                
                game.Actions(TypeAction.MoveMonsterToCamp, player, 1);
                break;

            case ConsoleKey.C:
                
                game.Actions(TypeAction.MoveMonsterToCamp, player, 2);
                break;
        }
    }

    static void ActionAttack(int player)
    {
        Console.Clear();

        if (!game.IsValidMovement(player))
        {
            Console.WriteLine("Debe tener monstruos en el campo para ejecutar esta accion.\nPresione una tecla para continuar...");
            Console.ReadKey();
            
            return;
        }

        Console.WriteLine("\nSeleccione el indice de su monstruo y el que desee atacar de la forma [#id-#id], (#id va de 0 - 2)");
        string[] option = Console.ReadLine().Split('-');

        int monsterSelf = int.Parse(option[0]);
        int monsterEnemy = int.Parse(option[1]);
        int enemy = player == 1 ? 2 : 1;

        game.Actions(TypeAction.SelectMonsterCard, player, monsterSelf);
        game.Actions(TypeAction.SelectMonsterCard, enemy, monsterEnemy);
    }

    static void ActionUsemagicCard(int player)
    {
        Console.Clear();

        if (!game.IsValidMovement(player))
        {
            Console.WriteLine("Debe tener monstruos en el campo para ejecutar esta accion.\nPresione una tecla para continuar...");
            Console.ReadKey();
            
            return;
        }

        Console.Write($"\nSeleccione la carta mágica a utilizar [B - N - M]: ");
        ConsoleKey key = Console.ReadKey(true).Key;

        switch(key)
        {
            case ConsoleKey.B:
                
                game.Actions(TypeAction.MagicUsage, player, 0);
                break;
            
            case ConsoleKey.N:
                
                game.Actions(TypeAction.MagicUsage, player, 1);
                break;

            case ConsoleKey.M:
                
                game.Actions(TypeAction.MagicUsage, player, 2);
                break;
        }
    }

    #endregion
}