using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace KeyPixels
{
    struct PlayerModel
    {
        public Model body;
        public Model arms;
        public Model legs;
    };

    class Player
    {
        public PlayerModel playerModel;
        
        public Matrix worldMatrix;

        private Vector3 playerPosition;

        private Dictionary<string, float> rotationMap = new Dictionary<string, float>();

        private float horizontalAngle = 0f;
        private float verticalAngle = 0f;
        private float angle = 0f;


        private float currentRotation = 0f;
        private float desiredRotation = 0f;

        public void initialize(ContentManager contentManager)
        {
            playerModel.body = contentManager.Load<Model>("Models/Body_Tria");
            playerModel.arms = contentManager.Load<Model>("Models/Arms_Skelett");
            playerModel.legs = contentManager.Load<Model>("Models/Legs_Skelett");
            playerPosition = new Vector3(0, 0, 0);
            buildRotationMap();
        }

        public void getPosition()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                angle = rotationMap["North"];
                playerPosition.Z += 0.01f;

                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                {
                    float reachPosition = playerPosition.Z + 0.05f;
                    while (playerPosition.Z < reachPosition) {
                        playerPosition.Z += 0.025f;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                angle = rotationMap["South"];
                playerPosition.Z -= 0.01f;
                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                {
                    float reachPosition = playerPosition.Z - 0.05f;
                    while (playerPosition.Z > reachPosition)
                    {
                        playerPosition.Z -= 0.025f;
                    }
                }

            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                angle = rotationMap["West"];
                playerPosition.X += 0.01f;

                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                {
                    float reachPosition = playerPosition.X + 0.05f;
                    while (playerPosition.X < reachPosition)
                    {
                        playerPosition.X += 0.025f;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                angle = rotationMap["East"];
                playerPosition.X -= 0.01f;

                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                {
                    float reachPosition = playerPosition.X - 0.05f;
                    while (playerPosition.X > reachPosition)
                    {
                        playerPosition.X -= 0.025f;
                    }
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                angle = rotationMap["NorthWest"];
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                angle = rotationMap["NorthEast"];
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                angle = rotationMap["SouthWest"];
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                angle = rotationMap["SouthEast"];
            }
            System.Diagnostics.Debug.WriteLine(angle);
        }


        public void getRotation()
        {

        }

        private void buildRotationMap()
        {
            rotationMap.Add("North", 0);
            rotationMap.Add("South", 180);
            rotationMap.Add("East", -90);
            rotationMap.Add("West", 90);
            rotationMap.Add("NorthWest", 45);
            rotationMap.Add("NorthEast", -45);
            rotationMap.Add("SouthWest", 135);
            rotationMap.Add("SouthEast", -135);
        }


        public void Draw()
        {
            worldMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(playerPosition);
            Game1.Draw3DModel(playerModel.body, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
            Game1.Draw3DModel(playerModel.arms, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
            Game1.Draw3DModel(playerModel.legs, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
        }

    }
}
