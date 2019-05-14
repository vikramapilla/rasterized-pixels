using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPixels
{
    class Map
    {
        Model _ground;
        Model _wall;
        Matrix _viewMatrix;
        Matrix _projectionMatrix;
        public List<Matrix> wallposMatrix;
        List<Matrix> groundposMatrix;
        List<Array> mapList;
        public Map(Model ground,Model wall, Matrix viewMatrix, Matrix projectionMatrix)
        {
            _ground = ground;
            _wall = wall;
            _viewMatrix = viewMatrix;
            _projectionMatrix = projectionMatrix;
            wallposMatrix = new List<Matrix>();
            groundposMatrix = new List<Matrix>();
            
        }
        public void CreateMap()
        {
            int posx;
            int posz;
            wallposMatrix.Clear();
            groundposMatrix.Clear();
            /*
             * built map from array 
             * numbers stand for one form of the tile
             * 0 = nothing,   1 = ground,       2 = ground + wall top,          3 = g + wall right,                 4 = g + wall bottom,            5 = g + wall left
             * 6 = g + wall (left + top),       7 = g + wall (top + right),     8 = g + wall (right + bottom),      9 = g + wall (bottom + left)
             * 10 = g + w (left top right),     11 = g + w (top right bottom),  12 = g + w (right bottom left),     13 = g + w (bottom left top)
             */
            int[,] a = new int[7, 7] {
                { 0, 0, 10, 0, 0, 0, 0 },
                { 0, 6, 3, 0, 6, 7, 0 },
                { 0, 5, 1, 2, 1, 3, 0 },
                { 13, 1, 1, 1, 1, 1, 11 },
                { 0, 5, 1, 1, 1, 3, 0 },
                { 0, 9, 4, 1, 4, 8, 0 },
                { 0, 0, 0, 12, 0, 0, 0 } };
            posx = a.GetLength(0)/2;// pos = lenght/2 so that the map is as central as possible
            posz = a.GetLength(1)/2;

            for (int i=0;i<a.GetLength(0);i++ )
            {
                for(int j=0;j< a.GetLength(1); j++)
                {

                    if (a[j, i] == 1)
                    {
                        groundposMatrix.Add(Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2));
                        // pos = posx/posy * 2 because the ground model is 2 units big, -i/j *2 for the right pos like array
                    }
                    else if (a[j, i] == 2)
                    {
                        groundposMatrix.Add(Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2 + 1));
                    }
                    else if (a[j, i] == 3)
                    {
                        groundposMatrix.Add(Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(posx * 2 - i * 2 - 1, 0, posz * 2 - j * 2));

                    }
                    else if (a[j, i] == 4)
                    {
                        groundposMatrix.Add(Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2 - 1));

                    }
                    else if (a[j, i] == 5)
                    {
                        groundposMatrix.Add(Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(posx * 2 - i * 2+1, 0, posz * 2 - j * 2 ));

                    }
                    else if (a[j, i] == 6)
                    {
                        groundposMatrix.Add(Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2 + 1));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(posx * 2 - i * 2 + 1, 0, posz * 2 - j * 2));

                    }
                    else if (a[j, i] == 7)
                    {
                        groundposMatrix.Add(Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2 + 1));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(posx * 2 - i * 2 - 1, 0, posz * 2 - j * 2));

                    }
                    else if (a[j, i] == 8)
                    {
                        groundposMatrix.Add(Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2 - 1));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(posx * 2 - i * 2 - 1, 0, posz * 2 - j * 2));

                    }
                    else if (a[j, i] == 9)
                    {
                        groundposMatrix.Add(Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2 - 1));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(posx * 2 - i * 2 + 1, 0, posz * 2 - j * 2));


                    }
                    else if (a[j, i] == 10)
                    {
                        groundposMatrix.Add(Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2 + 1));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(posx * 2 - i * 2 + 1, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(posx * 2 - i * 2 - 1, 0, posz * 2 - j * 2));

                    }
                    else if (a[j, i] == 11)
                    {
                        groundposMatrix.Add(Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2 + 1));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2 - 1));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(posx * 2 - i * 2 - 1, 0, posz * 2 - j * 2));

                    }
                    else if (a[j, i] == 12)
                    {
                        groundposMatrix.Add(Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2 - 1));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(posx * 2 - i * 2 + 1, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(posx * 2 - i * 2 - 1, 0, posz * 2 - j * 2));

                    }
                    else if (a[j, i] == 13)
                    {
                        groundposMatrix.Add(Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2 + 1));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(posx * 2 - i * 2 + 1, 0, posz * 2 - j * 2));
                        wallposMatrix.Add(Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateTranslation(posx * 2 - i * 2, 0, posz * 2 - j * 2 - 1));

                    }
                    else { }

                    
                }
            }
            
        }

        public void Draw()

        {

            foreach (Matrix m in wallposMatrix)
            {
                Draw(_wall, m);
            }
            foreach (Matrix m in groundposMatrix)
                {
                Draw(_ground, m);
                }

        }
        public void Draw(int posx, int posz, Model model, int angle)

        {

            foreach (ModelMesh meshn in model.Meshes)
            {
                foreach (BasicEffect effect in meshn.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.View = _viewMatrix;
                    effect.World = Matrix.CreateRotationY(MathHelper.ToRadians(angle))*Matrix.CreateTranslation(posx,0,posz);
                    effect.Projection = _projectionMatrix;
                    effect.DiffuseColor = Color.Blue.ToVector3();
                    effect.Alpha = 1.0f;
                }
                meshn.Draw();
            }

        }
        public void Draw( Model model, Matrix world)

        {

            foreach (ModelMesh meshn in model.Meshes)
            {
                foreach (BasicEffect effect in meshn.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.View = _viewMatrix;
                    effect.World = world;
                    effect.Projection = _projectionMatrix;
                    effect.DiffuseColor = Color.MistyRose.ToVector3();
                    effect.Alpha = 1.0f;
                }
                meshn.Draw();
            }

        }

        public void Update(GameTime gameTime, Matrix view)

        {

        }
     }
}
