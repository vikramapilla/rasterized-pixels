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
    class StartMenu : Component
    {
        Menu menu;
        public Texture2D resumeButtonTexture, resumeButtonTextureHover;
        bool downButtonFlag = false;
        bool upButtonFlag = false;

        public void LoadContent(ContentManager Content)
        {
            menu = new Menu();

            menu.addBackground(Content.Load<Texture2D>("UI/Backgrounds/start_menu_background"),
                Content.Load<Texture2D>("UI/Backgrounds/start_menu_background_1"));

            menu.addButton(new Button(Content.Load<Texture2D>("UI/Buttons/play_button"),
                Content.Load<Texture2D>("UI/Buttons/play_button_hover"), new Vector2(150, 375)));

            menu.addButton(new Button(Content.Load<Texture2D>("UI/Buttons/options_button"),
                Content.Load<Texture2D>("UI/Buttons/options_button_hover"), new Vector2(150, 510)));

            menu.addButton(new Button(Content.Load<Texture2D>("UI/Buttons/controls_button"),
                Content.Load<Texture2D>("UI/Buttons/controls_button_hover"), new Vector2(150, 645)));

            menu.addButton(new Button(Content.Load<Texture2D>("UI/Buttons/exit_button"),
                Content.Load<Texture2D>("UI/Buttons/exit_button_hover"), new Vector2(150, 780)));

            resumeButtonTexture = Content.Load<Texture2D>("UI/Buttons/resume_button");
            resumeButtonTextureHover = Content.Load<Texture2D>("UI/Buttons/resume_button_hover");
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            menu.Draw(gameTime, spriteBatch);
        }

        public int getButtonIndex()
        {
            return menu.buttonIndex;
        }

        public override void Update(GameTime gameTime)
        {
            if (!downButtonFlag)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    menu.buttonIndex++;
                    Game1.soundManager.menuclickEffect();
                    menu.buttonIndex %= 4;
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
                    menu.buttonIndex--;

                    Game1.soundManager.menuclickEffect();
                    if (menu.buttonIndex < 0)
                        menu.buttonIndex += 4;

                    menu.buttonIndex %= 4;
                    upButtonFlag = true;
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                upButtonFlag = false;
            }

            if (Game1.isGameStarted)
            {
                menu.ButtonList[0].changeButtonTexture(resumeButtonTexture, resumeButtonTextureHover);
            }

            menu.Update(gameTime, menu.buttonIndex);
        }
    }
}
