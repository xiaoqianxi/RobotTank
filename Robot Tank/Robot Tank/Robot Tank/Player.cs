using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Robot_Tank
{
    public class Player
    {
        public Texture2D tankTex;
        public Vector2 tankOrigin;
        public float tankRadius;
        public bool Alive;       
        public Rectangle tankRec;
        public Vector2 tankPos;
        float tankRotation;
        public int health;
        public int lives;

        public Vector2 tankVelocity;
        const float Speed = 2f;

        public Explosion e = new Explosion();
        public Player(int h, int l)
        {
            health = h;
            lives = l;
        }

        public void LoadContent(ContentManager c, String imageName)
        {
            tankTex = c.Load<Texture2D>(imageName);
            tankRadius = tankTex.Width / 2;
            Alive = true;
        }

        public void Update(GameTime gameTime, Game1 game)
        {
            //Tank rotation and movement
            tankRec = new Rectangle((int)tankPos.X, (int)tankPos.Y, tankTex.Width, tankTex.Height);
            tankPos = tankVelocity + tankPos;
            
            tankOrigin = new Vector2(tankTex.Bounds.Center.X, tankTex.Bounds.Center.Y);
            //gunOrigin = new Vector2(gun.Bounds.Center.X, gun.Bounds.Center.Y);
            if (!Alive)
            {
                e.Init(tankPos, new TimeSpan(0, 0, 0, 0, 500));
            }
        }

        public void CheckBounce(Game1 game)
        {
            if (tankPos.X - tankRec.Width / 2 <= 0)
            {
                tankPos.X = tankRec.Width / 2;
            }
            if (tankPos.Y - tankRec.Height / 2 <= 0)
            {
                tankPos.Y = tankRec.Height / 2;
            }
            if (tankPos.X + tankRec.Width / 2 >= game.background.Width)
            {
                tankPos.X = game.background.Width - tankRec.Width / 2;
            }
            if (tankPos.Y + tankRec.Height / 2 >= game.background.Height)
            {
                tankPos.Y = game.background.Height - tankRec.Height / 2;
            } 
        }

        public void TankRotation(string direction)
        {
            if(direction == "Right")
                tankRotation += 0.1f;
            if(direction == "Left")
                tankRotation -= 0.1f;
        }

        public void TankMovement(string direction)
        {
            if (direction == "Up")
            {
                tankVelocity.X = (float)Math.Cos(tankRotation) * Speed;
                tankVelocity.Y = (float)Math.Sin(tankRotation) * Speed;
            }
            
            if (direction == "Down")
            {
                tankVelocity.X = -(float)Math.Cos(tankRotation) * Speed;
                tankVelocity.Y = -(float)Math.Sin(tankRotation) * Speed;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tankTex, tankPos, null, Color.White, tankRotation, tankOrigin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
