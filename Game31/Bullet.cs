using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game31
{
    public class Bullet:GameObject
    {
        public float livingTime, maxLivingTime, targetingAmount;
        public Vector2 playerSpeed;
        public bool isVisible;

        public Bullet(ContentManager Content, Vector2 position, Vector2 playerSpeed, float angle, float bulletSpeed, float maxLivingTime)
        {
            this.angle = angle;
            image = Content.Load<Texture2D>("p29");
            origin = new Vector2(image.Width / 2, image.Height / 2);
            this.position = position;
            speed = -new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * bulletSpeed + playerSpeed;
            length = 12;
            livingTime = 0;
            this.maxLivingTime = maxLivingTime;
            isVisible = true;
            targetingAmount = MathHelper.Pi / 400;
        }
        public void Update(float elapsed, Vector2 targetPlayerPosition)
        {
            float angle1 = (float)Math.Atan2((position - targetPlayerPosition).Y, (position - targetPlayerPosition).X);
            if (Math.Min(Math.Abs(angle - targetingAmount - angle1), Math.Abs(MathHelper.Pi * 2 - angle + targetingAmount + angle1)) > Math.Min(Math.Abs(angle + targetingAmount - angle1), Math.Abs(MathHelper.Pi * 2 - angle - targetingAmount + angle1)))
                angle += targetingAmount;
            else
                angle -= targetingAmount;

            angle = (angle % (MathHelper.Pi * 2) + MathHelper.Pi * 2) % (MathHelper.Pi * 2);

            //speed = speed * 99 / 100 - (float)Math.Sqrt(speed.X * speed.X + speed.Y * speed.Y) * (position - targetPlayerPositon) / Distance(position, targetPlayerPositon) *1/100;
            speed = -new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Distance(Vector2.Zero, speed);
            position += speed * elapsed;
            livingTime += elapsed;
            if (livingTime >= maxLivingTime)
                isVisible = false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, null, color, angle, origin, length / image.Width, SpriteEffects.None, 0);
        }
    }
}
