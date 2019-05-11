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
        public ConvexHull2D ConvexHull2D;

        public struct _Value
        {
            public Matrix _matrix;
            public Vector3 _directionAddSpeed;
            public CreateBoundingBox _bbox;
            public Vector2[] _ConvexHull2D;
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

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed, Color _color)
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
            temp._bbox = new CreateBoundingBox(mModel, temp._matrix);
            posModel.Add(temp);
        }


        /// <summary>
        ///     createShot will be create a Shot of now time posPlayer. Y in Vector not use! faster!
        ///     E.g. posMatrix := Player Matrix (+ Translate Shot position)
        /// </summary>

        public void createShotnotY(Matrix posMatrix)
        {
            _Value temp = new _Value();
            temp._matrix = posMatrix;
            Matrix help = Matrix.CreateFromQuaternion(temp._matrix.Rotation);
            FastCalcMono3D.SmartMatrixVec3NotY(ref directionAddSpeed, ref help, ref temp._directionAddSpeed);
            posModel.Add(temp);
        }

        private void createConvexHull(ref _Value temp)
        {
            Vector2[] help2 = ConvexHull2D.getConvexHull();
            ConvexHull2D.calcMatrixToCH_2D(ref temp._matrix, ref help2);
            temp._ConvexHull2D = new Vector2[help2.Length];
            temp._ConvexHull2D = help2;
        }

        /// <summary>
        ///     clearAll will be remove all position Matrix.
        /// </summary>

        public void clearAll() { posModel.Clear(); }

        /// <summary>
        ///     updateShotsPos will be change all position Matrix with seperate Speed vector.
        /// </summary>

        public void updateShotsPos(GameTime tm)
        {
            int N = posModel.Count;
            for (int i = 0; i < N; i++)
            {
                var temp = posModel[i];
                FastCalcMono3D.SmartMatrixAddTransnotY(ref temp._matrix, ref temp._directionAddSpeed);
                temp._bbox.bBox.Max += temp._directionAddSpeed;
                temp._bbox.bBox.Min += temp._directionAddSpeed;
                posModel[i] = temp;
            }
        }

        /// <summary>
        ///     updateShotsPos will be change all position Matrix with seperate Speed vector. Y in Vector not use! faster!
        /// </summary>

        public void updateShotsPosnotY(GameTime tm)
        {
            int N = posModel.Count;
            for (int i = 0; i < N; i++)
            {
                var temp = posModel[i];
                FastCalcMono3D.SmartMatrixAddTransnotY(ref temp._matrix, ref temp._directionAddSpeed);
                temp._bbox.bBox.Max += temp._directionAddSpeed;
                temp._bbox.bBox.Min += temp._directionAddSpeed;
                posModel[i] = temp;
            }
        }

        /// <summary>
        /// IsCollision with a BoundingBox
        /// </summary>
        /// <param name="_bModel"></param>
        /// <returns></returns>
        /// 
        public bool IsCollision(ref BoundingBox _bModel)
        {
            bool hit= false;
            int N = posModel.Count;
            for (int i = 0; i < N; i++)
            {
                    if (posModel[i]._bbox.bBox.Intersects(_bModel))
                    {
                        posModel.Remove(posModel[i]);
                        hit = true;
                        N--;
                    }
            }
            return hit;
        }

        public bool IsCollision(ref Model _Model, ref List<Matrix> WorldMatrix, out List<int> _number) // wird entfernt in der zukunft
        {
            CreateBoundingBox bBox;
            bool ret = false;
            int N = posModel.Count;
            _number = new List<int>();
            for (int i = 0; i < N; i++)
            {
                for (int enemyMeshIndex = 0; enemyMeshIndex < mModel.Meshes.Count; enemyMeshIndex++)
                {
                    for (int z = 0; z < WorldMatrix.Count; ++z)
                    {
                        bBox = new CreateBoundingBox(_Model, WorldMatrix[i]);

                        if (posModel[i]._bbox.bBox.Intersects(bBox.bBox))
                        {
                            if (!ret)
                                ret = true;
                            posModel.Remove(posModel[i]);
                            N--;
                            _number.Add(z);
                        }
                    }
                }
            }
            return ret;
        }

        public bool IsCollision(Vector2[] CH1)
        {
            bool b = false;

            for (int i = posModel.Count-1; i>0; --i)
            {
                bool temp = ConvexHull2D.IsCollision2D_CH(CH1, posModel[i]._ConvexHull2D);
                if (temp)
                {
                    b = true;
                    posModel.Remove(posModel[i]);
                }
            }
            return b;
        }

        /// <summary>
        ///     Draw will be draw the Models in the World.
        ///     viewMatrix := Matrix.CreateLookAt(,,)
        ///     projectionMatrix := Matrix.CreatePerspectiveFieldOfView(,,,)
        /// </summary>

        public void Draw(ref Matrix viewMatrix, ref Matrix projectionMatrix)
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
            ConvexHull2D = new ConvexHull2D(mModel);
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
