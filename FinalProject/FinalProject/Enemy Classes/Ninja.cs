using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


/// <summary>
/// Kylian Hervet
/// Purpose: Create the fast moving enemy: Ninja Warrior
/// </summary>

namespace FinalProject.Enemy_Classes
{
    class Ninja : Enemy
    {

        private double damage;

        public Ninja(Rectangle rect, Texture2D texture)
            : base(rect, texture, 20, 5.5)
        {

            damage = Damage * 4;

        }

    }
}
