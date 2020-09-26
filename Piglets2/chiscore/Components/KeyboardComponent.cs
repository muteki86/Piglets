using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace chiscore.Components
{
    public class KeyboardComponent : Component
    {
        public TransformComponent Transform { get; set; }
        
        public GraphicsDeviceManager Graphics { get; set; }

        public SpriteComponent SpriteComponent { get; set; }
        
        public void Draw()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            
            var deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Up))
            {
                Transform.Position = new Vector2(Transform.Position.X, Transform.Position.Y - Transform.Speed*deltatime);
                SpriteComponent.SetAnimation("up");
            }
            if (kstate.IsKeyDown(Keys.Down))
            {
                Transform.Position = new Vector2(Transform.Position.X, Transform.Position.Y + Transform.Speed*deltatime);
                SpriteComponent.SetAnimation("down");
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                Transform.Position = new Vector2(Transform.Position.X - Transform.Speed*deltatime, Transform.Position.Y);
                SpriteComponent.SetAnimation("left");
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                Transform.Position = new Vector2(Transform.Position.X + Transform.Speed*deltatime, Transform.Position.Y);
                SpriteComponent.SetAnimation("right");
            }
            
            /*
            if (Transform.Position.X > Graphics.PreferredBackBufferWidth - Transform.Width / 2)
            {
                Transform.Position = new Vector2(Graphics.PreferredBackBufferWidth - Transform.Width / 2, Transform.Position.Y);
            }
            else if (Transform.Position.X < Transform.Width / 2)
            {
                Transform.Position = new Vector2(Transform.Width / 2, Transform.Position.Y);   
            }

            if (Transform.Position.Y > Graphics.PreferredBackBufferHeight - Transform.Height / 2)
            {
                Transform.Position = new Vector2(Transform.Position.X, Graphics.PreferredBackBufferHeight - Transform.Height / 2);
            }
            else if (Transform.Position.Y < Transform.Height / 2)
            {
                Transform.Position = new Vector2(Transform.Position.X, Transform.Height / 2);
            }*/
        }

        public void Initialize()
        {
        }
    }
}