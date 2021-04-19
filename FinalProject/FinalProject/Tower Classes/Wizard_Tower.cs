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
    class Wizard_Tower : Tower
    {
        //constructor
        public Wizard_Tower(int x, int y, Texture2D texture) : base(0.5, 15, 100, 40, 40, x, y, texture)
        {

        }


    }
}
