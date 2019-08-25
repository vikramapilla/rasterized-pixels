using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace KeyPixels
{
    class ParticleEngine2D
    {

        private Random random;
        public Vector2 EmitterLocation { get; set; }
        public float EmitterRotation { get; set; }
        private List<Particle2D> particles;
        private Texture2D Texture;
        private int TTE; //TimeToEmit

        private int particleCoolDown = 100;

        public ParticleEngine2D(Texture2D texture, Vector2 location, float rotation, int tte)
        {
            EmitterLocation = location;
            EmitterRotation = rotation;
            this.Texture = texture;
            random = new Random();
            this.particles = new List<Particle2D>();
            this.TTE = tte;
        }

        private Particle2D GenerateNewParticle()
        {
            return GenerateDefaultParticle();

        }
        private Particle2D GenerateDefaultParticle()
        {
            Vector2 position = new Vector2(random.Next(0, 1920),
                random.Next(950, 1000));
            Vector2 velocity = new Vector2(random.Next(-5,+5), random.Next(-10, -1));
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2);
            Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 150;

            return new Particle2D(Texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Update()
        {
            if (!Game1.isGamePlaying || Game1.isGameEnded)
            {

                if (TTE >= 0)
                {
                    int total = 10;

                    for (int i = 0; i < total; i++)
                    {
                        if (particleCoolDown < 0)
                        {
                            particleCoolDown = 100;
                            particles.Add(GenerateNewParticle());
                        }
                        else
                        {
                            particleCoolDown--;
                        }
                    }

                    for (int particle = 0; particle < particles.Count; particle++)
                    {
                        bool removeParticle = false;
                        particles[particle].Update();
                        if (particles[particle].TTL <= 0)
                        {
                            removeParticle = true;
                        }
                        if (removeParticle)
                        {
                            particles.RemoveAt(particle);
                            particle--;
                        }

                    }
                }
                else
                {
                    for (int particle = 0; particle < particles.Count; particle++)
                    {
                        particles.RemoveAt(particle);
                        particle--;
                    }
                }
                TTE--;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Draw(spriteBatch, new Color(168, 174, 144, 75));
            }
            
        }

    }
}
