using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


/// <summary>
/// Kylian Hervet
/// Purpose: Create the slow moving enemy: Swordman
/// </summary>

namespace FinalProject
{
    class Swordman : Enemy
    {

        private double damage;

        public Swordman(Rectangle rect, Texture2D texture)
            : base(rect, texture, 10, 2.5)
        {

            damage = Damage * 2;

        }

    }
}
