﻿using System;
using System.Collections.Generic;

namespace Obuchanik
{
    [Serializable]
    public enum Status
    {
        Passed,
        Failed,
        Incomplete,
    }

    [Serializable]
    public class Test
    {
        public Test() { }

        public string nameTest = null;

        public Status statusTest = Status.Incomplete;

        public List<Card> cards = new List<Card>();

        public void AddCard(Card card)
        {
            cards.Add(card);
        }
    }
}