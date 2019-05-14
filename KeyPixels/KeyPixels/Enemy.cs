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

        public List<Matrix> worldMatrix;

        private Vector3 enemyPosition;
        private Vector3 playerPosition;
        private Vector3 target;

        private float angle = 0f;
        private float enemyAngle = 0f;
        private float playerAngle = 0f;
        private float relativePositionX = 0f;
        private float relativePositionZ = 0f;

        public void initialize(ContentManager contentManager)
        {
            enemyModel._model = new List<Model>();
            player = new Player();
            enemyModel._model.Add(contentManager.Load<Model>("Models/Body_Tria"));
            enemyModel._model.Add(contentManager.Load<Model>("Models/Arms_Skelett"));
            enemyModel._model.Add(contentManager.Load<Model>("Models/Legs_Skelett"));
            worldMatrix = new List<Matrix>();
            enemyPosition = new Vector3(2, 0, 0);
            worldMatrix.Add(Matrix.CreateTranslation(enemyPosition));
        }

        public void enemyChase(Matrix playerPos)
        {
            target = playerPos.Translation - enemyPosition;
            target = Vector3.Normalize(target);
            target *= new Vector3(1, 1, 1) / 150;
            enemyPosition += target;
            playerAngle = 0;
            //getRotation();
            angle = (float)Math.Atan2(target.X, target.Z);
            //worldMatrix[0] = Matrix.CreateRotationY(MathHelper.ToRadians( angle)) * Matrix.CreateTranslation(enemyPosition);
            worldMatrix[0] = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(enemyPosition);
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



        public void Draw(ref Matrix viewMatrix, ref Matrix projectionMatrix)
        {
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
