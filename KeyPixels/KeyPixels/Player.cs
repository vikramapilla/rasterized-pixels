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

        public static Vector3 playerPosition;

        private Dictionary<string, float> rotationMap = new Dictionary<string, float>();

        private float horizontalAngle = 0f;
        private float verticalAngle = 0f;
        public static float angle = 0f;


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
                playerPosition.Z += 0.01f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                playerPosition.Z -= 0.01f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                playerPosition.X += 0.01f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                playerPosition.X -= 0.01f;
            }

        }


        public void getRotation()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if(angle > 90f)
                {
                    angle = -180f;
                }

                if (angle > -90f)
                {
                    angle -= 15f;
                }
                else if (angle < -90f)
                {
                    angle += 15f;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (angle <= -90f)
                {
                    angle -= 15f;

                    if(angle == -180f)
                    {
                        angle = 180f;
                    }
                }
                else if (angle < 90f)
                {
                    angle += 15f;
                }
                else if (angle > 90f)
                {
                    angle -= 15f;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (angle < 0f)
                {
                    angle += 15f;
                }else if(angle > 0f)
                {
                    angle -= 15f;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (angle >= -180f && angle != 180f)
                {
                    if (angle >= 0f)
                    {
                        angle += 15f;
                    }
                    else if (angle > -180f)
                    {
                        angle -= 15f;
                    }
                    else
                    {
                        angle = 180f;
                    }
                }
                else if (angle < 180f)
                {
                    angle += 15f;
                }

            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) && Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (angle < 45f )
                {
                    angle += 15f;
                }
                else if (angle > 45f)
                {
                    angle -= 15f;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) && Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (angle < -45f)
                {
                    angle += 15f;
                }
                else if (angle > -45f)
                {
                    angle -= 15f;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A) && Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (angle < 135f)
                {
                    angle += 15f;
                }
                else if (angle > 135f)
                {
                    angle -= 15f;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) && Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (angle < -135f)
                {
                    angle += 15f;
                }
                else if (angle > -135f)
                {
                    angle -= 15f;
                }
            }

        }

        public Vector3 getCurrentPlayerPosition()
        {
            return playerPosition;
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
