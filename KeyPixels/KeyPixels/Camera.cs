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
            position = new Vector3(0f, 10f, -11f);
            target = Vector3.Zero;
            fieldOfView = MathHelper.PiOver4;
            aspectRatio = graphics.PreferredBackBufferWidth / graphics.PreferredBackBufferHeight;
            nearPlane = 0.1f;
            farPlane = 100f;
        }

    }
}
