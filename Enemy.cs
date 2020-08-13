using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ace_game
{
    class Enemy : Entity
    {
        Point spawnPoint;
        int[] damageValues;
        public Enemy(Texture2D[] sprites, Point spawn, float grav, int hp, int[] attacks) : base(sprites, spawn, grav, hp)
        {
            spawnPoint = spawn;
            damageValues = attacks;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (hitbox.Intersects(Main.player.hitbox))
            {
                //contact damage
                Main.player.takeDamage(damageValues[0], this);
            }
        }
    }
}
