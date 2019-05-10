using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace KeyPixels
{
    struct EnemyModel
    {
        public Model body;
        public Model arms;
        public Model legs;
    };

    class Enemy
    {
        public EnemyModel enemyModel;

        public Matrix worldMatrix;

        private Vector3 enemyPosition;
        

        public void initialize(ContentManager contentManager)
        {
            enemyModel.body = contentManager.Load<Model>("Models/Body_Tria");
            enemyModel.arms = contentManager.Load<Model>("Models/Arms_Skelett");
            enemyModel.legs = contentManager.Load<Model>("Models/Legs_Skelett");
            enemyPosition = new Vector3(2, 0, 0);
            worldMatrix = Matrix.CreateTranslation(enemyPosition);
        }

        public void enemyChase()
        {

        }


        public void Draw()
        {
            Game1.Draw3DModel(enemyModel.body, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
            Game1.Draw3DModel(enemyModel.arms, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
            Game1.Draw3DModel(enemyModel.legs, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
        }

    }
}
