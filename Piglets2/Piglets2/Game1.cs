﻿using System.Collections.Generic;
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
            //this.Window.AllowUserResizing = true;
            //_graphics.IsFullScreen = true;

            Window.ClientSizeChanged+=Window_ClientSizeChanged;

            _graphics.PreferredBackBufferWidth = 800;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = 400;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
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
            var ballTexture = Content.Load<Texture2D>("images/chopper-spritesheet");
            var transform = new TransformComponent
            {
                Height = 32,
                Width = 32,
                Position =  new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2),
                Speed = 500f
            };
            player.AddComponent(transform);
            
            var dani = new Animation
            {
                Index = 0,
                Speed = 90,
                NumFrames = 2
            };
            var rani = new Animation
            {
                Index = 1,
                Speed = 90,
                NumFrames = 2
            };
            var lani = new Animation
            {
                Index = 2,
                Speed = 90,
                NumFrames = 2
            };
            var uani = new Animation
            {
                Index = 3,
                Speed = 90,
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
                DefaultAnimation = "down"
            };
            player.AddComponent(spriteCmp);
            player.AddComponent(new KeyboardComponent
            {
                Graphics = _graphics,
                Transform = transform,
                SpriteComponent = spriteCmp
            });
            _manager.AddEntity(player);
            
            _manager.Initialize();
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
            _spriteBatch.Begin();
            _manager.Draw();
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
