using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game31
{
    public class Planet : GameObject
    {
        public float mass;
        public Vector2 startingPosition, startingSpeed;

        public Planet(GraphicsDevice graphicsDevice, ContentManager Content, Vector2 position, Vector2 speed, float mass, float length, Color color)
        {
            //int width = 1366 * 2,
            //    height = 768 * 2;
            //Vector2 center = new Vector2(width / 2, height / 2);
            //Color[] colorList = new Color[width * height];
            //for (int i = 0; i < width; i++)
            //    for (int j = 0; j < height; j++)
            //        colorList[j * width + i] = Color.LerpPrecise(Color.Black, Color.White, 60 / (new Vector2(i, j) - center).Length());
            //image = new Texture2D(graphicsDevice, width, height);
            //image.SetData<Color>(colorList);
            image = Content.Load<Texture2D>("p2");
            this.position = position;
            this.speed = speed;
            origin = new Vector2(image.Width / 2, image.Height / 2);
            this.mass = mass;
            this.length = length;
            this.color = color;
            startingPosition = position;
            startingSpeed = speed;
        }
        public void Update(float elapsed)
        {
            position += speed * elapsed;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, null, color, angle, origin, length / image.Width, SpriteEffects.None, 0);
        }
        public void Restart()
        {
            position = startingPosition;
            speed = startingSpeed;
        }
    }
}
