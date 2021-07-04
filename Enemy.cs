using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ace_game
{
    public class Enemy : Entity
    {
        Point spawnPoint;
        int[] damageValues;
        public bool destroy = false;
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
            if (hspeed == 0f)
            {
                hspeed = Main.rnd.Next(-10, 10);
            }
        }

        public override void takeDamage(int dmg, Entity source)
        {
            base.takeDamage(dmg, source);
            if (health <= 0)
                destroy = true;
        }
    }
}