using SujinsCards;

namespace SujinsPlayer;

public class Bot : ModelPlayer
{
    public override ActionEjecuteInfo Play(int level, GameState state)
    {
        if (level == 1)
        {
            return PlayLevel1(state);
        }
        else
        {
            return PlayLevel2(state);
        }
    }

    

    /// <summary>
    /// Ejecuta una jugada de 1er nivel correspondiente al jugador virtual.
    /// </summary>

    public ActionEjecuteInfo PlayLevel1(GameState Status)
    {
        // Acciones que puede realizar el jugador virtual
        
        throw Exception("No implemented");
    }

    /// <summary>
    /// Ejecuta una jugada de 2do nivel correspondiente al jugador virtual.
    /// </summary>
    public ActionEjecuteInfo PlayLevel2(GameState Status)
    
        // Si no tiene monstruos en el campo se le impide realizar otras acciones y se le obliga
        // a colocar uno que no este muerto.
        if (!IsValidMovement(2))
        {
            for (int i = 0; i < Status.MonstersP2.Count; i++)
            {
                if (!Status.MonstersP2[i].IsDead() && !Status.MonstersP2[i].IsActive)
                {
                    return new ActionEjecuteInfo(2, 2, -1, i, -1);
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

            return ActionEjecuteInfo(2, 1, rnd.Next() % cant, -1, -1);
        }

        return ActionEjecuteInfo(2, 0, -1, GetMonsterCard(2), GetMonsterCard(1));
    }

    /// <summary>
    /// Localiza su monstruo con mayor poder de ataque y el monstruo enemigo con menor poder de defensa.
    /// </summary>
    /// <param value="player">
    /// Player asociado a la accion.
    /// </param>
    private int GetMonsterCard(int player)
    {
        int state = player == 1 ? int.MaxValue : 0;
        int cardId = 0;

        if (player == 1)
        {
            int i = 0;
            foreach (MonsterCard monster in Status.MonstersP1)
            {
                if (monster.IsActive && monster.HealtPoints < state)
                {
                    state = monster.HealtPoints;
                    cardId = i;
                }
                i++;
            }
        }
        else
        {
            int i = 0;
            foreach (MonsterCard monster in Status.MonstersP2)
            {
                if (monster.IsActive && monster.HealtPoints > state)
                {
                    state = monster.HealtPoints;
                    cardId = i;
                }
            }
            i++;
        }

        return cardId;
    }
}
