using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Robot_Tank
{
    class Bullets
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 origin;
        public Vector2 center;
        public float radius;

        public bool isVisible;
        public Bullets(Texture2D newTexture)
        {
            texture = newTexture;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            center = new Vector2(position.X + origin.X, position.Y + origin.Y);
            radius = texture.Width / 2;
            isVisible = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
                spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0);

        }        
    }
}
