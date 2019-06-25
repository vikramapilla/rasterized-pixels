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
        
        private bool burstFlag = false;
        public static float angle = 0f;
        
        private int burstCounter = 0;

        public void initialize(ContentManager contentManager)
        {
            playerModel.body = contentManager.Load<Model>("Models/Body_Tria");
            playerModel.arms = contentManager.Load<Model>("Models/Arms_Skelett");
            playerModel.legs = contentManager.Load<Model>("Models/Legs_Skelett");
            playerPosition = new Vector3(0, 0, 0);
            buildRotationMap();
        }

        public void getPosition(ref QuadTree<BoundingBox> _QTree)
        {
            if (!burstFlag)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                {
                    if (burstCounter == 0)
                    {
                        burstCounter = 7;
                        burstFlag = true;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
            {
                burstFlag = false;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.A) && Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (burstCounter > 0)
                {
                    float tempZ = playerPosition.Z;
                    float tempX = playerPosition.X;
                    playerPosition.Z += 0.15f;
                    playerPosition.X += 0.15f;
                    burstCounter--;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.Z = tempZ;
                        playerPosition.X = tempX;
                    }
                    
                }
                else
                {
                    float tempZ = playerPosition.Z;
                    float tempX = playerPosition.X;
                    playerPosition.Z += 0.01f;
                    playerPosition.X += 0.01f;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.Z = tempZ;
                        playerPosition.X = tempX;
                    }
                }
            }
            else
            if (Keyboard.GetState().IsKeyDown(Keys.D) && Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (burstCounter > 0)
                {
                    float tempZ = playerPosition.Z;
                    float tempX = playerPosition.X;
                    playerPosition.Z += 0.15f;
                    playerPosition.X -= 0.15f;
                    burstCounter--;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.Z = tempZ;
                        playerPosition.X = tempX;
                    }
                }
                else
                {
                    float tempZ = playerPosition.Z;
                    float tempX = playerPosition.X;
                    playerPosition.Z += 0.01f;
                    playerPosition.X -= 0.01f;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.Z = tempZ;
                        playerPosition.X = tempX;
                    }
                }
            }
            else
            if (Keyboard.GetState().IsKeyDown(Keys.A) && Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (burstCounter > 0)
                {
                    float tempZ = playerPosition.Z;
                    float tempX = playerPosition.X;
                    playerPosition.Z -= 0.15f;
                    playerPosition.X += 0.15f;
                    burstCounter--;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.Z = tempZ;
                        playerPosition.X = tempX;
                    }
                }
                else
                {
                    float tempZ = playerPosition.Z;
                    float tempX = playerPosition.X;
                    playerPosition.Z -= 0.01f;
                    playerPosition.X += 0.01f;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.Z = tempZ;
                        playerPosition.X = tempX;
                    }
                }
            }
            else
            if (Keyboard.GetState().IsKeyDown(Keys.D) && Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (burstCounter > 0)
                {
                    float tempZ = playerPosition.Z;
                    float tempX = playerPosition.X;
                    playerPosition.Z -= 0.15f;
                    playerPosition.X -= 0.15f;
                    burstCounter--;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.Z = tempZ;
                        playerPosition.X = tempX;
                    }
                }
                else
                {
                    float tempZ = playerPosition.Z;
                    float tempX = playerPosition.X;
                    playerPosition.Z -= 0.01f;
                    playerPosition.X -= 0.01f;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.Z = tempZ;
                        playerPosition.X = tempX;
                    }
                }
            }
            else
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (burstCounter > 0)
                {
                    float tempZ = playerPosition.Z;
                    playerPosition.Z += 0.15f;
                    burstCounter--;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.Z = tempZ;
                    }
                }
                else
                {
                    float tempZ = playerPosition.Z;
                    playerPosition.Z += 0.01f;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.Z = tempZ;
                    }
                }
            }
            else
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (burstCounter > 0)
                {
                    float tempZ = playerPosition.Z;
                    playerPosition.Z -= 0.15f;
                    burstCounter--;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.Z = tempZ;
                    }
                }
                else
                {
                    float tempZ = playerPosition.Z;
                    playerPosition.Z -= 0.01f;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.Z = tempZ;
                    }
                }
            }
            else
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (burstCounter > 0)
                {
                    float tempX = playerPosition.X;
                    playerPosition.X += 0.15f;
                    burstCounter--;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.X = tempX;
                    }
                }
                else
                {
                    float tempX = playerPosition.X;
                    playerPosition.X += 0.01f;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.X = tempX;
                    }
                }
            }
            else
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (burstCounter > 0)
                {
                    float tempX = playerPosition.X;
                    playerPosition.X -= 0.15f;
                    burstCounter--;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.X = tempX;
                    }
                }
                else
                {
                    float tempX = playerPosition.X;
                    playerPosition.X -= 0.01f;

                    if (IsCollision(ref _QTree) == true)
                    {
                        playerPosition.X = tempX;
                    }
                }
            }

        }
        

        public float getCurrentRotation()
        {
            return angle;
        }

        public void getRotation()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (angle > 90f)
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

                    if (angle == -180f)
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
                }
                else if (angle > 0f)
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
                if (angle < 45f)
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

        public bool IsCollision(ref QuadTree<BoundingBox> _QTree)
        {
            bool hit = false;
            for (int i = 0; i < 2; i++)
            {
                CreateBoundingBox cbBbody = new CreateBoundingBox(playerModel.body, Matrix.CreateRotationY(angle)* Matrix.CreateTranslation(playerPosition));//
                CreateBoundingBox cbBarm = new CreateBoundingBox(playerModel.arms, Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(playerPosition));
                CreateBoundingBox cbBarm2 = new CreateBoundingBox(playerModel.arms, Matrix.CreateTranslation(playerPosition));

                List<BoundingBox> temp = _QTree.seekData(new Vector2(cbBbody.bBox.Min.X, cbBbody.bBox.Min.Z),
                new Vector2(cbBbody.bBox.Max.X, cbBbody.bBox.Max.Z));

                for (int u = 0; u < temp.Count; ++u)
                {
                    if (cbBbody.bBox.Intersects(temp[u]))//test if playerbody hits map
                    {
                        hit = true;
                    }
                }
                temp = _QTree.seekData(new Vector2(cbBarm.bBox.Min.X, cbBarm.bBox.Min.Z),
                new Vector2(cbBarm.bBox.Max.X, cbBarm.bBox.Max.Z));

                for (int u = 0; u < temp.Count; ++u)
                {
                    if (cbBarm.bBox.Intersects(temp[u]))//test if playerarm hits map
                    {
                        hit = true;
                    }
                }
                temp = _QTree.seekData(new Vector2(cbBarm2.bBox.Min.X, cbBarm2.bBox.Min.Z),
                new Vector2(cbBarm2.bBox.Max.X, cbBarm2.bBox.Max.Z));

                for (int u = 0; u < temp.Count; ++u)
                {
                    if (cbBarm2.bBox.Intersects(temp[u]))//test if playerarm hits map
                    {
                        hit = true;
                    }
                }
            }
            return hit;
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
