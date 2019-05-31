using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace KeyPixels
{
    struct EnemyModel
    {
        public List<Model> _model;
    };

    class Enemy
    {
        public EnemyModel enemyModel;
        public Player player;
        Model particle;
        List<ParticleEngine> ParticleEngines;

        public List<Matrix> worldMatrix;

        private Vector3 enemyPosition;
        private Vector3 playerPosition;
        private Vector3 target;

        private float angle = 0f;
        private float relativePositionX = 0f;
        private float relativePositionZ = 0f;

        public void initialize(ContentManager contentManager)
        {
            enemyModel._model = new List<Model>();
            player = new Player();
            enemyModel._model.Add(contentManager.Load<Model>("Models/Body_Tria"));
            enemyModel._model.Add(contentManager.Load<Model>("Models/Arms_Skelett"));
            enemyModel._model.Add(contentManager.Load<Model>("Models/Legs_Skelett"));
            particle = contentManager.Load<Model>("Models/Shot_Tria");
            worldMatrix = new List<Matrix>();
            enemyPosition = new Vector3(2,0,0);
            worldMatrix.Add(Matrix.CreateTranslation(enemyPosition));
            ParticleEngines = new List<ParticleEngine>();
        }

        public void enemyChase(Matrix playerPos)
        {
            
            for (int i = 0;i<worldMatrix.Count;i++)
            {
                Matrix m = worldMatrix[i];
                target = playerPos.Translation - m.Translation;
                target = Vector3.Normalize(target);
                target *= new Vector3(1, 1, 1) / 150;
                enemyPosition = m.Translation + target;
                //getRotation();
                angle = (float)Math.Atan2(target.X, target.Z);
                //worldMatrix[0] = Matrix.CreateRotationY(MathHelper.ToRadians( angle)) * Matrix.CreateTranslation(enemyPosition);
                worldMatrix[i] = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(enemyPosition);
                
            }
            
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
            }else if (relativePositionX < 0)
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

        public bool IsCollision(ref QuadTree<BoundingBox> _QTree, Shots shots, Player player)
        {
            bool hit = false;
            int N = worldMatrix.Count;
            List<BoundingBox> box = new List<BoundingBox>();
            for (int n = N - 1; n > -1; --n)
            {
                
                for (int i = 0; i < 2; i++)
                {
                    CreateBoundingBox cbB = new CreateBoundingBox(enemyModel._model[i], worldMatrix[n]);
                    if (shots.IsCollision(ref cbB.bBox))
                    {
                        Vector3 enemyDisappearPosition = worldMatrix[n].Translation;
                        ParticleEngines.Add(new ParticleEngine(particle, enemyDisappearPosition, "Enemy"));
                        worldMatrix.Remove(worldMatrix[n]);//disapear
                        //hit = true;
                        N--;
                        break;
                    }
                    List<BoundingBox> temp = _QTree.seekData(new Vector2(cbB.bBox.Min.X, cbB.bBox.Min.Z),
                            new Vector2(cbB.bBox.Max.X, cbB.bBox.Max.Z));
                    if (i == 1) continue;
                    for (int u = 0; u < temp.Count && n < N; ++u)
                    {
                        if (cbB.bBox.Intersects(temp[u]))
                        {
                            target = player.worldMatrix.Translation - worldMatrix[n].Translation;
                            target = Vector3.Normalize(target);
                            target *= new Vector3(1, 1, 1) / 150;
                            worldMatrix[n]= Matrix.CreateTranslation(worldMatrix[n].Translation - target*new Vector3(1,0,0));
                            if (cbB.bBox.Intersects(temp[u]))
                            {
                                worldMatrix[n] = Matrix.CreateTranslation(worldMatrix[n].Translation - target * new Vector3(0, 0, 1));
                                if (cbB.bBox.Intersects(temp[u]))
                                {
                                }
                                else worldMatrix[n] = Matrix.CreateTranslation(worldMatrix[n].Translation + target * new Vector3(1, 0, 0));
                                    
                            }
                            else worldMatrix[n] = Matrix.CreateTranslation(worldMatrix[n].Translation + target * new Vector3(0, 0, 1));

                            //hit = true;
                            break;
                        }
                    }
                    if (box.Count == 0)
                    {
                        box.Add(cbB.bBox);
                    }
                    else
                    {
                        for (int e = 0; e < box.Count; e++)
                        {
                            if (cbB.bBox.Intersects(box[e]))
                            {
                                worldMatrix[n] = Matrix.CreateTranslation(worldMatrix[n].Translation - target);
                                break;
                            }
                        }
                        box.Add(cbB.bBox);
                    }
                }
            }
            
                CreateBoundingBox cbBbody = new CreateBoundingBox(player.playerModel.body, player.worldMatrix);
                CreateBoundingBox cbBarm = new CreateBoundingBox(player.playerModel.arms, player.worldMatrix);
            for (int i=0;i<box.Count;i++)
            {
                if (cbBarm.bBox.Intersects(box[i]) || cbBbody.bBox.Intersects(box[i]))
                {
                    hit = true;
                }
            }

            return hit;
        }
        public bool IsCollision(Matrix playerPos)
        {
            for (int n =0; n < worldMatrix.Count; n++)
            {
                for (int i = 0; i < 2; i++)
                {
                    CreateBoundingBox cbB = new CreateBoundingBox(enemyModel._model[i], worldMatrix[n]);
                    for (int k = 0; k < worldMatrix.Count; k++)
                    {
                        if (k == n) { continue; }
                        CreateBoundingBox cbB2 = new CreateBoundingBox(enemyModel._model[i], worldMatrix[k]);
                        if (cbB.bBox.Intersects(cbB2.bBox))
                        {
                            //target = playerPos.Translation - worldMatrix[n].Translation;
                            //target = Vector3.Normalize(target);
                            //target *= new Vector3(1, 1, 1) / 150;
                            worldMatrix[k] = Matrix.CreateTranslation(worldMatrix[k].Translation - target);
                        }

                    }
                }
            }

            return false;
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

                        effect.DiffuseColor = Color.Crimson.ToVector3();
                        effect.View = viewMatrix;
                        effect.Projection = projectionMatrix;
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
