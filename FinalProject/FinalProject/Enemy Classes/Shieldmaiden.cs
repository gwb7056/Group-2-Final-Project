using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


/// <summary>
/// Kylian Hervet
/// Purpose: Create the slow moving enemy: Shieldmaiden
/// </summary>


namespace FinalProject.Enemy_Classes
{
    class Shieldmaiden : Enemy
    {

        private double damage;

        public Shieldmaiden(Rectangle rect, Texture2D texture)
            : base(rect, texture, 50, 1)
        {

            damage = Damage;

        }

    }
}
