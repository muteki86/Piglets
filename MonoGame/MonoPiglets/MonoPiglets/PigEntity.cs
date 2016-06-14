using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPiglets
{
    public class PigEntity
    {
        private Vector2 _position;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public int Direction { get; set; }

        public List<Texture2D> Sprites { get; set; }

        public Viewport PigViewPort { get; set; }

        private readonly int _nextUpdateTicks;

        private int _elapsedMilliseconds;

        private readonly Random _random;

        public PigEntity()
        {
            Sprites = new List<Texture2D>();
            _random = new Random(Guid.NewGuid().GetHashCode());
            _nextUpdateTicks = _random.Next(200, 650);

        }

        public void LoadSprites(ContentManager content)
        {
            
            var newModifier = _random.Next(0, 4);

            switch (newModifier)
            {
                case 0:
                    Sprites.Add(content.Load<Texture2D>("ptipo1_right"));
                    Sprites.Add(content.Load<Texture2D>("ptipo1_left"));
                    Sprites.Add(content.Load<Texture2D>("ptipo1_up"));
                    Sprites.Add(content.Load<Texture2D>("ptipo1_down"));
                    break;
                case 1:
                    Sprites.Add(content.Load<Texture2D>("ptipo2_right"));
                    Sprites.Add(content.Load<Texture2D>("ptipo2_left"));
                    Sprites.Add(content.Load<Texture2D>("ptipo2_up"));
                    Sprites.Add(content.Load<Texture2D>("ptipo2_down"));
                    break;
                case 2:
                    Sprites.Add(content.Load<Texture2D>("ptipo3_right"));
                    Sprites.Add(content.Load<Texture2D>("ptipo3_left"));
                    Sprites.Add(content.Load<Texture2D>("ptipo3_up"));
                    Sprites.Add(content.Load<Texture2D>("ptipo3_down"));
                    break;
                case 3:
                    Sprites.Add(content.Load<Texture2D>("ptipo4_right"));
                    Sprites.Add(content.Load<Texture2D>("ptipo4_left"));
                    Sprites.Add(content.Load<Texture2D>("ptipo4_up"));
                    Sprites.Add(content.Load<Texture2D>("ptipo4_down"));
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_elapsedMilliseconds > _nextUpdateTicks)
            {
                Direction = _random.Next(0, 4);

                var newModifier = _random.Next(5, 35);
                switch (Direction)
                {
                    case 0: // right
                        if (_position.X + newModifier < PigViewPort.TitleSafeArea.Right)
                        {
                            _position.X += newModifier;
                        }

                        break;
                    case 1: // left
                        if (_position.X - newModifier > PigViewPort.TitleSafeArea.Left)
                        {
                            _position.X -= newModifier;
                        }
                        break;
                    case 2: // up
                        if (_position.Y - newModifier > PigViewPort.TitleSafeArea.Top)
                        {
                            _position.Y -= newModifier;
                        }

                        break;
                    case 3: // down
                        if (_position.Y + newModifier < PigViewPort.TitleSafeArea.Bottom)
                        {
                            _position.Y += newModifier;
                        }
                        break;
                }
                _elapsedMilliseconds = 0;
            }
            else
            {
                _elapsedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprites[Direction], Position);
        }
    }
}
