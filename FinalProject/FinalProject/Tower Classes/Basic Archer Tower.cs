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
    class Basic_Archer_Tower : Tower
    {
        //constructor
        public Basic_Archer_Tower(int x, int y, Texture2D texture):base(0.5, 5, 70, 30, 30, 0.5, x, y, texture)
        {

        }
    }
}
