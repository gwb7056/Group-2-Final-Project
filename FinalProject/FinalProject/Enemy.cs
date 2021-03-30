using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Kylian Hervet
/// Purpose: Create a basic enemy with base health, base speed, and base damage
/// </summary>

namespace FinalProject 
{

    public class Enemy
    {

        // Field
        private int health;
        private int speed;
        private Rectangle rect;
        private int[] lastPos;
        private Texture2D texture;

        // Properties
        /// <summary>
        /// Get or Set the Health of the enemy
        /// </summary>
        public int Health { get { return health; } set { health = value; } }

        /// <summary>
        /// Get or Set speed of the enemy
        /// </summary>
        public int Speed { get { return speed; } set { speed = value; } }

        /// <summary>
        /// Get or Set the X position of the enemy
        /// </summary>
        public int X { get { return rect.X; } set { rect.X = value; } }

        /// <summary>
        /// Get or Set the Y position of the enemy
        /// </summary>
        public int Y { get { return rect.Y; } set { rect.Y = value; } }

        /// <summary>
        /// Gets the width of the enemy rectangle for collision detection with tower range
        /// </summary>
        public int Width { get { return rect.Width; } }

        /// <summary>
        /// Get or Set the last position of the enemy
        /// </summary>
        public int[] LastPos { get { return lastPos; } set { lastPos = value; } }

        /// <summary>
        /// Get the base enemy damage
        /// </summary>
        public int Damage 
        { 
            get
            {

                int damage = 5;

                return damage;

            }

        }

        //Constructor

        /// <summary>
        /// Create an enemy character 
        /// </summary>
        /// <param name="rect">Where the enemy is on screen?</param>
        /// <param name="texture">What the enemy looks like?</param>
        /// <param name="health">How much health does he have?</param>
        /// <param name="speed">How fast is he?</param>
        public Enemy(Rectangle rect, Texture2D texture, int health, int speed)
            
        {
            this.health = health;
            this.speed = speed;
            this.rect = rect;
            lastPos = new int[2] {rect.X, rect.Y};
            this.texture = texture;
        }



        /// <summary>
        /// Draw the enemy on the screen
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {

            sb.Draw(texture, rect, Color.White);

        }
    }

}
