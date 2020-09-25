using System.Collections.Generic;
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
        
        public bool IsAnimated { get; set; }
        
        public bool HasOrientation { get; set; }
        
        public Dictionary<string, Animation> Animations { get; set; }
        public string DefaultAnimation { get; set; }

        private Animation _curAnimation;

        private Rectangle _srcRect;
        
        public SpriteComponent()
        {
            _srcRect = new Rectangle();
        }
        
        
        public void Draw()
        {
            SpriteBatch.Draw(
                    Texture,
                    Transform.Position,
                    _srcRect,
                    Color.White,
                    0f,
                    new Vector2(Transform.Width / 2, Transform.Height / 2),
                    Scale,
                    SpriteEffects.None,
                    0f
                );
        }

        public void Initialize()
        {
            _curAnimation = Animations[DefaultAnimation];
        }

        public void Update(GameTime gameTime)
        {
            this.Transform = Transform;
            var newX = Transform.Width *
                       ((gameTime.TotalGameTime.Milliseconds / _curAnimation.Speed) % _curAnimation.NumFrames);
            var newY = _curAnimation.Index * Transform.Height;
            _srcRect = new Rectangle {X = newX, Y = newY, Height = Transform.Height, Width = Transform.Width};
        }

        public void SetAnimation(string anikey)
        {
            _curAnimation = Animations[anikey];
        }
    }
}