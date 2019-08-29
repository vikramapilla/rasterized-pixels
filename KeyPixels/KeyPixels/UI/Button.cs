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

        private int buttonIndex;
        private int buttonListID;

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
        

        public void changeButtonTexture(Texture2D _buttonTexture, Texture2D _buttonHoverTexture)
        {
            ButtonTexture = _buttonTexture;
            ButtonRegularTexture = _buttonTexture;
            ButtonHoverTexture = _buttonHoverTexture;
        }

        bool isButtonHovered()
        {
            if(buttonListID == buttonIndex)
            {
                return true;
            }
            return false;
        }


        bool isButtonClicked()
        {
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

        public void Update(GameTime _gameTime, int _buttonIndex, int _buttonListID)
        {
            this.buttonIndex = _buttonIndex;
            this.buttonListID = _buttonListID;
            updateTexture();
        }

        public override void Draw(GameTime _gameTime, SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(ButtonTexture, ButtonRectangle, Color.White);
        }
    }
}
