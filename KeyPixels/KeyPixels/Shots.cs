using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace KeyPixels
{
    class Shots
    {
        private static Model mModel;
        private static List<_Value> posModel;
        private static Vector3 target;
        private static CreateBoundingBox bbModel;

        private struct _Value {
            public Matrix _matrix;
            public Vector3 _target;
        };

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _targetSpeed)
        {
            helpConstruct(contentManager, modelName, _speed, _targetSpeed);
            posModel = new List<_Value>();
        }

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _targetSpeed, int nStart)
        {
            helpConstruct(contentManager, modelName, _speed, _targetSpeed);
            posModel = new List<_Value>(nStart);
        }

        public Shots(ContentManager contentManager, string modelName,float _speed,Vector3 _targetSpeed, Color _color)
        {
            helpConstruct(contentManager, modelName, _speed, _targetSpeed);
            posModel = new List<_Value>();
            ColorModel(_color);
        }

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _targetSpeed, Color _color, int nStart)
        {
            helpConstruct(contentManager, modelName, _speed, _targetSpeed);
            posModel = new List<_Value>(nStart);
            ColorModel(_color);
        }

        public void createShot(Matrix posMatrix)
        {
            _Value temp = new _Value();
            temp._matrix = posMatrix;
            temp._target = Vector3.Transform(target, Matrix.CreateFromQuaternion(temp._matrix.Rotation));
            posModel.Add(temp);
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
                var temp = posModel[i];
                temp._matrix.Translation += temp._target;
                posModel[i] = temp;
            }
        }

        public bool IsCollision(BoundingBox _bModel, Matrix WorldMatrix)
        {
            BoundingBox bBox1;
            BoundingBox bBox2;
            int N = posModel.Count;
            for (int i = 0; i < N; i++)
            {
                bBox1.Max = Vector3.Transform(bbModel.bBox.Max, posModel[i]._matrix);
                bBox1.Min = Vector3.Transform(bbModel.bBox.Min, posModel[i]._matrix);

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

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            int N = posModel.Count;

            for (int i = 0; i < N; i++)
            {
                foreach (ModelMesh mesh in mModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;

                        effect.World = posModel[i]._matrix;
                        effect.View = viewMatrix;
                        effect.Projection = projectionMatrix;
                    }
                    mesh.Draw();
                }
            }
        }



        private void helpConstruct(ContentManager contentManager, string modelName, float _speed, Vector3 _targetSpeed)
        {
            mModel = contentManager.Load<Model>(modelName);
            _targetSpeed.Normalize();
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
