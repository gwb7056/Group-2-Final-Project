using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
    //Author: Liam Alexiou
    //Purpose: TO create a new tower with half the durration.
    class Backup : Spells
    {

        public Backup(Texture2D texture) : base(1, texture)
        {

        }

        //method, we think this way would be easier
        //Preform function
        //Purpose: TO do the desired task of each spell -- this one is to add a new tower with half the durration
        //Restrictions: Different for each class
        //returns a tower
        public Tower BackupTower(Tower tower)
        {
            Tower backup = new Tower(tower.FireRate, tower.Damage, tower.Range, tower.CurrentDuration/2, tower.MaxDurration,tower.X, tower.Y, tower.Texture);
            return backup;
        }

    }
}
