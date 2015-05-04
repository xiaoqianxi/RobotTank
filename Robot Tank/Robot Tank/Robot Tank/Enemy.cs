using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Robot_Tank
{
    public class Enemy
    {
        Texture2D texture;

        public Vector2 position = Vector2.Zero;
        public Vector2 velocity;
        public Vector2 origin;
        public Vector2 center;
        public float radius;
        public float rotation;
        bool alive;
        public bool isMove;
        bool isShoot;

        List<Bullets> bullets = new List<Bullets>();
        Texture2D bulletTex;
        public Explosion e = new Explosion();
        
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        public void LoadContent(ContentManager c, String imageName, Texture2D newBulletTex)
        {
            bulletTex = newBulletTex;
            texture = c.Load < Texture2D>(imageName);
            radius = texture.Width / 2;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            center = new Vector2(position.X + origin.X, position.Y + origin.Y);
            Alive = true;
            isMove = false;
            isShoot = false;
        }

        public void UpdateBullets()
        {
            foreach (Bullets bullet in bullets)
            {
                bullet.position += bullet.velocity;
                double range = Math.Sqrt((bullet.position.X - position.X) * (bullet.position.X - position.X)
                                 + (bullet.position.Y - position.Y) * (bullet.position.Y - position.Y));
                if (range > 800 || isShoot == true)
                    bullet.isVisible = false;

            }
            for (int i = 0; i < bullets.Count; i++)
                if(!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            if (isShoot == true)
                isShoot = false;
        }

        public void shootBullets()
        {
            Bullets newBullet = new Bullets(bulletTex);
            newBullet.velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 4f + velocity;
            newBullet.position = position + newBullet.velocity * 5;

            newBullet.isVisible = true;
            if (bullets.Count() < 3)
                bullets.Add(newBullet);

        }

        float shoot = 0;
        public void Update(GameTime gameTime, Player player)
        {
            Vector2 direction = player.tankPos - position;
            rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);

            const double radius = 400;
            const double range = 200;
            float friction = 0.1f;
            double distance = Math.Sqrt((position.X - player.tankPos.X) * (position.X - player.tankPos.X)
                                  + (position.Y - player.tankPos.Y) * (position.Y - player.tankPos.Y));
            if (distance < radius)
            {
                float speed = 1f;
                position = velocity + position;
                velocity.X = (float)Math.Cos(rotation) * speed;
                velocity.Y = (float)Math.Sin(rotation) * speed;
                isMove = true;
            }
            else if (velocity != Vector2.Zero)
            {
                Vector2 v = new Vector2(velocity.X, velocity.Y);
                velocity.X = v.X - friction * v.X;
                velocity.Y = v.Y - friction * v.Y;
            }
            
            if (distance < range && isMove)
            {
                velocity = Vector2.Zero;
                isMove = false; 
            }

            //shoot bullets
            shoot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (shoot > 1)
            {
                shoot = 0;
                if (distance < range && Alive && player.Alive)
                    shootBullets();
            }
            UpdateBullets();
            
            //destroy player
            foreach (Bullets bullet in bullets)
            {
                if (Vector2.Distance(bullet.position, player.tankPos) <= bullet.radius + player.tankRadius && player.Alive)
                {
                    if (player.lives >0 )
                    {
                        player.health -= 10;
                        player.lives -= 1;
                        isShoot = true;
                    }
                }

                if (player.lives == 0)
                {
                    player.Alive = false;
                }      
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Bullets bullet in bullets)
                bullet.Draw(spriteBatch);

            if (!alive) return;
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 1);
        }        
    }
}
