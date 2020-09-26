using Microsoft.Xna.Framework;

namespace chiscore.Components
{
    public class TransformComponent: Component
    {
        public Vector2 Position { get; set; }

        public int Speed { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public void Draw()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            //Position += Speed;
            /*Position = new Vector2((float)(Position.X + Speed.X * gameTime.ElapsedGameTime.TotalSeconds),
                (float)(Position.Y + Speed.Y * gameTime.ElapsedGameTime.TotalSeconds));*/
        }

        public void Initialize()
        {
            
        }
    }
}