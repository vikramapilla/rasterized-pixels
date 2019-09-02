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
    class EndMenu : Component
    {
        Menu menu;
        bool leftButtonFlag = false;
        bool rightButtonFlag = false;

        public void LoadContent(ContentManager Content)
        {
            menu = new Menu();

            menu.addBackground(Content.Load<Texture2D>("CutScenes/background"),
                Content.Load<Texture2D>("CutScenes/background"));

            menu.addButton(new Button(Content.Load<Texture2D>("UI/Buttons/restart_button"),
                Content.Load<Texture2D>("UI/Buttons/restart_button_hover"), new Vector2(650, 700)));

            menu.addButton(new Button(Content.Load<Texture2D>("UI/Buttons/exit_button"),
                Content.Load<Texture2D>("UI/Buttons/exit_button_hover"), new Vector2(970, 700)));

            menu.setNoMenuBackground(true);
        }

        public override void Update(GameTime gameTime)
        {

            if (!rightButtonFlag)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    Game1.soundManager.menuclickEffect();
                    menu.buttonIndex++;
                    menu.buttonIndex %= 2;
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
                    menu.buttonIndex--;

                    if (menu.buttonIndex < 0)
                        menu.buttonIndex += 2;

                    menu.buttonIndex %= 2;
                    leftButtonFlag = true;
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Left))
            {
                leftButtonFlag = false;
            }
            

            menu.Update(gameTime, menu.buttonIndex);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            menu.Draw(gameTime, spriteBatch);
        }

        public int getButtonIndex()
        {
            return menu.buttonIndex;
        }

    }
}
