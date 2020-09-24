using Microsoft.Xna.Framework;

namespace chiscore
{
    public interface Component
    {
         void Draw();

         void Update(GameTime gameTime);

         void Initialize();
    }
}