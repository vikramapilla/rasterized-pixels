using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace KeyPixels
{
    class ParticleEngine
    {

        private Random random;
        public Vector3 EmitterLocation { get; set; }
        public float EmitterRotation { get; set; }
        private List<Particle> particles;
        private Model model;
        private int TTE; //TimeToEmit
        private string ParticleType;
        

        public ParticleEngine(Model model, Vector3 location, float rotation, String particleType)
        {
            EmitterLocation = location;
            EmitterRotation = rotation;
            this.model = model;
            random = new Random();
            this.particles = new List<Particle>();
            this.TTE = 50;
            this.ParticleType = particleType;
        }

        private Particle GenerateNewParticle()
        {
            if(ParticleType == "Enemy")
            {
                return GenerateEnemyParticle();
            }
            if(ParticleType == "Wall")
            {
                return GenerateWallParticle();
            }

            return GenerateDefaultParticle();

        }

        private Particle GenerateEnemyParticle()
        {
            Vector3 position = EmitterLocation;
            Vector3 velocity = new Vector3(0.01f * (float)(random.NextDouble() * 2 - 1),
                0.01f * (float)(random.NextDouble() * 2 - 1),
                0.01f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2);
            Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 35;

            return new Particle(model, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        private Particle GenerateWallParticle()
        {
            Vector3 position = EmitterLocation;
            Vector3 velocity = new Vector3(0.01f * (float)(random.NextDouble() * 2 - 1),
                0.01f * (float)(random.NextDouble() * 2 - 1),
                0.01f * (float)(random.NextDouble() * 2 - 1));
            float angle = EmitterRotation;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2);
            Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 15;

            return new Particle(model, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        private Particle GenerateDefaultParticle()
        {
            Vector3 position = EmitterLocation;
            Vector3 velocity = new Vector3(0.01f * (float)(random.NextDouble() * 2 - 1),
                0.01f * (float)(random.NextDouble() * 2 - 1),
                0.01f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2);
            Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 5;

            return new Particle(model, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Update()
        {
            if (TTE >= 0)
            {
                int total = 25;

                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewParticle());
                }

                for (int particle = 0; particle < particles.Count; particle++)
                {
                    particles[particle].Update();
                    if (particles[particle].TTL <= 0)
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


        public void Draw()
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Draw();
            }
        }

    }
}
