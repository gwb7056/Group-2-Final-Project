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
        private Rectangle rect;
        private int[] lastPos;
        private int[] targetPos = new int[] { -1, -1 };
        private Texture2D texture;

        public Ninja(Rectangle rect, Texture2D texture)
            : base(rect, texture, 10, 50)
        {

            damage = Damage * 4;

        }

    }
}
