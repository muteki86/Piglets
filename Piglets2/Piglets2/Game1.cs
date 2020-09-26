using System.Collections.Generic;
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
        private Camera _camera;
        private Entity _player;
        private int WINDOW_WIDTH ;
        private int WINDOW_HEIGHT ;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //IsMouseVisible = true;
            //this.Window.AllowUserResizing = true;
            //_graphics.IsFullScreen = true;

            //Window.ClientSizeChanged+=Window_ClientSizeChanged;
            WINDOW_WIDTH = Window.ClientBounds.Width;// GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            WINDOW_HEIGHT = Window.ClientBounds.Height;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            //_graphics.PreferredBackBufferWidth = WINDOW_WIDTH;//;
            //_graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //_graphics.ApplyChanges();

            _camera = Camera.GetInstance();
            _camera.X = 0;
            _camera.Y = 0;
            _camera.W = WINDOW_WIDTH;
            _camera.H = WINDOW_HEIGHT;
        }

        private void HandleCamera()
        {
            TransformComponent playerTransform = (TransformComponent) _player.GetComponent(typeof(TransformComponent));
            var newx = (int) (playerTransform.Position.X - (WINDOW_WIDTH / 2));
            var newy = (int) (playerTransform.Position.Y - (WINDOW_HEIGHT / 2));
            
            Camera.GetInstance().X = newx;
            Camera.GetInstance().Y = newy;
            
            Camera.GetInstance().X = Camera.GetInstance().X < 0 ? 0 : Camera.GetInstance().X;
            Camera.GetInstance().Y = Camera.GetInstance().Y < 0 ? 0 : Camera.GetInstance().Y;
            Camera.GetInstance().X = Camera.GetInstance().X > Camera.GetInstance().W ? Camera.GetInstance().W : Camera.GetInstance().X;
            Camera.GetInstance().Y = Camera.GetInstance().Y > Camera.GetInstance().H ? Camera.GetInstance().H : Camera.GetInstance().Y;
            
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

            var mapTexture = Content.Load<Texture2D>("tiles/terrain");
            var mapgen = new Map { 
                Manager = _manager,
                SpriteBatch = _spriteBatch,
                Scale = 1,
                TileSize = 32,
                MapTextures = mapTexture
            };
            mapgen.LoadMap(null, 100,100);

            _player = new Entity();
            var ballTexture = Content.Load<Texture2D>("images/chopper-spritesheet");
            var transform = new TransformComponent
            {
                Height = 32,
                Width = 32,
                Position =  new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2),
                Speed = new Vector2(0,0)
            };
            _player.AddComponent(transform);
            
            var dani = new Animation
            {
                Index = 0,
                Speed = 60,
                NumFrames = 2
            };
            var rani = new Animation
            {
                Index = 1,
                Speed = 60,
                NumFrames = 2
            };
            var lani = new Animation
            {
                Index = 2,
                Speed = 60,
                NumFrames = 2
            };
            var uani = new Animation
            {
                Index = 3,
                Speed = 60,
                NumFrames = 2
            };

            var animations = new Dictionary<string, Animation>
            {
                ["right"] = rani,
                ["down"] = dani,
                ["left"] = lani,
                ["up"] = uani,
            };

            var spriteCmp = new SpriteComponent
            {
                Scale = 1,
                Texture = ballTexture,
                Transform = transform,
                SpriteBatch = _spriteBatch,
                IsAnimated = true,
                Animations = animations,
                DefaultAnimation = "down",
                IsFixed = false
            };
            _player.AddComponent(spriteCmp);
            _player.AddComponent(new KeyboardComponent
            {
                Graphics = _graphics,
                Transform = transform,
                SpriteComponent = spriteCmp
            });
            _manager.AddEntity(_player);
            
            _manager.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _manager.Update(gameTime);
            HandleCamera();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _manager.Draw();
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
