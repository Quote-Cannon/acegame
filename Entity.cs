using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ace_game
{
    class Entity
    {
        public Rectangle hitbox;
        public float hspeed, vspeed;
        protected float gravity;
        protected Texture2D[] spriteArr;
        protected int drawIndex = 0;

        public virtual void Draw()
        {
            Game1._spriteBatch.Draw(spriteArr[drawIndex], new Vector2(hitbox.Left, hitbox.Top), null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        public virtual void Update(GameTime gameTime, KeyboardState kstate, GamePadState gstate)
        {
            vspeed = Math.Clamp(vspeed, -100f, 30f);
            hspeed = Math.Clamp(hspeed, -15f, 15f);
            CheckMovement(vspeed, "V");
            CheckMovement(hspeed, "H");
            vspeed += gravity;
        }

        public void CheckMovement(float displacement, string direction)
        {
            Tile tilecheck;
            switch (direction)
            {
                //TODO: replace conditionals and tilechecks with lambdas to cut down on code duplication
                case "V":
                    //checks if there's a block below the player
                    tilecheck = Game1.currentMap[(hitbox.X + hitbox.Width / 2) / 32, Math.Clamp((int)((hitbox.Bottom + displacement) / 32), 0, Game1.currentMap.GetLength(1)-1)];
                    if (tilecheck.Collider("DOWN"))
                    {
                        //moves the entity as far as possible
                        hitbox.Y = tilecheck.frame.Top - spriteArr[drawIndex].Height;
                        vspeed = 0f;
                        break;
                    }
                    //checks if there's a block above the player
                    tilecheck = Game1.currentMap[(hitbox.X + hitbox.Width / 2) / 32, Math.Clamp((int)((hitbox.Top + displacement) / 32), 0, Game1.currentMap.GetLength(1)-1)];
                    if (tilecheck.Collider("UP"))
                    {
                        //moves the entity as far as possible
                        hitbox.Y = tilecheck.frame.Bottom + 1;
                        vspeed = 0f;
                        break;
                    }
                    //checks if the entity is at the top of the screen
                    if (hitbox.Top + displacement < 0f)
                    {
                        //moves the entity as far as possible
                        hitbox.Y = 0;
                        vspeed = 0f;
                        break;
                    }
                    //checks if the entity is at the bottom of the screen
                    if (hitbox.Bottom + displacement > Game1.screenHeight)
                    {
                        //moves the entity as far as possible
                        hitbox.Y = Game1.screenHeight-spriteArr[drawIndex].Height;
                        vspeed = 0f;
                        break;
                    }
                        //moves the entity
                        hitbox.Y = (int)(hitbox.Y + displacement);
                    break;
                case "H":
                    //check if there's a block right of the player
                    tilecheck = Game1.currentMap[Math.Clamp((int)((hitbox.Right + displacement) / 32), 0, Game1.currentMap.GetLength(0) - 1), (hitbox.Y + hitbox.Height / 2) / 32];
                    if (tilecheck.Collider("RIGHT"))
                    {
                        //moves the entity as far as possible
                        hitbox.X = tilecheck.frame.Left - 1 - spriteArr[drawIndex].Width;
                        hspeed = 0;
                        break;
                    }
                    //checks if there's a block left of the player
                    tilecheck = Game1.currentMap[Math.Clamp((int)((hitbox.Left + displacement) / 32), 0, Game1.currentMap.GetLength(0) - 1), (hitbox.Y + hitbox.Height / 2) / 32];
                    if (tilecheck.Collider("LEFT"))
                    {
                        //moves the entity as far as possible
                        hitbox.X = tilecheck.frame.Right + 1;
                        hspeed = 0f;
                        break;
                    }
                    //checks if the entity is at the right of the screen
                    if (hitbox.Right + displacement > Game1.screenWidth)
                    {
                        //moves the entity as far as possible
                        hitbox.X = Game1.screenWidth - spriteArr[drawIndex].Width;
                        hspeed = 0f;
                        break;
                    }
                    //checks if the entity is at the left of the screen
                    if (hitbox.Left + displacement < 0f)
                    {
                        //moves the entity as far as possible
                        hitbox.X = 0;
                        hspeed = 0f;
                        break;
                    }
                    //moves the entity
                    hitbox.X = (int)(hitbox.X + displacement);
                    break;
            }
        }
    }

}