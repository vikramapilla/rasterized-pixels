using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KeyPixels.UI
{
    class Menu : Component
    {

        List<Button> ButtonList = new List<Button>();
        Texture2D MenuBackground;
        Texture2D MenuCursor;
        Vector2 CursorPosition;


        public void addBackground(Texture2D _background)
        {
            MenuBackground = _background;
        }

        public void addCursor(Texture2D _cursor)
        {
            MenuCursor= _cursor;
            CursorPosition = Vector2.Zero;
        }

        public void addButton(Button _button)
        {
            ButtonList.Add(_button);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MenuBackground, Vector2.Zero, Color.White);
            foreach (Button button in ButtonList)
            {
                button.Draw(gameTime, spriteBatch);
            }
            spriteBatch.Draw(MenuCursor, CursorPosition, Color.White);
        }

        public override void Update(GameTime gameTime)
        {

            CursorPosition.X = Mouse.GetState().X;
            CursorPosition.Y = Mouse.GetState().Y;

            foreach (Button button in ButtonList)
            {
                button.Update(gameTime);
            }

        }
    }
}
