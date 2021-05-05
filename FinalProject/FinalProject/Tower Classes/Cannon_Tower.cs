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
    public class Cannon_Tower:Tower
    {
        //fields
        private Rectangle circleAOE;
        private List<Enemy> enemiesOnBoard;
        private int rangeAOE;

        //constructor

        public Cannon_Tower() : base() { }
      
        public Cannon_Tower(int x, int y, Texture2D texture) : base(2, 20, 140, 30, 30, x, y, texture)
        {
            this.rangeAOE = 5;
        }

        //override of EnemyInRange
        //Purpose: To attack the enemies in range, and deal damage based on an AOE
        //Accepts a list of enemies
        //No return value
        public override void EnemyInRange(List<Enemy> enemies)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                //Detecting if the distance between the two entities is less than their combined radii
                if (Math.Sqrt((Math.Pow((enemies[i].X) - (circle.X + range), 2)) + Math.Pow((enemies[i].Y) - (circle.Y + range), 2)) < (range + enemies[i].Width))
                {
                    this.circleAOE = new Rectangle(enemies[i].X - rangeAOE, enemies[i].Y - rangeAOE, rangeAOE, rangeAOE);

                    for (int z = 0; z < enemies.Count; z++)
                    {
                        if (Math.Sqrt((Math.Pow((enemies[z].X) - (circleAOE.X + rangeAOE), 2)) + Math.Pow((enemies[z].Y) - (circleAOE.Y + rangeAOE), 2)) < (rangeAOE + enemies[z].Width))
                        {
                            
                            enemies[z].Health -= damage;

                        }
                       
                    }
                    IsFiring = true;
                    break;
                }
                else
                {
                    IsFiring = false;
                }
            }
        }
    }
}
