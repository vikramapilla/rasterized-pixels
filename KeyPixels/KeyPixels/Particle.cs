using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace KeyPixels
{
    class Particle
    {
        public Model Model { get; set; }            // particle model to be drawn
        public Vector3 Position { get; set; }       // particle's position
        public Vector3 Velocity { get; set; }       // particle's velocity
        public float Angle { get; set; }            // particle's rotation
        public float AngularVelocity { get; set; }  // particle's speed of rotation
        public Color Color { get; set; }            // particle's color
        public float Size { get; set; }             // particle's size
        public int TTL { get; set; }                // particle's time to live

        Matrix worldMatrix;                         // matrix for the particle (rotation + position)


        public Particle(Model model, Vector3 position, Vector3 velocity, float angle, 
            float angularVelocity, Color color, float size, int ttl)
        {
            Model = model;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
        }

        public void Update()
        {
            TTL--;
            Position += Velocity;
            Angle += AngularVelocity;
        }

        public void Draw()
        {
            //worldMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Angle)) * Matrix.CreateTranslation(Position);
            worldMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Angle)) * Matrix.CreateTranslation(Position);
            Game1.Draw3DModel(Model, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
        }
    }
}
