using System.Collections.Generic;
using chiscore.Components;
using Microsoft.Xna.Framework;

namespace chiscore
{
    public class Entity
    {
        private List<Component> _components;

        public Entity()
        {
            _components = new List<Component>();
        }
        public Entity(List<Component> components)
        {
            _components = components;
        }

        public void Update(GameTime gameTime)
        {
            _components.ForEach(x=>x.Update(gameTime));
        }

        public void Draw()
        {
            _components.ForEach(x=>x.Draw());
        }

        public void AddComponent(Component component)
        {
            _components.Add(component);
        }
    }
}