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
        private List<Particle> particles;
        private Model model;


        public ParticleEngine(Model model, Vector3 location)
        {
            EmitterLocation = location;
            this.model = model;
            random = new Random();
            this.particles = new List<Particle>();
        }

        private Particle GenerateNewParticle()
        {
            Vector3 position = EmitterLocation;
            Vector3 velocity = new Vector3(0.1f * (float)(random.NextDouble() * 2 - 1),
                0.1f * (float)(random.NextDouble() * 2 - 1),
                0.1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color((float) random.NextDouble(), (float) random.NextDouble(), (float) random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 20;

            return new Particle(model, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Update()
        {
            int total = 10;

            for(int i=0; i<total; i++)
            {
                particles.Add(GenerateNewParticle());
            }

            for(int particle=0; particle<particles.Count; particle++)
            {
                particles[particle].Update();
                if(particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
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
