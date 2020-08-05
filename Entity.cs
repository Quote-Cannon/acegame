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
            Func<int, int, int> gridClamp = (input, dimension) => Math.Clamp(input, 0, Game1.currentMap.GetLength(dimension)-1);
            Func<int, Tile[]> tilecheck;
            switch (direction)
            {
                //TODO: replace conditionals and tilechecks with lambdas to cut down on code duplication
                case "V":
                    //checks if there's a block below the player
                    tilecheck = side => new Tile[] {
                        Game1.currentMap[hitbox.Left / 32, gridClamp((int)((side + displacement) / 32), 1)],
                        Game1.currentMap[(hitbox.X + hitbox.Width / 2) / 32, gridClamp((int)((side + displacement) / 32), 1)],
                        Game1.currentMap[hitbox.Right / 32, gridClamp((int)((side + displacement) / 32), 1)]
                    };
                    foreach (Tile t in tilecheck(hitbox.Bottom))
                    if (t.Collider("DOWN"))
                    {
                        //moves the entity as far as possible
                        hitbox.Y = t.frame.Top - spriteArr[drawIndex].Height;
                        vspeed = 0f;
                        return;
                    }
                    //checks if there's a block above the player
                    foreach (Tile t in tilecheck(hitbox.Top))
                        if (t.Collider("UP"))
                    {
                        //moves the entity as far as possible
                        hitbox.Y = t.frame.Bottom + 1;
                        vspeed = 0f;
                        return;
                    }
                    //checks if the entity is at the top of the screen
                    if (hitbox.Top + displacement < 0f)
                    {
                        //moves the entity as far as possible
                        hitbox.Y = 0;
                        vspeed = 0f;
                        return;
                    }
                    //checks if the entity is at the bottom of the screen
                    if (hitbox.Bottom + displacement > Game1.screenHeight)
                    {
                        //moves the entity as far as possible
                        hitbox.Y = Game1.screenHeight-spriteArr[drawIndex].Height;
                        vspeed = 0f;
                        return;
                    }
                        //moves the entity
                        hitbox.Y = (int)(hitbox.Y + displacement);
                    break;
                case "H":
                    tilecheck = side => new Tile[] {
                        Game1.currentMap[gridClamp((int)((hitbox.Right + displacement) / 32), 0), hitbox.Top / 32],
                        Game1.currentMap[gridClamp((int)((hitbox.Right + displacement) / 32), 0), (hitbox.Y + hitbox.Height / 2) / 32],
                        Game1.currentMap[gridClamp((int)((hitbox.Right + displacement) / 32), 0), hitbox.Bottom / 32]
                    };
                    //check if there's a block right of the player
                    foreach (Tile t in tilecheck(hitbox.Right))
                        if (t.Collider("RIGHT"))
                    {
                        //moves the entity as far as possible
                        hitbox.X = t.frame.Left - 1 - spriteArr[drawIndex].Width;
                        hspeed = 0;
                        return;
                    }
                    //checks if there's a block left of the player
                    foreach (Tile t in tilecheck(hitbox.Left))
                    if (t.Collider("LEFT"))
                    {
                        //moves the entity as far as possible
                        hitbox.X = t.frame.Right + 1;
                        hspeed = 0f;
                        return;
                    }
                    //checks if the entity is at the right of the screen
                    if (hitbox.Right + displacement > Game1.screenWidth)
                    {
                        //moves the entity as far as possible
                        hitbox.X = Game1.screenWidth - spriteArr[drawIndex].Width;
                        hspeed = 0f;
                        return;
                    }
                    //checks if the entity is at the left of the screen
                    if (hitbox.Left + displacement < 0f)
                    {
                        //moves the entity as far as possible
                        hitbox.X = 0;
                        hspeed = 0f;
                        return;
                    }
                    //moves the entity
                    hitbox.X = (int)(hitbox.X + displacement);
                    break;
            }
        }
    }

}