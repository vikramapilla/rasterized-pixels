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
        List<Texture2D> optionsButtons;
        List<List<Texture2D>> optionsButtonsValues;
        Texture2D optionActivated;
        int optionActivatedIndex;
        int[] optionActivatedMod = { 3, 2, 11};
        int[] optionActivatedValues = { 1, 0, 5 };
        bool upButtonFlag = false;
        bool downButtonFlag = false;
        bool leftButtonFlag = false;
        bool rightButtonFlag = false;

        public void LoadContent(ContentManager Content)
        {
            menu = new Menu();
            menu.addBackground(Content.Load<Texture2D>("UI/Options/options_background_0"),
                Content.Load<Texture2D>("UI/Options/options_background_1"));


            optionsButtons = new List<Texture2D>();
            optionsButtonsValues = new List<List<Texture2D>>();
            for(int i=0; i<3; i++)
            {
                optionsButtonsValues.Add(new List<Texture2D>());
            }


            optionsButtons.Add(Content.Load<Texture2D>("UI/Options/0"));
            optionsButtons.Add(Content.Load<Texture2D>("UI/Options/1"));
            optionsButtons.Add(Content.Load<Texture2D>("UI/Options/2"));

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

            optionActivated = optionsButtons[0];
            optionActivatedIndex = 0;
            

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            menu.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(optionsButtons[optionActivatedIndex], Vector2.Zero, Color.White);
            for(int i=0; i<3; i++)
                spriteBatch.Draw(optionsButtonsValues[i][optionActivatedValues[i]], Vector2.Zero, Color.White);
        }

        public int getButtonIndex()
        {
            return optionActivatedIndex;
        }

        public int[] getOptionValues()
        {
            return optionActivatedValues;
        }

        public override void Update(GameTime gameTime)
        {
            if (!downButtonFlag)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    optionActivatedIndex++;
                    optionActivatedIndex %= 3;
                    downButtonFlag = true;
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

                    if (optionActivatedIndex < 0)
                        optionActivatedIndex += 3;

                    optionActivatedIndex %= 3;
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
                    if (optionActivatedValues[optionActivatedIndex] < optionActivatedMod[optionActivatedIndex] - 1)
                    {
                        optionActivatedValues[optionActivatedIndex]++;
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
                    if (optionActivatedValues[optionActivatedIndex] > 0)
                    {
                        optionActivatedValues[optionActivatedIndex]--;
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
                return true;
            }
            return false;
        }
    }
}
