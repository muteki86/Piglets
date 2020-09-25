﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace chiscore
{
    public class Camera
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }

        private static Camera _instance;

        private Camera()
        {
        }

        public static Camera GetInstance()
        {
            if (_instance == null)
            {
                return new Camera();
            }
            return _instance;
        }

    }
}
