using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;


namespace KeyPixels.UI
{
    public class Button: Component
    {
        Texture2D ButtonTexture;
        Texture2D ButtonRegularTexture;
        Texture2D ButtonHoverTexture;
        Vector2 ButtonPosition;
        Rectangle ButtonRectangle;

        bool isHovered { get; set; }
        bool isClicked { get; set; }

        private MouseState PreviousMouseState;
        private MouseState CurrentMouseState = Mouse.GetState();

        public Button(Texture2D _buttonTexture, Texture2D _buttonHoverTexture, Vector2 _buttonPosition)
        {
            ButtonTexture = _buttonTexture;
            ButtonRegularTexture = _buttonTexture;
            ButtonHoverTexture = _buttonHoverTexture;
            ButtonPosition = _buttonPosition;
            ButtonRectangle = new Rectangle((int) ButtonPosition.X, (int) ButtonPosition.Y, ButtonTexture.Width, ButtonTexture.Height);
            isHovered = false;
            isClicked = false;
        }

        bool isButtonHovered()
        {
            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            if (ButtonRectangle.Contains(CurrentMouseState.X, CurrentMouseState.Y))
            {
                return true;
            }

            return false;
        }


        bool isButtonClicked()
        {
            if (isButtonHovered() && CurrentMouseState.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed)
            {
                return true;
            }

            return false;
        }

        void updateTexture()
        {
            if (isButtonHovered())
            {
                ButtonTexture = ButtonHoverTexture;
            }
            else
            {
                ButtonTexture = ButtonRegularTexture;
            }
        }

        public override void Update(GameTime _gameTime)
        {
            updateTexture();
        }

        public override void Draw(GameTime _gameTime, SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(ButtonTexture, ButtonRectangle, Color.White);
        }
    }
}
