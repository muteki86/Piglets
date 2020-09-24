using chiscore;
using chiscore.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Piglets2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private EntityManager _manager;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Window.AllowUserResizing = true;

            _graphics.IsFullScreen = true;

            Window.ClientSizeChanged+=Window_ClientSizeChanged;
            //_graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            Window.ClientSizeChanged -= Window_ClientSizeChanged;
            _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width < 100 ? 100 : Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height < 100 ? 100 : Window.ClientBounds.Height;

            _graphics.ApplyChanges();
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _manager = new EntityManager();
            
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        
            // TODO: use this.Content to load your game content here
            var player = new Entity();
            var ballTexture = Content.Load<Texture2D>("images/ball");
            var transform = new TransformComponent
            {
                Height = ballTexture.Height,
                Width = ballTexture.Width,
                Position =  new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2),
                Speed = 500f
            };
            player.AddComponent(transform);
            player.AddComponent(new SpriteComponent
            {
                Scale = 1,
                Texture = ballTexture,
                Transform = transform,
                SpriteBatch = _spriteBatch
            });
            player.AddComponent(new KeyboardComponent
            {
                Graphics = _graphics,
                Transform = transform
            });
            _manager.AddEntity(player);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _manager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _manager.Draw();
            base.Draw(gameTime);
        }
    }
}
