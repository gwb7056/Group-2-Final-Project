using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalProject 
{
    /// <summary>
    /// This class was made by Lance Noble. 
    /// It has a texture, position, color, health count, and mana count. 
    /// Player.cs creates the player in the game (the objective at the end of the path that the player will be defending the enemies from). 
    /// </summary>
    class Player 
    {
        // Player Properties: 
        Texture2D playerTexture;
        Rectangle playerPosition;
        Color color;
        int health;
        int currentHealth;
        int previousHealth;
        int mana;

        /// <summary>
        /// This is a constructor that initializes the Player's properties when it is first instantiated.
        /// </summary>
        /// <param name="playerTexture">The texture that the Player will be represented as</param>
        /// <param name="x">The X Coordinate (in pixels) that the Player will be placed in on the game screen</param>
        /// <param name="y">The Y Coordinate (in pixels) that the Player will be placed in on the game screen</param>
        /// <param name="width">How fat the player is</param>
        /// <param name="height">How tall the player is</param>
        public Player(Texture2D playerTexture, int x, int y, int width, int height)
        {
            this.playerTexture = playerTexture;
            playerPosition = new Rectangle(x, y, width, height);
            health = 100;
            mana = 30;
        }

        /// <summary>
        /// This is a property that gets and sets the Player's mana.
        /// </summary>
        public int Mana { get { return mana; } set { mana = value; } }
        
        /// <summary>
        /// This is a property that gets and sets the Player's health.
        /// </summary>
        public int Health { get { return health; } set { health = value; } }
   
        /// <summary>
        /// This is a property sets the Player's position on the board.
        /// </summary>
        public Rectangle PlayerPosition { get { return playerPosition; } set { playerPosition = value; } }

        /// <summary>
        /// This is a method that allows the Player to take damage.
        /// </summary>
        /// <param name="enemies">The list of enemies that are to damage the player</param>
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
        /// Draws the Player on the board
        /// </summary>
        /// <param name="sb">sprite batch to draw from</param>
        /// <param name="color">color to draw the player</param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(playerTexture, playerPosition, color);
        }
    }
}
