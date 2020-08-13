using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ace_game
{
    public class Camera
    {
        public Rectangle screen;
        public Matrix transMatrix;

        public Camera(Point p, Point s)
        {
            screen = new Rectangle(p.X, p.Y, s.X, s.Y);
            Update();
        }

        public void Update()
        {
            transMatrix = Matrix.CreateTranslation(screen.X, screen.Y, 0f);
        }

        public void Draw(string fps, Point playerCoords, int health)
        {
            Main._spriteBatch.DrawString(Main.defaultFont, fps + $"\n player hitbox: {playerCoords.X}, {playerCoords.Y}\ncamera coords: {screen.X}, {screen.Y}\nHealth: {health}", new Vector2(-screen.X, -screen.Y), Color.Black);
            Color healthColor = new Color((int)Math.Min(510 - health * 5.1f, 255), (int)Math.Min(health * 5.1f, 255), 0);
            Main._spriteBatch.Draw(Main.empty, new Rectangle(screen.Width - 176 - screen.X, 16 - screen.Y, (int)(1.6f * health), 16), healthColor);
        }
    }
}
