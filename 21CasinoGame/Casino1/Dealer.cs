﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Casino1
{
    public class Dealer
    {
        public string name { get; set; }
        public Deck Deck { get; set; }
        public int Balance { get; set; }
        public void Deal(List<Card> Hand)
        {
            Hand.Add(Deck.Cards.First());

            string card = string.Format((Deck.Cards.First().ToString() + "\n"));
            Console.WriteLine(card);
            using (StreamWriter file = new StreamWriter(@"C:\Users\mckay\OneDrive\Documents\GitHub\C-Sharp Projects\Basic_C#_Programs\log.txt", true))
            {
                file.WriteLine(DateTime.Now);
                file.WriteLine(card);
            }
            
            Deck.Cards.RemoveAt(0);
        }
    }
}
