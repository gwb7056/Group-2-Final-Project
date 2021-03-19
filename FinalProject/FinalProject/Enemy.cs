using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Kylian Hervet
/// </summary>

namespace FinalProject 
{

    class Enemy : Entity
    {

        // Field
        private int health;
        private int speed;
        private Rectangle rect;
        private int[] lastPos;
        private Texture2D texture;

        // Properties
        public int Health { get { return health; } set { health = value; } }

        public int Speed { get { return speed; } set { speed = value; } }

        public int X { get { return rect.X; } set { rect.X = value; } }
        public int Y { get { return rect.Y; } set { rect.Y = value; } }
        public int[] LastPos {
            get {
                return lastPos;
            }
            set {
                lastPos = value;
            }
        }

        //Placeholder constructor
        public Enemy(Rectangle rect, Texture2D texture, int health, int speed)
        {
            this.health = health;
            this.speed = speed;
            this.rect = rect;
            lastPos = new int[2] {rect.X, rect.Y};
            this.texture = texture;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, rect, Color.White);
        }
    }

}
