using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game31
{
    public class Button
    {
        public Texture2D image;
        public Vector2 origin;
        public Rectangle DestinationBox, mouseBox, visibleBox;
        public MouseState oldMouseState;
        bool isButtonPressed, isBoxGrowing;
        float length, totalGrowingAmount;

        public Button(Texture2D image, Vector2 position)
        {
            this.image = image;
            length = image.Width / 2;
            origin = new Vector2(image.Width / 2, image.Height / 2);
            DestinationBox = new Rectangle((int)(position.X - length/2), (int)(position.Y - origin.Y*length/image.Width), (int)length, (int)(origin.Y * 2*length/image.Width));
            mouseBox = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 0, 0);
            oldMouseState = Mouse.GetState();
            isButtonPressed = false;

            visibleBox = DestinationBox;
            isBoxGrowing = false;
            totalGrowingAmount = 0;
        }
        public bool IsButtonPressed()
        {
            Update();
            if (isButtonPressed)
            {
                isButtonPressed = false;
                totalGrowingAmount = 0;
                return true;
            }
            else
                return false;
        }
        public bool IsUpgradeButtonPressed()
        {
            Update();
            if (isButtonPressed)
            {
                isButtonPressed = false;
                return true;
            }
            else
                return false;
        }
        void Update()
        {
            float growingAmount = length;
            if (visibleBox.Intersects(mouseBox))
            {
                if (!isBoxGrowing && DestinationBox.X - visibleBox.X <= 0)
                    isBoxGrowing = true;
            }
            else isBoxGrowing = false;

            if (isBoxGrowing)
            {
                if (visibleBox.Width / DestinationBox.Width < 1.2)
                    totalGrowingAmount += growingAmount;
            }
            else if (DestinationBox.X - visibleBox.X > 0)
                totalGrowingAmount -= growingAmount;

            mouseBox = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 0, 0);
            if (visibleBox.Intersects(mouseBox) && oldMouseState.LeftButton == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Released)
                isButtonPressed = true;

            oldMouseState = Mouse.GetState();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            visibleBox = new Rectangle(
                (int)(DestinationBox.X - totalGrowingAmount / image.Height),
                (int)(DestinationBox.Y - totalGrowingAmount / image.Width),
                (int)(DestinationBox.Width + 2 * totalGrowingAmount / image.Height),
                (int)(DestinationBox.Height + 2 * totalGrowingAmount / image.Width));

            spriteBatch.Draw(image, visibleBox, Color.Yellow);
        }
    }
}
