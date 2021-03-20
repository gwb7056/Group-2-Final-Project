using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject 
{
    /*abstract*/ public class Tower
    {
        //tower properties
        Texture2D towerTexture;
        Texture2D towerBulletTexture;
        Rectangle towerBulletBox;
        Rectangle towerPosition;
        int damage;

        public Tower(Texture2D towerTexture, Texture2D towerBulletTexture, Rectangle towerBullet)
        {
            damage = 5;
        }
    }
}
