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
        private static Vector3 directionAddSpeed;
        private static CreateBoundingBox bbModel;

        private struct _Value {
            public Matrix _matrix;
            public Vector3 _directionAddSpeed;
        };

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel = new List<_Value>();
        }

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed, int nStart)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel = new List<_Value>(nStart);
        }

        public Shots(ContentManager contentManager, string modelName,float _speed,Vector3 _directionSpeed, Color _color)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel = new List<_Value>();
            ColorModel(_color);
        }

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed, Color _color, int nStart)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel = new List<_Value>(nStart);
            ColorModel(_color);
        }

        /// <summary>
        ///     createShot will be create a Shot of now time posPlayer.
        ///     E.g. posMatrix := Player Matrix (+ Translate Shot position)
        /// </summary>

        public void createShot(Matrix posMatrix)
        {
            _Value temp = new _Value();
            temp._matrix = posMatrix;
            temp._directionAddSpeed = Vector3.Transform(directionAddSpeed, Matrix.CreateFromQuaternion(temp._matrix.Rotation));
            posModel.Add(temp);
        }

        /// <summary>
        ///     clearAll will be remove all position Matrix.
        /// </summary>

        public void clearAll() {posModel.Clear();}

        /// <summary>
        ///     updateShotsPos will be change all position Matrix with seperate Speed vector.
        /// </summary>

        public void updateShotsPos(GameTime tm)
        {
            int N = posModel.Count;
            for (int i = 0; i < N; i++)
            {
                var temp = posModel[i];
                temp._matrix.Translation += temp._directionAddSpeed;
                posModel[i] = temp;
            }
        }

        public bool IsCollision(ref BoundingBox _bModel,ref Matrix WorldMatrix)
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

        public bool IsCollision(ref BoundingBox _bModel,ref Matrix[] WorldMatrix,out List<int> _number)
        {
            BoundingBox bBox1;
            BoundingBox bBox2;
            bool ret = false;
            int N = posModel.Count;
            _number = new List<int>();
            for (int i = 0; i < N; i++)
            {
                bBox1.Max = Vector3.Transform(bbModel.bBox.Max, posModel[i]._matrix);
                bBox1.Min = Vector3.Transform(bbModel.bBox.Min, posModel[i]._matrix);

                for (int enemyMeshIndex = 0; enemyMeshIndex < mModel.Meshes.Count; enemyMeshIndex++)
                {
                    for(int z=0;z<WorldMatrix.Length; ++z)
                    {
                        bBox2.Max = Vector3.Transform(_bModel.Max, WorldMatrix[z]);
                        bBox2.Min = Vector3.Transform(_bModel.Min, WorldMatrix[z]);

                        if (bBox1.Intersects(bBox2))
                        {
                            if (!ret)
                                ret = true;
                            posModel.Remove(posModel[i]);
                            _number.Add(z);

                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        ///     Draw will be draw the Models in the World.
        ///     viewMatrix := Matrix.CreateLookAt(,,)
        ///     projectionMatrix := Matrix.CreatePerspectiveFieldOfView(,,,)
        /// </summary>

        public void Draw(ref Matrix viewMatrix,ref Matrix projectionMatrix)
        {
            int N = posModel.Count;

            foreach (ModelMesh mesh in mModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;

                    for (int i = 0; i < N; i++)
                    {
                        effect.World = posModel[i]._matrix;
                        mesh.Draw();
                    }
                }
            }
        }



        private void helpConstruct(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed)
        {
            mModel = contentManager.Load<Model>(modelName);
            _directionSpeed.Normalize();
            directionAddSpeed = new Vector3(_directionSpeed.X * _speed, _directionSpeed.Y * _speed, _directionSpeed.Z * _speed);
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
