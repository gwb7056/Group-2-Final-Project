using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject 
{
    /// <summary>
    /// Author: Lance Noble
    /// Purpose: creates the player in the game (the thing the towers are protecting and the enemies are attacking)
    /// </summary>
    class Player 
    {

        //player properties
        Texture2D playerTexture;
        Rectangle playerPosition;
        int health;
        int currentHealth;
        int previousHealth;
        Color color;
        int mana;

        /// <summary>
        /// initializes player properties upon object creation
        /// </summary>
        /// <param name="playerTexture">what the player looks like</param>
        /// <param name="playerHealthTexture">what the player health bar looks like</param>
        /// <param name="x">x coord of the player</param>
        /// <param name="y">y coord of the player</param>
        /// <param name="width">width of the player</param>
        /// <param name="height">height of the player</param>
        public Player(Texture2D playerTexture, int x, int y, int width, int height)
        {

            this.playerTexture = playerTexture;
            playerPosition = new Rectangle(x, y, width, height);
            health = 100;
            mana = 30;
        }
        /// <summary>
        /// gets and sets the player's mana value
        /// </summary>
        public int Mana
        {
            get
            {
                return mana;
            }
            set
            {
                mana = value;
            }
        }
        /// <summary>
        /// gets and sets the player texture's color tint
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
        /// <summary>
        /// gets and sets player health
        /// </summary>
        public int Health
        {
            get
            {

                return health;

            }
            set
            {

                health = value;

            }

        }

        /// <summary>
        /// gets and sets player rectangle
        /// </summary>
        public Rectangle PlayerPosition
        {

            get
            {

                return playerPosition;

            }
            set {
                playerPosition = value;
            }

        }


        /// <summary>
        /// Loose health points from player attack
        /// </summary>
        /// <param name="damage">Damages dealt on enemy</param>
        public void TakeDamage(List<Enemy> enemies)
        {
            previousHealth = health;
            health -= enemies.Count * 5;
            currentHealth = health;
            if (currentHealth < previousHealth)
            {
                color = Color.Red;
            }
            else
            {
                color = Color.White;
            }
        }
        /// <summary>
        /// draws the player on the board
        /// </summary>
        /// <param name="sb">sprite batch to draw from</param>
        /// <param name="color">color to draw the player</param>
        public void Draw(SpriteBatch sb)
        {

            sb.Draw(playerTexture, playerPosition, color);

        }
        /// <summary>
        /// checks whether or not the player is in the process of summoning a card
        /// </summary>
        /// <returns></returns>
        /*public bool Possession()
        {

        }*/
    }
}
