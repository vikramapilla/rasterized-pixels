﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

namespace KeyPixels
{
    struct EnemyModel
    {
        public List<Model> _model;
    };

    class Enemy
    {
        static public EnemyModel enemyModel;
        public Player player;
        Model particle;
        List<ParticleEngine> ParticleEngines;
        static public List<BoundingBox> armlist1;
        static public List<BoundingBox> armlist2;


        Player playerPos;
        QuadTree<BoundingBox> map;

        static public List<Matrix> worldMatrix;

        private Vector3 enemyPosition;
        private Vector3 playerPosition;
        private Vector3 target;

        private float angle = 0f;
        private float relativePositionX = 0f;
        private float relativePositionZ = 0f;

        public int StartIndex { get; set; }
        public int Count { get; set; }

        public void initialize(ContentManager contentManager)
        {
            enemyModel._model = new List<Model>();
            player = new Player();
            enemyModel._model.Add(contentManager.Load<Model>("Models/Body_Enemy"));
            enemyModel._model.Add(contentManager.Load<Model>("Models/Arms_Skelett_Tex2"));
            enemyModel._model.Add(contentManager.Load<Model>("Models/Legs_Skelett_Walk2"));
            particle = contentManager.Load<Model>("Models/partical");
            worldMatrix = new List<Matrix>();
            enemyPosition = new Vector3(2, 0, 0);
            //worldMatrix.Add(Matrix.CreateTranslation(enemyPosition));
            ParticleEngines = new List<ParticleEngine>();
            armlist1 = new List<BoundingBox>();
            armlist2 = new List<BoundingBox>();
            //CreateBoundingBox cbBn = new CreateBoundingBox(enemyModel._model[1], Matrix.CreateTranslation(new Vector3(2, 0, 0)));
            //CreateBoundingBox cbBr = new CreateBoundingBox(enemyModel._model[1], Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(new Vector3(2, 0, 0)));
            //armlist1.Add(cbBn.bBox);
            //armlist2.Add(cbBr.bBox);
        }


        public void Thread(Player player, QuadTree<BoundingBox> map2)
        {

            // Prepare the list of threads and the list of tasks for those threads to work on.
            int threadCount = worldMatrix.Count / 3;

            if (worldMatrix.Count % 3 == 0)
            {
                Enemy[] tasks = new Enemy[threadCount];
                Thread[] threads = new Thread[threadCount];

                for (int index = 0; index < threadCount; index++)
                {
                    // Create the task, and set it up with the right properties--these are our
                    // parameters.
                    tasks[index] = new Enemy()
                    {
                        Count = 3,
                        StartIndex = index * 3,
                        playerPos = player,
                        map = map2
                    };

                    // Create the thread
                    threads[index] = new Thread(tasks[index].enemyChase);

                    // Start the thread running
                    threads[index].Start();
                }


                // Wait around and join up with all of the threads, so that when we 
                // move on, we're sure all of the work has been done.
                for (int index = 0; index < threadCount; index++)
                {
                    threads[index].Join();
                }
            }
            else
            {
                Enemy[] tasks = new Enemy[threadCount+1];
                Thread[] threads = new Thread[threadCount+1];
                int index=0;
                for (index = 0; index < threadCount; index++)
                {
                    // Create the task, and set it up with the right properties--these are our
                    // parameters.
                    tasks[index] = new Enemy()
                    {
                        Count = 3,
                        StartIndex = index * 3,
                        playerPos = player,
                        map = map2
                    };

                    // Create the thread
                    threads[index] = new Thread(tasks[index].enemyChase);

                    // Start the thread running
                    threads[index].Start();
                }
                    tasks[threadCount] = new Enemy()
                    {
                        Count = worldMatrix.Count % 3,
                        StartIndex = threadCount * 3,
                        playerPos = player,
                        map = map2
                    };
                // Create the thread
                threads[threadCount] = new Thread(tasks[threadCount].enemyChase);

                // Start the thread running
                threads[threadCount].Start();


                // Wait around and join up with all of the threads, so that when we 
                // move on, we're sure all of the work has been done.
                for (int index2 = 0; index2 < threadCount; index2++)
                {
                    threads[index2].Join();
                }
            }

        }



