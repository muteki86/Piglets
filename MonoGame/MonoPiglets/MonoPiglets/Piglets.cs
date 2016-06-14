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

        public Piglets()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _pigs = new List<PigEntity>();
            foreach (var i in Enumerable.Range(0, 10))
            {
                _pigs.Add(new PigEntity());
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            Random rnd = new Random();

            foreach (var pigEntity in _pigs)
            {
                Vector2 pigPos = new Vector2(rnd.Next(0, GraphicsDevice.Viewport.TitleSafeArea.Right),
                    rnd.Next(0, GraphicsDevice.Viewport.TitleSafeArea.Bottom));

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Mouse.GetState(Window).LeftButton == ButtonState.Pressed)
            {
                var newPig = new PigEntity();
                newPig.LoadSprites(Content);

                Vector2 pigPos = new Vector2(Mouse.GetState(Window).X, Mouse.GetState(Window).Y);

                newPig.Position = pigPos;
                newPig.PigViewPort = GraphicsDevice.Viewport;
                _pigs.Add(newPig);
            }

            _pigs.ForEach(x => x.Update(gameTime));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            _spriteBatch.Begin();

            _pigs.ForEach(x => x.Draw(_spriteBatch));

            _spriteBatch.End();
        }
    }
}
