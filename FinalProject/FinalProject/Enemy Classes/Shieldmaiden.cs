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
        private Rectangle rect;
        private int[] lastPos;
        private int[] targetPos = new int[] { -1, -1 };
        private Texture2D texture;

        public Shieldmaiden(Rectangle rect, Texture2D texture)
            : base(rect, texture, 50, 1)
        {

            damage = Damage;

        }

    }
}
