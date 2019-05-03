using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using System.Collections.Generic;

namespace KeyPixels
{
    class Shots
    {
        static Model shotModel;
        static List<_Value> posShot;
        static float speed;

        private struct _Value{
            Matrix posMatrix;
            float angleFly;

            public _Value(Matrix m,float aFly)
            {
                posMatrix = m;
                angleFly = aFly;
            }

            public Matrix getMatric()
            {
                return posMatrix;
            }

            public void setMatrix(Matrix m)
            {
                posMatrix = m;
            }

            public float getAngleFly()
            {
                return angleFly;
            }

            public _Value getValue()
            {
                return new _Value(posMatrix, angleFly);
            }


        };

        public Shots(ContentManager contentManager, string modelName,float _speed, Color _color)
        {
            shotModel = contentManager.Load<Model>(modelName);
            posShot = new List<_Value>();
            speed = _speed;
            ColorModel(_color);
        }

        public Shots(ContentManager contentManager, string modelName, float _speed, Color _color, int nStart)
        {
            shotModel = contentManager.Load<Model>(modelName);
            posShot = new List<_Value>(nStart);
            speed = _speed;
            ColorModel(_color);
        }


        public void createShot(Matrix pos,float ang)
        {
            posShot.Add(new _Value(pos, ang));
        }

        public void clearAll()
        {
            posShot.Clear();
        }




        public void Draw()
        {
            int N = posShot.Count;

            for (int i = 0; i < N; i++)
            {
                Game1.DrawModel(shotModel, posShot[i].getMatric());
            }
        }


        private void ColorModel(Color c)
        {
            foreach (ModelMesh mesh in shotModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.DiffuseColor = c.ToVector3();
                    effect.Alpha = 1f;
                }
            }
        }

    }
}
