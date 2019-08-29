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
    class ControlsMenu : Component
    {
        Menu menu;

        public void LoadContent(ContentManager Content)
        {
            menu = new Menu();

            menu.addBackground(Content.Load<Texture2D>("UI/Backgrounds/controls_screen_0"),
                Content.Load<Texture2D>("UI/Backgrounds/controls_screen_1"));
        }

        public override void Update(GameTime gameTime)
        {

            menu.Update(gameTime, menu.buttonIndex);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            menu.Draw(gameTime, spriteBatch);
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
