using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game31
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Player> playerList;
        List<Planet> planetList, createdPlanetList;
        List<Vector2> starPositionList;
        List<Color> starColorList;
        Random random;
        Texture2D star;
        enum State
        {
            Menu,
            Customing,
            Playing,
            Pause,
            GameOver
        };
        State gameState;
        Button newGame, exit, resume, restart, goToMainMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            //if ("abc" > "abd")
            //    ;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Viewport viewport = graphics.GraphicsDevice.Viewport;
            random = new Random();

            createdPlanetList = new List<Planet>();

            playerList = new List<Player>();
            playerList.Add(new Player(Content, random, 1.5f, 1, 500, Keys.Up, Keys.Right, Keys.Left, Keys.L, playerList.Count));
            playerList.Add(new Player(Content, random, 1.5f, 1, 500, Keys.W, Keys.D, Keys.A, Keys.Q, playerList.Count));

            planetList = new List<Planet>();
            //planetList.Add(new Planet(graphics.GraphicsDevice, Content, new Vector2((float)viewport.Width / 2, (float)viewport.Height / 2), new Vector2(0), 10000 * 60, 100, Color.White));
            planetList.Add(new Planet(graphics.GraphicsDevice, Content, new Vector2((float)viewport.Width / 2, (float)viewport.Height / 2), new Vector2(0, 10), 10000 * 60, 100, Color.Yellow));
            planetList.Add(new Planet(graphics.GraphicsDevice, Content, new Vector2((float)viewport.Width * 3 / 4, (float)viewport.Height / 2), new Vector2(0, -100), 1000 * 60, 50, Color.White));

            //planetList.Add(new Planet(Content, new Vector2((float)viewport.Width * 2 / 5, (float)viewport.Height / 2), new Vector2(0, 100), 10000 * 60, 50, Color.Yellow));
            //planetList.Add(new Planet(Content, new Vector2((float)viewport.Width * 3 / 5, (float)viewport.Height / 2), new Vector2(0, -100), 10000 * 60, 50, Color.White));
            //planetList.Add(new Planet(Content, new Vector2((float)viewport.Width / 2, (float)viewport.Height / 2 + 2f), new Vector2(0, 0), 0/*1000 * 60*/, 10, Color.Yellow));
            //planetList.Add(new Planet(Content, new Vector2((float)viewport.Width / 2, (float)viewport.Height / 2 - 2f), new Vector2(0, 0), 0/*1000 * 60*/, 10, Color.Yellow));
            //planetList.Add(new Planet(Content, new Vector2((float)viewport.Width / 2, (float)viewport.Height / 2 + 4f), new Vector2(0, 0), 0/*1000 * 60*/, 10, Color.Yellow));
            //planetList.Add(new Planet(Content, new Vector2((float)viewport.Width / 2, (float)viewport.Height / 2 - 4f), new Vector2(0, 0), 0/*1000 * 60*/, 10, Color.Yellow));

            //planetList.Add(new Planet(Content, new Vector2(viewport.Width * 2 / 5, viewport.Height * 3 / 4), new Vector2(130, 10), 0/*1000 * 60*/, 10, Color.Red));
            //planetList.Add(new Planet(Content, new Vector2(viewport.Width * 3 / 5, viewport.Height * 1 / 4), new Vector2(-130, -10), 0/*1000 * 60*/, 10, Color.Blue));
            //planetList.Add(new Planet(Content, new Vector2(viewport.Width / 2, viewport.Height / 2), new Vector2(0, 10), 10000 * 60, 100, Color.Yellow));
            //planetList.Add(new Planet(Content, new Vector2(viewport.Width * 3 / 4, viewport.Height / 2), new Vector2(0, -100), 1000 * 60, 50, Color.White));
            //planetList.Add(new Planet(Content, new Vector2(viewport.Width / 2, viewport.Height / 2), new Vector2(0, 30), 100000 * 60, 100, Color.Yellow));
            //planetList.Add(new Planet(Content, new Vector2(viewport.Width * 3 / 4, viewport.Height / 2), new Vector2(0, -300), 10000 * 60, 50, Color.White));


            star = Content.Load<Texture2D>("p2");
            starPositionList = new List<Vector2>();
            starColorList = new List<Color>();
            for (int i = 0; i < 100; i++)
            {
                starPositionList.Add(new Vector2(random.Next(0, viewport.Width), random.Next(0, viewport.Height)));
                starColorList.Add(Color.Lerp(Color.Lerp(Color.Red, Color.Blue, (float)random.NextDouble() * 0.5f), Color.Yellow, (float)random.NextDouble() * 0.5f));
            }

            gameState = State.Menu;

            newGame = new Button(Content.Load<Texture2D>("p86"), new Vector2(viewport.Width / 2, viewport.Height / 2 - 50));
            exit = new Button(Content.Load<Texture2D>("p88"), new Vector2(viewport.Width / 2, viewport.Height / 2 + 50));
            resume = new Button(Content.Load<Texture2D>("p95"), new Vector2(viewport.Width / 2, viewport.Height / 2 - 100));
            restart = new Button(Content.Load<Texture2D>("p96"), new Vector2(viewport.Width / 2, viewport.Height / 2));
            goToMainMenu = new Button(Content.Load<Texture2D>("p89"), new Vector2(viewport.Width / 2, viewport.Height / 2 + 100));
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                Exit();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch(gameState)
            {
                case State.Menu:
                    {
                        if (newGame.IsButtonPressed())
                        {
                            gameState = State.Playing;
                            foreach (Player player in playerList)
                                player.Restart();

                            foreach (Planet planet in planetList)
                                planet.Restart();
                        }

                        if (exit.IsButtonPressed())
                            Exit();

                        break;
                    }
                case State.Customing:
                    {
                        break;
                    }
                case State.Playing:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                            gameState = State.Pause;

                        foreach (Player player in playerList)
                        {
                            player.ApplyGravity(planetList, elapsed);
                            foreach (Bullet bullet in player.bulletList)
                                bullet.ApplyGravity(planetList, elapsed);

                            //foreach (Missile missile in player.missileList)
                            //    missile.ApplyGravity(planetList, elapsed);

                            //foreach (Shell shell in player.shellList)
                            //    shell.ApplyGravity(planetList, elapsed);

                            foreach (Engine e in player.engineList)
                                e.ApplyGravity(planetList, elapsed);
                        }

                        foreach (Planet planet in planetList)
                            planet.ApplyGravity(planetList, elapsed);

                        DoCollisions();

                        foreach (Player player in playerList)
                        {
                            if (playerList[0].position != player.position)
                                player.Update(elapsed, playerList[0]);
                            else
                                player.Update(elapsed, playerList[1]);

                            for (int i = 0; i < player.bulletList.Count; i++)
                                if (!player.bulletList[i].isVisible)
                                    player.bulletList.RemoveAt(i);

                            //for (int i = 0; i < player.shellList.Count; i++)
                            //    if (!player.shellList[i].isVisible)
                            //        player.shellList.RemoveAt(i);
                        }

                        foreach (Planet planet in planetList)
                            planet.Update(elapsed);

                        /*foreach (Player player in playerList)
                            if (player.lives <= 0)
                                gameState = State.GameOver;*/

                        break;
                    }
                case State.Pause:
                    {
                        if (resume.IsButtonPressed())
                            gameState = State.Playing;

                        if (restart.IsButtonPressed())
                        {
                            gameState = State.Playing;
                            foreach (Player player in playerList)
                                player.Restart();

                            foreach (Planet planet in planetList)
                                planet.Restart();
                        }

                        if (goToMainMenu.IsButtonPressed())
                            gameState = State.Menu;

                        break;
                    }
                case State.GameOver:
                    {
                        if (restart.IsButtonPressed())
                        {
                            gameState = State.Playing;
                            foreach (Player player in playerList)
                                player.Restart();

                            foreach (Planet planet in planetList)
                                planet.Restart();
                        }

                        if (goToMainMenu.IsButtonPressed())
                            gameState = State.Menu;

                        break;
                    }
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            switch(gameState)
            {
                case State.Menu:
                    {
                        IsMouseVisible = true;

                        newGame.Draw(spriteBatch);
                        exit.Draw(spriteBatch);

                        break;
                    }
                case State.Customing:
                    {
                        IsMouseVisible = true;

                        break;
                    }
                case State.Playing:
                    {
                        IsMouseVisible = false;

                        for (int i = 0; i < starPositionList.Count; i++)
                            spriteBatch.Draw(star, starPositionList[i], null, starColorList[i], 0, new Vector2(0.5f), new Vector2(0.05f), SpriteEffects.None, 0);

                        foreach (Planet planet in planetList)
                            planet.Draw(spriteBatch);

                        foreach (Player player in playerList)
                            player.Draw(spriteBatch);

                        break;
                    }
                case State.Pause:
                    {
                        IsMouseVisible = true;

                        resume.Draw(spriteBatch);
                        restart.Draw(spriteBatch);
                        goToMainMenu.Draw(spriteBatch);

                        break;
                    }
                case State.GameOver:
                    {
                        IsMouseVisible = true;

                        restart.Draw(spriteBatch);
                        goToMainMenu.Draw(spriteBatch);

                        break;
                    }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
        void DoCollisions()
        {
            foreach (Player player in playerList)
                foreach (Player enemyPlayer in playerList)
                {
                    if (player.position != enemyPlayer.position)
                    {
                        if (player.Distance(player.position, enemyPlayer.position) < player.length / 2 + enemyPlayer.length / 2)
                        {
                            player.Death();
                            enemyPlayer.Death();
                        }
                    }

                    foreach (Planet planet in planetList)
                        if (player.Distance(player.position, planet.position) < player.length / 2 + planet.length / 2)
                            player.Death();

                    foreach (Engine engine in player.engineList)
                        foreach (Planet planet in planetList)
                            if (player.Distance(engine.position, planet.position) < planet.length / 2)
                                engine.isVisible = false;

                    foreach (Bullet bullet in player.bulletList)
                    {
                        if (player.Distance(bullet.position, enemyPlayer.position) < bullet.length / 2 + enemyPlayer.length / 2)
                        {
                            bullet.isVisible = false;
                            enemyPlayer.Death();
                        }

                        foreach (Planet planet in planetList)
                            if (player.Distance(bullet.position, planet.position) < bullet.length / 2 + planet.length / 2)
                                bullet.isVisible = false;
                    }

                    /*foreach (Missile missile in player.missileList)
                    {
                        if (player.Distance(missile.position, enemyPlayer.position) < missile.length / 2 + enemyPlayer.length / 2)
                        {
                            missile.isVisible = false;
                            enemyPlayer.Death();
                        }

                        foreach (Planet planet in planetList)
                            if (player.Distance(missile.position, planet.position) < missile.length / 2 + planet.length / 2)
                                missile.isVisible = false;
                    }*/

                    /*foreach (Shell shell in player.shellList)
                    {
                        if (player.Distance(shell.position, enemyPlayer.position) < shell.length / 2 + enemyPlayer.length / 2)
                        {
                            shell.isVisible = false;
                            enemyPlayer.Death();
                        }

                        foreach (Planet planet in planetList)
                            if (player.Distance(shell.position, planet.position) < shell.length / 2 + planet.length / 2)
                                shell.isVisible = false;
                    }*/
                }
        }
    }
}