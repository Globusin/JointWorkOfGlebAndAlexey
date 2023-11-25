using System;
using System.Collections.Generic;

namespace Obuchanik
{
    [Serializable]
    public class Test
    {
        public Test() { }

        public string nameTest;

        public Status statusTest = Status.Passed;

        public List<Card> cards = new List<Card>();

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        [Serializable]
        public enum Status
        {
            Passed,
            Failed,
            Incomplete,
        }
    }
}