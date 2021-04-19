using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
    //Author: Liam Alexiou
    //Purpose: To be the parent class, thus organizing all of the things something needs to be a spell.
    //Restrictions: Not really, as the spells should have a lot of functions.
    public class Spells : Card
    {
        //fields
        private double manaCost;
        private Texture2D texture;

        //properties
        public double ManaCost { get { return manaCost; } }
        public Texture2D Texture { get { return texture; } }

        //constructor
        public Spells(double mana, Texture2D texture) : base(default, default)
        {
            this.manaCost = mana;
            this.texture = texture;
        }
        //--------Add some way of determining if a tower is being selected for the spell? Idk

        //No draw method, because we're drawing the cards, no spells have to be drawn.
    }
}
