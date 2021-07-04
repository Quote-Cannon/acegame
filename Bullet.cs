using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ace_game
{
    public class Bullet : Entity
    {
        Player source;
        public bool destroy = false;
        public Bullet(Point pos, Player src, float vs, float hs) : base(new Texture2D[] { Main.bullet_spr }, pos, 0.0f, -1)
        {
            source = src;
            vspeed = vs;
            hspeed = hs;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (Enemy e in Main.enemies)
            {
                if (hitbox.Intersects(e.hitbox))
                {
                    e.takeDamage(5, source);
                    destroy = true;
                    break;
                }

            }
            if (vspeed == 0 && hspeed == 0)
                destroy = true;
        }

    }
}
