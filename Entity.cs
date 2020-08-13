using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ace_game
{
    public class Entity
    {
        public Rectangle hitbox;
        public float hspeed, vspeed, hmax = 15;
        protected float gravity;
        protected Texture2D[] spriteArr;
        protected int drawIndex = 0;
        public int health { get; protected set; }
        protected float damageCooldown;

        public Entity(Texture2D[] sprites, Point pos, float grav, int hp)
        {
            hspeed = 0f;
            vspeed = 0f;
            spriteArr = sprites;
            hitbox = new Rectangle(pos, new Point(spriteArr[0].Width, spriteArr[0].Height));
            gravity = grav;
            health = hp;
            damageCooldown = 0f;
        }

        public virtual void Draw()
        {
            Main._spriteBatch.Draw(spriteArr[drawIndex], new Vector2(hitbox.Left, hitbox.Top), null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (damageCooldown > 0f)
                damageCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            CheckMovement(hspeed, "H");
            CheckMovement(vspeed, "V");
            vspeed = Math.Min(vspeed + gravity, 30f);
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
                    foreach (Tile t in Main.currentMap)
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
                    //checks if the entity is at the top of the level
                    if (hitbox.Top + displacement < -250f)
                    {
                        //moves the entity as far as possible
                        hitbox.Y = -150;
                        vspeed = 0f;
                        break;
                    }
                    //checks if the entity is at the bottom of the level
                    if (hitbox.Bottom + displacement > Main.mapBounds.Y)
                    {
                        //moves the entity as far as possible
                        hitbox.Y = Main.mapBounds.Y - spriteArr[drawIndex].Height;
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
                    foreach (Tile t in Main.currentMap)
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
                    //checks if the entity is at the left of the level
                    if (hitbox.Left + displacement < 0f)
                    {
                        //moves the entity as far as possible
                        hitbox.X = 0;
                        hspeed = 0f;
                        break;
                    }
                    //checks if the entity is at the right of the level
                    if (hitbox.Right + displacement > Main.mapBounds.X)
                    {
                        //moves the entity as far as possible
                        hitbox.X = Main.mapBounds.X - spriteArr[drawIndex].Width;
                        hspeed = 0f;
                        break;
                    }
                    //moves the entity
                    if (!collided)
                        hitbox.X += (int)displacement;
                    break;
            }
        }
        public virtual void takeDamage(int dmg, Entity source)
        {
            if (damageCooldown <= 0f)
            {
                health -= dmg;
                damageCooldown = 1f;
            }
        }
    }

}