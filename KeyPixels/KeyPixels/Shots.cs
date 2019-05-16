using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace KeyPixels
{
    class Shots
    {
        private static List<_Model> mOModel;
        private static List<List<_Value>> posModel;

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
            public CreateBoundingBox _bbox;
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
            ColorModel(_difcolor,0);
        }

        public void addModel(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel.Add(new List<_Value>());
        }

        public void addModel(ContentManager contentManager, string modelName, float _speed, Vector3 _directionSpeed,Color _difcolor)
        {
            helpConstruct(contentManager, modelName, _speed, _directionSpeed);
            posModel.Add(new List<_Value>());
            ColorModel(_difcolor,mOModel.Count-1);
        }

        /// <summary>
        ///     createShot will be create a Shot of now time posPlayer.
        ///     E.g. posMatrix := Player Matrix (+ Translate Shot position)
        /// </summary>

        public void createShot(Matrix posMatrix,int numberShot)
        {
            if (numberShot < posModel.Count && numberShot>=0)
            {
                _Value temp = new _Value();
                temp._matrix = posMatrix;
                temp._directionAddSpeed = Vector3.Transform(mOModel[numberShot].directionAddSpeed, Matrix.CreateFromQuaternion(temp._matrix.Rotation));
                temp._bbox = new CreateBoundingBox(mOModel[numberShot].mModel, temp._matrix);
                posModel[numberShot].Add(temp);
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
            for(int i=0;i<posModel.Count;++i)
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
            for (int n = posModel.Count-1; n >-1; --n)
            {
                int N = posModel[n].Count;
                for (int i = N-1; i >-1; --i)
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
                            firstStep = false;
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
            if(mOModel==null)
                mOModel = new List<_Model>();
            _Model temp = new _Model();
            temp.mModel=(contentManager.Load<Model>(modelName));
            temp.mDifColor = new List<Vector3>();

            foreach (ModelMesh mesh in temp.mModel.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    temp.mDifColor.Add(effect.DiffuseColor);

            temp.directionAddSpeed = new Vector3(_directionSpeed.X * _speed, _directionSpeed.Y * _speed, _directionSpeed.Z * _speed);
            mOModel.Add(temp);
        }

        private void ColorModel(Color c,int num)
        {
            var temp = mOModel[num];
            for (int i=0; i< temp.mDifColor.Count;++i)
                temp.mDifColor[i] = c.ToVector3();
            mOModel[num] = temp;
        }

    }
}
