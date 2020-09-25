using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace chiscore.Components
{
    public class TileComponent : Component
    {
        public int TileSize { get; set; }
        public int Scale { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        
        public void Draw()
        {
            
            SpriteBatch.Draw(
                Texture,
                Position,
                null,
                Color.White,
                0f,
                Position,
                Scale,
                SpriteEffects.None,
                0f
            );
            
        }

        public void Update(GameTime gameTime)
        {
            //todo figure out camera
        }

        public void Initialize()
        {
        }
    }
}