using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
    //Author: Liam Alexiou
    //Purpose: Resets a tower's duration
    class Ctrl_Alt_Del : Spells
    {
        //potentially code here to check if at a valid position

        public Ctrl_Alt_Del(Texture2D texture):base(1, texture)
        {

        }

        //method, we think this way would be easier
        //Preform function
        //Purpose: TO do the desired task of each spell --- this one is to reset the durration 
        //Restrictions: Different for each class
        //No return values
        public void ResetDur(Tower tower)
        {
            tower.CurrentDuration = tower.MaxDuration;
        }

    }
}
