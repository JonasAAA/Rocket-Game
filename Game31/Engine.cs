using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game31
{
    public class Engine : GameObject
    {
        float livingTime, time;
        public bool isVisible, isForRotation;

        public Engine(ContentManager Content, Vector2 position, Vector2 speed, float livingTime, bool isForRotation)
        {
            image = Content.Load<Texture2D>("p8");
            this.position = position;
            this.speed = speed;
            this.livingTime = livingTime;
            time = 0;
            isVisible = true;
            this.isForRotation = isForRotation;
        }
        public void Update(float elapsed)
        {
            time += elapsed;
            if (time >= livingTime)
                isVisible = false;

            position += speed * elapsed;

            if (isForRotation)
                color = Color.Lerp(Color.Red, Color.White, time / livingTime);

            else
                color = Color.Lerp(Color.Red, Color.Yellow, time / livingTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, null, color, angle, origin, 1, SpriteEffects.None, 0);
        }
    }
}
