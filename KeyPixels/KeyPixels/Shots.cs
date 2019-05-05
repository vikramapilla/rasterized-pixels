using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace KeyPixels
{
    class Shots
    {
        private static Model mModel;
        private static List<Matrix> posModel;
        private static Vector3 target;
        private static CreateBoundingBox bbModel;

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _targetSpeed)
        {
            helpConstruct(contentManager, modelName, _speed, _targetSpeed);
            posModel = new List<Matrix>();
        }

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _targetSpeed, int nStart)
        {
            helpConstruct(contentManager, modelName, _speed, _targetSpeed);
            posModel = new List<Matrix>(nStart);
        }

        public Shots(ContentManager contentManager, string modelName,float _speed,Vector3 _targetSpeed, Color _color)
        {
            helpConstruct(contentManager, modelName, _speed, _targetSpeed);
            posModel = new List<Matrix>();
            ColorModel(_color);
        }

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _targetSpeed, Color _color, int nStart)
        {
            helpConstruct(contentManager, modelName, _speed, _targetSpeed);
            posModel = new List<Matrix>(nStart);
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

        public void updateShotsPos(GameTime tm)
        {
            int N = posModel.Count;
            for (int i = 0; i < N; i++)
            {
                var v_temp = posModel[i];
                v_temp.Translation += Vector3.Transform(target, Matrix.CreateFromQuaternion(v_temp.Rotation));
                posModel[i] = v_temp;
            }
        }

        public bool IsCollision(BoundingBox _bModel, Matrix WorldMatrix)
        {
            BoundingBox bBox1;
            BoundingBox bBox2;
            int N = posModel.Count;
            for (int i = 0; i < N; i++)
            {
                bBox1.Max = Vector3.Transform(bbModel.bBox.Max, posModel[i]);
                bBox1.Min = Vector3.Transform(bbModel.bBox.Min, posModel[i]);

                for (int enemyMeshIndex = 0; enemyMeshIndex < mModel.Meshes.Count; enemyMeshIndex++)
                {
                    bBox2.Max = Vector3.Transform(_bModel.Max, WorldMatrix);
                    bBox2.Min = Vector3.Transform(_bModel.Min, WorldMatrix);

                    if (bBox1.Intersects(bBox2))
                    {
                        posModel.Remove(posModel[i]);
                        return true;
                    }
                }
            }
            return false;

        }

        public void Draw()
        {
            int N = posModel.Count;

            for (int i = 0; i < N; i++)
            {
                Game1.DrawModel(mModel, posModel[i]);
            }
        }



        private void helpConstruct(ContentManager contentManager, string modelName, float _speed, Vector3 _targetSpeed)
        {
            mModel = contentManager.Load<Model>(modelName);
            target = new Vector3(_targetSpeed.X * _speed, _targetSpeed.Y * _speed, _targetSpeed.Z * _speed);
            bbModel = new CreateBoundingBox(mModel, Matrix.Identity);
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
