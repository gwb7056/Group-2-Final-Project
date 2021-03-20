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
		private Rectangle rect;

		//properties
		public int FireRate { get{ return fireRate; } }
		public int Damage { get{ return damage; } }
		public int Range { get{ return range; } }
		public int CurrentDuration { get{ return currentDuration; } set { currentDuration = value; } }
		public int MaxDurration { get{ return maxDuration; } }
		public int X { get { return rect.X; } set { rect.X = value; } }
		public int Y { get { return rect.Y; } set { rect.Y = value; } }

		//constructor
		public Tower(int fr, int dmg, int rng, int curDur, int maxDur, int x, int y){
			this.fireRate = fr;
			this.damage = dmg;
			this.range = rng;
			this.currentDuration = curDur;
			this.maxDuration = maxDur;
			rect = new Rectangle(x, y, 40, 40);
		}

		//methods


		//*************************Make a circle, have tower attack the enemy closest to the gate & in circle,
		//*************************So just add circle colision with detection and utilize that.	
		//Damage method
		//Purpose: To deal damage to the enemies
		//
		public void DealDamage(Enemy enemy)
        {

        }



	}
}
