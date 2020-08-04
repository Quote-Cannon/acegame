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
            pos = new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2);
            hspeed = 0f;
            vspeed = 0f;
            spriteArr = sprites;
            jumpDelay = 0;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();
            base.Update(gameTime);
            vspeed += 1.4f;
            if (kstate.IsKeyDown(Keys.W) && jumpDelay < 10)
            {
                if (pos.Y == Game1.screenHeight)
                {
                    vspeed = -35f;
                    jumpDelay = 10;
                }
                else
                    jumpDelay++;
            }
            else
            {
                jumpDelay = 10;
            }
            if (kstate.IsKeyUp(Keys.W))
                jumpDelay = 0;
            if (kstate.IsKeyDown(Keys.A))
                hspeed -= 0.6f;
            else if (hspeed < 0f)
                hspeed = Math.Clamp(hspeed + 0.9f, -10f, 0f);
            if (kstate.IsKeyDown(Keys.D))
                hspeed += 0.6f;
            else if (hspeed > 0f)
                hspeed = Math.Clamp(hspeed - 0.9f, 0f, 10f);
        }
    }
}