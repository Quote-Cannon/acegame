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
            vspeed += 1f;
            if (kstate.IsKeyDown(Keys.W) && jumpDelay < 10)
            {
                if (pos.Y == Game1.screenHeight)
                {
                    vspeed = -30f;
                    jumpDelay = 10;
                }
                else
                    jumpDelay++;
            }
            else
            {
                jumpDelay = 10;
                if (vspeed < 0f)
                    vspeed += 6f;
            }
            if (kstate.IsKeyUp(Keys.W))
                jumpDelay = 0;
            if (kstate.IsKeyDown(Keys.S))
                vspeed += 4f;
            else if (vspeed < 0f)
                vspeed -= 6f;
            if (kstate.IsKeyDown(Keys.A))
                hspeed -= 4f;
            else if (hspeed < 0f)
                hspeed += 6f;
            if (kstate.IsKeyDown(Keys.D))
                hspeed += 4f;
            else if (hspeed > 0f)
                hspeed -= 5f;
        }
    }
}