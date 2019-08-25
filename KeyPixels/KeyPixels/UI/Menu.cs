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

        public List<Button> ButtonList = new List<Button>();
        Texture2D MenuBackgroundActive;
        Texture2D MenuBackground1;
        Texture2D MenuBackground2;
        bool menuFlag;
        bool noMenuBackgroundFlag;
        public int buttonIndex { get; set; }

        private float timer = 0.1f;
        private const float TIMER = 0.5f;

        public void addBackground(Texture2D _background1, Texture2D _background2)
        {
            MenuBackground1 = _background1;
            MenuBackground2 = _background2;
            MenuBackgroundActive = MenuBackground1;
        }
        
        public void addButton(Button _button)
        {
            ButtonList.Add(_button);
        }

        private void makeMenuBackgroundBlink()
        {
            if (menuFlag)
            {
                MenuBackgroundActive = MenuBackground2;
                menuFlag = false;
            }
            else
            {
                MenuBackgroundActive = MenuBackground1;
                menuFlag = true;

            }
        }

        public void setNoMenuBackground(bool _flag)
        {
            noMenuBackgroundFlag = _flag;
        }

        private bool isMenuBlink(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= timeElapsed;

            if(timer < 0)
            {
                timer = TIMER;
                return true;
            }

            return false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(!noMenuBackgroundFlag)
                spriteBatch.Draw(MenuBackgroundActive, Vector2.Zero, Color.White);
            foreach (Button button in ButtonList)
            {
                button.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime, int buttonID)
        {
            if (isMenuBlink(gameTime))
            {
                makeMenuBackgroundBlink();
            }

            for (int i = 0; i < ButtonList.Count; i++)
            {
                ButtonList[i].Update(gameTime, buttonIndex, i);
            }




        }
    }
}
