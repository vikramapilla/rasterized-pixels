using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace KeyPixels
{

    public class StackPickUp
    {
        public int pickedUpIndex;
        public float timer, TIMER;

        public StackPickUp(int _index, float _timer)
        {
            pickedUpIndex = _index;
            timer = _timer;
            TIMER = _timer;
        }

        public void update()
        {
            timer -= 0.075f;
        }


    }

    public class PickUps : Component
    {
        Model[] PickUpModel = new Model[6];
        Model PlayerModel;

        Texture2D[] PickUpTex = new Texture2D[6];
        Texture2D[] TimeTex = new Texture2D[4];


        CreateBoundingBox CBBPickUP, CBBPlayer;

        Vector3 pickUpLocation;
        Vector3 playerLocation;
        List<int[,]> mapList;
        Matrix pickUpWorldMatrix;
        Random random;

        static List<StackPickUp> pickUpStack;

        int mapIndex;
        int pickUpIndex;
        int pickedUpIndex;

        float posX, posZ;


        private float timer = 20f;
        private const float TIMER = 20f;

        static bool isPickUp;
        float spawnSpeed;


        private float pickUpHUDTimer = 10f;
        private const float HUDTIMER = 10f;


        public void LoadContent(ContentManager Content)
        {
            PlayerModel = Content.Load<Model>("Models/Legs_Skelett");
            PickUpModel[0] = Content.Load<Model>("Models/Pickup_Bazooka");
            PickUpModel[1] = Content.Load<Model>("Models/Pickup_Burst");
            PickUpModel[2] = Content.Load<Model>("Models/Pickup_Cooldown");
            PickUpModel[3] = Content.Load<Model>("Models/Pickup_Double");
            PickUpModel[4] = Content.Load<Model>("Models/Pickup_Piercing");
            PickUpModel[5] = Content.Load<Model>("Models/Pickup_Health");


            PickUpTex[0] = Content.Load<Texture2D>("HUD/PickUps/bazooka");
            PickUpTex[1] = Content.Load<Texture2D>("HUD/PickUps/burst");
            PickUpTex[2] = Content.Load<Texture2D>("HUD/PickUps/cooldown");
            PickUpTex[3] = Content.Load<Texture2D>("HUD/PickUps/double");
            PickUpTex[4] = Content.Load<Texture2D>("HUD/PickUps/piercing");
            PickUpTex[5] = Content.Load<Texture2D>("HUD/PickUps/health");

            TimeTex[0] = Content.Load<Texture2D>("HUD/PickUps/0_time");
            TimeTex[1] = Content.Load<Texture2D>("HUD/PickUps/1_time");
            TimeTex[2] = Content.Load<Texture2D>("HUD/PickUps/2_time");
            TimeTex[3] = Content.Load<Texture2D>("HUD/PickUps/3_time");

        }

        public void initialize()
        {
            pickUpLocation = new Vector3(0, 0, 0);
            random = new Random();
            pickUpStack = new List<StackPickUp>();
            mapList = Game1.getMapList();
            mapIndex = 0;
            isPickUp = false;
        }

        public override void Update(GameTime gameTime)
        {
            playerLocation = Game1.getPosition();

            if (isPickUp)
                spawnSpeed = 0.02f;
            else
                spawnSpeed = 0.05f;

            if (isCollision())
            {
                activatePower();
                timer = TIMER;
            }

            if (isTime())
            {
                generatePickUp();
            }

            for (int i = 0; i < pickUpStack.Count; i++)
            {

                pickUpStack[i].update();

                if (pickUpStack[i].pickedUpIndex == 1)
                {
                    if (Player.numberOfBursts <= 0)
                        pickUpStack[i].timer = 0;
                }
                if (pickUpStack[i].pickedUpIndex == 0)
                {
                    if (Game1.bazookashot == false)
                        pickUpStack[i].timer = 0;
                }
                if (pickUpStack[i].timer <= 0)
                {
                    if (pickUpStack[i].pickedUpIndex == 2)
                        Game1.morebullets = false;
                    else if (pickUpStack[i].pickedUpIndex == 3)
                        Game1.doubleshot = false;
                    else if (pickUpStack[i].pickedUpIndex == 4)
                        Shots.piercing = false;
                    pickUpStack.RemoveAt(i);
                    i--;
                }
            }
           /* System.Diagnostics.Debug.WriteLine("\nStack");
            for (int i = 0; i < pickUpStack.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine("Index: {0}, Time: {1}", pickUpStack[i].pickedUpIndex, pickUpStack[i].timer);
            }*/


        }

        private void activatePower()
        {
            if (pickedUpIndex == 0) //Bazooka
            {
                bool addedFlag = false;
                for (int i = 0; i < pickUpStack.Count; i++)
                {
                    if (pickUpStack[i].pickedUpIndex == 0)
                    {
                        pickUpStack[i].timer = float.MaxValue;
                        addedFlag = true;
                    }
                }

                if (!addedFlag)
                    pickUpStack.Add(new StackPickUp(0, float.MaxValue));
                Game1.bazookashot = true;
            }
            else if (pickedUpIndex == 1) //Burst
            {
                bool addedFlag = false;
                for (int i = 0; i < pickUpStack.Count; i++)
                {
                    if (pickUpStack[i].pickedUpIndex == 1)
                    {
                        pickUpStack[i].timer = float.MaxValue;
                        addedFlag = true;
                    }
                }

                if (!addedFlag)
                    pickUpStack.Add(new StackPickUp(1, float.MaxValue));
                Player.activateBurst();
               // System.Diagnostics.Debug.WriteLine("Burst Up");
            }
            else if (pickedUpIndex == 2) //Cooldown
            {
                for (int i = 0; i < pickUpStack.Count; i++)
                {
                    if (pickUpStack[i].pickedUpIndex == 3 || pickUpStack[i].pickedUpIndex == 4)
                    {
                        pickUpStack.RemoveAt(i);
                        i--;
                    }
                }
                bool addedFlag = false;
                for (int i = 0; i < pickUpStack.Count; i++)
                {
                    if (pickUpStack[i].pickedUpIndex == 2)
                    {
                        pickUpStack[i].timer = 50;
                        addedFlag = true;
                    }
                }

                if (!addedFlag)
                    pickUpStack.Add(new StackPickUp(2, 50));

                Game1.morebullets = true;
                Shots.piercing = false;
                Game1.doubleshot = false;
            }
            else if (pickedUpIndex == 3) //DoubleBullets
            {
                for (int i = 0; i < pickUpStack.Count; i++)
                {
                    if (pickUpStack[i].pickedUpIndex == 2 || pickUpStack[i].pickedUpIndex == 4)
                    {
                        pickUpStack.RemoveAt(i);
                        i--;
                    }
                }
                bool addedFlag = false;
                for (int i = 0; i < pickUpStack.Count; i++)
                {
                    if (pickUpStack[i].pickedUpIndex == 3)
                    {
                        pickUpStack[i].timer = 50;
                        addedFlag = true;
                    }
                }

                if (!addedFlag)
                    pickUpStack.Add(new StackPickUp(3, 50));
                Game1.doubleshot = true;
                Shots.piercing = false;
                Game1.morebullets = false;
            }
            else if (pickedUpIndex == 4) //Piercing
            {
                for (int i = 0; i < pickUpStack.Count; i++)
                {
                    if (pickUpStack[i].pickedUpIndex == 2 || pickUpStack[i].pickedUpIndex == 3)
                    {
                        pickUpStack.RemoveAt(i);
                        i--;
                    }
                }
                bool addedFlag = false;
                for (int i = 0; i < pickUpStack.Count; i++)
                {
                    if (pickUpStack[i].pickedUpIndex == 4)
                    {
                        pickUpStack[i].timer = 50;
                        addedFlag = true;
                    }
                }

                if (!addedFlag)
                    pickUpStack.Add(new StackPickUp(4, 50));
                Shots.piercing = true;
                Game1.morebullets = false;
                Game1.doubleshot = false;
            }
            else if (pickedUpIndex == 5) //Health
            {
                pickUpStack.Add(new StackPickUp(5, 5));
                Player.activateHealth();
            }
        }

        private bool isCollision()
        {
            if (isPickUp)
            {
                CBBPlayer = new CreateBoundingBox(PlayerModel, Matrix.CreateTranslation(playerLocation));

                if (CBBPickUP.bBox.Intersects(CBBPlayer.bBox))
                {
                    isPickUp = false;
                    pickedUpIndex = pickUpIndex;
                    return true;
                }
            }

            return false;

        }

        private bool isTime()
        {
            timer -= spawnSpeed;
            if (timer < random.Next(0, 2))
            {
                timer = TIMER;
                return true;
            }
            return false;
        }

        private bool isPickUpHUDTime()
        {
            pickUpHUDTimer -= 0.05f;
            if (pickUpHUDTimer < 0)
            {
                pickUpHUDTimer = HUDTIMER;
                return true;
            }
            return false;
        }

        private void generatePickUp()
        {
            mapIndex = Game1.getMapindex();
            int[,] currentMap = mapList[mapIndex];

            int i = random.Next(0, currentMap.GetLength(0));
            int j = random.Next(0, currentMap.GetLength(1));

            while (currentMap[j, i] == 0)
            {
                i = random.Next(0, currentMap.GetLength(0));
                j = random.Next(0, currentMap.GetLength(1));
            }

            if (currentMap[j, i] != 0)
            {
                posX = currentMap.GetLength(0) / 2;
                posZ = currentMap.GetLength(1) / 2;

                posX = posX * 2 - i * 2;
                posZ = posZ * 2 - j * 2;

                pickUpIndex = random.Next(0, 6);
                pickUpLocation = new Vector3(posX, 0, posZ);

                pickUpWorldMatrix = Matrix.CreateTranslation(pickUpLocation);

                CBBPickUP = new CreateBoundingBox(PickUpModel[pickUpIndex], Matrix.CreateTranslation(pickUpLocation));

                isPickUp = true;
                //System.Diagnostics.Debug.WriteLine("New Pick Up; {0}", pickUpIndex);
            }
        }

        public static void clearpickup()
        {
            isPickUp = false;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isPickUp)
            {
                Game1.Draw3DModel(PickUpModel[pickUpIndex], pickUpWorldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
            }

            for (int i = 0; i < pickUpStack.Count; i++)
            {
                spriteBatch.Draw(PickUpTex[pickUpStack[i].pickedUpIndex], new Rectangle(75 + (i * 100), 942, 75, 71), Color.White);
                if (pickUpStack[i].pickedUpIndex == 2 || pickUpStack[i].pickedUpIndex == 3 || pickUpStack[i].pickedUpIndex == 4)
                {
                    float timeDiff = pickUpStack[i].timer / pickUpStack[i].TIMER;
                    if (timeDiff > 0.5 && timeDiff <= 0.75)
                        spriteBatch.Draw(TimeTex[1], new Rectangle(75 + (i * 100), 850, 75, 75), Color.White);
                    else if (timeDiff > 0.25 && timeDiff <= 0.5)
                        spriteBatch.Draw(TimeTex[2], new Rectangle(75 + (i * 100), 850, 75, 75), Color.White);
                    else if (timeDiff > 0.0 && timeDiff <= 0.25)
                        spriteBatch.Draw(TimeTex[3], new Rectangle(75 + (i * 100), 850, 75, 75), Color.White);
                    else
                        spriteBatch.Draw(TimeTex[0], new Rectangle(75 + (i * 100), 850, 75, 75), Color.White);

                }
            }

        }
    }
}