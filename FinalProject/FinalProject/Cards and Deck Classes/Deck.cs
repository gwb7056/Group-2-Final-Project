using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


//Author: Liam Alexiou
//Purpose: To organize the cards and provide apropriate methods
//Restrictions: Not really any.

namespace FinalProject 
{

    class Deck 
    {
        //fields
        private List<Card> deck = new List<Card>();
        private List<Card> hand;
        private List<Card> discard;
        private Texture2D texture;

        public Deck()
        {
            /*for (int index = 0; index < 3; index++)
            {
                deck.Add(new Basic_Archer_Tower(0, 400, texture));
            }*/
            for (int index = 0; index < 3; index++)
            {
                deck.Add(new Card(400, 0));
            }
            for (int index = 0; index < 3; index++)
            {
                deck.Add(new Card(400,0));
            }
            /*for (int index = 0; index < 3; index++)
            {
                deck.Add(new Sniper_Tower(0, 400, texture));
            }
            for (int index = 0; index < 3; index++)
            {
                deck.Add(new Wizard_Tower(0, 400, texture));
            }
            for (int index = 0; index < 3; index++)
            {
                deck.Add(new Backup(texture));
            }
            for (int index = 0; index < 3; index++)
            {
                deck.Add(new Ctrl_Alt_Del(texture));
            }*/
        }

        public void DrawDeck(SpriteBatch sb)
        {
            foreach (Card card in deck)
            {
                card.DrawCard(sb);
            }
        }
        //properties
        //
        public List<Card> Cards
        {
            get
            {
                return deck;
            }
        }

        public Texture2D Texture
        {
            set
            {
                texture = value;
            }
        }





        //methods
        //Draw Hand method -- can't be called "Draw"
        //Purpose: Add a card from the deck to the hand;
        //Restrictions: ///////////////
        //No return values;
        public void DrawToHand()
        {
            hand.Add(deck[0]);
            //----------------could add more code to this method,
            deck.RemoveAt(0);
        }


        //Play Card method
        //Purpose: To play the card, adds it from hand to discard afterwards.
        //Restrictions: No clue
        //No return values.
        public void PlayCard(Card card)
        {
            discard.Add(card);
            //----------------could add more code to this method,
            hand.Remove(card);
        }

        //ReShuffle method
        //Purpose: When the deck is empty, reshuffle, -- handles if deck is empty elsewhere for less confusion
        //Restrictions: Should only be run when the deck is empty
        //No return values;
        public void ReShuffle()
        {
            List<Card> temporaryDeck = new List<Card>();
            Random myRNG = new Random(); //----For determining at random which card to move (so it actually shuffles)
            int control = 1; //----never changes, as discard.Count does change

            //while loop 
            while (control < discard.Count)
            {
                int temp = myRNG.Next(0, discard.Count);
                temporaryDeck.Add(discard[temp]);
                discard.RemoveAt(temp);
            }

            deck = temporaryDeck;
        }


        //spell methods, spells will just call these methods
        //Shuffle method
        //Purpose: TO shuffle the deck
        //Restrictions: ---ONLY--- operates on deck list,
        //No return values
        public void Shuffle()
        {
            List<Card> temporaryDeck = new List<Card>();
            Random myRNG = new Random(); //----For determining at random which card to move (so it actually shuffles)
            int control = 1; //----never changes, as deck.Count does change

            while (control < deck.Count)
            {
                int temp = myRNG.Next(0, deck.Count);
                temporaryDeck.Add(deck[temp]);
                deck.RemoveAt(temp);
            }

            deck = temporaryDeck;
        }
    }

}

