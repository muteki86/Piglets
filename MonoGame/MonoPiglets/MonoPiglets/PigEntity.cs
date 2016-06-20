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

        private SpriteFont font;

        public PigEntity()
        {
            Sprites = new List<Texture2D>();
            _random = new Random(Guid.NewGuid().GetHashCode());
            _nextUpdateTicks = _random.Next(200, 650);

        }

        public void LoadSprites(ContentManager content)
        {
            font = content.Load<SpriteFont>("MainFontSmall");

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

        public void Update(GameTime gameTime, TerrainEntity currentTerrain)
        {
            if (_elapsedMilliseconds > _nextUpdateTicks)
            {
                Direction = _random.Next(0, 4);

                var newModifier = _random.Next(5, 35);

                TerrainInfo terrainInfo;

                var newX = 0;
                var newY = 0;

                switch (Direction)
                {
                    case 0: // right
                        newX = (int) (_position.X + newModifier);
                        terrainInfo = currentTerrain.GetTerrainInfo(newX, (int) _position.Y);

                        if (terrainInfo.IsValidForPig)
                        {
                            _position.X = newX - terrainInfo.Height;
                        }

                        break;
                    case 1: // left

                        newX = (int) (_position.X - newModifier);
                        terrainInfo = currentTerrain.GetTerrainInfo(newX, (int)_position.Y);
                        if (terrainInfo.IsValidForPig)
                        {
                            _position.X -= newModifier - terrainInfo.Height;
                        }
                        break;
                    case 2: // up
                        newY = (int) (_position.Y - newModifier);

                        terrainInfo = currentTerrain.GetTerrainInfo((int) Position.X, newY);
                        if (terrainInfo.IsValidForPig)
                        {
                            _position.Y -= newModifier - terrainInfo.Height;
                        }

                        break;
                    case 3: // down

                        newY = (int)(_position.Y + newModifier);
                        terrainInfo = currentTerrain.GetTerrainInfo((int)Position.X, newY);
                        if (terrainInfo.IsValidForPig)
                        {
                            _position.Y += newModifier - terrainInfo.Height;
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

            spriteBatch.DrawString(font, $"{Position.X}, {Position.Y}", new Vector2(Position.X-10, Position.Y-10), Color.Black);
        }
    }
}
