using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace KeyPixels
{
    class Camera
    {
        public Vector3 position;
        public Vector3 target;

        public float fieldOfView;
        public float aspectRatio;
        public float nearPlane;
        public float farPlane;

        public Camera(GraphicsDeviceManager graphics)
        {
            position = new Vector3(10f, 0f, -10f);
            target = Vector3.Zero;
            fieldOfView = MathHelper.PiOver4;
            aspectRatio = graphics.PreferredBackBufferWidth / graphics.PreferredBackBufferHeight;
            nearPlane = 2f;
            farPlane = 1000f;
        }

    }
}
