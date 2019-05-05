using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Collections.Generic;

namespace KeyPixels
{
    class Shots
    {
        private static Model mModel;
        private static List<Matrix> posModel;
        private float speed;

        public Shots(ContentManager contentManager, string modelName,float _speed, Color _color)
        {
            mModel = contentManager.Load<Model>(modelName);
            posModel = new List<Matrix>();
            speed = _speed;
            ColorModel(_color);
        }

        public Shots(ContentManager contentManager, string modelName, float _speed, Color _color, int nStart)
        {
            mModel = contentManager.Load<Model>(modelName);
            posModel = new List<Matrix>(nStart);
            speed = _speed;
            ColorModel(_color);
        }

        public void createShot(Matrix posMatrix)
        {
            posModel.Add(posMatrix);
        }

        public void clearAll()
        {
            posModel.Clear();
        }

        public void updateShotPos(GameTime tm)
        {
            int N = posModel.Count;
            for (int i = 0; i < N; i++)
            {
                var temp = posModel[i];
                Matrix rotM = new Matrix();
                Matrix _transM = new Matrix();

                //extrahiere Rotations Matrix
                rotM.M11 = temp.M11; rotM.M12 = temp.M12; rotM.M13 = temp.M13; rotM.M21 = temp.M21; rotM.M22 = temp.M22;
                rotM.M23 = temp.M23; rotM.M31 = temp.M31; rotM.M32 = temp.M32; rotM.M33 = temp.M33; rotM.M44 = 1;

                //extrahiere Translations Matrix
                _transM.M11 = 1; _transM.M22 = 1; _transM.M33 = 1;
                _transM.M41 = temp.M41; _transM.M42 = temp.M42; _transM.M43 = temp.M43; _transM.M44 = 1;

                posModel[i] = Matrix.CreateTranslation(new Vector3(0, 0, speed)) * rotM * _transM;
            }
        }

        public void Draw()
        {
            int N = posModel.Count;

            for (int i = 0; i < N; i++)
            {
                Game1.DrawModel(mModel, posModel[i]);
            }
        }


        private void ColorModel(Color c)
        {
            foreach (ModelMesh mesh in mModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.DiffuseColor = c.ToVector3();
                    effect.Alpha = 1f;
                }
            }
        }

    }
}
