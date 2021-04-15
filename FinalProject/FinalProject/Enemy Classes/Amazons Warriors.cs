using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


/// <summary>
/// Kylian Hervet
/// Purpose: Create the fast moving enemy: Amazon Warrior
/// </summary>

namespace FinalProject.Enemy_Classes
{
    class Amazons_Warriors : Enemy
    {

        private double damage;
        private Rectangle rect;
        private int[] lastPos;
        private int[] targetPos = new int[] { -1, -1 };
        private Texture2D texture;

        public Amazons_Warriors(Rectangle rect, Texture2D texture)
            : base( rect, texture, 20, 10)
        {

            damage = Damage * 4;

        }



    }
}
