using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game31
{
    public class Player : GameObject
    {
        public float acceleration, rotationSpeed, rotationAcceleration,
            bulletLivingTime, shootingSpeed, timeFromLastShot, bulletSpeed, hasRestartedBefore;
        public List<Engine> engineList;
        public List<Bullet> bulletList;
        //public List<Missile> missileList;
        //public List<Shell> shellList;
        public List<Texture2D> numbers;
        Random random;
        public Keys up, right, left, shoot;
        ContentManager Content;
        public int lives, index;
        SpriteFont font;
        KeyboardState keyState;

        public Player(ContentManager Content, Random random, float bulletLivingTime, float shootingSpeed, float bulletSpeed, Keys up, Keys right, Keys left, Keys shoot, int index)
        {
            this.index = index;
            this.Content = Content;
            image = Content.Load<Texture2D>("p122");
            angle = MathHelper.ToRadians(random.Next(180 * index - 45 - 180, 180 * index + 45 - 180));
            position = -new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 500 + screenParameters / 2;
            origin = new Vector2(image.Width / 2, image.Height / 2);
            speed = Vector2.Zero;
            acceleration = 120;
            //acceleration = 240;
            rotationSpeed = 0;
            rotationAcceleration = 5;
            engineList = new List<Engine>();
            //missileList = new List<Missile>();
            bulletList = new List<Bullet>();
            //shellList = new List<Shell>();
            this.random = random;
            this.up = up;
            this.right = right;
            this.left = left;
            this.shoot = shoot;
            length = 32;
            this.bulletLivingTime = bulletLivingTime;
            this.shootingSpeed = shootingSpeed;
            this.bulletSpeed = bulletSpeed;
            timeFromLastShot = shootingSpeed;
            lives = 5;
            font = Content.Load<SpriteFont>("f1");
            hasRestartedBefore = 0;
            numbers = new List<Texture2D>();
            numbers.Add(Content.Load<Texture2D>("p124"));
            numbers.Add(Content.Load<Texture2D>("p125"));
            numbers.Add(Content.Load<Texture2D>("p126"));
        }
        public void Update(float elapsed, Player enemyPlayer)
        {
            keyState = Keyboard.GetState();

            for (int i = 0; i < engineList.Count; i++)
                if (!engineList[i].isVisible)
                    engineList.RemoveAt(i);

            if (keyState.IsKeyDown(left))
                rotationSpeed -= rotationAcceleration * elapsed;

            if (keyState.IsKeyDown(right))
                rotationSpeed += rotationAcceleration * elapsed;
            /*rotationSpeed = 0;
            if (keyState.IsKeyDown(left))
                rotationSpeed = -200 * elapsed;

            if (keyState.IsKeyDown(right))
                rotationSpeed = 200 * elapsed;*/

            if (keyState.IsKeyDown(up))
                speed -= new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * acceleration * elapsed;

            makingEngines();

            angle += rotationSpeed * elapsed;
            position += speed * elapsed;

            if (position.X - length / 2 < 0)
            {
                speed.X = 0;
                position.X = length / 2;
            }
            if (position.Y - length / 2 < 0)
            {
                speed.Y = 0;
                position.Y = length / 2;
            }
            if (position.X + length / 2 > screenParameters.X)
            {
                speed.X = 0;
                position.X = screenParameters.X - length / 2;
            }
            if (position.Y + length / 2 > screenParameters.Y)
            {
                speed.Y = 0;
                position.Y = screenParameters.Y - length / 2;
            }

            timeFromLastShot += elapsed;
            if (Keyboard.GetState().IsKeyDown(shoot) && timeFromLastShot >= shootingSpeed && hasRestartedBefore > 3 && enemyPlayer.hasRestartedBefore > 3)
            {
                timeFromLastShot = 0;

                bulletList.Add(new Bullet(Content,
                    position - new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (length / 2 + new Bullet(Content, Vector2.Zero, Vector2.Zero, 0, 0, 0).length / 2),
                    speed, angle, bulletSpeed, bulletLivingTime));

                //missileList.Add(new Missile(Content,
                //    position - new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (length / 2 + new Bullet(Content, Vector2.Zero, Vector2.Zero, 0, 0, 0).length / 2),
                //    speed, angle, random));

                //shellList.Add(new Shell(Content,
                //    position - new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (length / 2 + new Bullet(Content, Vector2.Zero, Vector2.Zero, 0, 0, 0).length / 2),
                //    speed, random));


            }

            foreach (Engine engine in engineList)
                engine.Update(elapsed);

            foreach (Bullet bullet in bulletList)
                bullet.Update(elapsed, enemyPlayer.position);

            //foreach (Missile missile in missileList)
            //    missile.Update(enemyPlayer, elapsed);

            //foreach (Shell shell in shellList)
            //    shell.Update(elapsed, enemyPlayer.position);

            hasRestartedBefore += elapsed;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, null, Color.White, angle, origin, length / image.Width, SpriteEffects.None, 0);

            foreach (Bullet bullet in bulletList)
                bullet.Draw(spriteBatch);

            //foreach (Missile missile in missileList)
            //    missile.Draw(spriteBatch);

            //foreach (Shell shell in shellList)
            //    shell.Draw(spriteBatch);

            foreach (Engine engine in engineList)
                engine.Draw(spriteBatch);

            spriteBatch.DrawString(font, "Lives " + lives, position - new Vector2(10, 30), Color.White);

            if (hasRestartedBefore <= 3)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("p123"), position, null, null, new Vector2(32, 32), 0, null, null, SpriteEffects.None, 0);
                spriteBatch.Draw(numbers[2 - (int)hasRestartedBefore], position, null, null, new Vector2(numbers[2 - (int)hasRestartedBefore].Width / 2, numbers[2 - (int)hasRestartedBefore].Height / 2), 0, null, Color.White * 0.5f, SpriteEffects.None, 0);
            }
        }
        public void Death()
        {
            for (int i = 0; i < 3600; i++)
                AddBurst(MathHelper.ToRadians((float)i / 10), (float)random.NextDouble(), position, false);
            angle = MathHelper.ToRadians(random.Next(180 * index - 45 - 180, 180 * index + 45 - 180));
            position = -new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 500 + screenParameters / 2;
            speed = Vector2.Zero;
            rotationSpeed = 0;
            lives--;
            hasRestartedBefore = 0;
        }
        public void Restart()
        {
            angle = MathHelper.ToRadians(random.Next(180 * index - 45 - 180, 180 * index + 45 - 180));
            position = -new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 500 + screenParameters / 2;
            speed = Vector2.Zero;
            rotationSpeed = 0;
            lives = 5;
            bulletList.Clear();
            //missileList.Clear();
            //shellList.Clear();
            engineList.Clear();
            hasRestartedBefore = 0;
        }
        void makingEngines()
        {
            int precise = 100000000;
            if (Keyboard.GetState().IsKeyDown(left))
                for (int i = 0; i < 10; i++)
                {
                    float engineAngle = angle + MathHelper.ToRadians(random.Next(-10 * precise, 10 * precise + 1) / precise);
                    AddEngine(engineAngle - MathHelper.ToRadians(90), 0.1f,
                        position - new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * length / 2, true);

                    engineAngle = angle + MathHelper.ToRadians(random.Next(-10 * precise, 10 * precise + 1) / precise);
                    AddEngine(engineAngle + MathHelper.ToRadians(90), 0.1f,
                        position + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * length / 2, true);
                }

            if (Keyboard.GetState().IsKeyDown(right))
                for (int i = 0; i < 10; i++)
                {
                    float engineAngle = angle + MathHelper.ToRadians(random.Next(-10 * precise, 10 * precise + 1) / precise);
                    AddEngine(engineAngle + MathHelper.ToRadians(90), 0.1f,
                        position - new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * length / 2, true);

                    engineAngle = angle + MathHelper.ToRadians(random.Next(-10 * precise, 10 * precise + 1) / precise);
                    AddEngine(engineAngle - MathHelper.ToRadians(90), 0.1f,
                        position + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * length / 2, true);
                }

            if (Keyboard.GetState().IsKeyDown(up))
                for (int i = 0; i < 100; i++)
                {
                    float engineAngle = angle + MathHelper.ToRadians(random.Next(-10 * precise, 10 * precise + 1) / precise);
                    AddEngine(engineAngle, 1,
                        position + new Vector2((float)Math.Sin(angle),
                        -(float)Math.Cos(angle)) * random.Next(-1, 2) + new Vector2((float)Math.Cos(angle),
                        (float)Math.Sin(angle)) * length / 2, false);
                }
        }
        void AddEngine(float rotation, float maxTime, Vector2 newPosition, bool isForRotation)
        {
            engineList.Add(new Engine(Content, newPosition,
                new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * random.Next(0, 100) + speed,
                (float)random.NextDouble() * maxTime, isForRotation));
        }
        void AddBurst(float rotation, float maxTime, Vector2 newPosition, bool isForRotation)
        {
            engineList.Add(new Engine(Content, newPosition,
                new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * (random.Next(0, 128) + random.Next(0, 64) + random.Next(0, 32) + random.Next(0, 16) + random.Next(0, 8) + random.Next(0, 4)) + speed,
                (float)random.NextDouble() * maxTime, isForRotation));
        }
    }
}
