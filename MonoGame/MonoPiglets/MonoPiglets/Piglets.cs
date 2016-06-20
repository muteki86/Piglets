using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPiglets
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Piglets : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private List<PigEntity> _pigs;

        private TerrainEntity _terrain;

        private int _elapsedMilliseconds;

        private Texture2D _mouseTexture;

        private float _mouseX;

        private float _mouseY;

        public Piglets()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 650,
                PreferredBackBufferWidth = 800
            };

            Content.RootDirectory = "Content";
            _pigs = new List<PigEntity>();
            foreach (var i in Enumerable.Range(0, 10))
            {
                _pigs.Add(new PigEntity());
            }
            _terrain = new TerrainEntity();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();

            _mouseTexture = Content.Load<Texture2D>("mouse");

            Random rnd = new Random();
            //GraphicsDevice.Viewport.TitleSafeArea.

            _terrain.X = 65;//GraphicsDevice.Viewport.TitleSafeArea.Bottom / 5;
            _terrain.Y = 65;//GraphicsDevice.Viewport.TitleSafeArea.Bottom / 5;
            _terrain.Initialize();

            foreach (var pigEntity in _pigs)
            {
                var terrainInfo = new TerrainInfo { IsValidForPig = false };

                Vector2 pigPos = new Vector2();
                while (!terrainInfo.IsValidForPig)
                {
                    pigPos = new Vector2(rnd.Next(0, 645),
                       rnd.Next(0, 645));

                    terrainInfo = _terrain.GetTerrainInfo((int)pigPos.X, (int)pigPos.Y);
                }
                pigEntity.Position = pigPos;
                pigEntity.PigViewPort = GraphicsDevice.Viewport;
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _terrain.LoadSprites(Content);

            // TODO: use this.Content to load your game content here
            _pigs.ForEach(x => x.LoadSprites(Content));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            else
            {
                if (Mouse.GetState(Window).LeftButton == ButtonState.Pressed && _elapsedMilliseconds > 200)
                {
                    var pigPos = new Vector2(Mouse.GetState(Window).X, Mouse.GetState(Window).Y);
                    var terrainInfo = _terrain.GetTerrainInfo((int)pigPos.X, (int)pigPos.Y);

                    if (terrainInfo.IsValidForPig)
                    {
                        var newPig = new PigEntity();
                        newPig.LoadSprites(Content);
                        newPig.Position = pigPos;
                        newPig.PigViewPort = GraphicsDevice.Viewport;
                        _pigs.Add(newPig);
                        _elapsedMilliseconds = 0;
                    }
                }
                else
                {
                    _elapsedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                }

                _mouseX = Mouse.GetState(Window).X;
                _mouseY = Mouse.GetState(Window).Y;

                _pigs.ForEach(x => x.Update(gameTime, _terrain));
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Beige);

            base.Draw(gameTime);
            _spriteBatch.Begin();

            _terrain.Draw(_spriteBatch);

            _pigs.ForEach(x => x.Draw(_spriteBatch));

            _spriteBatch.Draw(_mouseTexture, new Vector2(_mouseX, _mouseY));

            _spriteBatch.End();
        }
    }
}
