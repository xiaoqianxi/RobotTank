using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Robot_Tank
{
    class GUI
    {
        SpriteFont font;
        SpriteFont scoreBoardTexFont;
        Vector2 origin;
        public void LoadContent(ContentManager content, String imageName1, String imageName2) 
        {
            font = content.Load<SpriteFont>(imageName1);
            scoreBoardTexFont = content.Load<SpriteFont>(imageName2);
            origin = new Vector2(0, 0);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Game1 game, Player player)
        {
            spriteBatch.DrawString(font, "Score: " + game.score, new Vector2(30, 50), Color.Red, 0, origin, 0.4f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Lives: " + player.lives, new Vector2(30, 20), Color.Red, 0, origin, 0.4f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Enemies Left:" + game.aliveCount, new Vector2(350, 20), Color.Red, 0, origin, 0.4f, SpriteEffects.None, 1.0f);
          
        }

        public void DrawScoreBoard(SpriteBatch spriteBatch, Game1 game, Player player)
        {
            spriteBatch.DrawString(scoreBoardTexFont, "Your Score: " + game.score, new Vector2(350, 300), Color.Red, 0, origin, 0.8f, SpriteEffects.None, 1.0f);
           
            if (game.aliveCount == 0)
                spriteBatch.DrawString(scoreBoardTexFont, "You Win!", new Vector2(350, 240), Color.Yellow, 0, origin, 1.0f, SpriteEffects.None, 1.0f);
            else if (!player.Alive)
                spriteBatch.DrawString(scoreBoardTexFont, "Game Over!", new Vector2(350, 240), Color.Yellow, 0, origin, 1.0f, SpriteEffects.None, 1.0f);

            spriteBatch.DrawString(scoreBoardTexFont, "TAB to back to Menu.", new Vector2(350, 350), Color.Yellow, 0, origin, 0.5f, SpriteEffects.None, 1.0f);
        }
    }
}
