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
    class Sniper_Tower :Tower
    {
        //constructor
        public Sniper_Tower() : base() { }

        public Sniper_Tower(int x, int y, Texture2D texture) : base(4, 15, 280, 30, 30, x, y, texture) { }
      
    }
}
