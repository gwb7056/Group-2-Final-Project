using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
    //Author: Liam Alexiou
    //Purpose: To create/define a specific tower
    //Restrictions: Calls upon the parent class.
    class Mortar_Tower :Tower
    {
        //fields
        private Rectangle circleAOE;

        //constructor
        public Mortar_Tower(int x, int y, Texture2D texture):base(0.12, 10, 30, 55, 55, x, y, texture)
        {

        }

        //override of Enemy---------unfinished
        public override bool EnemyInRange(Enemy enemy)
        {
            return base.EnemyInRange(enemy);
        }
    }
}
