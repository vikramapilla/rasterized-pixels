using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace KeyPixels
{
    class Player
    {
        public Model playerModel;

        public Matrix worldMatrix;

        private Vector3 playerPosition;

        private float horizontalAngle = 0f;
        private float verticalAngle = 0f;
        private float angle = 0f;

        public void initialize(ContentManager contentManager)
        {
            playerModel = contentManager.Load<Model>("Models/Arms_Skelett");
            playerPosition = new Vector3(0, 0, 0);
        }

        public void Draw()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                angle = 0f;
                playerPosition += new Vector3(0, 0, 0.01f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                angle = 180f;
                playerPosition -= new Vector3(0, 0, 0.01f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                angle = 90f;
                playerPosition += new Vector3(0.01f, 0, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                angle = -90f;
                playerPosition -= new Vector3(0.01f, 0, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                angle = 45f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                angle = -45f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                angle = 135f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                angle = -135f;
            }
            worldMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(playerPosition);
            System.Diagnostics.Debug.WriteLine(angle);

            Game1.Draw3DModel(playerModel, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);

        }

    }
}
