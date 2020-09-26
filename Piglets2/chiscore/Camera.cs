using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace chiscore
{
    public class Camera
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }

        private static Camera _instance;

        private Camera()
        {
        }

        public static Camera GetInstance()
        {
            return _instance ?? (_instance = new Camera());
        }

    }
}
