using System;
using System.Collections.Generic;
using System.Collections;

namespace SujinsCards
{
    public class Deck<TCard> : ICard, IEnumerable<TCard>
    {
        private List<TCard> deck;

        public Deck()
        {
            this.deck = new List<TCard>();
        }

        public int Length => this.deck.Count;

        public bool Empty => this.Length == 0;

        public void Add(TCard card) => this.deck.Add(card);

        public bool Exisits(TCard card) => this.deck.Contains(card);
        
        public void Remove(TCard card) => this.deck.Remove(card);

        public void RemoveAt(int index) => this.deck.RemoveAt(index);

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