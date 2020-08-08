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
        float crouched;
        public Player(Texture2D[] sprites)
        {
            hspeed = 0f;
            vspeed = 0f;
            spriteArr = sprites;
            hitbox = new Rectangle(100, 100, spriteArr[0].Width, spriteArr[0].Height);
            jumpDelay = 0;
            gravity = 1.4f;
            crouched = 0;
        }

        public override void Update(GameTime gameTime, KeyboardState kstate, GamePadState gstate)
        {
            if (crouched > 0f)
                crouched -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (crouched < 0f)
            {
                crouched = 0f;
                hitbox = new Rectangle(hitbox.X, hitbox.Y - hitbox.Height, spriteArr[0].Width, spriteArr[0].Height);
            }

            base.Update(gameTime, kstate, gstate);
            //if any of the jump buttons are pressed and a jump has been buffered <10 frames ago
            if ((kstate.IsKeyDown(Keys.W) || kstate.IsKeyDown(Keys.Up) || gstate.IsButtonDown(Buttons.A)) && jumpDelay < 10)
            {
                //yeet that sucker skyward
                if (checkGrounded())
                {
                    vspeed = -27f;
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
            if ((kstate.IsKeyDown(Keys.S) || kstate.IsKeyDown(Keys.Down) || gstate.IsButtonDown(Buttons.RightShoulder)) && crouched <= 0 && checkGrounded())
            {
                crouched = 0.5f;
                hitbox = new Rectangle(hitbox.X, hitbox.Y + hitbox.Height / 2, spriteArr[3].Width, spriteArr[3].Height);
            }
        }

        private bool checkGrounded()
        {
            Tile[,] temp = Main.currentMap;
            //if the player is on the ground and on a suitable platform (technically, when the bottom of the player and the top of the platform intersect)
            foreach (Tile t in new Tile[] {
                    Main.currentMap[(hitbox.X + hitbox.Width / 2) / 32, Math.Clamp(hitbox.Bottom / 32, 0, temp.GetLength(1) - 1)],
                    Main.currentMap[Math.Clamp((hitbox.X + hitbox.Width-1) / 32, 0, temp.GetLength(0)), Math.Clamp(hitbox.Bottom / 32, 0, temp.GetLength(1) - 1)],
                    Main.currentMap[Math.Clamp(hitbox.X / 32, 0, temp.GetLength(0)), Math.Clamp(hitbox.Bottom / 32, 0, temp.GetLength(1) - 1)]
                })
                if (hitbox.Bottom > Main.screenHeight - 2 || (t.Collider("DOWN") && t.frame.Top - hitbox.Bottom < 1))
                    return true;
            return false;
        }

        private int findDrawIndex()
        {
            if (crouched > 0f)
                return 3;
            //if (Math.Abs(vspeed) > 10f)
                //return 2;
            //if (Math.Abs(hspeed) > 10f)
                //return 1;
            return 0;
        }

        public override void Draw()
        {
            drawIndex = findDrawIndex();
            base.Draw();
        }
    }
}