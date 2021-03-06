﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace KeyPixels
{
    class Shots
    {
        private static List<_Model> mOModel;
        private static List<List<_Value>> posModel;

        List<ParticleEngine> ParticleEngines;
        Model particle;

        public float speed = 0.05f;
        public static bool piercing = false;


        private Player player = new Player();

        public struct _Model
        {
            public Model mModel;
            public List<Vector3> mDifColor;
            public Vector3 directionAddSpeed;
        };

        public struct _Value
        {
            public Matrix _matrix;
            public Vector3 _directionAddSpeed;
            public Vector3 _maximumDistance;
            public CreateBoundingBox _bbox;
            public float _shotAngle;
            public bool _bazookaShot;
        };

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel = new List<List<_Value>>();
            posModel.Add(new List<_Value>());
        }

        public Shots(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed, Color _difcolor)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel = new List<List<_Value>>();
            posModel.Add(new List<_Value>());
            ColorModel(_difcolor, 0);
        }
        public void initialize(ContentManager contentManager)
        {
            particle = contentManager.Load<Model>("Models/Shot_Tria");
            ParticleEngines = new List<ParticleEngine>();
        }
        public void addModel(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel.Add(new List<_Value>());
        }

        public void addModel(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed, Color _difcolor)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel.Add(new List<_Value>());
            ColorModel(_difcolor, mOModel.Count - 1);
        }

        /// <summary>
        ///     createShot will be create a Shot of now time posPlayer.
        ///     E.g. posMatrix := Player Matrix (+ Translate Shot position)
        /// </summary>

        public void createShot(Matrix posMatrix, int numberShot)
        {
            if (numberShot < posModel.Count && numberShot >= 0)
            {
                _Value temp = new _Value();
                temp._matrix = posMatrix;
                //Vector3 directionAddSpeed = new Vector3(new Vector3(0, 0, 1).X * speed, new Vector3(0, 0, 1).Y * speed, new Vector3(0, 0, 1).Z * speed);
                temp._directionAddSpeed = Vector3.Transform(mOModel[numberShot].directionAddSpeed, Matrix.CreateFromQuaternion(temp._matrix.Rotation));//mOModel[numberShot].directionAddSpeed
                temp._bbox = new CreateBoundingBox(mOModel[numberShot].mModel, temp._matrix);
                temp._shotAngle = player.getCurrentRotation();
                posModel[numberShot].Add(temp);
                //System.Diagnostics.Debug.WriteLine("Shot: {0}", temp._shotAngle);
                temp._bazookaShot = false;
            }
        }
        public void createBazookaShot(Matrix posMatrix, int numberShot, int directionIndex)
        {
            if (numberShot < posModel.Count && numberShot >= 0)
            {
                _Value temp = new _Value();
                temp._bazookaShot = true;
                temp._matrix = posMatrix;
                temp._maximumDistance = Vector3.Zero;
                Vector3 transformVector = Vector3.Zero;
                if (directionIndex == 0)
                {
                    transformVector = new Vector3(0, 0, 0.05f);
                }
                else if (directionIndex == 1)
                {
                    transformVector = new Vector3(-0.05f, 0, 0.05f);
                }
                else if (directionIndex == 2)
                {
                    transformVector = new Vector3(-0.05f, 0, 0);
                }
                else if (directionIndex == 3)
                {
                    transformVector = new Vector3(-0.05f, 0, -0.05f);
                }
                else if (directionIndex == 4)
                {
                    transformVector = new Vector3(0, 0, -0.05f);
                }
                else if (directionIndex == 5)
                {
                    transformVector = new Vector3(0.05f, 0, -0.05f);
                }
                else if (directionIndex == 6)
                {
                    transformVector = new Vector3(0.05f, 0, 0);
                }
                else if (directionIndex == 7)
                {
                    transformVector = new Vector3(0.05f, 0, 0.05f);
                }
                temp._directionAddSpeed = Vector3.Transform(transformVector, Matrix.CreateFromQuaternion(temp._matrix.Rotation));
                temp._bbox = new CreateBoundingBox(mOModel[numberShot].mModel, temp._matrix);
                temp._shotAngle = player.getCurrentRotation();
                posModel[numberShot].Add(temp);
                //System.Diagnostics.Debug.WriteLine("Shot: {0}", temp._shotAngle);
            }
        }

        /// <summary>
        ///     createShot will be create a Shot of now time posPlayer. Y in Vector not use! faster!
        ///     E.g. posMatrix := Player Matrix (+ Translate Shot position)
        /// </summary>

        public void createShotnotY(Matrix posMatrix, int numberShot)
        {
            if (numberShot < posModel.Count && numberShot >= 0)
            {
                _Value temp = new _Value();
                temp._matrix = posMatrix;
                temp._shotAngle = player.getCurrentRotation();
                Matrix help = Matrix.CreateFromQuaternion(temp._matrix.Rotation);
                Vector3 help_vec = mOModel[numberShot].directionAddSpeed;
                FastCalcMono3D.SmartMatrixVec3NotY(ref help_vec, ref help, ref temp._directionAddSpeed);
                posModel[numberShot].Add(temp);
            }
        }

        /// <summary>
        ///     clearAll will be remove all position Matrix.
        /// </summary>

        public void clearAll()
        {
            for (int i = 0; i < posModel.Count; ++i)
                posModel[i].Clear();
        }

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
                    temp._matrix.Translation += temp._directionAddSpeed;
                    temp._maximumDistance += temp._directionAddSpeed;
                    temp._bbox.bBox.Max += temp._directionAddSpeed;
                    temp._bbox.bBox.Min += temp._directionAddSpeed;

                    posModel[n][i] = temp;
                    if (temp._bazookaShot)
                    {
                        if (Math.Abs(temp._maximumDistance.X) > 1 || Math.Abs(temp._maximumDistance.Z) > 1)
                        {
                            posModel[n].Remove(posModel[n][i]);
                            N--;
                        }
                    }
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
            for (int n = 0; n < 2; ++n)
            {
                int N = posModel[n].Count;
                for (int i = 0; i < N; i++)
                {
                    if (posModel[n][i]._bbox.bBox.Intersects(_bModel))
                    {
                        hit = true;
                        if (piercing == false)
                        {
                            posModel[n].Remove(posModel[n][i]);
                            N--;
                        }
                    }
                }
            }
            return hit;
        }
        public bool playerIsCollision(ref BoundingBox _bModel)
        {
            bool hit = false;
            for (int n = 2; n < 4; ++n)
            {
                int N = posModel[n].Count;
                for (int i = 0; i < N; i++)
                {
                    if (posModel[n][i]._bbox.bBox.Intersects(_bModel))
                    {
                        hit = true;
                        
                        posModel[n].Remove(posModel[n][i]);
                        N--;
                        
                    }
                }
            }
            return hit;
        }

        /// <summary>
        /// IsCollision with a QuadTree
        /// </summary>
        /// <param name="_QTree"></param>
        /// <returns></returns>

        public bool IsCollision(ref QuadTree<BoundingBox> _QTree)
        {
            bool hit = false;
            for (int n = posModel.Count - 1; n > -1; --n)
            {
                int N = posModel[n].Count;
                for (int i = N - 1; i > -1; --i)
                {
                    List<BoundingBox> temp = _QTree.seekData(new Vector2(posModel[n][i]._bbox.bBox.Min.X, posModel[n][i]._bbox.bBox.Min.Z),
                        new Vector2(posModel[n][i]._bbox.bBox.Max.X, posModel[n][i]._bbox.bBox.Max.Z));
                    for (int u = 0; u < temp.Count && i < N; ++u)
                    {
                        if (posModel[n][i]._bbox.bBox.Intersects(temp[u]))
                        {
                            //Vector3 shotDisappearPosition = posModel[n][i]._matrix.Translation;
                            //float shotDisappearRotation = posModel[n][i]._shotAngle;
                            //System.Diagnostics.Debug.WriteLine(shotDisappearRotation);
                            //ParticleEngines.Add(new ParticleEngine(particle, shotDisappearPosition, shotDisappearRotation, "Wall"));
                            //Game1.soundManager.wallShotEffect();
                            posModel[n].Remove(posModel[n][i]);
                            hit = true;
                            N--;
                        }
                    }
                }
            }
            return hit;
        }

        /// <summary>
        /// IsCollision with a QuadTree and return List Collision
        /// </summary>
        /// <param name="_QTree"></param>
        /// <param name="_ret_collision"></param>
        /// <returns></returns>

        public bool IsCollision(ref QuadTree<BoundingBox> _QTree, out List<BoundingBox> _ret_collision)
        {
            _ret_collision = new List<BoundingBox>();
            bool hit = false;
            for (int n = posModel.Count - 1; n > -1; --n)
            {
                int N = posModel[n].Count;
                for (int i = N - 1; i > -1; --i)
                {
                    List<BoundingBox> temp = _QTree.seekData(new Vector2(posModel[n][i]._bbox.bBox.Min.X, posModel[n][i]._bbox.bBox.Min.Z),
                        new Vector2(posModel[n][i]._bbox.bBox.Max.X, posModel[n][i]._bbox.bBox.Max.Z));
                    for (int u = 0; u < temp.Count && i < N; ++u)
                    {
                        if (posModel[n][i]._bbox.bBox.Intersects(temp[u]))
                        {
                            posModel[n].Remove(posModel[n][i]);
                            _ret_collision.Add(temp[u]);
                            hit = true;
                            N--;
                        }
                    }
                }
            }
            return hit;
        }

        /// <summary>
        ///     Draw will be draw the Models in the World.
        ///     viewMatrix := Matrix.CreateLookAt(,,)
        ///     projectionMatrix := Matrix.CreatePerspectiveFieldOfView(,,,)
        /// </summary>

        public void Draw(ref Matrix viewMatrix, ref Matrix projectionMatrix)
        {

            for (int i = 0; i < ParticleEngines.Count; i++)
            {
                ParticleEngines[i].Update();
                ParticleEngines[i].Draw();
            }
            Vector3 DifC = new Vector3();
            bool firstStep = true;
            for (int n = 0; n < posModel.Count; ++n)
            {
                int N = posModel[n].Count;
                int neffect = 0;
                foreach (ModelMesh mesh in mOModel[n].mModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        if (firstStep)
                        {
                            effect.EnableDefaultLighting();
                            effect.PreferPerPixelLighting = true;
                            effect.View = viewMatrix;
                            effect.Projection = projectionMatrix;
                            DifC = effect.DiffuseColor;
                            //firstStep = false;
                        }
                        for (int i = 0; i < N; i++)
                        {
                            effect.World = posModel[n][i]._matrix;
                            effect.DiffuseColor = mOModel[n].mDifColor[neffect];
                            mesh.Draw();
                        }
                        neffect++;
                    }
                }
            }
            foreach (ModelMesh mesh in mOModel[0].mModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.DiffuseColor = DifC;
                }
            }
        }


        private void helpConstruct(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed)
        {
            if (mOModel == null)
                mOModel = new List<_Model>();
            _Model temp = new _Model();
            temp.mModel = (contentManager.Load<Model>(modelName));
            temp.mDifColor = new List<Vector3>();

            foreach (ModelMesh mesh in temp.mModel.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    temp.mDifColor.Add(effect.DiffuseColor);

            temp.directionAddSpeed = new Vector3(_directionSpeed.X * _speed, _directionSpeed.Y * _speed, _directionSpeed.Z * _speed);
            mOModel.Add(temp);
        }

        private void ColorModel(Color c, int num)
        {
            var temp = mOModel[num];
            for (int i = 0; i < temp.mDifColor.Count; ++i)
                temp.mDifColor[i] = c.ToVector3();
            mOModel[num] = temp;
        }

    }
}
