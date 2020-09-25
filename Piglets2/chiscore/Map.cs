using chiscore.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Security;

namespace chiscore
{
    public class Map
    {
        public Texture2D MapTextures { get; set; }
        
        public int Scale { get; set; }
        
        public int TileSize { get; set; }
        
        public EntityManager Manager { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

        private Entity _map;

        public Map()
        {
        }

        public void LoadMap(string filePath, int mapsizeX, int mapsizeY)
        {
            _map = new Entity();
            Manager.AddEntity(_map);

            var fractalTerrain = new FractalTerrain {
                X = mapsizeX,
                Y = mapsizeY
            };
            var terrain = fractalTerrain.GetRandomTerrain();

            int width = terrain.GetLength(0);
            int height = terrain.GetLength(1);

            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    var value = terrain[i, k] / 5 > 5 ? 5 : terrain[i, k] / 5;
                    value = value < 0 ? 6 : value;

                    var srcRect = new Rectangle(TileSize * value, 0 , TileSize, TileSize);
                    var pos = new Vector2(i*TileSize, k*TileSize);
                    LoadTile(srcRect, pos);

                }
            }

        }

        private void LoadTile(Rectangle rect, Vector2 position)
        {
            var tile = new TileComponent
            {
                Scale = 1,
                SpriteBatch = SpriteBatch,
                srcRect = rect,
                Texture = MapTextures,
                TileSize = TileSize,
                Position = position
            };

            _map.AddComponent(tile);
        }
        
    }
}