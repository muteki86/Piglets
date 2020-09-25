using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace chiscore
{
    public class EntityManager
    {
        private List<Entity> _entities;

        public EntityManager()
        {
            _entities = new List<Entity>();
        }
        
        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }
            
        public void Update(GameTime gameTime)
        {
            _entities.ForEach(ent=>ent.Update(gameTime));
        }

        public void Draw()
        {
            _entities.ForEach(ent=> ent.Draw());
        }

        public void Initialize()
        {
            _entities.ForEach(x=>x.Initialize());
        }
    }
}
