using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace KeyPixels
{
    class KeyCutScene
    {
        Random random = new Random();
        public int keyFoundIndex { get; set; }

        String[] oneKeyText = { ". . . . . .", ". . . . . .", ". . . . . .",
                                "Kenny: Ummmm?", "Kenny: It is only the half of the key!",
                                "Kenny: I still need the handle to unlock the portal door!" };
        String[] twoKeyText = {  ". . . . . .", ". . . . . .", ". . . . . .",
                                "Kenny: Hah! Here is the handle!", "Kenny: Dang it, it took me ages!!" };
        String[] fullKeyText = {  ". . . . . .", ". . . . . .", ". . . . . .",
                                "Kenny: Finally, I got the Key", "Kenny: I gotta defeat Boss Enemy now!!!" };

        SpriteFont font;
        Color color = new Color(248, 158, 158);
        Vector2 position = new Vector2(80, 870);

        String text = "";
        Queue<char> stringQueue = new Queue<char>();

        Texture2D SceneBackground;
        Texture2D DialogBackground;

        List<Texture2D> OneKeyTex = new List<Texture2D>();
        List<Texture2D> TwoKeyTex = new List<Texture2D>();
        List<Texture2D> FullKeyTex = new List<Texture2D>();
        List<Texture2D> Kenny = new List<Texture2D>();
        List<Texture2D> KeyTex;
       
        bool flag = true;
        bool incrementFlag = false;

        int textIndex = 0;
        int kennyIndex = 0;
        bool textInQueue = false;

        private float keyTimer = 0f;
        private int textTimer = 0;
        private int animationTimer = 0;


        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Fonts/Dialog");

            SceneBackground = Content.Load<Texture2D>("CutScenes/background");
            DialogBackground = Content.Load<Texture2D>("CutScenes/dialog_system");

            OneKeyTex.Add(Content.Load<Texture2D>("CutScenes/Key/1_1"));
            OneKeyTex.Add(Content.Load<Texture2D>("CutScenes/Key/1_2"));
            OneKeyTex.Add(Content.Load<Texture2D>("CutScenes/Key/1_3"));

            TwoKeyTex.Add(Content.Load<Texture2D>("CutScenes/Key/2_1"));
            TwoKeyTex.Add(Content.Load<Texture2D>("CutScenes/Key/2_2"));
            TwoKeyTex.Add(Content.Load<Texture2D>("CutScenes/Key/2_3"));

            FullKeyTex.Add(Content.Load<Texture2D>("CutScenes/Key/3_1"));
            FullKeyTex.Add(Content.Load<Texture2D>("CutScenes/Key/3_2"));
            FullKeyTex.Add(Content.Load<Texture2D>("CutScenes/Key/3_3"));

            Kenny.Add(Content.Load<Texture2D>("CutScenes/Kenny/normal"));
            Kenny.Add(Content.Load<Texture2D>("CutScenes/Kenny/blink"));
            Kenny.Add(Content.Load<Texture2D>("CutScenes/Kenny/surprised"));
            Kenny.Add(Content.Load<Texture2D>("CutScenes/Kenny/blink_surprised"));

            KeyTex = OneKeyTex;
            keyFoundIndex = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (!incrementFlag)
            {
                keyFoundIndex++;
                incrementFlag = true;
            }

            if (keyFoundIndex == 1)
            {
                KeyTex = OneKeyTex;
                createScene(oneKeyText);
            }
            else if (keyFoundIndex == 2)
            {
                KeyTex = TwoKeyTex;
                createScene(twoKeyText);
            }
            else if (keyFoundIndex == 3)
            {
                KeyTex = FullKeyTex;
                createScene(fullKeyText);
            }
        }

        public void createScene(String[] SceneText)
        {
            if (!textInQueue && textIndex < SceneText.Length)
            {
                for (int i = 0; i < SceneText[textIndex].Length; i++)
                {
                    stringQueue.Enqueue(SceneText[textIndex][i]);
                }
                textInQueue = true;
            }

            animateText();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                keyTimer += 0.05f;
                if (keyTimer > 5f)
                    textIndex = SceneText.Length;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                keyTimer = 0f;
            }

            if (textIndex == SceneText.Length)
            {
                textIndex = 0;
                incrementFlag = false;
                Game1.isKeyFound = false;
                Game1.isScenePlaying = false;
                Game1.isGamePlaying = true;
            }

        }

        public int getKeyFoundIndex()
        {
            return keyFoundIndex;
        }

        private void animateText()
        {
            if (stringQueue.Count > 0)
            {
                if (flag)
                {
                    text += stringQueue.Dequeue();
                    flag = !flag;
                }
                else
                {
                    flag = !flag;
                }
            }
            else if (stringQueue.Count == 0)
            {
                if (isHoldText(70))
                {
                    text = "";
                    textInQueue = false;
                    textIndex++;
                }
            }
        }


        private bool isHoldText(int TIMER)
        {
            textTimer++;

            if (textTimer > TIMER)
            {
                textTimer = 0;
                return true;
            }

            return false;
        }

        private bool isHoldAnimation(int TIMER)
        {
            animationTimer++;

            if (animationTimer > TIMER)
            {
                animationTimer = 0;
                return true;
            }

            return false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Draw(SceneBackground, Vector2.Zero, Color.White);
            if (textIndex < 3)
                spriteBatch.Draw(KeyTex[textIndex], Vector2.Zero, Color.White);
            else
            {
                if (isHoldAnimation(35))
                {
                    kennyIndex = random.Next(0, 3);
                }
                spriteBatch.Draw(Kenny[kennyIndex], Vector2.Zero, Color.White);
            }
            spriteBatch.Draw(DialogBackground, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

        }

    }
}
