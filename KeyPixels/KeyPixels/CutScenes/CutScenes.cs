using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;


namespace KeyPixels
{
    class CutScenes
    {
        Random random = new Random();
        SpriteFont font;
        Color color = new Color(248, 158, 158);

        Texture2D SceneBackground;
        Texture2D DialogBackground;
        List<Texture2D> Jenny = new List<Texture2D>();
        List<Texture2D> JennyScale = new List<Texture2D>();

        String[] textArray = { "Somewhere in the future", "Time: around 6029 A.D", "Place: High Beam, 5th Dimension",
                                ". . . . . . . . . .", ". . . . . . . . . .", ". . . . . . . . . .",
                                "Jenny: Where am I?", "Jenny: I do not remember anything!",
                                "Jenny: Kenny!", "Jenny: What happened to Kenny?", "Jenny: Where is he?" };
        String text = "";
        Queue<char> stringQueue = new Queue<char>();

        Vector2 storyPosition = new Vector2(50, 50);
        Vector2 dialogPosition = new Vector2(50, 620);

        bool flag = true;

        int textIndex = 0;
        bool textInQueue = false;
        bool isDialog = false;
        bool isScale = false;

        private float timer = 1f;
        private float animationTimer = 1f;
        private const float TIMER = 1f;

        int jennyIndex = 0;
        int jennyScaleIndex = 0;
        bool jennyNextScene = false;

        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Fonts/Dialog");
            SceneBackground = Content.Load<Texture2D>("CutScenes/background");
            DialogBackground = Content.Load<Texture2D>("CutScenes/dialog_system");

            Jenny.Add(Content.Load<Texture2D>("CutScenes/Jenny/normal"));
            Jenny.Add(Content.Load<Texture2D>("CutScenes/Jenny/blink"));
            Jenny.Add(Content.Load<Texture2D>("CutScenes/Jenny/surprised"));
            Jenny.Add(Content.Load<Texture2D>("CutScenes/Jenny/blink_surprised"));

            JennyScale.Add(Content.Load<Texture2D>("CutScenes/Jenny/Scale/scale0"));
            JennyScale.Add(Content.Load<Texture2D>("CutScenes/Jenny/Scale/scale1"));
            JennyScale.Add(Content.Load<Texture2D>("CutScenes/Jenny/Scale/scale2"));
            JennyScale.Add(Content.Load<Texture2D>("CutScenes/Jenny/Scale/scale3"));
            JennyScale.Add(Content.Load<Texture2D>("CutScenes/Jenny/Scale/scale4"));
            JennyScale.Add(Content.Load<Texture2D>("CutScenes/Jenny/Scale/scale5"));

        }

        private void makeEpilogue(GameTime gameTime)
        {
            if (!textInQueue && textIndex < textArray.Length)
            {
                for (int i = 0; i < textArray[textIndex].Length; i++)
                {
                    stringQueue.Enqueue(textArray[textIndex][i]);
                }
                textInQueue = true;
            }
            animateText(gameTime);
            if (textIndex == 3)
            {
                isScale = true;
                isDialog = true;
            }
            if (stringQueue.Count == 0 && textIndex == textArray.Length)
            {
                Game1.isScenePlaying = false;
                isDialog = false;
            }

        }


        private void animateText(GameTime gameTime)
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
                if (isHoldText(gameTime))
                {
                    text = "";
                    textInQueue = false;
                    textIndex++;
                }
            }
        }

        private bool isHoldText(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= timeElapsed;

            if (timer < 0)
            {
                timer = TIMER;
                return true;
            }

            return false;
        }

        private bool isHoldAnimation(GameTime gameTime, float _TIMER)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            animationTimer -= timeElapsed;

            if (animationTimer < 0)
            {
                animationTimer = _TIMER;
                return true;
            }

            return false;
        }

        public void Update(GameTime gameTime, int cutsceneIndex)
        {
            if (cutsceneIndex == 0)
            {
                makeEpilogue(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            Vector2 textMiddlePoint = font.MeasureString(text) / 2;
            Vector2 position = storyPosition;
            if (isDialog)
            {
                position = dialogPosition;
            }
            spriteBatch.Draw(SceneBackground, Vector2.Zero, Color.White);
            if (isScale)
            {
                if (jennyScaleIndex < 6)
                    spriteBatch.Draw(JennyScale[jennyScaleIndex], Vector2.Zero, Color.White);
                else { 
                    jennyNextScene = true;
                    isScale = false;
                }
                if (isHoldAnimation(gameTime, 0.5f))
                    jennyScaleIndex++;
            }
            if (jennyNextScene)
            {
                if (isHoldAnimation(gameTime, 0.35f))
                    jennyIndex = random.Next(0, 3);
                spriteBatch.Draw(Jenny[jennyIndex], Vector2.Zero, Color.White);
            }
            if (isDialog)
                spriteBatch.Draw(DialogBackground, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
        }

    }
}
