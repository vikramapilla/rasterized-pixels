using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace KeyPixels
{
    public class PickUps: Component
    {
        Model[] PickUpModel = new Model[6];
        Model PlayerModel;
        CreateBoundingBox CBBPickUP, CBBPlayer;

        Vector3 pickUpLocation;
        Vector3 playerLocation;
        List<int[,]> mapList;
        Matrix pickUpWorldMatrix;
        Random random;

        int mapIndex;
        int pickUpIndex;
        int pickedUpIndex;

        float posX, posZ;


        private float timer = 20f;
        private const float TIMER = 20f;

        bool isPickUp;

        public void LoadContent(ContentManager Content)
        {
            PlayerModel = Content.Load<Model>("Models/Legs_Skelett");
            PickUpModel[0] = Content.Load<Model>("Models/Pickup_Bazooka");
            PickUpModel[1] = Content.Load<Model>("Models/Pickup_Burst");
            PickUpModel[2] = Content.Load<Model>("Models/Pickup_Cooldown");
            PickUpModel[3] = Content.Load<Model>("Models/Pickup_Double");
            PickUpModel[4] = Content.Load<Model>("Models/Pickup_Piercing");
            PickUpModel[5] = Content.Load<Model>("Models/Pickup_Health");
        }

        public void initialize()
        {
            pickUpLocation = new Vector3(0, 0, 0);
            random = new Random();
            mapList = Game1.getMapList();
            mapIndex = 0;
            isPickUp = false;
        }

        public override void Update(GameTime gameTime)
        {
            playerLocation = Game1.getPosition();

            if (isCollision())
            {
                activatePower();
                timer = TIMER;
            }

            if (isTime())
            {
                generatePickUp();
            }


        }

        private void activatePower()
        {
            if (pickedUpIndex == 1)
            {
                Player.activateBurst();
                System.Diagnostics.Debug.WriteLine("Burst Up");
            }
            else if (pickedUpIndex == 2)
            {
                Game1.morebullets = true;
                Shots.piercing = false;
            }
            else if (pickedUpIndex == 3)
            {
                Shots.piercing = false;
                Game1.morebullets = false;
            }
            else if (pickedUpIndex == 4)
            {
                Shots.piercing = true;
                Game1.morebullets = false;
            }
            else if (pickedUpIndex == 5)
            {
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
            timer -= 0.05f;
            if (timer < random.Next(0, 2))
            {
                timer = TIMER;
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

            while(currentMap[j, i] == 0)
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

                pickUpIndex = random.Next(1, 2);
                pickUpLocation = new Vector3(posX, 0, posZ);

                pickUpWorldMatrix = Matrix.CreateTranslation(pickUpLocation);

                CBBPickUP = new CreateBoundingBox(PickUpModel[pickUpIndex], Matrix.CreateTranslation(pickUpLocation));

                isPickUp = true;
                System.Diagnostics.Debug.WriteLine("New Pick Up; {0}", pickUpIndex);
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isPickUp)
            {
                Game1.Draw3DModel(PickUpModel[pickUpIndex], pickUpWorldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
            }
        }
    }
}