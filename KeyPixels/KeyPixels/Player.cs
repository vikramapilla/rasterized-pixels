using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

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

        public CreateBoundingBox cbBbody;
        public CreateBoundingBox cbBarm;
        public CreateBoundingBox cbBarm2;
        public CreateBoundingBox cbBarm3;

        Model particle;
        List<ParticleEngine> ParticleEngines;

        private Dictionary<string, float> rotationMap = new Dictionary<string, float>();

        private bool burstFlag = false;
        public static float angle = 0f;

        private int burstCounter = 0;
        public static int numberOfBursts = 0;
        public int shotsCounter { get; set; }
        public static int healthCounter { get; set; }

        public void initialize(ContentManager contentManager)
        {
            playerModel.body = contentManager.Load<Model>("Models/Body_Kenny");
            playerModel.arms = contentManager.Load<Model>("Models/Arms_Skelett_Tex");
            playerModel.legs = contentManager.Load<Model>("Models/Legs_Skelett_Walk");
            playerPosition = new Vector3(0, 0, 0);
            cbBbody = new CreateBoundingBox(playerModel.body, Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(playerPosition));
            cbBarm = new CreateBoundingBox(playerModel.arms, Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(playerPosition));
            cbBarm2 = new CreateBoundingBox(playerModel.arms, Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(playerPosition));
            cbBarm3 = new CreateBoundingBox(playerModel.arms, Matrix.CreateTranslation(playerPosition));
            buildRotationMap();
            shotsCounter = 10;
            healthCounter = 10;
            particle = contentManager.Load<Model>("Models/Shot_Tria");
            ParticleEngines = new List<ParticleEngine>();
        }

        public static void activateHealth()
        {
            healthCounter += 3;
            if (healthCounter > 10)
                healthCounter = 10;
        }

        public static void activateBurst()
        {
            numberOfBursts = 3;
        }

        public void getPosition(ref QuadTree<BoundingBox> _QTree)
        {
            //playerupdate
            if (healthCounter < 1)
            {
                //?
            }

            if (!burstFlag)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                {
                    if (numberOfBursts > 0)
                    {
                        if (burstCounter == 0)
                        {
                            burstCounter = 7;
                            burstFlag = true;
                            Game1.soundManager.burstEffect();
                        }
                    }
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
            {
                if (burstFlag)
                    numberOfBursts--;
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

                    if (IsCollision(ref _QTree, new Vector3(0.15f, 0, 0.15f)) == true)
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

                    if (IsCollision(ref _QTree, new Vector3(0.01f, 0, 0.01f)) == true)
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

                    if (IsCollision(ref _QTree, new Vector3(-0.15f, 0, 0.15f)) == true)
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

                    if (IsCollision(ref _QTree, new Vector3(-0.01f, 0, 0.01f)) == true)
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

                    if (IsCollision(ref _QTree, new Vector3(0.15f, 0, -0.15f)) == true)
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

                    if (IsCollision(ref _QTree, new Vector3(0.01f, 0, -0.01f)) == true)
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

                    if (IsCollision(ref _QTree, new Vector3(-0.15f, 0, -0.15f)) == true)
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

                    if (IsCollision(ref _QTree, new Vector3(-0.01f, 0, -0.01f)) == true)
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

                    if (IsCollision(ref _QTree, new Vector3(0, 0, 0.15f)) == true)
                    {
                        playerPosition.Z = tempZ;
                    }
                }
                else
                {
                    float tempZ = playerPosition.Z;
                    playerPosition.Z += 0.01f;

                    if (IsCollision(ref _QTree, new Vector3(0, 0, 0.01f)) == true)
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

                    if (IsCollision(ref _QTree, new Vector3(0, 0, -0.15f)) == true)
                    {
                        playerPosition.Z = tempZ;
                    }
                }
                else
                {
                    float tempZ = playerPosition.Z;
                    playerPosition.Z -= 0.01f;

                    if (IsCollision(ref _QTree, new Vector3(0, 0, -0.01f)) == true)
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

                    if (IsCollision(ref _QTree, new Vector3(0.15f, 0, 0)) == true)
                    {
                        playerPosition.X = tempX;
                    }
                }
                else
                {
                    float tempX = playerPosition.X;
                    playerPosition.X += 0.01f;

                    if (IsCollision(ref _QTree, new Vector3(0.01f, 0, 0)) == true)
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

                    if (IsCollision(ref _QTree, new Vector3(-0.15f, 0, 0)) == true)
                    {
                        playerPosition.X = tempX;
                    }
                }
                else
                {
                    float tempX = playerPosition.X;
                    playerPosition.X -= 0.01f;

                    if (IsCollision(ref _QTree, new Vector3(-0.01f, 0, 0)) == true)
                    {
                        playerPosition.X = tempX;
                    }
                }
            }

        }

        public void teleport()
        {
            playerPosition.X = 0;
            playerPosition.Z = -4;
            
        }

        public void teleportback()
        {
            playerPosition.Y = 0;
            angle = 0;
        }

        public void teleportup(float speed)
        {
            playerPosition.Y += 0.05f;
            angle += speed;
        }

        public void teleportdown(float speed)
        {
            playerPosition.Y -= 0.05f;
            angle += speed;
        }

        public void resetbbox()
        {
            cbBarm2 = new CreateBoundingBox(playerModel.arms, Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(playerPosition));
            cbBarm3 = new CreateBoundingBox(playerModel.arms, Matrix.CreateTranslation(playerPosition));
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

        public bool IsCollision(Shots shots)// for shot to player
        {
            bool hit = false;
            
                CreateBoundingBox cbB = new CreateBoundingBox(playerModel.body, worldMatrix);
                if (shots.playerIsCollision(ref cbB.bBox))// test if shot hits enemy
                {
                    Vector3 enemyDisappearPosition = playerPosition;
                    Game1.soundManager.enemyShotEffect();
                    ParticleEngines.Add(new ParticleEngine(particle, enemyDisappearPosition, 0f, "Enemy"));
                    healthCounter--;
                    //worldMatrix.Remove(worldMatrix[n]);//disapear

                    return true;
                }
                CreateBoundingBox cbBa = new CreateBoundingBox(playerModel.arms, worldMatrix);
                if (shots.playerIsCollision(ref cbB.bBox))// test if shot hits enemy
                {
                    Vector3 enemyDisappearPosition = playerPosition;
                    Game1.soundManager.enemyShotEffect();
                    ParticleEngines.Add(new ParticleEngine(particle, enemyDisappearPosition, 0f, "Enemy"));
                    healthCounter--;
                    //worldMatrix.Remove(worldMatrix[n]);//disapear

                    return true;
                }


            return hit;
        }

        public bool IsCollision(ref QuadTree<BoundingBox> _QTree, Vector3 target)
        {
            bool hit = false;
            
            cbBarm2.bBox.Max += target;
            cbBarm2.bBox.Min += target;
            cbBarm3.bBox.Max += target;
            cbBarm3.bBox.Min += target;

            List<BoundingBox> temp = _QTree.seekData(new Vector2(cbBarm2.bBox.Min.X, cbBarm2.bBox.Min.Z),
                new Vector2(cbBarm2.bBox.Max.X, cbBarm2.bBox.Max.Z));

            
            for (int u = 0; u < temp.Count; ++u)
            {
                if (cbBarm2.bBox.Intersects(temp[u]))//test if playerarm hits map
                {
                    cbBarm2.bBox.Max -= target;
                    cbBarm2.bBox.Min -= target;
                    cbBarm3.bBox.Max -= target;
                    cbBarm3.bBox.Min -= target;
                    return true;
                    //hit = true;
                }
            }

            temp = _QTree.seekData(new Vector2(cbBarm3.bBox.Min.X, cbBarm3.bBox.Min.Z),
            new Vector2(cbBarm3.bBox.Max.X, cbBarm3.bBox.Max.Z));
            
            for (int u = 0; u < temp.Count; ++u)
            {
                if (cbBarm3.bBox.Intersects(temp[u]))//test if playerarm hits map
                {
                    cbBarm2.bBox.Max -= target;
                    cbBarm2.bBox.Min -= target;
                    cbBarm3.bBox.Max -= target;
                    cbBarm3.bBox.Min -= target;
                    return true;
                    //hit = true;
                }
            }

            cbBbody = new CreateBoundingBox(playerModel.body, Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(playerPosition));
            
            temp = _QTree.seekData(new Vector2(cbBbody.bBox.Min.X, cbBbody.bBox.Min.Z),
            new Vector2(cbBbody.bBox.Max.X, cbBbody.bBox.Max.Z));

            for (int u = 0; u < temp.Count; ++u)
            {
                if (cbBbody.bBox.Intersects(temp[u]))//test if playerbody hits map
                {
                    cbBarm2.bBox.Max -= target;
                    cbBarm2.bBox.Min -= target;
                    cbBarm3.bBox.Max -= target;
                    cbBarm3.bBox.Min -= target;
                    return true;
                    //hit = true;
                }
            }

            cbBarm = new CreateBoundingBox(playerModel.arms, Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(playerPosition));

            temp = _QTree.seekData(new Vector2(cbBarm.bBox.Min.X, cbBarm.bBox.Min.Z),
            new Vector2(cbBarm.bBox.Max.X, cbBarm.bBox.Max.Z));

            for (int u = 0; u < temp.Count; ++u)
            {
                if (cbBarm.bBox.Intersects(temp[u]))//test if playerbody hits map
                {
                    cbBarm2.bBox.Max -= target;
                    cbBarm2.bBox.Min -= target;
                    cbBarm3.bBox.Max -= target;
                    cbBarm3.bBox.Min -= target;
                    return true;
                    //hit = true;
                }
            }
            //if (hit == true)
            //{
            //    cbBarm2.bBox.Max -= target;
            //    cbBarm2.bBox.Min -= target;
            //    cbBarm3.bBox.Max -= target;
            //    cbBarm3.bBox.Min -= target;
            //}
            return hit;
        }

        public void Draw()
        {
            for (int i = 0; i < ParticleEngines.Count; i++)
            {
                ParticleEngines[i].Update();
                ParticleEngines[i].Draw();
            }
            worldMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(playerPosition);
            Game1.Draw3DModel(playerModel.body, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
            Game1.Draw3DModel(playerModel.arms, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
            Game1.Draw3DModel(playerModel.legs, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
        }

    }
}
