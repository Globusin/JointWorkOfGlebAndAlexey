using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obuchanik
{
    [Serializable]
    public class Test
    {
        public string NameTest;

        List<Card> cards = new List<Card>();

        public void AddCard(Card card)
        {
            cards.Add(card);
        }
    }
}