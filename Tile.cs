using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ace_game
{
    public class Tile
    {
        protected Texture2D sprite;
        public Rectangle frame;

        public void Draw()
        {
            if (sprite != null)
                Game1._spriteBatch.Draw(sprite, new Vector2(frame.X, frame.Y), null, Color.White, 0f, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
        }

        public virtual bool Collider(string direction)
        {
            return false;
        }
    }
}
