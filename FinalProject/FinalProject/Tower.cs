using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject 
{
    //Author: Liam Alexiou
	//Purpose: To create a stationary tower entity that damages enemy entities in passing.
	//Restrictions: N/A?

	public class Tower
{
		//fields
		private int fireRate;
		private int damage;
		private int range;
		private int currentDuration;
		private int maxDuration;
		private Texture2D texture;
		private Rectangle rect;
		private Rectangle circle;

		//properties
		public int FireRate { get{ return fireRate; } }
		public int Damage { get{ return damage; } }
		public int Range { get{ return range; } }
		public int CurrentDuration { get{ return currentDuration; } set { currentDuration = value; } }
		public int MaxDurration { get{ return maxDuration; } }
		public int X { get { return rect.X; } set { rect.X = value; } }
		public int Y { get { return rect.Y; } set { rect.Y = value; } }

		//constructor
		public Tower(int fr, int dmg, int rng, int curDur, int maxDur, int x, int y, Texture2D texture){
			this.fireRate = fr;
			this.damage = dmg;
			this.range = rng;
			this.currentDuration = curDur;
			this.maxDuration = maxDur;
			rect = new Rectangle(x, y, 40, 40);
			this.texture = texture;

			this.circle = new Rectangle(x - rng, y - rng, rng*2, rng*2);
		}

		//methods


		//*************************Make a circle, have tower attack the enemy closest to the gate & in circle,
		//*************************So just add circle colision with detection and utilize that.	
		//Enemy in Rnage method
		//Purpose: To deal damage to the enemies if they are in range
		//Restrictions: accepts an enemy entitiy
		//Returns a boolean value
		public bool EnemyInRange(Enemy enemy)
        {
			//Detecting if the distance between the two entities is less than their combined radii
			if (Math.Sqrt((Math.Pow((enemy.X) - (circle.X + range), 2)) + Math.Pow((enemy.Y) - (circle.Y + range), 2)) < (range + enemy.Width))
			{
				enemy.Health -= damage;
                return true;
			}
			else
			{
				return false;
			}
		}




		public void Draw(SpriteBatch sb) {
			sb.Draw(texture, rect, Color.White);
        }
	}
}
