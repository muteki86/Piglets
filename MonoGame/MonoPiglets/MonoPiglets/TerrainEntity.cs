using System;
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

        private int _maxHeight = 15;

        private readonly Random _random;

        public TerrainEntity()
        {
            _random = new Random(Guid.NewGuid().GetHashCode());

        }

        public void Initialize()
        {
            CreateNewHeightDistribution();
        }

        public void LoadSprites(ContentManager content)
        {


        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

        private void CreateNewHeightDistribution()
        {
            // crear array
            var terrainArray = new int[X, Y];

            // set up initial values
            terrainArray[0, 0] = _random.Next(0, _maxHeight);
            terrainArray[X - 1, 0] = _random.Next(0, _maxHeight);
            terrainArray[0, Y - 1] = _random.Next(0, _maxHeight);
            terrainArray[X - 1, Y - 1] = _random.Next(0, _maxHeight);

            DiamondStep(terrainArray, new Point(0, 0), new Point(0, Y - 1), new Point(X - 1, 0), new Point(X - 1, Y - 1), 3);

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
                             4 + _random.Next(0, maxRandom);

                // top center
                terrain[topLeft.X, halfSideLength + topLeft.Y] = (
                    terrain[topLeft.X, topLeft.Y]
                    + terrain[topLeft.X, topRight.Y]
                    + terrain[topLeft.X + halfSideLength, topLeft.Y + halfSideLength]
                    + terrain[topLeft.X, halfSideLength + topLeft.Y]
                    ) / 4 + _random.Next(0, maxRandom);

                //// left center
                terrain[halfSideLength + topLeft.X, topLeft.Y] = (
                            terrain[topLeft.X, topLeft.Y]
                            + terrain[bottomLeft.X, bottomLeft.Y]
                            + terrain[topLeft.X + halfSideLength, topLeft.Y + halfSideLength]
                            + terrain[halfSideLength + topLeft.X, topLeft.Y]
                            ) / 4 + _random.Next(0, maxRandom);

                //// right center
                terrain[halfSideLength + topLeft.X, bottonRight.Y] = (
                            +terrain[topRight.X, topRight.Y]
                            + terrain[bottonRight.X, bottonRight.Y]
                            + terrain[topLeft.X + halfSideLength, topLeft.Y + halfSideLength]
                            + terrain[halfSideLength + topLeft.X, bottonRight.Y]
                            ) / 4 + _random.Next(0, maxRandom);

                //// bottom center
                terrain[bottomLeft.X, halfSideLength + topLeft.Y] = (
                            +terrain[bottonRight.X, bottonRight.Y]
                            + terrain[bottomLeft.X, bottomLeft.Y]
                            + terrain[topLeft.X + halfSideLength, topLeft.Y + halfSideLength]
                            + terrain[bottomLeft.X, halfSideLength + topLeft.Y]
                            ) / 4 + _random.Next(0, maxRandom);

                var newRandom = (maxRandom - 1) == 0 ? 1 : (maxRandom - 1);

                DiamondStep(terrain, topLeft, new Point(topLeft.X, topLeft.Y + halfSideLength), new Point(topLeft.X + halfSideLength, topLeft.Y), new Point(topLeft.X + halfSideLength, topLeft.Y + halfSideLength), newRandom);

                DiamondStep(terrain, new Point(topLeft.X, topLeft.Y + halfSideLength), topRight, new Point(topLeft.X + halfSideLength, topLeft.Y + halfSideLength), new Point(topLeft.X + halfSideLength, topLeft.Y + sideLength), newRandom);

                DiamondStep(terrain, new Point(topLeft.X + halfSideLength, topLeft.Y), new Point(topLeft.X + halfSideLength, topLeft.Y + halfSideLength), bottomLeft, new Point(topLeft.X + sideLength, topLeft.Y + halfSideLength), newRandom);

                DiamondStep(terrain, new Point(topLeft.X + halfSideLength, topLeft.Y + halfSideLength), new Point(topLeft.X + halfSideLength, topLeft.Y + sideLength), new Point(topLeft.X + sideLength, topLeft.Y + halfSideLength), bottonRight, newRandom);

            }

        }

    }
}