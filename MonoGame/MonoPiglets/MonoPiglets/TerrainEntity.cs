using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPiglets
{
    public class TerrainEntity
    {
        public int X { get; set; }

        public int Y { get; set; }

        private int _maxHeight = 35;

        private readonly Random _random;

        private List<Texture2D> _textures;

        private int[,] _terrainArray;

        private SpriteFont font;

        public TerrainEntity()
        {
            _random = new Random(Guid.NewGuid().GetHashCode());
            _textures = new List<Texture2D>();
        }

        public void Initialize()
        {
            CreateNewHeightDistribution();
        }

        public void LoadSprites(ContentManager content)
        {
            font = content.Load<SpriteFont>("MainFontSmall");

            _textures.Add(content.Load<Texture2D>("terrain0"));
            _textures.Add(content.Load<Texture2D>("terrain1"));
            _textures.Add(content.Load<Texture2D>("terrain2"));
            _textures.Add(content.Load<Texture2D>("terrain3"));
            _textures.Add(content.Load<Texture2D>("terrain4"));
            _textures.Add(content.Load<Texture2D>("terrain5"));
            _textures.Add(content.Load<Texture2D>("terrain6"));
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = _terrainArray.GetLength(0);
            int height = _terrainArray.GetLength(1);

            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    var point = new Vector2(i * 10, k * 10);

                    var value = _terrainArray[i, k] / 5  > 5 ? 5 : _terrainArray[i, k]/5;
                    value = value < 0 ? 6 : value;

                    spriteBatch.Draw(_textures[value], point);
                    if (i%3 == 0 && k%3 == 0)
                    {
                        var wat = _terrainArray[i, k];

                        spriteBatch.DrawString(font, $"{value}", point, Color.Black);
                    }
                }
            }
            
        }

        public TerrainInfo GetTerrainInfo(int x, int y)
        {
            var result = new TerrainInfo
            {
                X = (x/10),
                Y = (y/10)
            };
            
            if ( result.X+1 >= X || result.Y+1 >= Y || result.X < 0 || result.Y < 0 )
            {
                result.IsValidForPig = false;
            }
            else
            {
                result.Height = _terrainArray[result.X, result.Y]/5 ;

                result.IsValidForPig = result.Height >= 0;
            }

            return result;
        }

        private void CreateNewHeightDistribution()
        {
            _terrainArray = new int[X, Y];

            // set up initial values
            _terrainArray[0, 0] = _random.Next(_maxHeight/2, _maxHeight*2);
            _terrainArray[X - 1, 0] = _random.Next(-_maxHeight, _maxHeight/2);
            _terrainArray[0, Y - 1] = _random.Next(-_maxHeight, _maxHeight/2);
            _terrainArray[X - 1, Y - 1] = _random.Next(0, _maxHeight);

            DiamondStep(_terrainArray, new Point(0, 0), new Point(0, Y - 1), new Point(X - 1, 0), new Point(X - 1, Y - 1), 4);

        }

        private void DiamondStep(int[,] terrain, Point topLeft, Point topRight, Point bottomLeft, Point bottonRight, int maxRandom)
        {
            var sideLength = bottomLeft.X - topLeft.X;
            var halfSideLength = sideLength / 2;

            if (sideLength >= 2)
            {
                terrain[halfSideLength + topLeft.X, halfSideLength + topLeft.Y] =
                            (terrain[topLeft.X, topLeft.Y] +
                              terrain[topRight.X, topRight.Y] +
                              terrain[bottomLeft.X, bottomLeft.Y] +
                              terrain[bottonRight.X, bottonRight.Y]) /
                             4 + _random.Next(-maxRandom, maxRandom);

                // top 
                var fourthPoint = topLeft.X - halfSideLength > 0 ? topLeft.X - halfSideLength : 0;

                terrain[topLeft.X, halfSideLength + topLeft.Y] = (
                    terrain[topLeft.X, topLeft.Y]
                    + terrain[topLeft.X, topRight.Y]
                    + terrain[topLeft.X + halfSideLength, topLeft.Y + halfSideLength]
                    + terrain[fourthPoint, halfSideLength + topLeft.Y]
                    )/4 + _random.Next(-maxRandom, maxRandom);

                //// left center
                fourthPoint = topLeft.Y - halfSideLength > 0 ? topLeft.Y - halfSideLength : 0;

                terrain[halfSideLength + topLeft.X, topLeft.Y] = (
                    terrain[topLeft.X, topLeft.Y]
                    + terrain[bottomLeft.X, bottomLeft.Y]
                    + terrain[topLeft.X + halfSideLength, topLeft.Y + halfSideLength]
                    + terrain[halfSideLength + topLeft.X, fourthPoint]
                    )/4 + _random.Next(-maxRandom, maxRandom);

                //// right center
                fourthPoint = topRight.Y + sideLength < Y ? topRight.Y + sideLength : Y-1;

                terrain[halfSideLength + topLeft.X, bottonRight.Y] = (
                    +terrain[topRight.X, topRight.Y]
                    + terrain[bottonRight.X, bottonRight.Y]
                    + terrain[topLeft.X + halfSideLength, topLeft.Y + halfSideLength]
                    + terrain[halfSideLength + topLeft.X, fourthPoint]
                    ) / 4 + _random.Next(-maxRandom, maxRandom);

                //// bottom center
                fourthPoint = bottonRight.X + sideLength + 1 < X ? bottonRight.X + sideLength : X-1;

                terrain[bottomLeft.X, halfSideLength + topLeft.Y] = (
                    +terrain[bottonRight.X, bottonRight.Y]
                    + terrain[bottomLeft.X, bottomLeft.Y]
                    + terrain[topLeft.X + halfSideLength, topLeft.Y + halfSideLength]
                    + terrain[ fourthPoint, halfSideLength + topLeft.Y]
                    ) / 4 + _random.Next(-maxRandom, maxRandom);

                var newRandom = (maxRandom - 1) == 0 ? 1 : (maxRandom - 1);

                DiamondStep(terrain, topLeft, new Point(topLeft.X, topLeft.Y + halfSideLength), new Point(topLeft.X + halfSideLength, topLeft.Y), new Point(topLeft.X + halfSideLength, topLeft.Y + halfSideLength), newRandom);
                DiamondStep(terrain, new Point(topLeft.X, topLeft.Y + halfSideLength), topRight, new Point(topLeft.X + halfSideLength, topLeft.Y + halfSideLength), new Point(topLeft.X + halfSideLength, topLeft.Y + sideLength), newRandom);
                DiamondStep(terrain, new Point(topLeft.X + halfSideLength, topLeft.Y), new Point(topLeft.X + halfSideLength, topLeft.Y + halfSideLength), bottomLeft, new Point(topLeft.X + sideLength, topLeft.Y + halfSideLength), newRandom);
                DiamondStep(terrain, new Point(topLeft.X + halfSideLength, topLeft.Y + halfSideLength), new Point(topLeft.X + halfSideLength, topLeft.Y + sideLength), new Point(topLeft.X + sideLength, topLeft.Y + halfSideLength ), new Point(bottonRight.X, bottonRight.Y), newRandom);

            }

        }

    }
}