using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


//Author: Liam Alexiou
//Purpose: To provide methods to be called on the individual cards.
//Restrictions: Nah

namespace FinalProject 
{
    
    public class Card 
    {
        //fields
        protected Texture2D texture;
        protected Rectangle position;
        protected int cost;

        //properties

        public Texture2D Texture 
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }
        public Rectangle Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        public int PosX
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
            }
        }

        public int PosY
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
            }
        }

        public int ManaCost
        {
            get
            {
                return cost;
            }
            set
            {
                cost = value;
            }
        }

        public Card()
        {

        }
        //constructor
        public Card(Texture2D texture, int cost, int x, int y)
        {
            this.texture = texture;
            this.cost = cost;
            position = new Rectangle(x, y, 50, 50);
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

            sb.Draw(texture, position, Color.White);

        }
    }

}
