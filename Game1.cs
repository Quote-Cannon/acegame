using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ace_game
{
    public class Game1 : Game
    {
        Texture2D aceDash_spr, aceIdle_spr, aceJump_spr, aceSlide_spr, block_spr, bullet_spr, enemy_spr, sword_spr;
        Player player;

        private GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;
        public static int screenWidth;
        public static int screenHeight;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            aceDash_spr = Content.Load<Texture2D>("aceflag_dash");
            aceIdle_spr = Content.Load<Texture2D>("aceflag_idle");
            aceJump_spr = Content.Load<Texture2D>("aceflag_jump");
            aceSlide_spr = Content.Load<Texture2D>("aceflag_slide");
            block_spr = Content.Load<Texture2D>("block_placeholder");
            bullet_spr = Content.Load<Texture2D>("bullet_placeholder");
            enemy_spr = Content.Load<Texture2D>("enemy_placeholder");
            sword_spr = Content.Load<Texture2D>("sword_placeholder");
            player = new Player(new Texture2D[] { aceIdle_spr, aceDash_spr, aceJump_spr, aceSlide_spr });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here
            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            player.Draw();
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}