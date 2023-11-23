using System;
using System.Collections.Generic;

namespace Obuchanik
{
    [Serializable]
    public class Test
    {
        public string nameTest;

        public bool statusTest;

        public List<Card> cards = new List<Card>();

        public void AddCard(Card card)
        {
            cards.Add(card);
        }
    }
}