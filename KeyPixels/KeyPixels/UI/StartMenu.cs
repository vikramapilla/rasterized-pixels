using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KeyPixels.UI
{
    class StartMenu: Component
    {
        Menu menu;

        public void LoadContent(ContentManager Content)
        {
            menu = new Menu();
            
            menu.addBackground(Content.Load<Texture2D>("UI/Backgrounds/start_menu_background"));

            menu.addCursor(Content.Load<Texture2D>("UI/mouse_cursor"));

            menu.addButton(new Button(Content.Load<Texture2D>("UI/Buttons/play_button"), 
                Content.Load<Texture2D>("UI/Buttons/play_button_hover"), new Vector2(103, 250)));

            menu.addButton(new Button(Content.Load<Texture2D>("UI/Buttons/options_button"),
                Content.Load<Texture2D>("UI/Buttons/options_button_hover"), new Vector2(103, 340)));

            menu.addButton(new Button(Content.Load<Texture2D>("UI/Buttons/credits_button"),
                Content.Load<Texture2D>("UI/Buttons/credits_button_hover"), new Vector2(103, 420)));

            menu.addButton(new Button(Content.Load<Texture2D>("UI/Buttons/exit_button"),
                Content.Load<Texture2D>("UI/Buttons/exit_button_hover"), new Vector2(103, 510)));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            menu.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            menu.Update(gameTime);
        }
    }
}
