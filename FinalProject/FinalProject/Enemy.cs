using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject {
    abstract class Enemy : Entity{

        //Placeholder constructor
        public Enemy(Rectangle rect, Texture2D _texture) : base(rect, _texture) {

        }
    }
}
