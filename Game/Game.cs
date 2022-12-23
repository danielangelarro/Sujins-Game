using System.Reflection.Metadata;
using System.Net;
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
    public enum TypeAction
    {
        None,
        MoveMonsterToCamp,
        SelectMonsterCard,
        MonsterAttack,
        MagicUsage,
        ShowInfoCard,
        GetMagicCards
    }
    
    public class Game : Config
    {
        public GameState Status;
        public string Mode { get; }

        public bool SelectObjective { private set; get; }
        public bool FinishAction { private set; get; }

        public MonsterCard MonsterAttack;
        public MonsterCard MonsterDefender;

        public Game(string mode) : base("Debug")
        {
            Status = new GameState();
            
            this.Mode = mode;
        }

        #region Auxiliar Methods

        public bool IsValidMovement(int player)
        {
            if (player == 1)
                return Status.MonstersP1.Exists(card => card.IsActive);
            return Status.MonstersP2.Exists(card => card.IsActive);
        }

        private List<MagicCard> GetMagicCardsHand()
        {
            List<MagicCard> hand = new List<MagicCard>();
            int cant = 0;

            foreach (var card in Boveda.PublicMagicDeck)
            {
                if (cant < CantMagicsForPlayerAtHand)
                {
                    hand.Add(card);
                    cant++;
                }
                else 
                    break;
            }

            return hand;
        }

        private bool IsGameOver()
        {
            return Status.IsLosser(1) || Status.IsLosser(2);
        }

        #endregion

        #region Select Card

        private int MonsterCardID1 = 0;
        private int MonsterCardID2 = 0;
        private int CantOfCards = Boveda.PublicMonsterDeck.Count;

        public MonsterCard GetMonsterSelect(int player)
        {
            if (player == 1)
                return Boveda.PublicMonsterDeck[MonsterCardID1];
            
            return Boveda.PublicMonsterDeck[MonsterCardID2];
        }

        public string ShowMonster(int player)
        {
            string dir = Config.MediaBase + "image_card/";

            if (player == 1)
                return dir + Boveda.PublicMonsterDeck[MonsterCardID1].Image;
    
            return dir + Boveda.PublicMonsterDeck[MonsterCardID2].Image;
        }

        public void NextCard(int player)
        {
            if (player == 1)
                MonsterCardID1 = (MonsterCardID1 + 1) % CantOfCards;
            else
                MonsterCardID2 = (MonsterCardID2 + 1) % CantOfCards;
        }

        public void PrevCard(int player)
        {
            if (player == 1)
                MonsterCardID1 = (MonsterCardID1 + CantOfCards - 1) % CantOfCards;
            else
                MonsterCardID2 = (MonsterCardID2 + CantOfCards - 1) % CantOfCards;
        }

        public void SelectCard(int player)
        {
            if(player == 1)
                Status.MonstersP1.Add(Boveda.PublicMonsterDeck[MonsterCardID1].Clone());
            else
                Status.MonstersP2.Add(Boveda.PublicMonsterDeck[MonsterCardID2].Clone());
        }
        
        public void RemoveCard(int player)
        {
            if(player == 1)
                Status.MonstersP1.Remove(Boveda.PublicMonsterDeck[MonsterCardID1]);
            else
                Status.MonstersP2.Remove(Boveda.PublicMonsterDeck[MonsterCardID2]);
        }

        public void SelectCardOfHand(int player, int id)
        {
            if (player == 1)
            {
                MonsterCard monster = Status.MonstersP1[id];
                MonsterCardID1 = Boveda.PublicMonsterDeck.FindIndex(m => m.Equals(monster));
            }
            else
            {
                MonsterCard monster = Status.MonstersP2[id];
                MonsterCardID2 = Boveda.PublicMonsterDeck.FindIndex(m => m.Equals(monster));
            }
        }

        #endregion

        #region Game Logic
        
        public void NextTurn(GameState Status)
        {
            Status.Turn = (Status.Turn + 1) % 2;
            
            SelectObjective = false;
            FinishAction = false;

            Actions(TypeAction.GetMagicCards, Status.Turn + 1, 0);
        }
    
        private void MoveMonsterToCamp(GameState Status, int player, int id)
        {
            if (player == 1)
                Status.MonstersP1[id].IsActive = true;
            else
                Status.MonstersP2[id].IsActive = true;
            
            FinishAction = true;
        }

        public void SelectMonsterCard(GameState Status, int player, int id) 
        {
            if (!SelectObjective && player == Status.Turn + 1)
            {
                if (player == 1 && Status.MonstersP1[id].IsActive)
                {
                    SelectObjective = true;
                    MonsterAttack = Status.MonstersP1[id];
                }
                else if(player == 2 && Status.MonstersP2[id].IsActive)
                {
                    SelectObjective = true;
                    MonsterAttack = Status.MonstersP2[id];
                }
            }

            else if(SelectObjective && player != Status.Turn + 1)
            {
                if (player == 1 && Status.MonstersP1[id].IsActive)
                    MonsterDefender = Status.MonstersP1[id];
                else if (player == 2 && Status.MonstersP2[id].IsActive)
                    MonsterDefender = Status.MonstersP2[id];

                ActionsManager(TypeAction.MonsterAttack, player, id, Status);

                SelectObjective = false;
            }
        }

        private void ActionMonsterAttack(GameState Status, int id)
        {
            int attack = MonsterAttack.Attack;
            int defense = MonsterDefender.Defense;

            int danger = ReverseAttack ? attack - defense : Math.Max(0, attack - defense);

            MonsterDefender.UpdateHealtPoints( -danger);

            if (MonsterDefender.IsDead())
            {
                if (Status.Turn == 0)
                    Status.MonstersP1[id].IsActive = false;
                else
                    Status.MonstersP2[id].IsActive = false;                   
            }

            FinishAction = true;
        }

        private void MagicUsage(GameState Status, int player, int id)
        {
            MagicCard card = (player == 1) ? Status.MagicsP1[id] : Status.MagicsP2[id];

            if (card.Position != -1)
            {
                if (player == 1 && (!Status.MonstersP1[card.Position].IsActive || Status.MonstersP1[card.Position].IsDead()))
                    return;
                
                if (player == 2 && (!Status.MonstersP2[card.Position].IsActive || Status.MonstersP2[card.Position].IsDead()))
                    return;
            }

            Lexer lexer = new Lexer(card.Action, "run");
            Parser parser = new Parser(lexer);

            MonsterCard monster = new MonsterCard();

            if (player == 1)
                monster = Status.MonstersP1[card.Position];

            else 
                monster = Status.MonstersP2[card.Position];

            List<MonsterCard> thisMonster = player == 1 ? Status.MonstersP1 : Status.MonstersP2;
            List<MonsterCard> enemyMonster = player == 2 ? Status.MonstersP1 : Status.MonstersP2;

            Interpreter interprete = new Interpreter(parser, thisMonster, enemyMonster);

            // TODO: Corregir metodo interpreter.interpret()

            interprete.GetCards();

            FinishAction = true;
        }

        private void ShowInfoCard(GameState Status, int player, int id, string type)
        {
            if (player == 1)
            {
                if (type == "monster")
                    Status.TableOfInfo.UpdateInfo(Status.MonstersP1[id], type);
                
                else if (type == "magic")
                    Status.TableOfInfo.UpdateInfo(Status.MagicsP1[id], type);
            }

            else
            {
                if (type == "monster")
                    Status.TableOfInfo.UpdateInfo(Status.MonstersP2[id], type);
                
                else if (type == "magic")
                    Status.TableOfInfo.UpdateInfo(Status.MagicsP2[id], type);
            }
        }

        private void GetMagicCards(GameState Status, int player)
        {
            if (player == 1)
                Status.MagicsP1 = GetMagicCardsHand();
            
            else 
                Status.MagicsP2 = GetMagicCardsHand();
        }

        #endregion

        #region Ejecute Actions

        public void Actions(TypeAction action, int player, int cardId, string type="")
        {
            ActionsManager(action, player, cardId, Status, type);
        }

        public string ActionsManager(TypeAction action, int player, int cardId, GameState status, string type="")
        {
            switch (action)
            {
                case TypeAction.None:

                    NextTurn(status);

                    return "";
                
                case TypeAction.MoveMonsterToCamp:

                    MoveMonsterToCamp(status, player, cardId);

                    break;
                
                case TypeAction.SelectMonsterCard:

                    SelectMonsterCard(status, player, cardId);

                    break;
                
                case TypeAction.MonsterAttack:

                    ActionMonsterAttack(status, cardId);
                    
                    break;
                
                case TypeAction.MagicUsage:

                    MagicUsage(status, player, cardId);
                    
                    break;

                case TypeAction.ShowInfoCard:

                    ShowInfoCard(status, player, cardId, type);

                    break;

                case TypeAction.GetMagicCards:

                    GetMagicCards(status, player);

                    break;
            }

            if (FinishAction)
            {
                FinishAction = false;
                NextTurn(status);
            }

            return "";
        }

        #endregion

        #region Bot

        public void BotLoadMonsterCards()
        {
            Random rand = new Random();

            for (int i = 0; i < CantMonsterForPlayer; i++)
            {
                MonsterCardID2 = rand.Next() % CantMonsterForPlayer;

                SelectCard(2);
            }
        }

        public void BotPlay()
        {
            TypeAction[] actions = {
                TypeAction.MonsterAttack,
                TypeAction.MagicUsage,
                TypeAction.MoveMonsterToCamp
            };

            Random rand = new Random();

            if (!IsValidMovement(2))
            {
                Console.Clear();
                Console.WriteLine("Jugador Virtual ha colocado una carta en el campo.");
                Console.ReadKey();

                for (int i = 0; i < Status.MonstersP2.Count; i++)
                {
                    if (!Status.MonstersP2[i].IsDead() && !Status.MonstersP2[i].IsActive)
                    {
                        Actions(TypeAction.MoveMonsterToCamp, 2, i);

                        break;
                    }
                }

                return;
            }

            if (rand.Next() % 3 == 0)
            {
                Random rnd = new Random();
                int cant = Status.MagicsP2.Count;

                ActionsManager(TypeAction.GetMagicCards, 2, 0, Status);
                ActionsManager(TypeAction.MagicUsage, 2, rnd.Next() % cant, Status);

                Console.Clear();
                Console.WriteLine("Jugador Virtual ha usado una carta magica.");
                Console.ReadKey();
            }
            else
            {
                MonsterAttack = GetMonsterCard(2);
                MonsterDefender = GetMonsterCard(1);

                ActionsManager(TypeAction.MonsterAttack, 2, 0, Status);

                Console.Clear();
                Console.WriteLine($"Jugador Virtual ha atacado con {MonsterAttack.Name} a {MonsterDefender.Name}");
                Console.ReadKey();
            }
        }

        private MonsterCard GetMonsterCard(int player)
        {
            MonsterCard card = new MonsterCard();
            int state = player == 1 ? int.MaxValue : 0;

            if (player == 1)
            {
                foreach (MonsterCard monster in Status.MonstersP1)
                {
                    if (monster.IsActive && monster.HealtPoints < state)
                    {
                        state = monster.HealtPoints;
                        card = monster;
                    }
                }
            }
            else
            {
                foreach (MonsterCard monster in Status.MonstersP2)
                {
                    if (monster.IsActive && monster.HealtPoints > state)
                    {
                        state = monster.HealtPoints;
                        card = monster;
                    }
                }
            }

            return card;
        }

        #endregion

    }
}
