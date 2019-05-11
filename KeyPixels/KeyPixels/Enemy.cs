using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace KeyPixels
{
    struct EnemyModel
    {
        public List<Model> _model;
    };

    class Enemy
    {
        public EnemyModel enemyModel;

        public List<Matrix> worldMatrix;

        private Vector3 enemyPosition;
        private Vector3 target;

        private float enemyAngle = 0f;
        private float playerAngle = 0f;


        public void initialize(ContentManager contentManager)
        {
            enemyModel._model = new List<Model>();
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
            target *= new Vector3(1, 1, 1) / 100;
            enemyPosition += target;
            playerAngle = 0;
            if(enemyAngle < playerAngle)
            {
                enemyAngle += 15f;
            }
            else
            {
                enemyAngle -= 15f;
            }
            worldMatrix[0] = Matrix.CreateRotationY(MathHelper.ToRadians(enemyAngle)) * Matrix.CreateTranslation(enemyPosition);
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
