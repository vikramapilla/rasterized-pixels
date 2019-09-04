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
        int n;
        int spawnrate;
        int maxenemy=40;
        int samemax=30;
        public static bool isspawnended = false;

        public Spawning(List<int[,]> mapList)
        {
            _mapList = mapList;
            enemy = new Enemy();
            n = 0;
            isspawnended = false;
        }

        public void clearEnemy()
        {
            n = 0;
            Enemy.worldMatrix.Clear();
            Enemy.armlist1.Clear();
            Enemy.armlist2.Clear();
            enemy.clearList();
            spawnrate = -1;
        }

        public Enemy GetEnemy()
        {
            return enemy;
        }
        //public void minusnumber() { n--; }
        public void SpawnEnemy(int index, Player player)
        {
            int count = Enemy.worldMatrix.Count;
            int posx;
            int posz;
            spawnrate--;
            /* 
             * numbers stand for one form of the tile
             * 0 = nothing,   1 = ground,       2 = ground + wall top,          3 = g + wall right,                 4 = g + wall bottom,            5 = g + wall left
             * 6 = g + wall (left + top),       7 = g + wall (top + right),     8 = g + wall (right + bottom),      9 = g + wall (bottom + left)
             * 10 = g + w (left top right),     11 = g + w (top right bottom),  12 = g + w (right bottom left),     13 = g + w (bottom left top)
             */
            if (n == maxenemy) { isspawnended = true; }
            else if(count < samemax && spawnrate<0) {

                int[,] a = _mapList[index];
                posx = a.GetLength(0) / 2;// pos = lenght/2 so that the map is as central as possible
                posz = a.GetLength(1) / 2;
                Random r = new Random();
                int i = r.Next(0,a.GetLength(0));
                int j = r.Next(0, a.GetLength(1));
                
                if (a[j, i] != 0)
                {
                    
                    if (((player.getCurrentPlayerPosition().X - ((posx * 2) - (i * 2))) > 1 || player.getCurrentPlayerPosition().X - (posx * 2 - i * 2) < -1) && (player.getCurrentPlayerPosition().Z - (posz * 2 - j * 2) < 1 || player.getCurrentPlayerPosition().Z - (posz * 2 - j * 2) < -1))
                    {
                        if (count==0)
                        {
                            Enemy.worldMatrix.Add(Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                            CreateBoundingBox cbBn = new CreateBoundingBox(Enemy.enemyModel._model[1], Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                            CreateBoundingBox cbBr = new CreateBoundingBox(Enemy.enemyModel._model[1], Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                            Enemy.armlist1.Add(cbBn.bBox);
                            Enemy.armlist2.Add(cbBr.bBox);
                            n++;
                            spawnrate = 50;
                        }
                        else
                        {

                            for (int l = 0; l < count; l++)
                            {
                                
                                    if ((Enemy.worldMatrix[l].Translation.X - posx * 2 - i * 2 > 2 || Enemy.worldMatrix[l].Translation.X - posx * 2 - i * 2 < -2) && (Enemy.worldMatrix[l].Translation.Z - posz * 2 - j * 2 < 2 || Enemy.worldMatrix[l].Translation.Z - posz * 2 - j * 2 < -2))
                                    {
                                        Enemy.worldMatrix.Add(Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                                    CreateBoundingBox cbBn = new CreateBoundingBox(Enemy.enemyModel._model[1], Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                                    CreateBoundingBox cbBr = new CreateBoundingBox(Enemy.enemyModel._model[1], Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(new Vector3(posx * 2 - i * 2, 0, posz * 2 - j * 2)));
                                    Enemy.armlist1.Add(cbBn.bBox);
                                    Enemy.armlist2.Add(cbBr.bBox);
                                    n++;
                                    spawnrate = 50;

                                    break;
                                }
                            }
                        }
                    }
                    //posx * 2 - i * 2, 0, posz * 2 - j * 2;
                    // pos = posx/posy * 2 because the ground model is 2 units big, -i/j *2 for the right pos like array
                }

            }

                
        }

    }
}
