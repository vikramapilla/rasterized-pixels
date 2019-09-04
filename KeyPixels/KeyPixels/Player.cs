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

        //SoundManager soundManager;

        Model particle;
        List<ParticleEngine> ParticleEngines;

        private Dictionary<string, float> rotationMap = new Dictionary<string, float>();

        private bool burstFlag = false;
        public static float angle = 0f;


        private int burstCounter = 0;
        public static int numberOfBursts = 0;
        public int shotsCounter { get; set; }
        public static int healthCounter { get; set; }
        public static float healthCoolDown = 15;
        public static float HealthCoolDown = 60;
        Vector3 movement;

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
            healthCounter = 5;
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
            healthCoolDown--;

            //
            movement = new Vector3(0, 0, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                movement.Z += 0.02f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                movement.X += 0.02f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                movement.Z -= 0.02f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                movement.X -= 0.02f;
            }

            if (movement.X != 0 || movement.Z != 0)
            {
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
                float temp_Z = playerPosition.Z;
                float temp_X = playerPosition.X;
                movement = Vector3.Normalize(movement) / 50;
                if (burstCounter > 0)
                {
                    movement.Z *= 7;
                    movement.X *= 7;
                    burstCounter--;
                }

                playerPosition.Z += movement.Z;
                playerPosition.X += movement.X;
                float temp_angle = angle;
                angle = (float)Math.Atan2(movement.X, movement.Z);
                angle = (angle + temp_angle) / 2;
                if (IsCollision(ref _QTree, movement) == true)
                {
                    playerPosition.Z = temp_Z;
                    playerPosition.X = temp_X;
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
                if (healthCoolDown < 1)
                {
                    Vector3 enemyDisappearPosition = playerPosition;
                    Game1.soundManager.enemyShotEffect();
                    ParticleEngines.Add(new ParticleEngine(particle, enemyDisappearPosition, 0f, "Enemy"));
                    healthCounter--;
                    Game1.soundManager.hurtEffect();
                    healthCoolDown = HealthCoolDown;
                    Game1.damage = true;
                }
                //worldMatrix.Remove(worldMatrix[n]);//disapear

                return true;
            }
            CreateBoundingBox cbBa = new CreateBoundingBox(playerModel.arms, worldMatrix);
            if (shots.playerIsCollision(ref cbBa.bBox))// test if shot hits enemy
            {

                if (healthCoolDown < 1)
                {
                    Vector3 enemyDisappearPosition = playerPosition;
                    Game1.soundManager.enemyShotEffect();
                    ParticleEngines.Add(new ParticleEngine(particle, enemyDisappearPosition, 0f, "Enemy"));

                    healthCounter--;
                    Game1.soundManager.hurtEffect();
                    Game1.damage = true;

                    healthCoolDown = HealthCoolDown;
                }
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
            worldMatrix = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(playerPosition);
            //worldMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(playerPosition);
            Game1.Draw3DModel(playerModel.body, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
            Game1.Draw3DModel(playerModel.arms, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
            Game1.Draw3DModel(playerModel.legs, worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
        }

    }
}
