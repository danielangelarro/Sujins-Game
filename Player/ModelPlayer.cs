namespace SujinsPlayer;

protected interface class ModelPlayer
{
    public GameState Play(GameState state);
}

public class ActionEjecuteInfo
{
    public int PlayerId { get; private set; }
    public int ActionId { get; private set; }
    public int MagicId { get; private set; }
    public int MonsterSelfId { get; private set; }
    public int MonsterEnemyId { get; private set; }

    public ActionEjecuteInfo(int player, int action, magic, int monsterSelf, int monsterEnemy)
    {
        PlayerId = player;
        ActionId = action;
        MagicId = magic;
        MonsterSelfId = monsterSelf;
        MonsterEnemyId = monsterEnemy;
    }
}