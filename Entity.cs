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
            Point gridCoords = Point.Zero;
            Func<int, int, string, bool> tilecheck;
            switch (direction)
            {
                //TODO: replace conditionals and tilechecks with lambdas to cut down on code duplication
                case "V":
                        //checks if there's a block above the player
                        gridCoords = new Point((hitbox.X + hitbox.Width / 2) / 32, Math.Clamp((int)((hitbox.Top + displacement) / 32), 0, Game1.currentMap.GetLength(1) - 1));
                    tilecheck = (offsetX, offsetY, direction) => Game1.currentMap[Math.Clamp(gridCoords.X + offsetX, 0, Game1.currentMap.GetLength(0) - 1), Math.Clamp(gridCoords.Y + offsetY, 0, Game1.currentMap.GetLength(0) - 1)].Collider(direction);
                    if (tilecheck(0, 0, "UP")|| tilecheck(-1, 0, "UP")|| tilecheck(1, 0, "UP"))
                        {
                            //moves the entity as far as possible
                            hitbox.Y = Game1.currentMap[gridCoords.X, gridCoords.Y].frame.Bottom + 1;
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
                        //checks if there's a block below the player
                        gridCoords = new Point((hitbox.X + hitbox.Width / 2) / 32, Math.Clamp((int)((hitbox.Bottom + displacement) / 32), 0, Game1.currentMap.GetLength(1) - 1));
                    tilecheck = (offsetX, offsetY, direction) => Game1.currentMap[Math.Clamp(gridCoords.X + offsetX, 0, Game1.currentMap.GetLength(0) - 1), Math.Clamp(gridCoords.Y + offsetY, 0, Game1.currentMap.GetLength(0) - 1)].Collider(direction);
                    if (tilecheck(0, 0, "DOWN") || tilecheck(-1, 0, "DOWN") || tilecheck(1, 0, "DOWN"))
                        {
                            //moves the entity as far as possible
                            hitbox.Y = Game1.currentMap[gridCoords.X, gridCoords.Y].frame.Top - spriteArr[drawIndex].Height;
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
                        hitbox.Y = (int)(hitbox.Y + displacement);
                    break;
                case "H":
                        //checks if there's a block left of the player
                        gridCoords = new Point(Math.Clamp((int)((hitbox.Left + displacement) / 32), 0, Game1.currentMap.GetLength(0) - 1), (hitbox.Y + hitbox.Height / 2) / 32);
                    tilecheck = (offsetX, offsetY, direction) => Game1.currentMap[Math.Clamp(gridCoords.X + offsetX, 0, Game1.currentMap.GetLength(0) - 1), Math.Clamp(gridCoords.Y + offsetY, 0, Game1.currentMap.GetLength(0) - 1)].Collider(direction);
                    if (tilecheck(0, 0, "LEFT")|| tilecheck(0, -1, "LEFT") || tilecheck(0, 1, "LEFT"))
                        {
                            //moves the entity as far as possible
                            hitbox.X = Game1.currentMap[gridCoords.X, gridCoords.Y].frame.Right;
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
                        //check if there's a block right of the player
                        gridCoords = new Point(Math.Clamp((int)((hitbox.Right + displacement) / 32), 0, Game1.currentMap.GetLength(0) - 1), (hitbox.Y + hitbox.Height / 2) / 32);
                    tilecheck = (offsetX, offsetY, direction) => Game1.currentMap[Math.Clamp(gridCoords.X + offsetX, 0, Game1.currentMap.GetLength(0) - 1), Math.Clamp(gridCoords.Y + offsetY, 0, Game1.currentMap.GetLength(0) - 1)].Collider(direction);
                    if (tilecheck(0, 0, "RIGHT") || tilecheck(0, -1, "RIGHT") || tilecheck(0, 1, "RIGHT"))
                        {
                            //moves the entity as far as possible
                            hitbox.X = Game1.currentMap[gridCoords.X, gridCoords.Y].frame.Left - spriteArr[drawIndex].Width;
                            hspeed = 0;
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
                    //moves the entity
                    hitbox.X = (int)(hitbox.X + displacement);
                    break;
            }
        }
    }

}