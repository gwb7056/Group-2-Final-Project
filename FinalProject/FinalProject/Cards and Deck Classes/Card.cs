using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//****TO BE IMPLEMENTED LATER****
//Author: Liam Alexiou
//Purpose: To provide methods to be called on the individual cards.
//Restrictions: Nah

namespace FinalProject 
{
    //Liam Calls dibs on coding this, maybe even the Deck class so its easier, final choice is up to everyone else.
    public class Card 
    {
        //fields
        protected Texture2D cardTexture;
        protected Rectangle cardPosition;
        protected int manaCost;

        //properties

        public Texture2D CardTexture
        {
            get
            {
                return cardTexture;
            }
            set
            {
                cardTexture = value;
            }
        }
        public Rectangle CardPosition
        {
            get
            {
                return cardPosition;
            }
        }
        public int PosX
        {
            get
            {
                return cardPosition.X;
            }
            set
            {
                cardPosition.X = value;
            }
        }

        public int PosY
        {
            get
            {
                return cardPosition.Y;
            }
            set
            {
                cardPosition.Y = value;
            }
        }

        public int ManaCost
        {
            get
            {
                return manaCost;
            }
            set
            {
                manaCost = value;
            }
        }

        //constructor
        public Card(int x, int y)
        {
            cardPosition = new Rectangle(x, y, 50, 50);
            manaCost = 1;
        }
        /*public Card(Spell spell, int xPos, int yPos)
        {
            this.cardTexture = spell.Texture;
            this.cardRect = new Rectangle(xPos, yPos, 30, 80)
        }*/

        //methods
        //Draw method
        //Purpose: Draw the card, duh
        //Restrictions: accepts a spritebatch
        //No return values.
        public void DrawCard(SpriteBatch sb)
        {

            sb.Draw(cardTexture, cardPosition, Color.White);

        }
    }

}
