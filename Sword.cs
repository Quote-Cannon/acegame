using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ace_game
{
    public class Sword : Entity
    {
        Player source;
        float timer;
        int side;
        public Sword(Point pos, Player src, int s) : base(new Texture2D[] { Main.sword_spr }, pos, 0.0f, -1)
        {
            source = src;
            timer = 0.5f;
            side = s;
        }

        public override void Update(GameTime gameTime)
        {
            if (side == 1)
                hitbox.X = source.hitbox.Right;
            else
                hitbox.X = source.hitbox.X - hitbox.Width;
            hitbox.Y = source.hitbox.Y + (int)((source.hitbox.Height - hitbox.Height) * 2 * (0.5f - timer));
            foreach (Enemy e in Main.enemies)
            {
                if (hitbox.Intersects(e.hitbox))
                {
                    e.takeDamage(20, source);
                    break;
                }

            }
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer <= 0)
                source.sword = null;
        }
    }
}
