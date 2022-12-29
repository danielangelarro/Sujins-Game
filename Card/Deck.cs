using System;
using System.Collections.Generic;
using System.Collections;

namespace SujinsCards
{
    ///<summary>
    /// Contenedor para guardar las cartas de los jugadores.
    ///</summary>
    public class Deck<TCard> : IEnumerable<TCard>
    {
        private List<TCard> deck;

        public Deck()
        {
            this.deck = new List<TCard>();
        }

        /// <summary>
        /// Devuelve la cantidad de cartas que contiene el deck
        /// </summary>
        public int Length => this.deck.Count;

        /// <summary>
        /// Verifica si el mazo de cartas esta vacio.
        /// </summary>
        public bool Empty => this.Length == 0;

        /// <summary>
        /// Añade una carta al mazo del jugador
        /// </summary>
        public void Add(TCard card) => this.deck.Add(card);

        /// <summary>
        /// Comprueba si una carta existe ya en el mazo
        /// </summary>
        public bool Exisits(TCard card) => this.deck.Contains(card);
        
        /// <summary>
        /// Remeuve una carta del mazo especificando la carta que se desea eliminar
        /// </summary>
        public void Remove(TCard card) => this.deck.Remove(card);

        /// <summary>
        /// Remeuve una carta del mazo especificando la carta que se desea eliminar
        /// </summary>
        /// <param value="index">
        /// indice que ocupa la carta que se desea eliminar.
        /// </param>
        public void RemoveAt(int index) => this.deck.RemoveAt(index);

        /// <summary>
        /// Devuelve cartas al azadr del mazo.
        /// </summary>
        public IEnumerator<TCard> GetEnumerator()
        {
            Random rnd = new Random();
            int position;

            while (true)
            {
                position = rnd.Next() % this.Length;

                yield return this.deck[position];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}