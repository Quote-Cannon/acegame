using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ace_game
{
    public class Player : Entity
    {
        int jumpDelay;
        bool crouched;
        public Camera camera;
        public Sword sword;
        public List<Bullet> bullets = new List<Bullet>();
        double bulletCooldown;
        public float mana;
        //left = -1, right = 1
        int facing;
        public Player(Texture2D[] sprites, Point pos, float grav, Point screen) : base(sprites, pos, grav, 100)
        {
            jumpDelay = 0;
            crouched = false;
            camera = new Camera(pos, screen);
            bulletCooldown = 0f;
            mana = 100;
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState gstate = GamePad.GetState(PlayerIndex.One);
            KeyboardState kstate = Keyboard.GetState();
            MouseState mstate = Mouse.GetState();

            base.Update(gameTime);
            //if any of the jump buttons are pressed and a jump has been buffered <10 frames ago
            if ((kstate.IsKeyDown(Keys.W) || kstate.IsKeyDown(Keys.Z) || gstate.IsButtonDown(Buttons.A)) && jumpDelay < 10)
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
            if (kstate.IsKeyUp(Keys.W) && kstate.IsKeyUp(Keys.Z) && gstate.IsButtonUp(Buttons.A))
                jumpDelay = 0;
            if (kstate.IsKeyDown(Keys.A) || kstate.IsKeyDown(Keys.Left) || gstate.IsButtonDown(Buttons.DPadLeft) || gstate.ThumbSticks.Left.X < -0.5f)
                hspeed = Math.Max(hspeed - 0.6f, -hmax);
            else if (hspeed < 0f)
                hspeed = Math.Min(hspeed + 0.9f, 0f);
            if (kstate.IsKeyDown(Keys.D) || kstate.IsKeyDown(Keys.Right) || gstate.IsButtonDown(Buttons.DPadRight) || gstate.ThumbSticks.Left.X > 0.5f)
                hspeed = Math.Min(hspeed + 0.6f, hmax);
            else if (hspeed > 0f)
                hspeed = Math.Max(hspeed - 0.9f, 0f);
            if (!crouched && (kstate.IsKeyDown(Keys.S) || kstate.IsKeyDown(Keys.Down) || gstate.IsButtonDown(Buttons.DPadDown) || gstate.ThumbSticks.Left.Y < -0.5f) && checkGrounded())
            {
                crouched = true;
                hmax /= 4f;
                hitbox = new Rectangle(hitbox.X, hitbox.Y + hitbox.Height / 2, spriteArr[3].Width, spriteArr[3].Height);
            }
            if (crouched && kstate.IsKeyUp(Keys.S) && kstate.IsKeyUp(Keys.Down) && gstate.IsButtonUp(Buttons.DPadDown) && gstate.ThumbSticks.Left.Y > -0.5f && !checkCrawling())
            {
                crouched = false;
                hmax *= 4f;
                hitbox = new Rectangle(hitbox.X, hitbox.Y - hitbox.Height, spriteArr[0].Width, spriteArr[0].Height);
            }

            if (hspeed < 0)
                facing = -1;
            else if (hspeed > 0)
                facing = 1;

            if ((mstate.LeftButton == ButtonState.Pressed || kstate.IsKeyDown(Keys.X) || gstate.IsButtonDown(Buttons.X)) && sword == null)
            {
                Point p;
                if (facing == 1)
                    p = new Point(hitbox.Right, hitbox.Y);
                else
                    p = new Point(hitbox.X - hitbox.Width, hitbox.Y);
                sword = new Sword(p, this, facing);
            }
            if ((mstate.RightButton == ButtonState.Pressed || kstate.IsKeyDown(Keys.C) || gstate.IsButtonDown(Buttons.RightShoulder)) && bullets.Count < 5 && bulletCooldown <= 0 && mana >= 5)
            {
                Point p;
                if (facing == 1)
                    p = new Point(hitbox.Right, hitbox.Y + hitbox.Height / 2);
                else
                    p = new Point(hitbox.X, hitbox.Y + hitbox.Height / 2);
                bullets.Add(new Bullet(p, this, 0, 30f * facing));
                mana -= 5;
                bulletCooldown = 0.5;
            }
            else if (sword != null)
                sword.Update(gameTime);
            foreach (Bullet b in bullets)
                b.Update(gameTime);
            bullets.RemoveAll(b => b.destroy);
            bulletCooldown -= gameTime.ElapsedGameTime.TotalSeconds;
            if (hitbox.X + camera.screen.X > camera.screen.Width / 3 * 2)
                camera.screen.X = Math.Clamp(camera.screen.Width / 3 * 2 - hitbox.X, -(Main.mapBounds.X - camera.screen.Width), 0);
            if (camera.screen.X + hitbox.X < camera.screen.Width / 3)
                camera.screen.X = Math.Clamp(camera.screen.Width / 3 - hitbox.X, -(Main.mapBounds.X - camera.screen.Width), 0);
            if (hitbox.Y + camera.screen.Y > camera.screen.Height / 3 * 2)
                camera.screen.Y = Math.Clamp(camera.screen.Height / 3 * 2 - hitbox.Y, -(Main.mapBounds.Y - camera.screen.Height), 0);
            if (camera.screen.Y + hitbox.Y < camera.screen.Height / 3)
                camera.screen.Y = Math.Clamp(camera.screen.Height / 3 - hitbox.Y, -(Main.mapBounds.Y - camera.screen.Height), 0);
            if (mana < 100)
                mana += (float)gameTime.ElapsedGameTime.TotalSeconds * 2f;
            camera.Update();
        }

        private bool checkGrounded()
        {
            Tile[,] temp = Main.currentMap;
            //if the player is on the ground and on a suitable platform (technically, when the bottom of the player and the top of the platform intersect)
            foreach (Tile t in new Tile[] {
                    temp[(hitbox.X + hitbox.Width / 2) / 32, Math.Clamp(hitbox.Bottom / 32, 0, temp.GetLength(1) - 1)],
                    temp[Math.Clamp((hitbox.X + hitbox.Width-1) / 32, 0, temp.GetLength(0)), Math.Clamp(hitbox.Bottom / 32, 0, temp.GetLength(1) - 1)],
                    temp[Math.Clamp(hitbox.X / 32, 0, temp.GetLength(0)), Math.Clamp(hitbox.Bottom / 32, 0, temp.GetLength(1) - 1)]
                })
                if (t.Collider("DOWN") && t.frame.Top - hitbox.Bottom < 1)
                    return true;
            return false;
        }

        private bool checkCrawling()
        {
            Tile[,] temp = Main.currentMap;
            //if the player is on the ground and on a suitable platform (technically, when the bottom of the player and the top of the platform intersect)
            foreach (Tile t in new Tile[] {
                    temp[(hitbox.X + hitbox.Width / 2) / 32, Math.Clamp(hitbox.Top / 32-1, 0, temp.GetLength(1) - 1)],
                    temp[Math.Clamp((hitbox.X + hitbox.Width-1) / 32, 0, temp.GetLength(0)), Math.Clamp(hitbox.Top / 32 -1, 0, temp.GetLength(1) - 1)],
                    temp[Math.Clamp(hitbox.X / 32, 0, temp.GetLength(0)), Math.Clamp(hitbox.Top / 32-1, 0, temp.GetLength(1) - 1)]
                })
                if (t.Collider("UP") && hitbox.Top >= t.frame.Bottom)
                    return true;
            return false;
        }

        private int findDrawIndex()
        {
            if (crouched)
                return 3;
            /*if (Math.Abs(vspeed) > 10f)
                return 2;
            if (Math.Abs(hspeed) > 10f)
                return 1;*/
            return 0;
        }

        public override void takeDamage(int dmg, Entity source)
        {
            if (damageCooldown <= 0f)
            {
                health -= dmg;
                damageCooldown = 1f;
                Point origin = new Point(source.hitbox.X + source.hitbox.Width, source.hitbox.Y + source.hitbox.Height);
                Point center = new Point(hitbox.X + hitbox.Width, hitbox.Y + hitbox.Height);
                hspeed = (center.X - origin.X) / 2;
                vspeed = (center.Y - origin.Y) / 2;
                if (health <= 0)
                    Main.kill = true;
            }
        }

        public override void Draw()
        {
            drawIndex = findDrawIndex();
            if ((int)(damageCooldown * 100) % 2 == 0)
                base.Draw();
            if (sword != null)
                sword.Draw();
            foreach (Bullet b in bullets)
                b.Draw();
        }
    }
}