using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
	/// <summary>
	/// This class was made by Liam Alexiou. 
	/// It has a fire rate, a damage amount, a range, a duration, a texture, and a position. 
	/// Notice that Tower.cs has a default constructor. 
	/// This is because all the cards 
	/// The point of this class is for other tower classes of different types to inherit from it. 
	/// Those other child towers are the ones that will actually be instantiated. 
	/// </summary>
	public class Tower : Card
	{
		// Tower Properties: What every tower must have
		protected double fireRate;
		protected int damage;
		protected int range;
		protected int currentDuration;
		protected int maxDuration;
		protected Texture2D texture;
		protected Rectangle rect;
		protected Rectangle circle;
		protected bool isFiring;

		public bool IsFiring
        {
            get
            {
				return isFiring;
            }
            set
            {
				isFiring = value;
            }
        }

		public Rectangle TowerPosition
		{
			get
			{
				return rect;
			}

		}

		/// <summary>
		/// Gets the tower's fire rate
		/// </summary>
		public double FireRate { get { return fireRate; } }

		/// <summary>
		/// Gets the tower's damage
		/// </summary>
		public int Damage { get { return damage; } }

		/// <summary>
		/// Gets the tower's range
		/// </summary>
		public int Range { get { return range; } }

		/// <summary>
		/// Gets and sets the tower's current duration (how long it has left until it disappears)
		/// </summary>
		public int CurrentDuration { get { return currentDuration; } set { currentDuration = value; } }

		/// <summary>
		/// Gets the tower's max duration (the max amount of time it can last on the board)
		/// </summary>
		public int MaxDuration { get { return maxDuration; } }

		/// <summary>
		/// Gets and sets the tower's X position on the board
		/// </summary>
		public int X { get { return rect.X; } set { rect.X = value; } }

		/// <summary>
		/// Gets and sets the tower's Y position on the board
		/// </summary>
		public int Y { get { return rect.Y; } set { rect.Y = value; } }

		/// <summary>
		/// Gets the tower's texture
		/// </summary>
		public Texture2D TowerTexture { get { return texture; } }

		//constructor
		public Tower() : base() { }

		public Tower(double fr, int dmg, int rng, int curDur, int maxDur, int x, int y, Texture2D texture) : base() //(null, 2, x, y)
		{
			this.fireRate = fr;
			this.damage = dmg;
			this.range = rng;
			this.currentDuration = maxDur;
			this.maxDuration = maxDur;
			rect = new Rectangle(x, y, 40, 40);
			this.texture = texture;
			isFiring = false;
			this.circle = new Rectangle(x - rng, y - rng, rng * 2, rng * 2);
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
					IsFiring = true;
					break;
				}
                else
                {
					IsFiring = false;
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
		//
	}
}