        //multithread
        public void enemyChase()
        {
            //for (int index = 0; index < worldMatrix.Count; index++)
            //{
            for (int index = StartIndex; index < StartIndex + Count; index++)
            {

                Matrix m = worldMatrix[index];
                Vector3 tar = playerPos.worldMatrix.Translation - m.Translation;
                tar = Vector3.Normalize(tar) / 66;
                target = tar * new Vector3(1, 1, 1);
                enemyPosition = m.Translation + target;
                //getRotation();
                angle = (float)Math.Atan2(target.X, target.Z);
                //worldMatrix[0] = Matrix.CreateRotationY(MathHelper.ToRadians( angle)) * Matrix.CreateTranslation(enemyPosition);
                //Console.WriteLine(target);
                worldMatrix[index] = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(enemyPosition);
                if (IsCollision(playerPos, ref map, index, target) == true)
                {
                    //worldMatrix[i] = m;
                    target = tar * new Vector3(1, 1, 0);
                    //Console.WriteLine("mist0" + target);
                    //enemyPosition = m.Translation + target;
                    angle = (float)Math.Atan2(target.X, target.Z);
                    worldMatrix[index] = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(m.Translation + target);
                    if (IsCollision(playerPos, ref map, index, target) == true)
                    {
                        //worldMatrix[i] = m;
                        target = tar * new Vector3(0, 1, 1);
                        //Console.WriteLine("mist1" + target);
                        //enemyPosition = m.Translation + target;
                        angle = (float)Math.Atan2(target.X, target.Z);
                        worldMatrix[index] = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(m.Translation + target);
                        if (IsCollision(playerPos, ref map, index, target) == true)
                        {
                            worldMatrix[index] = m;
                            //Console.WriteLine("mist2");
                        }


                    }

                }

            }

        }

        //normal
        public void enemyChase(Player playerPos,ref QuadTree<BoundingBox> map)
        {
            for (int index = 0; index < worldMatrix.Count; index++)
            {

                Matrix m = worldMatrix[index];
                Vector3 tar = playerPos.worldMatrix.Translation - m.Translation;
                tar = Vector3.Normalize(tar) / 100;
                target = tar * new Vector3(1, 1, 1);
                enemyPosition = m.Translation + target;
                //getRotation();
                angle = (float)Math.Atan2(target.X, target.Z);
                //worldMatrix[0] = Matrix.CreateRotationY(MathHelper.ToRadians( angle)) * Matrix.CreateTranslation(enemyPosition);
                //Console.WriteLine(target);
                worldMatrix[index] = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(enemyPosition);
                if (IsCollision(playerPos, ref map, index, target) == true)
                {
                    //worldMatrix[i] = m;
                    target = tar * new Vector3(1, 1, 0);
                    //Console.WriteLine("mist0" + target);
                    //enemyPosition = m.Translation + target;
                    angle = (float)Math.Atan2(target.X, target.Z);
                    worldMatrix[index] = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(m.Translation + target);
                    if (IsCollision(playerPos, ref map, index, target) == true)
                    {
                        //worldMatrix[i] = m;
                        target = tar * new Vector3(0, 1, 1);
                        //Console.WriteLine("mist1" + target);
                        //enemyPosition = m.Translation + target;
                        angle = (float)Math.Atan2(target.X, target.Z);
                        worldMatrix[index] = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(m.Translation + target);
                        if (IsCollision(playerPos, ref map, index, target) == true)
                        {
                            worldMatrix[index] = m;
                            //Console.WriteLine("mist2");
                        }


                    }

                }

            }

        }

