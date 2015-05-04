//##############################################################################################################
//Xiaoqian Xi  ID:C00163428
//*******************************
//Robot Tank is a shoot style game
//How to play: Player holds a tank, use W, S keys to move Up & Down and use A, D keys to rotate Left & Right.
//             Tank gun rotate by mouse moving, it looks at mouse position.
//             Player shooting enemies use mouse click.
//Player have 5 lives.
//Game play: 
//Game over if you lose five lives
//Game win if you kill all the enemy
//Get score if you kill an enemy
//##############################################################################################################
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Robot_Tank
{
    class Camera
    {
        public Matrix transform;
        Viewport view;
        Vector2 centre;
        public Camera(Viewport newView)
        {
            view = newView;
        }

        public void Update(GameTime gameTime, Player tank, Game1 game)
        {
            if (tank.tankPos.X + (tank.tankRec.Width / 2) >= 0 && tank.tankPos.Y + (tank.tankRec.Height / 2) >= 0)
            {
                if (tank.tankPos.X + (tank.tankRec.Width / 2) <= game.background.Width
                    && tank.tankPos.Y + (tank.tankRec.Height / 2) <= game.background.Height)
                {
                    centre = new Vector2(tank.tankPos.X + (tank.tankRec.Width / 2) - 400, tank.tankPos.Y + (tank.tankRec.Height / 2) - 240);
                    transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0));
                }
            }
        }
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameState
        {
            MainMenu,
            Playing,
            GameOver,
            HelpPage,
        }
        GameState CurrentGameState = GameState.MainMenu;

        public Texture2D menuImg;
        public Texture2D gameTitleImg;
        public Texture2D restartImg;
        public Texture2D scoreBoardImg;
        public Texture2D helpPageImg;

        cButton btnPlay;
        cButton btnHelp;
        cButton btnBack;

        public Texture2D background;
        public float tankRadius;
        float friction = 0.1f;

        Texture2D gun;
        Vector2 gunOrigin, gunPos;
        Vector2 direction;
        float rotation;
        Vector2 mousePosition;

        Camera camera;
        public Rectangle viewRec;
        public Vector2 backgroundPos;

        MouseState pastMouse;

        Scrolling scrolling1;
        Scrolling scrolling2;

        List<Bullets> bullets = new List<Bullets>();

        public Enemy[] enemy0 = new Enemy[10];
        public Enemy[] enemy1 = new Enemy[10];
        public Enemy[] enemy2 = new Enemy[10];
        public Enemy[] enemy3 = new Enemy[10];
        public Enemy[] enemy4 = new Enemy[10];

        public Vector2 tankCenter;
        List<Explosion> explosions = new List<Explosion>();
        Player tank;
        GUI text;
        Texture2D healthTexture;
        Rectangle healthRectangle;
        public int aliveCount;
        public int score;

        Radar radar;
        bool isGameOver;
       
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            menuImg = null;
            gameTitleImg = null;
            scoreBoardImg = null;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            InitGame();
            base.Initialize();
        }

        private void InitRadar()
        {
            radar = new Radar(Content, "redDotSmall", "yellowDotSmall", "blackDotLarge");
        }

        private void InitGame()
        {
            backgroundPos = new Vector2(0, 0);
            tank = new Player(50, 5);
            tank.tankPos = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            gunPos = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            
            EnemyContent();
            camera = new Camera(GraphicsDevice.Viewport);
            viewRec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            InitRadar();
            isGameOver = false;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            IsMouseVisible = true;
            menuImg = Content.Load<Texture2D>("menu");
            gameTitleImg = Content.Load<Texture2D>("title");
            restartImg = Content.Load<Texture2D>("restartButton");
            scoreBoardImg = Content.Load<Texture2D>("boardImg");
            helpPageImg = Content.Load<Texture2D>("helpImg");
            btnPlay = new cButton(Content.Load<Texture2D>("playButton"));
            btnPlay.setPosition(new Vector2(290, 250));
            btnHelp = new cButton(Content.Load<Texture2D>("helpButton"));
            btnHelp.setPosition(new Vector2(500, 350));
            btnBack = new cButton(Content.Load<Texture2D>("backButton"));
            btnBack.setPosition(new Vector2(600, 350));

            tank.LoadContent(Content, "base1");
            healthTexture = Content.Load<Texture2D>("healthBar");
            text = new GUI();
            text.LoadContent(Content,"texfont1", "texfont2");

            gun = Content.Load<Texture2D>("gun1");
            gunOrigin = new Vector2(gun.Bounds.Center.X, gun.Bounds.Center.Y);
            rotation = 0;

            scrolling1 = new Scrolling(Content.Load<Texture2D>("fog1"), new Rectangle(0, 0, 800, 480));
            scrolling2 = new Scrolling(Content.Load<Texture2D>("fog2"), new Rectangle(800, 480, 800, 480));

            background = Content.Load<Texture2D>("map1");

            Explosion.myTexture = Content.Load<Texture2D>("explosion");
    
        }

        public void EnemyContent()
        {
            for (int i = 0; i < 10; i++)
            {
                enemy0[i] = new Enemy();
                enemy0[i].LoadContent(Content, "tank", Content.Load<Texture2D>("eBullet"));
                enemy0[i].Alive = true;
                int centerX = 590;
                int centerY = 1200;
                int r = 300;
                enemy0[i].position = new Vector2(centerX + (float)(Math.Cos(i * 36) * r), centerY + (float)(Math.Sin(i * 36) * r));
            }

            for (int i = 0; i < 10; i++)
            {
                enemy1[i] = new Enemy();
                enemy1[i].LoadContent(Content, "tank", Content.Load<Texture2D>("eBullet"));
                enemy1[i].Alive = true;
                int centerX = 1170;
                int centerY = 670;
                int r = 250;
                enemy1[i].position = new Vector2(centerX + (float)(Math.Cos(i * 36) * r), centerY + (float)(Math.Sin(i * 36) * r));
            }

            for (int i = 0; i < 10; i++)
            {
                enemy2[i] = new Enemy();
                enemy2[i].LoadContent(Content, "tank", Content.Load<Texture2D>("eBullet"));
                enemy2[i].Alive = true;
                int centerX = 1840;
                int centerY = 1030;
                int r = 200;
                enemy2[i].position = new Vector2(centerX + (float)(Math.Cos(i * 36) * r), centerY + (float)(Math.Sin(i * 36) * r));
            }

            for (int i = 0; i < 10; i++)
            {
                enemy3[i] = new Enemy();
                enemy3[i].LoadContent(Content, "tank", Content.Load<Texture2D>("eBullet"));
                enemy3[i].Alive = true;
                int centerX = 2500;
                int centerY = 330;
                int r = 300;
                enemy3[i].position = new Vector2(centerX + (float)(Math.Cos(i * 36) * r), centerY + (float)(Math.Sin(i * 36) * r));
            }

            for (int i = 0; i < 10; i++)
            {
                enemy4[i] = new Enemy();
                enemy4[i].LoadContent(Content, "tank", Content.Load<Texture2D>("eBullet"));
                enemy4[i].Alive = true;
                int centerX = 2450;
                int centerY = 1530;
                int r = 200;
                enemy4[i].position = new Vector2(centerX + (float)(Math.Cos(i * 36) * r), centerY + (float)(Math.Sin(i * 36) * r));
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            MouseState mouse = Mouse.GetState();
            switch (CurrentGameState)
            {
                case GameState.Playing:
                    updateGame(gameTime, mouse);
                    break;

                case GameState.MainMenu:      
                    btnPlay.Update(mouse);
                    btnHelp.Update(mouse);
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        if (btnPlay.isClicked == true)
                        {          
                            CurrentGameState = GameState.Playing;
                        }
                        if (btnHelp.isClicked == true)
                        {
                            CurrentGameState = GameState.HelpPage;
                        }
                    }
                    break;

                case GameState.GameOver:
                    KeyboardState keyState = Keyboard.GetState();
                    if (keyState.IsKeyDown(Keys.Enter))
                    {                
                        CurrentGameState = GameState.Playing;
                        RestartGame();
                        isGameOver = false;
                    }
                    break;

                case GameState.HelpPage:
                    btnBack.Update(mouse);
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        if (btnBack.isClicked == true)
                        {
                            CurrentGameState = GameState.MainMenu;
                        }
                    }
                    break;
            }       
            base.Update(gameTime);
        }

        public void updateGame(GameTime gameTime, MouseState mouse)
        {
            mousePosition = new Vector2(mouse.X, mouse.Y);
            direction = mousePosition - gunPos;
            rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);

            tank.CheckBounce(this);
            tank.Update(gameTime, this);

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                tank.TankRotation("Right");
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                tank.TankRotation("Left");

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                tank.TankMovement("Up");
            }
            else if (tank.tankVelocity != Vector2.Zero)
            {
                Vector2 v = new Vector2(tank.tankVelocity.X, tank.tankVelocity.Y);
                tank.tankVelocity.X = v.X - friction * v.X;
                tank.tankVelocity.Y = v.Y - friction * v.Y;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                tank.TankMovement("Down");
            }

            //Shoot bullets
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && pastMouse.LeftButton == ButtonState.Released && tank.Alive)
                Shoot(Content);
            pastMouse = Mouse.GetState();
            UpdateBullets();

            //Scrolling clouds
            if (scrolling1.rectangle.X + scrolling1.rectangle.Width <= 0)
            {
                scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.rectangle.Width;
                scrolling1.rectangle.Y = scrolling2.rectangle.Y + scrolling2.rectangle.Height;
            }
            if (scrolling2.rectangle.X + scrolling2.rectangle.Width <= 0)
            {
                scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling1.rectangle.Width;
                scrolling2.rectangle.Y = scrolling1.rectangle.Y + scrolling1.rectangle.Height;
            }
            //Update scrolling 
            scrolling1.Update();
            scrolling2.Update();

            for (int i = 0; i < 10; i++)
                enemy0[i].Update(gameTime, tank);
            for (int i = 0; i < 10; i++)
                enemy1[i].Update(gameTime, tank);
            for (int i = 0; i < 10; i++)
                enemy2[i].Update(gameTime, tank);
            for (int i = 0; i < 10; i++)
                enemy3[i].Update(gameTime, tank);
            for (int i = 0; i < 10; i++)
                enemy4[i].Update(gameTime, tank);

            foreach (Explosion e in explosions)
            {
                e.UpdateSprite(gameTime, graphics);
            }
            if (!tank.Alive)
            {
                explosions.Add(tank.e);
            }

            healthRectangle = new Rectangle((int)gunPos.X, (int)gunPos.Y, tank.health, 5);

            CountEnemies();

            if (isGameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Tab))
                {
                    CurrentGameState = GameState.GameOver;
                    isGameOver = false;
                }
            }
            camera.Update(gameTime, tank, this);
        }

        private void RestartGame()
        {
            tank.Alive = true;
            tank.health = 50;
            tank.lives = 5;
            tank.tankPos = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            EnemyContent();
            score = 0;
        }
        //Count alive enemies
        public void CountEnemies()
        {
            aliveCount = 0;
            for (int i = 0; i < 10; i++)
            {
                if (enemy0[i].Alive)
                {
                    aliveCount++;
                }
                if (enemy1[i].Alive)
                {
                    aliveCount++;
                }
                if (enemy2[i].Alive)
                {
                    aliveCount++;
                }
                if (enemy3[i].Alive)
                {
                    aliveCount++;
                }
                if (enemy4[i].Alive)
                {
                    aliveCount++;
                }
            }
        }

        public void UpdateBullets()
        {
            foreach (Bullets bullet in bullets)
            {
                bullet.position += bullet.velocity;
                if (Vector2.Distance(bullet.position, tank.tankPos) > 500)
                    bullet.isVisible = false;
                CollisionDetection();
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Shoot(ContentManager Content)
        {
            Bullets newBullet = new Bullets(Content.Load<Texture2D>("bullet"));
            newBullet.velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 4f + tank.tankVelocity;
            newBullet.position = tank.tankPos + newBullet.velocity * 5;
            newBullet.isVisible = true;

            if (bullets.Count() < 20)
                bullets.Add(newBullet);
        }

        public void CollisionDetection()
        {
            foreach (Bullets bullet in bullets)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (Vector2.Distance(bullet.position, enemy0[i].position) <= enemy0[i].radius + bullet.radius && enemy0[i].Alive)
                    {
                        enemy0[i].Alive = false;
                        bullet.isVisible = false;
                        score += 1;
                        Explosion e = new Explosion();
                        e.Init(enemy0[i].position, new TimeSpan(0, 0, 0, 0, 500));
                        explosions.Add(e);
                    }

                    if (Vector2.Distance(bullet.position, enemy1[i].position) <= enemy1[i].radius + bullet.radius && enemy1[i].Alive)
                    {
                        enemy1[i].Alive = false;
                        bullet.isVisible = false;
                        score += 1;
                        Explosion e = new Explosion();
                        e.Init(enemy1[i].position, new TimeSpan(0, 0, 0, 0, 500));
                        explosions.Add(e);
                    }

                    if (Vector2.Distance(bullet.position, enemy2[i].position) <= enemy2[i].radius + bullet.radius && enemy2[i].Alive)
                    {
                        enemy2[i].Alive = false;
                        bullet.isVisible = false;
                        score += 1;
                        Explosion e = new Explosion();
                        e.Init(enemy2[i].position, new TimeSpan(0, 0, 0, 0, 500));
                        explosions.Add(e);
                    }

                    if (Vector2.Distance(bullet.position, enemy3[i].position) <= enemy3[i].radius + bullet.radius && enemy3[i].Alive)
                    {
                        enemy3[i].Alive = false;
                        bullet.isVisible = false;
                        score += 1;
                        Explosion e = new Explosion();
                        e.Init(enemy3[i].position, new TimeSpan(0, 0, 0, 0, 500));
                        explosions.Add(e);
                    }

                    if (Vector2.Distance(bullet.position, enemy4[i].position) <= enemy4[i].radius + bullet.radius && enemy4[i].Alive)
                    {
                        enemy4[i].Alive = false;
                        bullet.isVisible = false;
                        score += 1;
                        Explosion e = new Explosion();
                        e.Init(enemy4[i].position, new TimeSpan(0, 0, 0, 0, 500));
                        explosions.Add(e);
                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Olive);
            // TODO: Add your drawing code here
            switch (CurrentGameState)
            {
                case GameState.Playing:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);
                    spriteBatch.Draw(background, backgroundPos, Color.White);
                    foreach (Bullets bullet in bullets)
                    bullet.Draw(spriteBatch);
                    if (tank.Alive)
                    {
                        tank.Draw(gameTime, spriteBatch);
                        spriteBatch.Draw(gun, tank.tankPos, null, Color.White, rotation, gunOrigin, 1.0f, SpriteEffects.None, 1.0f);
                    }
                    DrawEnemies(gameTime);

                    foreach (Explosion e in explosions)
                    {
                        e.Draw(gameTime, spriteBatch);
                    }         
                    spriteBatch.End();

                    spriteBatch.Begin();
                    scrolling1.Draw(spriteBatch);
                    scrolling2.Draw(spriteBatch);
                    spriteBatch.Draw(healthTexture, healthRectangle, Color.White);

                    if (aliveCount == 0 && score == 50 || !tank.Alive)
                    {
                        spriteBatch.Draw(scoreBoardImg, new Vector2(320, 200), Color.White);
                        text.DrawScoreBoard(spriteBatch, this, tank);
                        isGameOver = true;
                        
                    }
                    text.Draw(gameTime, spriteBatch, this, tank);
                    for (int i = 0; i < 10; i++)
                    {
                        radar.Draw(spriteBatch, tank, this);                      
                    }

                    spriteBatch.End();
                    break;
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    spriteBatch.Draw(menuImg, new Vector2(0, 0), Color.White);
                    spriteBatch.Draw(gameTitleImg, new Vector2(150, 100), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnHelp.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.GameOver:
                    spriteBatch.Begin();
                    spriteBatch.Draw(menuImg, new Vector2(0, 0), Color.White);
                    spriteBatch.Draw(gameTitleImg, new Vector2(150, 100), Color.White);
                    spriteBatch.Draw(restartImg, new Vector2(150, 300), Color.White);
                    spriteBatch.End();
                    break;
                case GameState.HelpPage:
                    spriteBatch.Begin();
                    spriteBatch.Draw(menuImg, new Vector2(0, 0), Color.White);
                    spriteBatch.Draw(helpPageImg, new Vector2(0, 0), Color.White);
                    btnBack.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
            }    

            base.Draw(gameTime);
        }

        public void DrawEnemies(GameTime gameTime)
        {
            for (int i = 0; i < 10; i++)
            {
                enemy0[i].Draw(gameTime, spriteBatch);
            }
            for (int i = 0; i < 10; i++)
            {
                enemy1[i].Draw(gameTime, spriteBatch);
            }
            for (int i = 0; i < 10; i++)
            {
                enemy2[i].Draw(gameTime, spriteBatch);
            }
            for (int i = 0; i < 10; i++)
            {
                enemy3[i].Draw(gameTime, spriteBatch);
            }
            for (int i = 0; i < 10; i++)
            {
                enemy4[i].Draw(gameTime, spriteBatch);
            } 
        }
    }
}
