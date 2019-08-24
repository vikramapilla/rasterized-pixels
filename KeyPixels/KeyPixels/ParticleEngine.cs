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

        private int particleCoolDown = 65;

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

        public ParticleEngine(Model model, Vector3 location, float rotation, String particleType, int tte)
        {
            EmitterLocation = location;
            EmitterRotation = rotation;
            this.model = model;
            random = new Random();
            this.particles = new List<Particle>();
            this.TTE = tte;
            this.ParticleType = particleType;
        }

        private Particle GenerateNewParticle()
        {
            if (ParticleType == "Enemy")
            {
                return GenerateEnemyParticle();
            }
            if (ParticleType == "Wall")
            {
                return GenerateWallParticle();
            }
            if (ParticleType == "Portal")
            {
                return GeneratePortalParticle();
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
            Vector3 velocity;
            if (EmitterRotation % 180 == 0)
            {
                velocity = new Vector3(0.01f * (float)(random.NextDouble() * 2 - 1),
                    0.01f * (float)(random.NextDouble() * 2 - 1),
                   0);
            }
            else
            {

                velocity = new Vector3(0,
                    0.01f * (float)(random.NextDouble() * 2 - 1),
                    0.01f * (float)(random.NextDouble() * 2 - 1));
            }
            float angle = EmitterRotation;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2);
            Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 15;

            return new Particle(model, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        private Particle GeneratePortalParticle()
        {
            Vector3 position = new Vector3(EmitterLocation.X + (float)random.NextDouble() * random.Next(-1, 2) * 0.5f,
                0,
                EmitterLocation.Z + (float)random.NextDouble() * random.Next(-1, 2) * 0.5f);
            Vector3 velocity;
            velocity = new Vector3(0.007f * (float)(random.NextDouble() * 2 - 1),
                0.01f * (float)(random.NextDouble() * 2 - 1),
                0.007f * (float)(random.NextDouble() * 2 - 1));
            float angle = EmitterRotation;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2);
            Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 70;

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
            if (Game1.isGamePlaying)
            {

                if (TTE >= 0)
                {
                    int total = 25;

                    for (int i = 0; i < total; i++)
                    {
                        if (ParticleType == "Portal")
                        {
                            if (particleCoolDown < 0)
                            {
                                particleCoolDown = 65;
                                particles.Add(GenerateNewParticle());
                            }
                            else
                            {
                                particleCoolDown--;
                            }
                        }
                        else
                        {
                            particles.Add(GenerateNewParticle());
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
                        if (ParticleType == "Portal")
                        {
                            if (particles[particle].Position.X < -0.5f || particles[particle].Position.X > 0.5f)
                            {
                                removeParticle = true;
                            }

                            if (particles[particle].Position.Z < -4.5f || particles[particle].Position.Z > -3.5f)
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


        public void Draw()
        {


            if (ParticleType == "Portal")
            {
                for (int i = 0; i < particles.Count; i++)
                {
                    particles[i].Draw(new Color(204, 255, 0, 75));
                }
            }
            else
            {
                for (int i = 0; i < particles.Count; i++)
                {
                    particles[i].Draw();
                }
            }

        }

    }
}
