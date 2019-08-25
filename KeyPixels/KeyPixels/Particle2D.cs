using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace KeyPixels
{
    class Particle2D
    {
        public Texture2D Texture { get; set; }            // particle model to be drawn
        public Vector2 Position { get; set; }       // particle's position
        public Vector2 Velocity { get; set; }       // particle's velocity
        public float Angle { get; set; }            // particle's rotation
        public float AngularVelocity { get; set; }  // particle's speed of rotation
        public Color Color { get; set; }            // particle's color
        public float Size { get; set; }             // particle's size
        public int TTL { get; set; }                // particle's time to live
        


        public Particle2D(Texture2D texture, Vector2 position, Vector2 velocity, float angle,
            float angularVelocity, Color color, float size, int ttl)
        {
            Texture = texture;
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
        
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Texture.Height, Texture.Width), color);
        }
    }
}
