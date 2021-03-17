using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
    abstract class Entity
    {
        Rectangle boundingBox;
        Texture2D texture;

        public Entity(Rectangle rect, Texture2D _texture)
        {
            boundingBox = rect;
            texture = _texture;
        }

        public Entity(int x, int y, int width, int height, Texture2D _texture)
        {
            texture = _texture;
            boundingBox = new Rectangle(x, y, width, height);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, boundingBox, Color.White);
        }
    }
}
