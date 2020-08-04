using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ace_game
{
    class Entity
    {
        public Vector2 pos;
        public float hspeed;
        public float vspeed;
        protected Texture2D[] spriteArr;
        protected int drawIndex = 0;

        public virtual void Draw()
        {
            Game1._spriteBatch.Draw(spriteArr[drawIndex], pos, null, Color.White, 0f, new Vector2(spriteArr[drawIndex].Width, spriteArr[drawIndex].Height), Vector2.One, SpriteEffects.None, 0f);
        }

        public virtual void Update(GameTime gameTime)
        {
            vspeed = Math.Clamp(vspeed, -100f, 30f);
            hspeed = Math.Clamp(hspeed, -15f, 15f);
            CheckMovement(vspeed, "V");
            CheckMovement(hspeed, "H");

        }

        public void CheckMovement(float displacement, string direction)
        {
            switch (direction)
            {
                case "V":
                    if (pos.Y + displacement < spriteArr[drawIndex].Height)
                    {
                        pos.Y = spriteArr[drawIndex].Height;
                        vspeed = 0f;
                    }
                    else if (pos.Y + displacement > Game1.screenHeight)
                    {
                        pos.Y = Game1.screenHeight;
                        vspeed = 0f;
                    }
                    else
                        pos.Y += displacement;
                    break;
                case "H":
                    if (pos.X + displacement < spriteArr[drawIndex].Width)
                    {
                        pos.X = spriteArr[drawIndex].Width;
                        hspeed = 0f;
                    }
                    else if (pos.X + displacement > Game1.screenWidth)
                    {
                        pos.X = Game1.screenWidth;
                        hspeed = 0f;
                    }
                    else
                        pos.X += displacement;
                    break;
            }
        }
    }

}