using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game31
{
    public class GameObject
    {
        public Texture2D image;
        public Vector2 position, origin, speed;
        public float angle, length;
        public Color color;
        public float gravitationalConstant;
        public Vector2 screenParameters = new Vector2(1366, 768);

        public GameObject()
        {
            angle = 0;
            color = Color.White;
            gravitationalConstant = 6.673848080808080808f;
        }
        public void ApplyGravity(List<Planet> planetList, float elapsed)
        {
            foreach (Planet planet in planetList)
                if (planet.position != position)
                    speed += elapsed * (planet.position - position) * gravitationalConstant * planet.mass / Distance(planet.position, position) / Distance(planet.position, position) / Distance(planet.position, position);
        }
        public float Distance(Vector2 point1, Vector2 point2)
        {
            return (float)Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
        }
    }
}
