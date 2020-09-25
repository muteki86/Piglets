using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace chiscore
{
    public class Map
    {
        public Texture MapTextures { get; set; }
        
        public int Scale { get; set; }
        
        public int TileSize { get; set; }
        
        public EntityManager Manager { get; set; }

        public void LoadMap(string filePath, int mapsizeX, int mapsizeY)
        {
            
        }

        private void LoadTile(Rectangle rect)
        {
            
        }
        
    }
}