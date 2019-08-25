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
        List<Texture2D> BossEnemy = new List<Texture2D>();
        List<Texture2D> BossEnemyScale = new List<Texture2D>();
        List<Texture2D> Kenny = new List<Texture2D>();
        List<Texture2D> KennyScale = new List<Texture2D>();
        List<Texture2D> KennyCrying = new List<Texture2D>();

        String[] textArray = { "Somewhere in the future", "Time: around 6029 A.D", "Place: High Beam, 5th Dimension", //2
                                ". . . . . . . .", ". . . . . . . .", ". . . . . . . .", //5
                                "Jenny: Where am I?", "Jenny: I do not remember anything!",
                                "Jenny: Kenny!", "Jenny: What happened to Kenny?", "Jenny: Where is he?", //10
                                ". . . . . . . .", ". . . . . . . .", ". . . . . . . .", //13
                                "Boss Enemy: Oh Jenny, poor Jenny!", "Boss Enemy: Don't you see that you are in the future!",
                                "Boss Enemy: I locked you here in the 5th Dimension", "Boss Enemy: Who can save you?", //17
                                "Jenny: My friend Kenny will!", //18
                                "Boss Enemy: That is exactly the name I wanted to hear", "Boss Enemy: Let me make his death call", //20
                                "Somewhere now", "Time: around 2019 A.D", "Place: No Beam, 3rd Dimension", //23
                                ". . . . . . . .", ". . . . . . . .", ". . . . . . . .", //26
                                "Kenny: Huh? I got a message!", "Kenny: Who is this? Boss Enemy?",  "Kenny: Who is Boss Enemy?", //29
                                "Kenny: \"I locked your friend Jenny\"", "Kenny: \"If you want to save her, come to me!\"",//31
                                "Kenny: \"The key to the door is everything you need.\"","Kenny: I gotta save Jenny, now!" //33
        };
        String text = "";
        Queue<char> stringQueue = new Queue<char>();

        Vector2 storyPosition = new Vector2(75, 75);
        Vector2 dialogPosition = new Vector2(75, 870);

        bool flag = true;

        int textIndex = 0;
        bool textInQueue = false;
        bool isDialog = false;
        bool isScale = false;

        private float timer = 1f;
        private float animationTimer = 1f;
        private float keyTimer = 0f;
        private const float TIMER = 1f;

        int jennyIndex = 0;
        int jennyScaleIndex = 0;
        bool jennyScene = false;

        int bossEnemyIndex = 0;
        int bossEnemyScaleIndex = 0;
        bool bossEnemyScene = false;

        int kennyIndex = 0;
        int kennyScaleIndex = 0;
        bool kennyScene = false;

        int prevTextIndex = 0;
        bool isTypingEffect = false;


        int cryingIndex = 0;
        bool isGameEnded = false;


        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Fonts/Dialog");
            SceneBackground = Content.Load<Texture2D>("CutScenes/background");
            DialogBackground = Content.Load<Texture2D>("CutScenes/dialog_system");

            Jenny.Add(Content.Load<Texture2D>("CutScenes/Jenny/normal"));
            Jenny.Add(Content.Load<Texture2D>("CutScenes/Jenny/blink"));
            Jenny.Add(Content.Load<Texture2D>("CutScenes/Jenny/surprised"));
            Jenny.Add(Content.Load<Texture2D>("CutScenes/Jenny/blink_surprised"));

            JennyScale.Add(Content.Load<Texture2D>("CutScenes/Jenny/Scale/scale"));
            JennyScale.Add(Content.Load<Texture2D>("CutScenes/Jenny/Scale/scale0"));
            JennyScale.Add(Content.Load<Texture2D>("CutScenes/Jenny/Scale/scale1"));
            JennyScale.Add(Content.Load<Texture2D>("CutScenes/Jenny/Scale/scale2"));
            JennyScale.Add(Content.Load<Texture2D>("CutScenes/Jenny/Scale/scale3"));
            JennyScale.Add(Content.Load<Texture2D>("CutScenes/Jenny/Scale/scale4"));
            JennyScale.Add(Content.Load<Texture2D>("CutScenes/Jenny/Scale/scale5"));

            BossEnemy.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/normal"));
            BossEnemy.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/blink"));
            BossEnemy.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/surprised"));
            BossEnemy.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/blink_surprised"));

            BossEnemyScale.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/Scale/scale"));
            BossEnemyScale.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/Scale/scale0"));
            BossEnemyScale.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/Scale/scale1"));
            BossEnemyScale.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/Scale/scale2"));
            BossEnemyScale.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/Scale/scale3"));
            BossEnemyScale.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/Scale/scale4"));
            BossEnemyScale.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/Scale/scale5"));

            Kenny.Add(Content.Load<Texture2D>("CutScenes/Kenny/normal"));
            Kenny.Add(Content.Load<Texture2D>("CutScenes/Kenny/blink"));
            Kenny.Add(Content.Load<Texture2D>("CutScenes/Kenny/surprised"));
            Kenny.Add(Content.Load<Texture2D>("CutScenes/Kenny/blink_surprised"));

            KennyScale.Add(Content.Load<Texture2D>("CutScenes/Kenny/Scale/scale"));
            KennyScale.Add(Content.Load<Texture2D>("CutScenes/Kenny/Scale/scale0"));
            KennyScale.Add(Content.Load<Texture2D>("CutScenes/Kenny/Scale/scale1"));
            KennyScale.Add(Content.Load<Texture2D>("CutScenes/Kenny/Scale/scale2"));
            KennyScale.Add(Content.Load<Texture2D>("CutScenes/Kenny/Scale/scale3"));
            KennyScale.Add(Content.Load<Texture2D>("CutScenes/Kenny/Scale/scale4"));
            KennyScale.Add(Content.Load<Texture2D>("CutScenes/Kenny/Scale/scale5"));

            KennyCrying.Add(Content.Load<Texture2D>("CutScenes/Kenny/Crying/0"));
            KennyCrying.Add(Content.Load<Texture2D>("CutScenes/Kenny/Crying/1"));
            KennyCrying.Add(Content.Load<Texture2D>("CutScenes/Kenny/Crying/2"));
            KennyCrying.Add(Content.Load<Texture2D>("CutScenes/Kenny/Crying/3"));
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
            
            if(prevTextIndex == textIndex && !isTypingEffect && textIndex < 3)
            {
                isTypingEffect = true;
                Game1.soundManager.typingEffect();
            }
            else if(prevTextIndex != textIndex)
            {
                prevTextIndex = textIndex;
                isTypingEffect = false;
            }
            if (textIndex == 3)
            {
                isScale = true;
                jennyScene = true;
                isDialog = true;
            }
            if(textIndex == 11)
            {
                jennyScene = false;
                bossEnemyScene = true;
                isScale = true;
            }
            if (textIndex == 18)
            {
                jennyScene = true;
                bossEnemyScene = false;
            }
            if (textIndex == 19)
            {
                jennyScene = false;
                bossEnemyScene = true;
            }
            if (textIndex == 21)
            {
                bossEnemyScene = false;
                isDialog = false;
            }
            if (prevTextIndex == textIndex && !isTypingEffect && textIndex > 20 && textIndex < 24)
            {
                isTypingEffect = true;
                Game1.soundManager.typingEffect();
            }
            else if (prevTextIndex != textIndex)
            {
                prevTextIndex = textIndex;
                isTypingEffect = false;
            }
            if (textIndex == 24)
            {
                isScale = true;
                isDialog = true;
                kennyScene = true;
            }
            if (textIndex == 27)
            {
                isScale = false;
                kennyScene = true;
            }
            if (stringQueue.Count == 0 && textIndex == textArray.Length)
            {
                Game1.isScenePlaying = false;
                isDialog = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                keyTimer += 0.05f;
                if (keyTimer > 7f)
                    textIndex = textArray.Length;
            }
            else if(Keyboard.GetState().IsKeyUp(Keys.N))
            {
                keyTimer = 0f;
            }
        }

        public void makeGameOver()
        {
            isGameEnded = true;
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
            if (isGameEnded)
            {
                spriteBatch.Draw(SceneBackground, Vector2.Zero, Color.Black);
                if (cryingIndex <= 3)
                spriteBatch.Draw(KennyCrying[cryingIndex], Vector2.Zero, Color.White);
                else
                    cryingIndex = 0;

                if (isHoldAnimation(gameTime, 0.2f))
                    cryingIndex++;
            }
            else
            {
                Vector2 textMiddlePoint = font.MeasureString(text) / 2;
                Vector2 position = storyPosition;
                if (isDialog)
                {
                    position = dialogPosition;
                }
                spriteBatch.Draw(SceneBackground, Vector2.Zero, Color.Black);
                if (isScale && jennyScene)
                {
                    if (jennyScaleIndex <= 6)
                        spriteBatch.Draw(JennyScale[jennyScaleIndex], Vector2.Zero, Color.White);
                    else
                        isScale = false;

                    if (isHoldAnimation(gameTime, 0.5f))
                        jennyScaleIndex++;
                }
                if (!isScale && jennyScene)
                {
                    if (isHoldAnimation(gameTime, 0.35f))
                        jennyIndex = random.Next(0, 3);
                    spriteBatch.Draw(Jenny[jennyIndex], Vector2.Zero, Color.White);
                }
                if (isScale && bossEnemyScene)
                {
                    if (bossEnemyScaleIndex <= 6)
                        spriteBatch.Draw(BossEnemyScale[bossEnemyScaleIndex], Vector2.Zero, Color.White);
                    else
                        isScale = false;

                    if (isHoldAnimation(gameTime, 0.5f))
                        bossEnemyScaleIndex++;
                }
                if (!isScale && bossEnemyScene)
                {
                    if (isHoldAnimation(gameTime, 0.35f))
                        bossEnemyIndex = random.Next(0, 3);
                    spriteBatch.Draw(BossEnemy[bossEnemyIndex], Vector2.Zero, Color.White);
                }
                if (isScale && kennyScene)
                {
                    if (kennyScaleIndex <= 6)
                        spriteBatch.Draw(KennyScale[kennyScaleIndex], Vector2.Zero, Color.White);
                    else
                        isScale = false;

                    if (isHoldAnimation(gameTime, 0.5f))
                        kennyScaleIndex++;
                }
                if (!isScale && kennyScene)
                {
                    if (isHoldAnimation(gameTime, 0.35f))
                        kennyIndex = random.Next(0, 3);
                    spriteBatch.Draw(Kenny[kennyIndex], Vector2.Zero, Color.White);
                }
                if (isDialog)
                    spriteBatch.Draw(DialogBackground, Vector2.Zero, Color.White);
                spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            }
        }

    }
}
