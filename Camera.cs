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

        public void Draw(string fps, Point playerCoords, int health, float mana)
        {
            Main._spriteBatch.DrawString(Main.defaultFont, fps + $"\n player hitbox: {playerCoords.X}, {playerCoords.Y}\ncamera coords: {screen.X}, {screen.Y}", new Vector2(-screen.X, -screen.Y), Color.Black);
            Color healthColor = new Color((int)Math.Min(510 - health * 5.1f, 255), (int)Math.Min(health * 5.1f, 255), 0);
            Color manaColor = new Color(1-mana/100f, 1-mana/100f, 1);
            Main._spriteBatch.Draw(Main.empty, new Rectangle(screen.Width - 352 - screen.X, 32 - screen.Y, (int)(3.2f * health), 32), healthColor);
            Main._spriteBatch.Draw(Main.empty, new Rectangle(screen.Width - 352 - screen.X, 80 - screen.Y, (int)(3.2f * mana), 32), manaColor);
        }
    }
}