        public void clearList()
        {
            armlist1.Clear();
            armlist2.Clear();
        }


        private void getRotation()
        {
            playerPosition = Player.playerPosition;
            relativePositionX = enemyPosition.X - playerPosition.X;
            relativePositionZ = enemyPosition.Z - playerPosition.Z;

            if (relativePositionX >= 0)
            {

                if (relativePositionX > -0.5 && relativePositionX < 0.5 && relativePositionZ < 0)
                {
                    angle = 0f;
                }
                else if (relativePositionZ > -0.65 && relativePositionZ < 0.65)
                {
                    angle = -90f;
                }
                else if (relativePositionZ > 0.65)
                {
                    angle = -135f;
                }
                else if (relativePositionZ < -0.65)
                {
                    angle = -45f;
                }
            }
            else if (relativePositionX < 0)
            {

                if (relativePositionX > -0.5 && relativePositionX < 0.5 && relativePositionZ > 0)
                {
                    angle = 180f;
                }
                else if (relativePositionZ > -0.65 && relativePositionZ < 0.65)
                {
                    angle = 90f;
                }
                else if (relativePositionZ > 0.65)
                {
                    angle = 135f;
                }
                else if (relativePositionZ < -0.65)
                {
                    angle = 45f;
                }
            }
            System.Diagnostics.Debug.WriteLine("{0}, {1}", relativePositionX, relativePositionZ);
        }



        public bool IsCollision(Shots shots)// for shot to enemy
        {
            bool hit = false;
            int N = worldMatrix.Count;

            for (int n = N - 1; n > -1; --n)
            {

                for (int i = 0; i < 2; i++)
                {
                    CreateBoundingBox cbB = new CreateBoundingBox(enemyModel._model[i], worldMatrix[n]);
                    if (shots.IsCollision(ref cbB.bBox))// test if shot hits enemy
                    {
                        Vector3 enemyDisappearPosition = worldMatrix[n].Translation;
                        Game1.soundManager.enemyShotEffect();
                        ParticleEngines.Add(new ParticleEngine(particle, enemyDisappearPosition, 0f, "Enemy"));
                        worldMatrix.Remove(worldMatrix[n]);//disapear
                        armlist1.Remove(armlist1[n]);
                        armlist2.Remove(armlist2[n]);

                        hit = true;
                        N--;
                        break;
                    }


                }
            }

            return hit;
        }

