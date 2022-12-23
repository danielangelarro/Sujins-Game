using System;
using System.Collections.Generic;

using SujinsCards;

namespace SujinsInterpreter
{
    public class Methods
    {
        public void IncrementHP(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type, int value)
        {
            if (type == 1)
                monsterSelf[position].UpdateHealtPoints(value);
            
            else 
                monsterEnemy[position].UpdateHealtPoints(value);
        }

        public void DecrementHP(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type, int value)
        {
            if (type == 1)
                monsterSelf[position].UpdateHealtPoints(-value);
            
            else 
                monsterEnemy[position].UpdateHealtPoints(-value);
        }

        public void IncrementATK(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type, int value)
        {
            if (type == 1)
                monsterSelf[position].UpdateAttack(value);
            
            else 
                monsterEnemy[position].UpdateAttack(value);
        }

        public void DecrementATK(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type, int value)
        {
            if (type == 1)
                monsterSelf[position].UpdateAttack(-value);
            
            else 
                monsterEnemy[position].UpdateAttack(-value);
        }

        public void IncrementDEF(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type, int value)
        {
            if (type == 1)
                monsterSelf[position].UpdateDeffense(value);
            
            else 
                monsterEnemy[position].UpdateDeffense(value);
        }

        public void DecrementDEF(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type, int value)
        {
            if (type == 1)
                monsterSelf[position].UpdateDeffense(-value);
            
            else 
                monsterEnemy[position].UpdateDeffense(-value);
        }

        public string GetType(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type)
        {
            if (type == 1)
                return monsterSelf[position].Type.ToString();
            
            return monsterEnemy[position].Type.ToString();
        }
    }
}