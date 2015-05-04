using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Robot_Tank
{
    class Radar
    {
        private Texture2D PlayerDotImage;
        private Texture2D EnemyDotImage;
        private Texture2D RadarImage;

        // radar center
        private Vector2 RadarImageCenter;

        // Range of radar
        private const float RadarRange = 700.0f;

        // Radius of radar circle on the screen
        private const float RadarScreenRadius = 50.0f;

        // Center position of the radar on the screen. 
        static Vector2 RadarCenterPos = new Vector2(700, 60);

        public Radar(ContentManager Content, string playerDotPath, string enemyDotPath, string radarImagePath)
        {
            PlayerDotImage = Content.Load<Texture2D>(playerDotPath);
            EnemyDotImage = Content.Load<Texture2D>(enemyDotPath);
            RadarImage = Content.Load<Texture2D>(radarImagePath);

            RadarImageCenter = new Vector2(RadarImage.Width / 2.0f, RadarImage.Height / 2.0f);
        }

        public void Draw(SpriteBatch spriteBatch, Player player, Game1 game)
        {
            // The last parameter of the color determines how transparent the radar circle will be
            spriteBatch.Draw(RadarImage, RadarCenterPos, null, new Color(100, 100, 100, 150), 0.0f,
                RadarImageCenter, 2 * (RadarScreenRadius / RadarImage.Height), SpriteEffects.None, 0.0f);

            // If enemy is in range
            for(int i=0; i<10; i++)
            {
                Vector2 diffVect = game.enemy0[i].position - player.tankPos;
                float distance = diffVect.Length();

                // Check if enemy is within RadarRange
                if (distance < RadarRange && game.enemy0[i].Alive)
                {
                    // Scale the distance from world coords to radar coords
                    diffVect *= RadarScreenRadius / RadarRange;

                    // Offset coords from radar's center
                    diffVect += RadarCenterPos;

                    // Draw enemy on radar
                    spriteBatch.Draw(EnemyDotImage, diffVect, Color.White);
                }

                Vector2 diffVect1 = game.enemy1[i].position - player.tankPos;
                float distance1 = diffVect1.Length();

                // Check if enemy is within RadarRange
                if (distance1 < RadarRange && game.enemy1[i].Alive)
                {
                    // Scale the distance from world coords to radar coords
                    diffVect1 *= RadarScreenRadius / RadarRange;

                    // Offset coords from radar's center
                    diffVect1 += RadarCenterPos;

                    // Draw enemy dot on radar
                    spriteBatch.Draw(EnemyDotImage, diffVect1, Color.White);
                }

                Vector2 diffVect2 = game.enemy2[i].position - player.tankPos;
                float distance2 = diffVect2.Length();

                // Check if enemy is within RadarRange
                if (distance2 < RadarRange && game.enemy2[i].Alive)
                {
                    // Scale the distance from world coords to radar coords
                    diffVect2 *= RadarScreenRadius / RadarRange;

                    // Offset coords from radar's center
                    diffVect2 += RadarCenterPos;

                    // Draw enemy dot on radar
                    spriteBatch.Draw(EnemyDotImage, diffVect2, Color.White);
                }

                Vector2 diffVect3 = game.enemy3[i].position - player.tankPos;
                float distance3 = diffVect3.Length();

                // Check if enemy is within RadarRange
                if (distance3 < RadarRange && game.enemy3[i].Alive)
                {
                    // Scale the distance from world coords to radar coords
                    diffVect3 *= RadarScreenRadius / RadarRange;

                    // Offset coords from radar's center
                    diffVect3 += RadarCenterPos;

                    // Draw enemy dot on radar
                    spriteBatch.Draw(EnemyDotImage, diffVect3, Color.White);
                }

                Vector2 diffVect4 = game.enemy4[i].position - player.tankPos;
                float distance4 = diffVect4.Length();

                // Check if enemy is within RadarRange
                if (distance4 < RadarRange && game.enemy4[i].Alive)
                {
                    // Scale the distance from world coords to radar coords
                    diffVect4 *= RadarScreenRadius / RadarRange;

                    // Offset coords from radar's center
                    diffVect4 += RadarCenterPos;

                    // Draw enemy dot on radar
                    spriteBatch.Draw(EnemyDotImage, diffVect4, Color.White);
                }

            }

            // Draw player on radar
            if(player.Alive)
                spriteBatch.Draw(PlayerDotImage, RadarCenterPos, Color.White);
        }
    }
}
