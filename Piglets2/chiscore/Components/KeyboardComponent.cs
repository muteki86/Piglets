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
            
            var deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;//.TotalSeconds;

            var speed = 100;

            
            if (kstate.IsKeyDown(Keys.Up))
            {
                //Transform.Speed = new Vector2(0, -speed);
                Transform.Position = new Vector2(Transform.Position.X, Transform.Position.Y - Transform.Speed*deltatime);
                SpriteComponent.SetAnimation("up");
            }
            if (kstate.IsKeyDown(Keys.Down))
            {
                Transform.Position = new Vector2(Transform.Position.X, Transform.Position.Y + Transform.Speed*deltatime);
                //Transform.Speed = new Vector2(0, speed);
                SpriteComponent.SetAnimation("down");
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                Transform.Position = new Vector2(Transform.Position.X - Transform.Speed*deltatime, Transform.Position.Y);
                //Transform.Speed = new Vector2(-speed, 0);
                SpriteComponent.SetAnimation("left");
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                Transform.Position = new Vector2(Transform.Position.X + Transform.Speed*deltatime, Transform.Position.Y);
                //Transform.Speed = new Vector2(speed, 0);
                SpriteComponent.SetAnimation("right");
            }

            /*if (kstate.IsKeyUp(Keys.Up))
            {
                Transform.Speed = new Vector2(0, 0);
            }
            if (kstate.IsKeyUp(Keys.Down))
            {
                Transform.Speed = new Vector2(0, 0);
            }
            if (kstate.IsKeyUp(Keys.Left))
            {
                Transform.Speed = new Vector2(0, 0);
            }
            if (kstate.IsKeyUp(Keys.Right))
            {
                Transform.Speed = new Vector2(0, 0);
            }
            */

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