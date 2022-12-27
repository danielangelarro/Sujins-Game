using System;
using System.Collections.Generic;

using SujinsCards;

namespace SujinsInterpreter
{
    /// <summary>
    /// Clase que almacena las funciones predefinidas del intérprete.
    /// </summary>
    public class Methods
    {
        /// <summary>
        /// Método encargado de Incrementar los puntos de vida del monstruo que ocupa la posición indicada.
        /// </summary>
        /// <param name="monsterSelf">
        /// Lista de monstruos que me pertenecen.
        /// </param>
        /// <param name="monsterEnemy">
        /// Lista de monstruos que pertenecen al jugador oponente.
        /// </param>
        /// <param name="position">
        /// Posicion que ocupa el monstruo enla lista que desea ser modificado.
        /// </param>
        /// <param name="type">
        /// Representa sobre que monstruos va a interactuar: 1 si son los propios y 2 si son los del oponente.
        /// </param>
        /// <param name="value">
        /// Valor por el cual va a ser modificado el parámetro asocido.
        /// </param>
        public void IncrementHP(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type, int value)
        {
            if (type == 1)
                monsterSelf[position].UpdateHealtPoints(value);
            
            else 
                monsterEnemy[position].UpdateHealtPoints(value);
        }

        /// <summary>
        /// Método encargado de Decrementar los puntos de vida del monstruo que ocupa la posición indicada.
        /// </summary>
        /// <param name="monsterSelf">
        /// Lista de monstruos que me pertenecen.
        /// </param>
        /// <param name="monsterEnemy">
        /// Lista de monstruos que pertenecen al jugador oponente.
        /// </param>
        /// <param name="position">
        /// Posicion que ocupa el monstruo enla lista que desea ser modificado.
        /// </param>
        /// <param name="type">
        /// Representa sobre que monstruos va a interactuar: 1 si son los propios y 2 si son los del oponente.
        /// </param>
        /// <param name="value">
        /// Valor por el cual va a ser modificado el parámetro asocido.
        /// </param>
        public void DecrementHP(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type, int value)
        {
            if (type == 1)
                monsterSelf[position].UpdateHealtPoints(-value);
            
            else 
                monsterEnemy[position].UpdateHealtPoints(-value);
        }

        /// <summary>
        /// Método encargado de Incrementar los puntos de ataque del monstruo que ocupa la posición indicada.
        /// </summary>
        /// <param name="monsterSelf">
        /// Lista de monstruos que me pertenecen.
        /// </param>
        /// <param name="monsterEnemy">
        /// Lista de monstruos que pertenecen al jugador oponente.
        /// </param>
        /// <param name="position">
        /// Posicion que ocupa el monstruo enla lista que desea ser modificado.
        /// </param>
        /// <param name="type">
        /// Representa sobre que monstruos va a interactuar: 1 si son los propios y 2 si son los del oponente.
        /// </param>
        /// <param name="value">
        /// Valor por el cual va a ser modificado el parámetro asocido.
        /// </param>
        public void IncrementATK(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type, int value)
        {
            if (type == 1)
                monsterSelf[position].UpdateAttack(value);
            
            else 
                monsterEnemy[position].UpdateAttack(value);
        }

        /// <summary>
        /// Método encargado de Decrementar los puntos de ataque del monstruo que ocupa la posición indicada.
        /// </summary>
        /// <param name="monsterSelf">
        /// Lista de monstruos que me pertenecen.
        /// </param>
        /// <param name="monsterEnemy">
        /// Lista de monstruos que pertenecen al jugador oponente.
        /// </param>
        /// <param name="position">
        /// Posicion que ocupa el monstruo enla lista que desea ser modificado.
        /// </param>
        /// <param name="type">
        /// Representa sobre que monstruos va a interactuar: 1 si son los propios y 2 si son los del oponente.
        /// </param>
        /// <param name="value">
        /// Valor por el cual va a ser modificado el parámetro asocido.
        /// </param>
        public void DecrementATK(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type, int value)
        {
            if (type == 1)
                monsterSelf[position].UpdateAttack(-value);
            
            else 
                monsterEnemy[position].UpdateAttack(-value);
        }

        /// <summary>
        /// Método encargado de Incrementar los puntos de defensa del monstruo que ocupa la posición indicada.
        /// </summary>
        /// <param name="monsterSelf">
        /// Lista de monstruos que me pertenecen.
        /// </param>
        /// <param name="monsterEnemy">
        /// Lista de monstruos que pertenecen al jugador oponente.
        /// </param>
        /// <param name="position">
        /// Posicion que ocupa el monstruo enla lista que desea ser modificado.
        /// </param>
        /// <param name="type">
        /// Representa sobre que monstruos va a interactuar: 1 si son los propios y 2 si son los del oponente.
        /// </param>
        /// <param name="value">
        /// Valor por el cual va a ser modificado el parámetro asocido.
        /// </param>
        public void IncrementDEF(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type, int value)
        {
            if (type == 1)
                monsterSelf[position].UpdateDeffense(value);
            
            else 
                monsterEnemy[position].UpdateDeffense(value);
        }

        /// <summary>
        /// Método encargado de Decrementar los puntos de defensa del monstruo que ocupa la posición indicada.
        /// </summary>
        /// <param name="monsterSelf">
        /// Lista de monstruos que me pertenecen.
        /// </param>
        /// <param name="monsterEnemy">
        /// Lista de monstruos que pertenecen al jugador oponente.
        /// </param>
        /// <param name="position">
        /// Posicion que ocupa el monstruo enla lista que desea ser modificado.
        /// </param>
        /// <param name="type">
        /// Representa sobre que monstruos va a interactuar: 1 si son los propios y 2 si son los del oponente.
        /// </param>
        /// <param name="value">
        /// Valor por el cual va a ser modificado el parámetro asocido.
        /// </param>
        public void DecrementDEF(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type, int value)
        {
            if (type == 1)
                monsterSelf[position].UpdateDeffense(-value);
            
            else 
                monsterEnemy[position].UpdateDeffense(-value);
        }

        /// <summary>
        /// Método encargado de obtener el tipo de elemento del monstruo que ocupa la posición indicada.
        /// </summary>
        /// <param name="monsterSelf">
        /// Lista de monstruos que me pertenecen.
        /// </param>
        /// <param name="monsterEnemy">
        /// Lista de monstruos que pertenecen al jugador oponente.
        /// </param>
        /// <param name="position">
        /// Posicion que ocupa el monstruo enla lista que desea ser modificado.
        /// </param>
        /// <param name="type">
        /// Representa sobre que monstruos va a interactuar: 1 si son los propios y 2 si son los del oponente.
        /// </param>
        /// <param name="value">
        /// Valor por el cual va a ser modificado el parámetro asocido.
        /// </param>
        public string GetType(List<MonsterCard> monsterSelf, List<MonsterCard> monsterEnemy, 
            int position, int type)
        {
            if (type == 1)
                return monsterSelf[position].Type.ToString();
            
            return monsterEnemy[position].Type.ToString();
        }
    }
}