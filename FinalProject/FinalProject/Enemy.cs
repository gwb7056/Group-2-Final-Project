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

        // Properties
        public int Health { get { return health; } set { health = value; } }

        public int Speed { get { return speed; } set { speed = value; } }

        //Placeholder constructor
        public Enemy(Rectangle rect, Texture2D _texture, int health, int speed) : base(rect, _texture) 
        {

            this.health = health;
            this.speed = speed;

        }

    }

}
