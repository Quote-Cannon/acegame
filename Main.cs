using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ace_game
{
    public class Main : Game
    {
        //TODO: use get/sets everywhere
        public static bool kill = false;
        public static Random rnd = new Random();
        public static Texture2D aceDash_spr, aceIdle_spr, aceJump_spr, aceSlide_spr, block_spr, bullet_spr, enemy_spr, sword_spr;
        public static Texture2D empty;
        public static SpriteFont defaultFont;
        public static Player player;
        public FrameCounter _frameCounter = new FrameCounter();
        public static Tile[,] currentMap;
        public static List<Enemy> enemies = new List<Enemy>();

        private GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;
        public static Point mapBounds;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
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
            defaultFont = Content.Load<SpriteFont>("defaultFont");
            empty = Content.Load<Texture2D>("pixel");
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            player = new Player(new Texture2D[] { aceIdle_spr, aceDash_spr, aceJump_spr, aceSlide_spr }, new Point(100, 100), 1.4f, new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
            currentMap = loadMap(@"./layouts/testmap_large.txt");
            mapBounds = new Point(currentMap.GetLength(0) * 32, currentMap.GetLength(1) * 32);
        }

        protected override void Update(GameTime gameTime)
        {
            GamePadState gstate = GamePad.GetState(PlayerIndex.One);
            KeyboardState kstate = Keyboard.GetState();
            if (gstate.Buttons.Back == ButtonState.Pressed || kstate.IsKeyDown(Keys.Escape) || kill)
                Exit();
            if (kstate.IsKeyDown(Keys.F4))
            {
                _graphics.IsFullScreen = true;
                _graphics.ApplyChanges();
            }

            // TODO: Add your update logic here
            player.Update(gameTime);
            foreach (Enemy e in enemies)
                e.Update(gameTime);
            enemies.RemoveAll(e => e.destroy);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //fps counter
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameCounter.Update(deltaTime);

            var fps = string.Format("FPS: {0}", Math.Round(_frameCounter.AverageFramesPerSecond));


            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, player.camera.transMatrix);
            drawMap(currentMap);
            foreach (Enemy e in enemies)
                e.Draw();
            player.Draw();
            player.camera.Draw(fps, player.hitbox.Location, player.health, player.mana);
            _spriteBatch.End();


            base.Draw(gameTime);
        }

        private Tile[,] loadMap(string path)
        {
            string[] lines = File.ReadAllLines(path);
            Tile[,] map = new Tile[lines[0].Length, lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    char c = line[j];
                    switch (c)
                    {
                        case '*':
                            map[j, i] = new Block(j * 32, i * 32, block_spr);
                            break;
                        case 'e':
                            map[j, i] = new Tile();
                            enemies.Add(new Enemy(new Texture2D[] { enemy_spr }, new Point(j * 32, i * 32), 1.4f, 25, new int[] { 10 }));
                            break;
                        default:
                            map[j, i] = new Tile();
                            break;
                    }
                }
            }
            return map;
        }

        private void drawMap(Tile[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    if (map[i, j] != null)
                        map[i, j].Draw();
        }
    }

    public class FrameCounter
    {
        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }

        public const int MAXIMUM_SAMPLES = 100;

        private Queue<float> _sampleBuffer = new Queue<float>();

        public bool Update(float deltaTime)
        {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            _sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (_sampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                _sampleBuffer.Dequeue();
                AverageFramesPerSecond = _sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }
            TotalFrames++;
            TotalSeconds += deltaTime;
            return true;
        }
    }
}