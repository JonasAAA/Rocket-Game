using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game31
{
    public class Shell : GameObject
    {
        public float acceleration;
        List<Engine> engineList;
        ContentManager Content;
        Random random;
        public bool isVisible;

        public Shell(ContentManager Content, Vector2 position, Vector2 playerSpeed, Random random)
        {
            this.random = random;
            this.Content = Content;
            image = Content.Load<Texture2D>("p35");
            this.position = position;
            speed = playerSpeed;
            origin = new Vector2(image.Width / 2, image.Height / 2);
            engineList = new List<Engine>();
            length = image.Width / 2;
            acceleration = 120;
            isVisible = true;
        }
        public void Update(float elapsed, Vector2 targetPosition)
        {
            speed += acceleration * new Vector2((-position + targetPosition).X, (-position + targetPosition).Y) / Distance(position, targetPosition) * elapsed;

            for (int i = 0; i < 100; i++)
                AddEngine(random.Next(-10, 10) + (float)Math.Atan2((-position+targetPosition).X, (-position + targetPosition).Y), random.Next(0, 1), position, false);

            foreach (Engine engine in engineList)
                engine.Update(elapsed);

            for (int i = 0; i < engineList.Count; i++)
                if (!engineList[i].isVisible)
                    engineList.RemoveAt(i);

            position += speed * elapsed;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position - origin / 2, Color.White);
            foreach (Engine engine in engineList)
                engine.Draw(spriteBatch);
        }
        void AddEngine(float rotation, float maxTime, Vector2 newPosition, bool isForRotation)
        {
            engineList.Add(new Engine(Content, newPosition,
                new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * random.Next(0, 100) + speed,
                (float)random.NextDouble() * maxTime, isForRotation));
        }
    }
}
