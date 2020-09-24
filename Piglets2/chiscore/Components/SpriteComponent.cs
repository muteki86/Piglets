using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace chiscore.Components
{
    public class SpriteComponent : Component
    {
        public SpriteBatch SpriteBatch { get; set; }
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }
        public TransformComponent Transform { get; set; }
        
        public bool IsAnimation { get; set; }
        
        public void Draw()
        {
            //throw new System.NotImplementedException();
            SpriteBatch.Begin();
            SpriteBatch.Draw(
                    Texture,
                    Transform.Position,
                    null,
                    Color.White,
                    0f,
                    new Vector2(Transform.Width / 2, Transform.Height / 2),
                    Scale,
                    SpriteEffects.None,
                    0f
                );
            SpriteBatch.End();
        }

        public void Initialize()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            this.Transform = Transform;
        }
    }
}