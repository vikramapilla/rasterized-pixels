using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KeyPixels.UI
{
    class OptionsMenu : Component
    {
        Menu menu;

        List<List<Texture2D>> optionsButtonsValues;
        List<List<Texture2D>> optionsButtonsValuesActivated;

        int optionActivatedIndex;
        int[] optionActivatedMod = { 3, 2, 11, 1 };
        public int[] optionActivatedValues = { 1, 0, 5, 0 };
        public int[] tempOptionActivatedValues = { 0, 0, 0, 0 };

        bool upButtonFlag = false;
        bool downButtonFlag = false;
        bool leftButtonFlag = false;
        bool rightButtonFlag = false;
        bool enterButtonFlag = false;

        public bool initializeFlag = true;
        public bool tempChangesFlag = false;

        public void LoadContent(ContentManager Content)
        {
            menu = new Menu();
            menu.addBackground(Content.Load<Texture2D>("UI/Options/options_background_0"),
                Content.Load<Texture2D>("UI/Options/options_background_1"));


            optionsButtonsValues = new List<List<Texture2D>>();
            optionsButtonsValuesActivated = new List<List<Texture2D>>();

            for (int i = 0; i < 4; i++)
            {
                optionsButtonsValues.Add(new List<Texture2D>());
                optionsButtonsValuesActivated.Add(new List<Texture2D>());
            }

            optionsButtonsValues[0].Add(Content.Load<Texture2D>("UI/Options/0_0"));
            optionsButtonsValues[0].Add(Content.Load<Texture2D>("UI/Options/0_1"));
            optionsButtonsValues[0].Add(Content.Load<Texture2D>("UI/Options/0_2"));

            optionsButtonsValues[1].Add(Content.Load<Texture2D>("UI/Options/1_0"));
            optionsButtonsValues[1].Add(Content.Load<Texture2D>("UI/Options/1_1"));

            optionsButtonsValues[2].Add(Content.Load<Texture2D>("UI/Options/2_0"));
            optionsButtonsValues[2].Add(Content.Load<Texture2D>("UI/Options/2_1"));
            optionsButtonsValues[2].Add(Content.Load<Texture2D>("UI/Options/2_2"));
            optionsButtonsValues[2].Add(Content.Load<Texture2D>("UI/Options/2_3"));
            optionsButtonsValues[2].Add(Content.Load<Texture2D>("UI/Options/2_4"));
            optionsButtonsValues[2].Add(Content.Load<Texture2D>("UI/Options/2_5"));
            optionsButtonsValues[2].Add(Content.Load<Texture2D>("UI/Options/2_6"));
            optionsButtonsValues[2].Add(Content.Load<Texture2D>("UI/Options/2_7"));
            optionsButtonsValues[2].Add(Content.Load<Texture2D>("UI/Options/2_8"));
            optionsButtonsValues[2].Add(Content.Load<Texture2D>("UI/Options/2_9"));
            optionsButtonsValues[2].Add(Content.Load<Texture2D>("UI/Options/2_10"));

            optionsButtonsValues[3].Add(Content.Load<Texture2D>("UI/Options/3_0"));

            optionsButtonsValuesActivated[0].Add(Content.Load<Texture2D>("UI/Options/0_0_0"));
            optionsButtonsValuesActivated[0].Add(Content.Load<Texture2D>("UI/Options/0_1_0"));
            optionsButtonsValuesActivated[0].Add(Content.Load<Texture2D>("UI/Options/0_2_0"));

            optionsButtonsValuesActivated[1].Add(Content.Load<Texture2D>("UI/Options/1_0_0"));
            optionsButtonsValuesActivated[1].Add(Content.Load<Texture2D>("UI/Options/1_1_0"));

            optionsButtonsValuesActivated[2].Add(Content.Load<Texture2D>("UI/Options/2_0_0"));
            optionsButtonsValuesActivated[2].Add(Content.Load<Texture2D>("UI/Options/2_1_0"));
            optionsButtonsValuesActivated[2].Add(Content.Load<Texture2D>("UI/Options/2_2_0"));
            optionsButtonsValuesActivated[2].Add(Content.Load<Texture2D>("UI/Options/2_3_0"));
            optionsButtonsValuesActivated[2].Add(Content.Load<Texture2D>("UI/Options/2_4_0"));
            optionsButtonsValuesActivated[2].Add(Content.Load<Texture2D>("UI/Options/2_5_0"));
            optionsButtonsValuesActivated[2].Add(Content.Load<Texture2D>("UI/Options/2_6_0"));
            optionsButtonsValuesActivated[2].Add(Content.Load<Texture2D>("UI/Options/2_7_0"));
            optionsButtonsValuesActivated[2].Add(Content.Load<Texture2D>("UI/Options/2_8_0"));
            optionsButtonsValuesActivated[2].Add(Content.Load<Texture2D>("UI/Options/2_9_0"));
            optionsButtonsValuesActivated[2].Add(Content.Load<Texture2D>("UI/Options/2_10_0"));

            optionsButtonsValuesActivated[3].Add(Content.Load<Texture2D>("UI/Options/3_0_0"));

            optionActivatedIndex = 0;


        }
        

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            /*for (int i = 0; i < 4; i++)
            {
                System.Diagnostics.Debug.Write(optionActivatedValues[i] + " ");
            }
            System.Diagnostics.Debug.Write("\nTemp:");
            for (int i = 0; i < 4; i++)
            {
                System.Diagnostics.Debug.Write(tempOptionActivatedValues[i] + " ");
            }
            System.Diagnostics.Debug.Write("\n");*/

            menu.Draw(gameTime, spriteBatch);
            if (tempChangesFlag)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i != optionActivatedIndex)
                        spriteBatch.Draw(optionsButtonsValues[i][optionActivatedValues[i] + tempOptionActivatedValues[i]], Vector2.Zero, Color.White);
                    else
                        spriteBatch.Draw(optionsButtonsValuesActivated[optionActivatedIndex][optionActivatedValues[i] + tempOptionActivatedValues[optionActivatedIndex]], Vector2.Zero, Color.White);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i != optionActivatedIndex)
                        spriteBatch.Draw(optionsButtonsValues[i][optionActivatedValues[i]], Vector2.Zero, Color.White);
                    else
                        spriteBatch.Draw(optionsButtonsValuesActivated[optionActivatedIndex][optionActivatedValues[optionActivatedIndex]], Vector2.Zero, Color.White);
                }
            }
        }

        public int getButtonIndex()
        {
            return optionActivatedIndex;
        }

        public int[] getOptionValues()
        {
            return optionActivatedValues;
        }
        public void setOptionValues(int[] value)
        {
            optionActivatedValues = value;
        }

        public override void Update(GameTime gameTime)
        {

            if (initializeFlag)
            {
                optionActivatedIndex = 0;
                initializeFlag = false;

                for (int i = 0; i < 4; i++)
                {
                    tempOptionActivatedValues[i] = 0;
                }
            }

            if (!downButtonFlag)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    optionActivatedIndex++;
                    optionActivatedIndex %= 4;
                    downButtonFlag = true;
                    Game1.soundManager.menuclickEffect();
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Down))
            {
                downButtonFlag = false;
            }

            if (!upButtonFlag)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    optionActivatedIndex--;
                    Game1.soundManager.menuclickEffect();

                    if (optionActivatedIndex < 0)
                        optionActivatedIndex += 4;

                    optionActivatedIndex %= 4;
                    upButtonFlag = true;
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                upButtonFlag = false;
            }


            if (!rightButtonFlag)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {

                    Game1.soundManager.menuclickEffect();
                    if (optionActivatedValues[optionActivatedIndex] + tempOptionActivatedValues[optionActivatedIndex] < optionActivatedMod[optionActivatedIndex] - 1)
                    {
                        tempOptionActivatedValues[optionActivatedIndex]++;
                    }
                    rightButtonFlag = true;
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Right))
            {
                rightButtonFlag = false;
            }


            if (!leftButtonFlag)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {

                    Game1.soundManager.menuclickEffect();
                    if (optionActivatedValues[optionActivatedIndex] + tempOptionActivatedValues[optionActivatedIndex] > 0)
                    {
                        tempOptionActivatedValues[optionActivatedIndex]--;
                    }
                    leftButtonFlag = true;
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Left))
            {
                leftButtonFlag = false;
            }



            menu.Update(gameTime, menu.buttonIndex);
        }

        public bool goBackFlag()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Game1.soundManager.menuclickEffect();
                return true;
            }
            return false;
        }

        public bool saveChanges()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && optionActivatedIndex == 3)
            {
                if (!enterButtonFlag)
                {
                    Game1.soundManager.menuclickEffect();
                    enterButtonFlag = true;
                }
                return true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                enterButtonFlag = false;
            }
            return false;
        }
    }
}
