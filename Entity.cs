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
            CheckMovement(hspeed, "H");
            CheckMovement(vspeed, "V");
            vspeed += gravity;
        }

        public void CheckMovement(float displacement, string direction)
        {
            Rectangle temp;
            bool collided = false;
            switch (direction)
            {
                //TODO: replace conditionals and tilechecks with lambdas to cut down on code duplication
                //TODO: more efficient algorithm (check only the blocks around the entity)
                case "V":
                    temp = hitbox;
                    temp.Y += (int)displacement;
                    foreach (Tile t in Game1.currentMap)
                        if (t.frame.Intersects(temp))
                            if (displacement < 0 && t.Collider("UP"))
                            {
                                //moves the entity as far as possible
                                hitbox.Y = t.frame.Bottom;
                                vspeed = 0f;
                                collided = true;
                            }
                            else if (t.Collider("DOWN"))
                            {
                                //moves the entity as far as possible
                                hitbox.Y = t.frame.Top - spriteArr[drawIndex].Height;
                                vspeed = 0f;
                                collided = true;
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
                        hitbox.Y = Game1.screenHeight - spriteArr[drawIndex].Height;
                        vspeed = 0f;
                        break;
                    }
                    //moves the entity
                    if (!collided)
                        hitbox.Y += (int)displacement;
                    break;
                case "H":
                    temp = hitbox;
                    temp.X += (int)displacement;
                    foreach (Tile t in Game1.currentMap)
                        if (t.frame.Intersects(temp))
                            if (displacement < 0 && t.Collider("LEFT"))
                            {
                                //moves the entity as far as possible
                                hitbox.X = t.frame.Right;
                                hspeed = 0f;
                                collided = true;
                            }
                            else if (t.Collider("RIGHT"))
                            {
                                //moves the entity as far as possible
                                hitbox.X = t.frame.Left - spriteArr[drawIndex].Width;
                                hspeed = 0;
                                collided = true;
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
                    if (!collided)
                        hitbox.X += (int)displacement;
                    break;
            }
        }
    }

}