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

	 public class Tower : Card
{
		//fields
		protected double fireRate;
		protected int damage;
		protected int range;
		protected int currentDuration;
		protected int maxDuration;
		protected Texture2D texture;
		protected Rectangle rect;
		protected Rectangle circle;

		//properties
		public double FireRate { get{ return fireRate; } }
		public int Damage { get{ return damage; } }
		public int Range { get{ return range; } }
		public int CurrentDuration { get{ return currentDuration; } set { currentDuration = value; } }
		public int MaxDuration { get{ return maxDuration; } }
		public int X { get { return rect.X; } set { rect.X = value; } }
		public int Y { get { return rect.Y; } set { rect.Y = value; } }
		public Texture2D TowerTexture { get { return texture; } }


		//constructor
		public Tower(double fr, int dmg, int rng, int curDur, int maxDur, int x, int y, Texture2D texture) : base ()//(null, 2, x, y)
		{
			this.fireRate = fr;
			this.damage = dmg;
			this.range = rng;
			this.currentDuration = maxDur;
			this.maxDuration = maxDur;
			rect = new Rectangle(x, y, 40, 40);
			this.texture = texture;

			this.circle = new Rectangle(x - rng, y - rng, rng*2, rng*2);
		}

		//methods

		//Enemy in Rnage method
		//Purpose: To deal damage to the enemies if they are in range
		//Restrictions: accepts a list of enemies
		//No return value
		public virtual void EnemyInRange(List<Enemy> enemies)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
				//Detecting if the distance between the two entities is less than their combined radii
				if (Math.Sqrt((Math.Pow((enemies[i].X) - (circle.X + range), 2)) + Math.Pow((enemies[i].Y) - (circle.Y + range), 2)) < (range + enemies[i].Width))
				{
					enemies[i].Health -= damage;
					break;
				}
			}
			
		}


		//Draw method
		//Purpose: Draw the tower, duh
		//Restrictions: accepts a spritebatch
		//No return values.
		public void Draw(SpriteBatch sb)
		{ 

			sb.Draw(texture, rect, Color.White);

        }
		
		//Spells methods -----Not sure if we're doing 
		//Reset method
		//Purpose: Reset the duration to max
		//Restrictions: Not really, pretty basic
		//No return values

	}
}