        public bool IsCollision(Player player, ref QuadTree<BoundingBox> _QTree, int index, Vector3 target)// 
        {

            bool hit = false;

            BoundingBox b = armlist1[index];
            b.Max += target;
            b.Min += target;
            BoundingBox b2 = armlist2[index];
            b2.Max += target;
            b2.Min += target;


            for (int i = 0; i < 2; i++)
            {
                CreateBoundingBox cbB = new CreateBoundingBox(enemyModel._model[i], worldMatrix[index]);
                //if (bodylist.Count <= index)
                //{

                //    if (i == 0)
                //    {
                //        if (bodylist.Count == 0)
                //        {
                //            bodylist.Add(cbB.bBox);
                //        }
                //        else
                //        {
                //            for (int e = 0; e < bodylist.Count; e++)
                //            {
                //                if (cbB.bBox.Intersects(bodylist[e]))//test if enemy hit enemybody
                //                {

                //                    hit = true;
                //                    break;
                //                }
                //            }
                //            if (hit == true)
                //            {
                //                cbB.bBox.Max -= target;
                //                cbB.bBox.Min -= target;
                //            }
                //            bodylist.Add(cbB.bBox);
                //        }
                //    }
                //    if (i == 1)
                //    {
                //        if (armlist.Count == 0)
                //        {
                //            armlist.Add(cbB.bBox);
                //        }
                //        else
                //        {
                //            for (int e = 0; e < armlist.Count; e++)
                //            {
                //                if (cbB.bBox.Intersects(armlist[e]))//test if enemy hit enemyarm
                //                {

                //                    hit = true;
                //                    break;
                //                }
                //            }
                //            if (hit == true)
                //            {
                //                cbB.bBox.Max -= target;
                //                cbB.bBox.Min -= target;
                //            }
                //            armlist.Add(cbB.bBox);
                //        }
                //    }
                //    if (hit == true) return true;
                //}
                //if (i == 0)
                //{
                //List<BoundingBox> temp = _QTree.seekData(new Vector2(cbB.bBox.Min.X, cbB.bBox.Min.Z),
                //new Vector2(cbB.bBox.Max.X, cbB.bBox.Max.Z));

                //for (int u = 0; u < temp.Count; ++u)
                //{
                //    if (cbB.bBox.Intersects(temp[u]))//test if enemybody hits map
                //    {
                //        return true;
                //    }
                //}
                //}
                if (i == 1)
                {
                    //CreateBoundingBox cbBn = new CreateBoundingBox(enemyModel._model[i], Matrix.CreateTranslation(worldMatrix[index].Translation));
                    //CreateBoundingBox cbBr = new CreateBoundingBox(enemyModel._model[i], Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(worldMatrix[index].Translation));

                    //CreateBoundingBox cbBr = new CreateBoundingBox(enemyModel._model[i], Matrix.CreateRotationY(MathHelper.ToRadians(90)) * worldMatrix[index]);
                    List<BoundingBox> temp = _QTree.seekData(new Vector2(b.Min.X, b.Min.Z),
                    new Vector2(b.Max.X, b.Max.Z));

                    for (int u = 0; u < temp.Count; ++u)
                    {
                        if (b.Intersects(temp[u]))//test if enemyarm hits map
                        {
                            return true;
                        }
                    }
                    temp = _QTree.seekData(new Vector2(b2.Min.X, b2.Min.Z),
                    new Vector2(b2.Max.X, b2.Max.Z));

                    for (int u = 0; u < temp.Count; ++u)
                    {
                        if (b2.Intersects(temp[u]))//test if enemyarm hits map
                        {
                            return true;
                        }
                    }
                }

                CreateBoundingBox cbBbody = player.cbBbody;
                CreateBoundingBox cbBarm = player.cbBarm;
                //CreateBoundingBox cbBbody = new CreateBoundingBox(player.playerModel.body, player.worldMatrix);
                //CreateBoundingBox cbBarm = new CreateBoundingBox(player.playerModel.arms, player.worldMatrix);
                for (int n = 0; n < 2; n++)
                {
                    if (cbBarm.bBox.Intersects(cbB.bBox) || cbBbody.bBox.Intersects(cbB.bBox))
                    {
                        if (Player.healthCoolDown < 1)
                        {
                            Player.healthCounter--;
                            Player.healthCoolDown = Player.HealthCoolDown;
                            Game1.soundManager.hurtEffect();
                            Game1.damage = true;
                        }
                        return true;
                    }
                }


            }
            if (hit == false)
            {
                armlist1[index] = b;
                armlist2[index] = b2;
            }
            return hit;

        }


        public void Draw(ref Matrix viewMatrix, ref Matrix projectionMatrix)
        {

            for (int i = 0; i < ParticleEngines.Count; i++)
            {
                ParticleEngines[i].Update();
                ParticleEngines[i].Draw();
            }
            for (int z = 0; z < enemyModel._model.Count; ++z)
            {
                foreach (ModelMesh mesh in enemyModel._model[z].Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;

                        effect.View = viewMatrix;
                        effect.Projection = projectionMatrix;

                        //effect.DiffuseColor = Color.MediumBlue.ToVector3();

                        for (int i = 0; i < worldMatrix.Count; i++)
                        {
                            effect.World = worldMatrix[i];
                            mesh.Draw();
                        }
                    }
                }
            }
        }

    }
}
