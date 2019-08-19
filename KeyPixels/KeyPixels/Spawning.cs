using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace KeyPixels
{
    class Spawning
    {
        List<int[,]> _mapList;
        Enemy enemy;
        //List<Enemy> elist;
        int n;
        int spawnrate;

        public Spawning(List<int[,]> mapList)
        {
            _mapList = mapList;
            enemy = new Enemy();
            //elist = new List<Enemy>();
            n = 1;
        }

        public void clearEnemy()
        {
            n = 0;
            enemy.worldMatrix.Clear();
            enemy.armlist1.Clear();
            enemy.armlist2.Clear();
            spawnrate = -1;
        }

        public Enemy GetEnemy()
        {
            return enemy;
        }
        //public void minusnumber() { n--; }
        public void SpawnEnemy(int index)
        {
            int posx;
            int posz;
            spawnrate--;
            /* 
             * numbers stand for one form of the tile
             * 0 = nothing,   1 = ground,       2 = ground + wall top,          3 = g + wall right,                 4 = g + wall bottom,            5 = g + wall left
             * 6 = g + wall (left + top),       7 = g + wall (top + right),     8 = g + wall (right + bottom),      9 = g + wall (bottom + left)
             * 10 = g + w (left top right),     11 = g + w (top right bottom),  12 = g + w (right bottom left),     13 = g + w (bottom left top)
             */
            
            if (n< 3&& spawnrate<0) {

                int[,] a = _mapList[index];
                posx = a.GetLength(0) / 2;// pos = lenght/2 so that the map is as central as possible
                posz = a.GetLength(1) / 2;
                Random r = new Random();
                int i = r.Next(0,a.GetLength(0));
                int j = r.Next(0, a.GetLength(1));
                
                if (a[j, i] != 0)
                {
                    

                    if ((Game1.getPosition().X - posx * 2 - i * 2 > 2 && Game1.getPosition().Z - posz * 2 - j * 2 > 2)|| (Game1.getPosition().X - posx * 2 - i * 2 < -2 && Game1.getPosition().Z - posz * 2 - j * 2 < -2)||
                        (Game1.getPosition().X - posx * 2 - i * 2 > 2 && Game1.getPosition().Z - posz * 2 - j * 2 > -2)|| (Game1.getPosition().X - posx * 2 - i * 2 < -2 && Game1.getPosition().Z - posz * 2 - j * 2 < 2))
                    {
                        if (n==0)
                        {
                            enemy.worldMatrix.Add(Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                            CreateBoundingBox cbBn = new CreateBoundingBox(enemy.enemyModel._model[1], Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                            CreateBoundingBox cbBr = new CreateBoundingBox(enemy.enemyModel._model[1], Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                            enemy.armlist1.Add(cbBn.bBox);
                            enemy.armlist2.Add(cbBr.bBox);
                            n++;
                            spawnrate = 50;
                        }
                        else
                        {
                            int count = enemy.worldMatrix.Count;

                            for (int l = 0; l < count; l++)
                            {
                                if ((enemy.worldMatrix[l].Translation.X - posx * 2 - i * 2 > 2 && enemy.worldMatrix[l].Translation.Z - posz * 2 - j * 2 > 2) || (enemy.worldMatrix[l].Translation.X - posx * 2 - i * 2 < -2 && enemy.worldMatrix[l].Translation.Z - posz * 2 - j * 2 < -2) ||
                                    (enemy.worldMatrix[l].Translation.X - posx * 2 - i * 2 > 2 && enemy.worldMatrix[l].Translation.Z - posz * 2 - j * 2 > -2) || (enemy.worldMatrix[l].Translation.X - posx * 2 - i * 2 < -2 && enemy.worldMatrix[l].Translation.Z - posz * 2 - j * 2 < 2))
                                {
                                    enemy.worldMatrix.Add(Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                                    CreateBoundingBox cbBn = new CreateBoundingBox(enemy.enemyModel._model[1], Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                                    CreateBoundingBox cbBr = new CreateBoundingBox(enemy.enemyModel._model[1], Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                                    enemy.armlist1.Add(cbBn.bBox);
                                    enemy.armlist2.Add(cbBr.bBox);
                                    n++;
                                    spawnrate = 50;

                                    break;
                                }
                            }
                        }
                        //if (!elist.Any())
                        //{
                        //    enemy.worldMatrix.Add(Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                        //    n++;
                        //    System.Diagnostics.Debug.WriteLine("1-2 funktioniert");
                        //}
                        //else
                        //{
                        //    foreach (Enemy e in elist)
                        //    {
                        //        System.Diagnostics.Debug.WriteLine("1-4 funktioniert");

                        //        if (e.getPosition().X - posx * 2 - i * 2 > 3 && e.getPosition().Z - posz * 2 - j * 2 > 3)
                        //        {
                        //            enemy.worldMatrix.Add(Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                        //            n++;
                        //            System.Diagnostics.Debug.WriteLine("2 funktioniert");
                        //        }
                        //    }
                        //}
                    }
                    //posx * 2 - i * 2, 0, posz * 2 - j * 2;
                    // pos = posx/posy * 2 because the ground model is 2 units big, -i/j *2 for the right pos like array
                }

            }

                
        }

    }
}
