using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject 
{
    /// <summary>
    /// reminder: focus on tower enemy board and player class first... cards can be  under extra goals...
    /// this game idea is way way larger than you think...
    /// Author: Lance Noble
    /// Purpose: create the player in the game
    /// </summary>
    class Player 
    {
        //fields to define the player's properties
        //our player needs mana and health so far basically and a texture and a hitbox
        Texture2D playerTexture;
        Rectangle playerPosition;
        int maxMana;
        int currentMana;
        int health;

        //Constructor to initialize the player's properties' values
        public Player(Texture2D playerTexture, int x, int y, int width, int height)
        {
            this.playerTexture = playerTexture;
            playerPosition = new Rectangle(x, y, width, height);
            maxMana = 10;
            currentMana = 10;
            health = 100;
        }

        //properties to change the player's properties' values over time
        //if you want a cards field, might wanna make an indexer property also (just a reminder)
        public int MaxMana
        {
            get
            {
                return maxMana;
            }
            set
            {
                maxMana = value;
            }
        }
        public int CurrentMana
        {
            get
            {
                return currentMana;
            }
            set
            {
                currentMana = value;
            }
        }
        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }
        public Rectangle Rectangle
        {
            get
            {
                return playerPosition;
            }
        }
        //maybe a deck property?
        
        //methods: stuff the player can do
        //can...
        //lose health
        //place towers
        //gain health
        //lose mana
        //gain max mana
        //gain mana
        //player needs to be drawn
       /// <summary>
       /// subtracts enemy damage amount from health
       /// </summary>
       /// <param name="damage">damage the enemy deals</param>
       /// <returns>remaining health</returns>
        public void LoseHealth(int damage)
        {
            health -= damage;
        }
        public void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(playerTexture, playerPosition, color);
        }
    }
}
