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
        public Map(Model ground,Model wall, Matrix viewMatrix, Matrix projectionMatrix)
        {
            _ground = ground;
            _wall = wall;
            _viewMatrix = viewMatrix;
            _projectionMatrix = projectionMatrix;
        }
        public void CreateMap()
        {
            int posx;
            int posz;
            
            int[,] a = new int[7, 7] {
                { 0, 0, 10, 0, 0, 0, 0 },
                { 0, 6, 3, 0, 6, 7, 0 },
                { 0, 5, 1, 2, 1, 3, 0 },
                { 13, 1, 1, 1, 1, 1, 11 },
                { 0, 5, 1, 1, 1, 3, 0 },
                { 0, 9, 4, 1, 4, 8, 0 },
                { 0, 0, 0, 12, 0, 0, 0 } };
            posx = a.GetLength(0)/2;
            posz = a.GetLength(1)/2;
            for (int i=0;i<a.GetLength(0);i++ )
            {
                for(int j=0;j< a.GetLength(1); j++)
                {

                    if (a[j, i] == 1)
                    {
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2, _ground, 0);

                    }
                    else if (a[j, i] == 2)
                    {
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2, _ground, 0);
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2 + 1, _wall, 0);

                    }
                    else if (a[j, i] == 3)
                    {
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2, _ground, 0);
                        Draw(posx * 2 - i * 2 - 1, posz * 2 - j * 2, _wall, 90);

                    }
                    else if (a[j, i] == 4)
                    {
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2, _ground, 0);
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2 - 1, _wall, 0);

                    }
                    else if (a[j, i] == 5)
                    {
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2, _ground, 0);
                        Draw(posx * 2 - i * 2 + 1, posz * 2 - j * 2, _wall, 90);

                    }
                    else if (a[j, i] == 6)
                    {
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2, _ground, 0);
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2 + 1, _wall, 0);
                        Draw(posx * 2 - i * 2 + 1, posz * 2 - j * 2, _wall, 90);

                    }
                    else if (a[j, i] == 7)
                    {
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2, _ground, 0);
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2 + 1, _wall, 0);
                        Draw(posx * 2 - i * 2 - 1, posz * 2 - j * 2, _wall, 90);

                    }
                    else if (a[j, i] == 8)
                    {
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2, _ground, 0);
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2 - 1, _wall, 0);
                        Draw(posx * 2 - i * 2 - 1, posz * 2 - j * 2, _wall, 90);

                    }
                    else if (a[j, i] == 9)
                    {
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2, _ground, 0);
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2 - 1, _wall, 0);
                        Draw(posx * 2 - i * 2 + 1, posz * 2 - j * 2, _wall, 90);


                    }
                    else if (a[j, i] == 10)
                    {
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2, _ground, 0);
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2 + 1, _wall, 0);
                        Draw(posx * 2 - i * 2 + 1, posz * 2 - j * 2, _wall, 90);
                        Draw(posx * 2 - i * 2 - 1, posz * 2 - j * 2, _wall, 90);

                    }
                    else if (a[j, i] == 11)
                    {
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2, _ground, 0);
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2 + 1, _wall, 0);
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2 - 1, _wall, 0);
                        Draw(posx * 2 - i * 2 - 1, posz * 2 - j * 2, _wall, 90);

                    }
                    else if (a[j, i] == 12)
                    {
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2, _ground, 0);
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2 - 1, _wall, 0);
                        Draw(posx * 2 - i * 2 + 1, posz * 2 - j * 2, _wall, 90);
                        Draw(posx * 2 - i * 2 - 1, posz * 2 - j * 2, _wall, 90);

                    }
                    else if (a[j, i] == 13)
                    {
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2, _ground, 0);
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2 - 1, _wall, 0);
                        Draw(posx * 2 - i * 2 + 1, posz * 2 - j * 2, _wall, 90);
                        Draw(posx * 2 - i * 2, posz * 2 - j * 2 + 1, _wall, 0);

                    }
                    else { }

                    
                }
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

        public void Update(GameTime gameTime, Matrix view)

        {

        }
     }
}
