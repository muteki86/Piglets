using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace chiscore
{
    public class FractalTerrain
    {
        private int[,] _terrainArray;
        private int _maxHeight = 35;
        private readonly Random _random;
        public int X { get; set; }
        public int Y { get; set; }

        public FractalTerrain()
        {
            _random = new Random(Guid.NewGuid().GetHashCode());
        }

        public int[,] GetRandomTerrain()
        {
            CreateNewHeightDistribution();
            return _terrainArray;
        }

        private void CreateNewHeightDistribution()
        {
            _terrainArray = new int[X, Y];

            // set up initial values
            _terrainArray[0, 0] = _random.Next(_maxHeight / 2, _maxHeight * 2);
            _terrainArray[X - 1, 0] = _random.Next(-_maxHeight, _maxHeight / 2);
            _terrainArray[0, Y - 1] = _random.Next(-_maxHeight, _maxHeight / 2);
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
                    ) / 4 + _random.Next(-maxRandom, maxRandom);

                //// left center
                fourthPoint = topLeft.Y - halfSideLength > 0 ? topLeft.Y - halfSideLength : 0;

                terrain[halfSideLength + topLeft.X, topLeft.Y] = (
                    terrain[topLeft.X, topLeft.Y]
                    + terrain[bottomLeft.X, bottomLeft.Y]
                    + terrain[topLeft.X + halfSideLength, topLeft.Y + halfSideLength]
                    + terrain[halfSideLength + topLeft.X, fourthPoint]
                    ) / 4 + _random.Next(-maxRandom, maxRandom);

                //// right center
                fourthPoint = topRight.Y + sideLength < Y ? topRight.Y + sideLength : Y - 1;

                terrain[halfSideLength + topLeft.X, bottonRight.Y] = (
                    +terrain[topRight.X, topRight.Y]
                    + terrain[bottonRight.X, bottonRight.Y]
                    + terrain[topLeft.X + halfSideLength, topLeft.Y + halfSideLength]
                    + terrain[halfSideLength + topLeft.X, fourthPoint]
                    ) / 4 + _random.Next(-maxRandom, maxRandom);

                //// bottom center
                fourthPoint = bottonRight.X + sideLength + 1 < X ? bottonRight.X + sideLength : X - 1;

                terrain[bottomLeft.X, halfSideLength + topLeft.Y] = (
                    +terrain[bottonRight.X, bottonRight.Y]
                    + terrain[bottomLeft.X, bottomLeft.Y]
                    + terrain[topLeft.X + halfSideLength, topLeft.Y + halfSideLength]
                    + terrain[fourthPoint, halfSideLength + topLeft.Y]
                    ) / 4 + _random.Next(-maxRandom, maxRandom);

                var newRandom = (maxRandom - 1) == 0 ? 1 : (maxRandom - 1);

                DiamondStep(terrain, topLeft, new Point(topLeft.X, topLeft.Y + halfSideLength), new Point(topLeft.X + halfSideLength, topLeft.Y), new Point(topLeft.X + halfSideLength, topLeft.Y + halfSideLength), newRandom);
                DiamondStep(terrain, new Point(topLeft.X, topLeft.Y + halfSideLength), topRight, new Point(topLeft.X + halfSideLength, topLeft.Y + halfSideLength), new Point(topLeft.X + halfSideLength, topLeft.Y + sideLength), newRandom);
                DiamondStep(terrain, new Point(topLeft.X + halfSideLength, topLeft.Y), new Point(topLeft.X + halfSideLength, topLeft.Y + halfSideLength), bottomLeft, new Point(topLeft.X + sideLength, topLeft.Y + halfSideLength), newRandom);
                DiamondStep(terrain, new Point(topLeft.X + halfSideLength, topLeft.Y + halfSideLength), new Point(topLeft.X + halfSideLength, topLeft.Y + sideLength), new Point(topLeft.X + sideLength, topLeft.Y + halfSideLength), new Point(bottonRight.X, bottonRight.Y), newRandom);

            }

        }

    }
}
