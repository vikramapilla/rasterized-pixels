﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace KeyPixels
{
    class Shots
    {
        private static List<Model> mModel;
        private static List<List<_Value>> posModel;
        private static List<Vector3> directionAddSpeed;

        public struct _Value
        {
            public Matrix _matrix;
            public Vector3 _directionAddSpeed;
            public CreateBoundingBox _bbox;
        };

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel = new List<List<_Value>>();
        }

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed, int nStart)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel = new List<List<_Value>>();
        }

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed, Color _color)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel = new List<List<_Value>>();
            ColorModel(_color, 0);
        }

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed, Color _color, int nStart)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel = new List<List<_Value>>();
            ColorModel(_color, 0);
        }

        public void addModel(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
        }

        public void addModel(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed,Color _color)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            ColorModel(_color, mModel.Count-1);
        }

        /// <summary>
        ///     createShot will be create a Shot of now time posPlayer.
        ///     E.g. posMatrix := Player Matrix (+ Translate Shot position)
        /// </summary>

        public void createShot(Matrix posMatrix,int numberShot)
        {
            _Value temp = new _Value();
            temp._matrix = posMatrix;
            temp._directionAddSpeed = Vector3.Transform(directionAddSpeed[numberShot], Matrix.CreateFromQuaternion(temp._matrix.Rotation));
            temp._bbox = new CreateBoundingBox(mModel[numberShot], temp._matrix);
            if (posModel.Count-1 < numberShot)
            {
                posModel.Add(new List<_Value>());
                posModel[numberShot].Add(temp);
            }
            else
                  posModel[numberShot].Add(temp);
        }


        /// <summary>
        ///     createShot will be create a Shot of now time posPlayer. Y in Vector not use! faster!
        ///     E.g. posMatrix := Player Matrix (+ Translate Shot position)
        /// </summary>

        public void createShotnotY(Matrix posMatrix, int numberShot)
        {
            _Value temp = new _Value();
            temp._matrix = posMatrix;
            Matrix help = Matrix.CreateFromQuaternion(temp._matrix.Rotation);
            Vector3 help_vec = directionAddSpeed[numberShot];
            FastCalcMono3D.SmartMatrixVec3NotY(ref help_vec, ref help, ref temp._directionAddSpeed);
            if (posModel.Count < numberShot)
            {
                posModel.Add(new List<_Value>());
                posModel[0].Add(temp);
            }
                posModel[numberShot].Add(temp);
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
            for (int n = 0; n < posModel.Count; ++n)
            {
                int N = posModel[n].Count;
                for (int i = 0; i < N; i++)
                {
                    var temp = posModel[n][i];
                    Vector3 help_vec = directionAddSpeed[n];
                    FastCalcMono3D.SmartMatrixAddTransnotY(ref temp._matrix, ref temp._directionAddSpeed);
                    temp._bbox.bBox.Max += temp._directionAddSpeed;
                    temp._bbox.bBox.Min += temp._directionAddSpeed;
                    posModel[n][i] = temp;
                }
            }
        }

        /// <summary>
        ///     updateShotsPos will be change all position Matrix with seperate Speed vector. Y in Vector not use! faster!
        /// </summary>

        public void updateShotsPosnotY(GameTime tm)
        {
            for (int n = 0; n < posModel.Count; ++n)
            {
                int N = posModel[n].Count;
                for (int i = 0; i < N; i++)
                {
                    var temp = posModel[n][i];
                    FastCalcMono3D.SmartMatrixAddTransnotY(ref temp._matrix, ref temp._directionAddSpeed);
                    temp._bbox.bBox.Max += temp._directionAddSpeed;
                    temp._bbox.bBox.Min += temp._directionAddSpeed;
                    posModel[n][i] = temp;
                }
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
            bool hit = false;
            for (int n = 0; n < posModel.Count; ++n)
            { 
                int N = posModel[n].Count;
                for (int i = 0; i < N; i++)
                {
                    if (posModel[n][i]._bbox.bBox.Intersects(_bModel))
                    {
                        posModel[n].Remove(posModel[n][i]);
                        hit = true;
                        N--;
                    }
                }
            }
            return hit;
        }

        public bool IsCollision(ref Model _Model, ref List<Matrix> WorldMatrix, out List<int> _number) // wird entfernt in der zukunft
        {
            CreateBoundingBox bBox;
            bool ret = false;
            _number = new List<int>();

            for (int n = 0; n < posModel.Count; ++n)
            {
                int N = posModel.Count;
                for (int i = 0; i < N; i++)
                {
                    for (int enemyMeshIndex = 0; enemyMeshIndex < mModel[n].Meshes.Count; enemyMeshIndex++)
                    {
                        for (int z = 0; z < WorldMatrix.Count; ++z)
                        {
                            bBox = new CreateBoundingBox(_Model, WorldMatrix[i]);

                            if (posModel[n][i]._bbox.bBox.Intersects(bBox.bBox))
                            {
                                if (!ret)
                                    ret = true;
                                posModel[n].Remove(posModel[n][i]);
                                N--;
                                _number.Add(z);
                            }
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

        public void Draw(ref Matrix viewMatrix, ref Matrix projectionMatrix)
        {
            
            for (int n = 0; n < posModel.Count; ++n)
            {
                int N = posModel[n].Count;
                foreach (ModelMesh mesh in mModel[n].Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;

                        effect.View = viewMatrix;
                        effect.Projection = projectionMatrix;

                        for (int i = 0; i < N; i++)
                        {
                            effect.World = posModel[n][i]._matrix;
                            mesh.Draw();
                        }
                    }
                }
            }
        }


        private void helpConstruct(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed)
        {
            if(mModel==null)
                mModel = new List<Model>();
            mModel.Add(contentManager.Load<Model>(modelName));
            _directionSpeed.Normalize();
            if(directionAddSpeed==null)
                directionAddSpeed = new List<Vector3>();
            directionAddSpeed.Add(new Vector3(_directionSpeed.X * _speed, _directionSpeed.Y * _speed, _directionSpeed.Z * _speed));
        }

        private void ColorModel(Color c, int number)
        {
            foreach (ModelMesh mesh in mModel[number].Meshes)
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
