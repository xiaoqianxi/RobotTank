using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Robot_Tank
{
    public class Explosion
    {
        public static Texture2D myTexture;

        // Set the coordinates to draw the sprite at.  
        public Vector2 position = Vector2.Zero;

        Vector2 origin;

        bool alive;
        TimeSpan timeToLive;
        float blend;
        float scale;

        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        public void Init(Vector2 pos, TimeSpan tTL)
        {
            DateTime a = DateTime.Now;

            position = pos;
            timeToLive = tTL;

            origin = new Vector2(myTexture.Width / 2, myTexture.Height / 2);
            Alive = true;
        }

        public void UpdateSprite(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            if (!alive) return;
            // Move the sprite by speed, scaled by elapsed time.    
            DateTime a = DateTime.Now;

            timeToLive = timeToLive.Subtract(gameTime.ElapsedGameTime);
            if (timeToLive.CompareTo(TimeSpan.Zero) < 0)
            {
                alive = false;
            }

            blend = (float)timeToLive.TotalMilliseconds / 500;
            scale = (1000 - (float)timeToLive.TotalMilliseconds) / 1000;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!alive) return;
            Color c = new Color(blend, blend, blend, blend);
            spriteBatch.Draw(myTexture, position, null, c, 0, origin, scale, SpriteEffects.None, 0.5f);
        }
    }
}
