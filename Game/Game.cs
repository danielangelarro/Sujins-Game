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
    /// <summary>
    /// Acciones que puede realizar un jugador durante una partida
    /// </summary>
    /// <remarks>
    /// Hereda de la clase Config por lo que se adapta a las configuraciones\
    /// declaradas globales
    /// </remarks>
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
    
    /// <summary>
    /// Clase encargada de controlar toda la logica relacionada al juego.
    /// </summary>
    public class Game : Config
    {
        public GameState Status;    // Estado actual del juego
        public string Mode { get; } //Modo de juego pvp o single players 

        // Indica si se escogio un monstruo, en caso de realizar un ataque
        public bool SelectObjective { private set; get; }

        //Indica si ya se realizo una accion durante el turno
        public bool FinishAction { private set; get; }

        public MonsterCard MonsterAttack;       // Guarda la referrencia del monstruo atacante
        public MonsterCard MonsterDefender;     // Guarda la referencai del monstruo defensor

        /// <summary>
        /// Clase constructora del juego
        /// </summary>
        /// <param value="mode">
        /// Modo del juego. Puede ser pvp(2 jugadores) o AI(contra pc)
        /// </param>
        public Game(string mode) : base("Debug")
        {
            Status = new GameState();
            
            this.Mode = mode;
        }

        #region Auxiliar Methods

        /// <summary>
        /// Comprueba si pueden ser realizados movimientos de tipo ataque o uso
        /// de cartas magicas. Esto solo e posible en el caso de que se encuentren
        /// cartas de tipo monstruos puestas en el campo de batalla.
        /// </summary>
        public bool IsValidMovement(int player)
        {
            if (player == 1)
                return Status.MonstersP1.Exists(card => card.IsActive);
            return Status.MonstersP2.Exists(card => card.IsActive);
        }

        /// <summary>
        /// Devuelve las cartas magicas que puede usar el player durante el turno
        /// </summary>
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

        /// <summary>
        /// Verifica si la partida ha concluido.
        /// </summary>
        private bool IsGameOver()
        {
            return Status.IsLosser(1) || Status.IsLosser(2);
        }

        #endregion

        #region Select Card

        private int MonsterCardID1 = 0;
        private int MonsterCardID2 = 0;
        private int CantOfCards = Boveda.PublicMonsterDeck.Count;

        /// <summary>
        /// Retorna el monstruo escogido por el player
        /// </summary>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        public MonsterCard GetMonsterSelect(int player)
        {
            if (player == 1)
                return Boveda.PublicMonsterDeck[MonsterCardID1];
            
            return Boveda.PublicMonsterDeck[MonsterCardID2];
        }

        /// <summary>
        /// Retorna la direccion de la imagen asociada a la carta seleccionada.
        /// </summary>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        public string ShowMonster(int player)
        {
            string dir = Config.MediaBase + "image_card/";

            if (player == 1)
                return dir + Boveda.PublicMonsterDeck[MonsterCardID1].Image;
    
            return dir + Boveda.PublicMonsterDeck[MonsterCardID2].Image;
        }

        /// <summary>
        /// Pasa a la siguiente carta monstruo disponible para su eleccion
        /// </summary>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        public void NextCard(int player)
        {
            if (player == 1)
                MonsterCardID1 = (MonsterCardID1 + 1) % Boveda.PublicMonsterDeck.Count;
            else
                MonsterCardID2 = (MonsterCardID2 + 1) % Boveda.PublicMonsterDeck.Count;
        }

        /// <summary>
        /// Pasa a la carta anterior de monstruo disponible para su eleccion.
        /// </summary>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        public void PrevCard(int player)
        {
            if (player == 1)
                MonsterCardID1 = (MonsterCardID1 + Boveda.PublicMonsterDeck.Count - 1) % Boveda.PublicMonsterDeck.Count;
            else
                MonsterCardID2 = (MonsterCardID2 + Boveda.PublicMonsterDeck.Count - 1) % Boveda.PublicMonsterDeck.Count;
        }

        /// <summary>
        /// Adiciona la carta escogida a los monstruos que participaran en la batalla
        /// del bando del player.
        /// </summary>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        public void SelectCard(int player)
        {
            if(player == 1)
                Status.MonstersP1.Add(Boveda.PublicMonsterDeck[MonsterCardID1].Clone());
            else
                Status.MonstersP2.Add(Boveda.PublicMonsterDeck[MonsterCardID2].Clone());
        }
        
        /// <summary>
        /// Elimina el monstruo seleccionado
        /// </summary>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        public void RemoveCard(int player)
        {
            if(player == 1)
                Status.MonstersP1.Remove(Boveda.PublicMonsterDeck[MonsterCardID1]);
            else
                Status.MonstersP2.Remove(Boveda.PublicMonsterDeck[MonsterCardID2]);
        }

        /// <summary>
        /// Elimina un monstruo de la mano del jugador determinado por su indice
        /// </summary>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        /// <param value="id">
        /// Indice que ocupa el monstruo a eliminar en la mano del jugador
        /// </param>
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
        
        /// <summary>
        /// Pasa al siguiente turno y reinicia todas las acciones realizadas en el turno actual
        /// </summary>
        /// <param value="Status">
        /// Estado actual del juego.
        /// </param>
        public void NextTurn(GameState Status)
        {
            Status.Turn = (Status.Turn + 1) % 2;
            
            SelectObjective = false;
            FinishAction = false;

            Actions(TypeAction.GetMagicCards, Status.Turn + 1, 0);
        }
    
        /// <summary>
        /// Se mueve el monstruo seleccionado hacia el campo y se actualiza la 
        /// variable FinishAction para indicar que ya se realizo una
        /// durante la partida
        /// </summary>
        /// <param value="Status">
        /// Estado actual del juego.
        /// </param>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        /// <param value="id">
        /// Indica el indice del MOnstruo sobre el que se va a ejecutar la accion.
        /// </param>
        private void MoveMonsterToCamp(GameState Status, int player, int id)
        {
            if (player == 1)
                Status.MonstersP1[id].IsActive = true;
            else
                Status.MonstersP2[id].IsActive = true;
            
            FinishAction = true;
        }

        /// <summary>
        /// Cuando se va a realizar un ataque se necesita saber el monstruo
        /// con el cual se va a atacar y sobre el cual se va a realizar el 
        /// ataque, este metodo se llama dos veces la primera para guardar en 
        /// MonsterAttack la carta con la que se va a realizar el ataque y la
        /// segunda para guardar en en MonsterDeffense sobre el cual se va a realizar
        /// ataque y ejecutar la accion
        /// </summary>
        /// <param value="Status">
        /// Estado actual del juego.
        /// </param>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        /// <param value="id">
        /// Indica el indice del MOnstruo sobre el que se va a ejecutar la accion.
        /// </param>
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

        /// <summary>
        /// Una vez seleccionado los monstruos con los cuales se va a realizar
        /// el ataque este metodo lo ejecuta, verifica si el monstruo atacado murio
        /// e indica con FinishAction que ya se realizo la accion
        /// </summary>
        /// <param value="Status">
        /// Estado actual del juego.
        /// </param>
        /// <param value="id">
        /// Indica el indice del MOnstruo sobre el que se va a ejecutar la accion.
        /// </param>
        private void ActionMonsterAttack(GameState Status, int id)
        {
            int attack = MonsterAttack.Attack;
            int defense = MonsterDefender.Defense;

            int danger = ReverseAttack ? attack - defense : Math.Max(0, attack - defense);

            MonsterDefender.UpdateHealtPoints( -danger);

            if (MonsterDefender.IsDead())
            {
                MonsterDefender.IsActive = false;
            }

            FinishAction = true;
        }

        /// <summary>
        /// Ejecuta el efecto de una carta magica. Verifica que la carta sobre
        /// la cual se va a aplicar esta en el campo y si es asi le pasa el
        /// el codigo al interprete que es el encargado de ejecutarlo
        /// </summary>
        /// <param value="Status">
        /// Estado actual del juego.
        /// </param>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        /// <param value="id">
        /// Indica el indice de la carta magica que se va a autilizar.
        /// </param>
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

            interprete.GetCards();

            FinishAction = true;
        }

        /// <summary>
        /// Muestra la informacion asociada a una carta. La informacion se guarda
        /// en la variable `TableOfInfo` del estado del juego.
        /// </summary>
        /// <param value="Status">
        /// Estado actual del juego.
        /// </param>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        /// <param value="id">
        /// Indica el indice de la carta magica que se va a autilizar.
        /// </param>
        /// <param value="type">
        /// Tipo de carta. Puede ser de monstruo o magica.
        /// </param>
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

        /// <summary>
        /// Retorna las cartas magicas que puede usar el player durante el turno.
        /// </summary>
        /// <param value="Status">
        /// Estado actual del juego.
        /// </param>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        private void GetMagicCards(GameState Status, int player)
        {
            if (player == 1)
                Status.MagicsP1 = GetMagicCardsHand();
            
            else 
                Status.MagicsP2 = GetMagicCardsHand();
        }

        #endregion

        #region Ejecute Actions

        /// <summary>
        /// Ejecuta las distintas acciones que se pueden realizar durante
        /// un turno. LLama a una sobrecarga del metodo pasandole como referencia
        /// el estado actual que posee el juego.
        /// </summary>
        /// <param value="action">
        /// Accion que se quiere realizar.
        /// </param>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        /// <param value="cardId">
        /// Indica el indice de la carta magica que se va a autilizar.
        /// </param>
        /// <param value="type">
        /// Tipo de carta. Puede ser de monstruo o magica.
        /// </param>
        public void Actions(TypeAction action, int player, int cardId, string type="")
        {
            ActionsManager(action, player, cardId, Status, type);
        }

        /// <summary>
        /// Ejecuta las distintas acciones que se pueden realizar durante
        /// un turno.
        /// </summary>
        /// <param value="action">
        /// accion que se va a realizar
        /// </param>
        /// <param value="player">
        /// Indica el indice del jugador con el que interactua en ese momento.
        /// </param>
        /// <param value="cardId">
        /// Indica el indice de la carta magica que se va a autilizar.
        /// </param>
        /// <param value="status">
        /// Estado actual del juego sobre el que se va a realizar la accion.
        /// </param>
        /// <param value="type">
        /// Tipo de carta. Puede ser de monstruo o magica.
        /// </param>
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

        /// <summary>
        /// Carga los monstruos que usara el jugador virtual en la partida.
        /// </summary>
        public void BotLoadMonsterCards()
        {
            Random rand = new Random();

            for (int i = 0; i < CantMonsterForPlayer; i++)
            {
                MonsterCardID2 = rand.Next() % CantMonsterForPlayer;

                SelectCard(2);
            }
        }

        /// <summary>
        /// Ejecuta una jugada correspondiente al jugador virtual.
        /// </summary>
        public void BotPlay()
        {
            // Acciones que puede realizar el jugador virtual
            TypeAction[] actions = {
                TypeAction.MonsterAttack,
                TypeAction.MagicUsage,
                TypeAction.MoveMonsterToCamp
            };

            // Si no tiene monstruos en el campo se le impide realizar otras acciones y se le obliga
            // a colocar uno que no este muerto.
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

            // En caso de que tenga monstruos en el campo utiliza una carta magica cada
            // cierta cantidad de turnos aleatorios. En caso contrario ejecuta un ataque.

            Random rand = new Random();
            int option = rand.Next();

            if (option % 3 == 0)
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

        /// <summary>
        /// Localiza su monstruo con mayor poder de ataque y el monstruo enemigo con menor poder de defensa.
        /// </summary>
        /// <param value="player">
        /// Player asociado a la accion.
        /// </param>
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
