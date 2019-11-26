using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game31
{
    public class Missile : GameObject
    {
        public float rotationSpeed, acceleration, rotationAcceleration;
        public List<Engine> engineList;
        Random random;
        ContentManager Content;
        public bool isVisible;

        public Missile(ContentManager Content, Vector2 position, Vector2 playerSpeed, float angle, Random random)
        {
            this.Content = Content;
            image = Content.Load<Texture2D>("p127");
            this.position = position;
            origin = new Vector2(image.Width / 2, image.Height / 2);
            speed = playerSpeed;
            //speed = Vector2.Zero;
            engineList = new List<Engine>();
            this.random = random;
            rotationSpeed = 0;
            rotationAcceleration = /*0.01f*/50;
            acceleration = /*120*/1200;
            this.angle = angle;
            length = image.Width / 2;
            isVisible = true;
        }
        public void Update(Player targetPlayer, float elapsed)
        {
            for (int i = 0; i < engineList.Count; i++)
                if (!engineList[i].isVisible)
                    engineList.RemoveAt(i);

            float targetingAmount = MathHelper.Pi / 1000;
            float angle1 = (float)Math.Atan2((position - targetPlayer.position).Y, (position - targetPlayer.position).X);
            if (/*((rotationSpeed > 0 && rotationSpeed < 0.01) || rotationSpeed <= 0) &&*/ Math.Min(Math.Abs(angle - targetingAmount - angle1), Math.Abs(MathHelper.Pi * 2 - angle + targetingAmount + angle1)) > Math.Min(Math.Abs(angle + targetingAmount - angle1), Math.Abs(MathHelper.Pi * 2 - angle - targetingAmount + angle1)))
                rotationSpeed += rotationAcceleration * elapsed;
            else// if ((rotationSpeed < 0 && rotationSpeed > -0.01) || rotationSpeed >= 0)
                rotationSpeed -= rotationAcceleration * elapsed;

            makingEngines(targetPlayer);

            angle += rotationSpeed * elapsed;
            position += speed * elapsed;

            angle = (angle % (MathHelper.Pi * 2) + MathHelper.Pi * 2) % (MathHelper.Pi * 2);
            //if (Distance(speed, Vector2.Zero) < 10)
            speed -= new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * acceleration * elapsed;
            foreach (Engine engine in engineList)
                engine.Update(elapsed);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, null, Color.White, angle, origin, 0.5f, SpriteEffects.None, 0);
            foreach (Engine engine in engineList)
                engine.Draw(spriteBatch);
        }
        void makingEngines(Player targetPlayer)
        {
            float targetingAmount = MathHelper.Pi / 1000;
            int precise = 100000000;
            float angle1 = (float)Math.Atan2((position - targetPlayer.position).Y, (position - targetPlayer.position).X);

            if (Math.Min(Math.Abs(angle - targetingAmount - angle1), Math.Abs(MathHelper.Pi * 2 - angle + targetingAmount + angle1)) > Math.Min(Math.Abs(angle + targetingAmount - angle1), Math.Abs(MathHelper.Pi * 2 - angle - targetingAmount + angle1)))
                for (int i = 0; i < 10; i++)
                {
                    float engineAngle = angle + MathHelper.ToRadians(random.Next(-10 * precise, 10 * precise + 1) / precise);
                    AddEngine(engineAngle + MathHelper.ToRadians(90), 0.1f,
                        position - new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * length / 2, true);

                    engineAngle = angle + MathHelper.ToRadians(random.Next(-10 * precise, 10 * precise + 1) / precise);
                    AddEngine(engineAngle - MathHelper.ToRadians(90), 0.1f,
                        position + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * length / 2, true);
                }

            else
                for (int i = 0; i < 10; i++)
                {
                    float engineAngle = angle + MathHelper.ToRadians(random.Next(-10 * precise, 10 * precise + 1) / precise);
                    AddEngine(engineAngle - MathHelper.ToRadians(90), 0.1f,
                        position - new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * length / 2, true);

                    engineAngle = angle + MathHelper.ToRadians(random.Next(-10 * precise, 10 * precise + 1) / precise);
                    AddEngine(engineAngle + MathHelper.ToRadians(90), 0.1f,
                        position + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * length / 2, true);
                }

            //if (Keyboard.GetState().IsKeyDown(up))
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
    }
}
