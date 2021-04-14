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
    class Card
    {
        //fields
        private Rectangle cardRect;
        private Texture2D cardTexture;

        //properties
        private Rectangle CardRect { get { return cardRect; } set { cardRect = value; } }
        private Texture2D CardTexture { get { return cardTexture; } }

        //constructor
        public Card(Tower tower, int xPos, int yPos)
        {
            //so that we can use "cardTexture" in this class rather than constantly needing a tower input
            this.cardTexture = tower.Texture;
            this.cardRect = new Rectangle(xPos, yPos, 30, 80);
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
        public void Draw(SpriteBatch sb)
        {

            sb.Draw(cardTexture, cardRect, Color.White);

        }
    }

}
