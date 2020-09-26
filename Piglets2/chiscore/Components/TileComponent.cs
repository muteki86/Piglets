using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace chiscore.Components
{
    public class TileComponent : Component
    {
        public int TileSize { get; set; }
        public int Scale { get; set; }
        public Vector2 Position { get; set; }

        public Rectangle srcRect { get; set; }

        public Texture2D Texture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        
        public void Draw()
        {
            SpriteBatch.Draw(
                Texture,
                Position,
                srcRect,
                Color.White,
                0f,
                new Vector2(srcRect.Width/ 2, srcRect.Height/ 2),
                /*Position,*/
                Scale,
                SpriteEffects.None,
                0f
            );   
        }

        public void Update(GameTime gameTime)
        {
            var newx = Position.X - Camera.GetInstance().X;
            var newy = Position.Y - Camera.GetInstance().Y;
            Position = new Vector2(newx, newy);
        }

        public void Initialize()
        {
        }
    }
}