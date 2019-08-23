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
        Model PickUpModel;
        Model PlayerModel;
        CreateBoundingBox CBBPickUP, CBBPlayer;

        Vector3 pickUpLocation;
        Vector3 playerLocation;
        List<int[,]> mapList;
        Matrix pickUpWorldMatrix;
        Random random;

        int mapIndex;
        int enemyCounter;

        float posX, posZ;


        private float timer = 20f;
        private const float TIMER = 20f;

        bool isPickUp;

        public void LoadContent(ContentManager Content)
        {
            PickUpModel = Content.Load<Model>("Models/Pickup");
            PlayerModel = Content.Load<Model>("Models/Legs_Skelett");
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
                timer = TIMER;
                generatePickUp();
            }
            if (isTime())
            {
                generatePickUp();
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
                    System.Diagnostics.Debug.WriteLine("Collided");
                    return true;
                }
            }

            return false;

        }

        private bool isTime()
        {
            timer -= 0.05f;
            if (timer < random.Next(0, 5))
            {
                timer = TIMER;
                System.Diagnostics.Debug.WriteLine("Time Up");
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
            if (currentMap[j, i] != 0)
            {
                posX = currentMap.GetLength(0) / 2;
                posZ = currentMap.GetLength(1) / 2;

                posX = posX * 2 - i * 2;
                posZ = posZ * 2 - j * 2;

                pickUpLocation = new Vector3(posX, 0, posZ);

                pickUpWorldMatrix = Matrix.CreateTranslation(pickUpLocation);

                CBBPickUP = new CreateBoundingBox(PickUpModel, Matrix.CreateTranslation(pickUpLocation));

                isPickUp = true;
                System.Diagnostics.Debug.WriteLine("New Pick Up");
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Game1.Draw3DModel(PickUpModel, pickUpWorldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
        }
    }
}