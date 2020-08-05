using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ace_game
{
    class Player : Entity
    {
        int jumpDelay;
        public Player(Texture2D[] sprites)
        {
            hspeed = 0f;
            vspeed = 0f;
            spriteArr = sprites;
            hitbox = new Rectangle(100, 100, spriteArr[0].Width, spriteArr[0].Height);
            jumpDelay = 0;
            gravity = 1.4f;
        }

        public override void Update(GameTime gameTime, KeyboardState kstate, GamePadState gstate)
        {
            base.Update(gameTime, kstate, gstate);
            //if any of the jump buttons are pressed and a jump has been buffered <10 frames ago
            if ((kstate.IsKeyDown(Keys.W) || kstate.IsKeyDown(Keys.Up) || gstate.IsButtonDown(Buttons.A)) && jumpDelay < 10)
            {
                //if the player is on the ground and on a suitable platform (technically, when the bottom of the player and the top of the platform intersect)
                Tile collidable = Game1.currentMap[(hitbox.X + hitbox.Width / 2) / 32, Math.Clamp(hitbox.Bottom / 32, 0, Game1.currentMap.GetLength(1) - 1)];
                if (hitbox.Bottom > Game1.screenHeight - 2 || (collidable.Collider("DOWN") && collidable.frame.Top - hitbox.Bottom < 1))
                {
                    //yeet that sucker skyward
                    vspeed = -26f;
                    jumpDelay = 10;
                }
                else
                    //if the player is in the air, the buffer timer counts up
                    jumpDelay++;
            }
            //if no jump buttons are pressed, the buffer is reset
            if (kstate.IsKeyUp(Keys.W) && kstate.IsKeyUp(Keys.Up) && gstate.IsButtonUp(Buttons.A))
                jumpDelay = 0;
            if (kstate.IsKeyDown(Keys.A) || kstate.IsKeyDown(Keys.Left) || gstate.IsButtonDown(Buttons.DPadLeft) || gstate.ThumbSticks.Left.X < -0.5f)
                hspeed -= 0.6f;
            else if (hspeed < 0f)
                hspeed = Math.Clamp(hspeed + 0.9f, -10f, 0f);
            if (kstate.IsKeyDown(Keys.D) || kstate.IsKeyDown(Keys.Right) || gstate.IsButtonDown(Buttons.DPadRight) || gstate.ThumbSticks.Left.X > 0.5f)
                hspeed += 0.6f;
            else if (hspeed > 0f)
                hspeed = Math.Clamp(hspeed - 0.9f, 0f, 10f);
        }
    }
}