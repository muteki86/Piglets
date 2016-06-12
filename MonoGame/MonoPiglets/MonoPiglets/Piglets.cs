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

        private readonly PigEntity _pig1;

        public Piglets()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _pig1 = new PigEntity();
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

            Vector2 pigPos = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height/2);

            _pig1.Position = pigPos;
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
            _pig1.LoadSprites(Content);
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

            _pig1.Update();
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            _spriteBatch.Begin();

            _pig1.Draw(_spriteBatch);

            _spriteBatch.End();
        }
    }
}
