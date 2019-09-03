using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace KeyPixels
{
    class EndCutScene
    {
        Random random = new Random();

        String[] sceneText1 = { "Boss Enemy: At last, I meet you Kenny!", "Boss Enemy: Fun fact, I am the Kenny from the future!",
                                "Kenny: What?", "Kenny: How is it possible?",
                                "Boss Enemy: Haha, little Kenny!!", "Boss Enemy: You will realize when you get hit from me! LoL!"
                              };
        String[] sceneText2 = { "Boss Enemy: Kenny, you should have not defeated me!", "Boss Enemy: See you in a new dimension soon, bye!!!",
                                "Jenny: What was that?", "Jenny: You were the Boss Enemy?",
                                "Kenny: Huh? I am still confused!", "Kenny: I do not know what just happened!!",
                                "Jenny: Well, let's go back to 3rd dimension, shall we?",
                                "Kenny: Yea, let's go Jenny!!!", ". . . . . . . . . ."};

        SpriteFont font;
        Color color = new Color(248, 158, 158);
        Vector2 position = new Vector2(80, 870);

        String text = "";
        Queue<char> stringQueue = new Queue<char>();

        Texture2D SceneBackground;
        Texture2D DialogBackground;
        Texture2D CreditTexture;

        List<Texture2D> BossEnemy = new List<Texture2D>();
        List<Texture2D> Kenny = new List<Texture2D>();
        List<Texture2D> Jenny = new List<Texture2D>();
        List<Texture2D> Credits = new List<Texture2D>();
        List<Texture2D> CreditsAnimate = new List<Texture2D>();

        bool flag = true;
        bool incrementFlag = false;
        bool textInQueue = false;
        bool creditFlag = false;

        int textIndex = 0;
        int sceneIndex = 0;
        int bossEnemyIndex = 0;
        int kennyIndex = 0;
        int jennyIndex = 0;
        int creditIndex = 0;

        private float keyTimer = 0f;
        private int textTimer = 0;
        private int animationTimer = 0;


        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Fonts/Dialog");

            SceneBackground = Content.Load<Texture2D>("CutScenes/background");
            DialogBackground = Content.Load<Texture2D>("CutScenes/dialog_system");

            BossEnemy.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/normal"));
            BossEnemy.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/blink"));
            BossEnemy.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/surprised"));
            BossEnemy.Add(Content.Load<Texture2D>("CutScenes/BossEnemy/blink_surprised"));

            Kenny.Add(Content.Load<Texture2D>("CutScenes/Kenny/normal"));
            Kenny.Add(Content.Load<Texture2D>("CutScenes/Kenny/blink"));
            Kenny.Add(Content.Load<Texture2D>("CutScenes/Kenny/surprised"));
            Kenny.Add(Content.Load<Texture2D>("CutScenes/Kenny/blink_surprised"));

            Jenny.Add(Content.Load<Texture2D>("CutScenes/Jenny/normal"));
            Jenny.Add(Content.Load<Texture2D>("CutScenes/Jenny/blink"));
            Jenny.Add(Content.Load<Texture2D>("CutScenes/Jenny/surprised"));
            Jenny.Add(Content.Load<Texture2D>("CutScenes/Jenny/blink_surprised"));

            Credits.Add(Content.Load<Texture2D>("CutScenes/Credits/0"));
            CreditsAnimate.Add(Content.Load<Texture2D>("CutScenes/Credits/0_0"));
            Credits.Add(Content.Load<Texture2D>("CutScenes/Credits/1"));
            CreditsAnimate.Add(Content.Load<Texture2D>("CutScenes/Credits/1_0"));
            Credits.Add(Content.Load<Texture2D>("CutScenes/Credits/2"));
            CreditsAnimate.Add(Content.Load<Texture2D>("CutScenes/Credits/2_0"));
            Credits.Add(Content.Load<Texture2D>("CutScenes/Credits/3"));
            CreditsAnimate.Add(Content.Load<Texture2D>("CutScenes/Credits/3_0"));
            Credits.Add(Content.Load<Texture2D>("CutScenes/Credits/4"));
            CreditsAnimate.Add(Content.Load<Texture2D>("CutScenes/Credits/4_0"));

            CreditTexture = Credits[0];
        }

        public void Update(GameTime gameTime)
        {
            if (!incrementFlag)
            {
                sceneIndex++;
                incrementFlag = true;
                text = "";
                textInQueue = false;
                stringQueue = new Queue<char>();
            }

            if (sceneIndex == 1)
            {
                createScene(sceneText1);
            }
            else if (sceneIndex == 2)
            {
                createScene(sceneText2);
            }
            else if (sceneIndex == 3)
            {
                Game1.isCreditsPlaying = true;
                if (creditIndex == Credits.Count)
                {
                    Game1.isCreditsPlaying = false;
                    Game1.isGameCompletelyEnded = true;
                }
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
                {
                    textIndex = SceneText.Length;
                    keyTimer = 0f;
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                keyTimer = 0f;
            }

            if (textIndex == SceneText.Length)
            {
                textIndex = 0;
                if (sceneIndex == 1)
                {
                    Game1.isGamePlaying = true;
                    Game1.isScenePlaying = false;
                    Game1.isEndCutScene1 = true;
                    incrementFlag = false;
                    System.Diagnostics.Debug.WriteLine("Scene 1 Ended");

                    Game1.soundManager.isCutscenePlay = false;
                    Game1.soundManager.BackgroundMusicPlay();
                }
                else if (sceneIndex == 2)
                {
                    Game1.isEndCutScene2 = true;
                    Game1.isCreditsPlaying = true;
                    Game1.isGameEnded = true;
                    incrementFlag = false;
                    System.Diagnostics.Debug.WriteLine("Scene 2 Ended");
                }
            }
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

        private void makeCreditBackgroundBlink()
        {
            if (creditFlag)
            {
                CreditTexture = CreditsAnimate[creditIndex];
                creditFlag = false;
            }
            else
            {
                CreditTexture = Credits[creditIndex];
                creditFlag = true;

            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Draw(SceneBackground, Vector2.Zero, Color.White);

            if (sceneIndex == 1)
            {
                if (textIndex < 2)
                {
                    if (isHoldAnimation(35))
                    {
                        bossEnemyIndex = random.Next(0, 3);
                    }
                    spriteBatch.Draw(BossEnemy[bossEnemyIndex], Vector2.Zero, Color.White);
                }
                else if (textIndex < 4)
                {
                    if (isHoldAnimation(35))
                    {
                        kennyIndex = random.Next(0, 3);
                    }
                    spriteBatch.Draw(Kenny[kennyIndex], Vector2.Zero, Color.White);
                }
                else if (textIndex < 6)
                {
                    if (isHoldAnimation(35))
                    {
                        bossEnemyIndex = random.Next(0, 3);
                    }
                    spriteBatch.Draw(BossEnemy[bossEnemyIndex], Vector2.Zero, Color.White);
                }
            }
            else if (sceneIndex == 2)
            {
                if (textIndex < 2)
                {
                    if (isHoldAnimation(35))
                    {
                        bossEnemyIndex = random.Next(0, 3);
                    }
                    spriteBatch.Draw(BossEnemy[bossEnemyIndex], Vector2.Zero, Color.White);
                }
                else if (textIndex < 4)
                {
                    if (isHoldAnimation(35))
                    {
                        jennyIndex = random.Next(0, 3);
                    }
                    spriteBatch.Draw(Jenny[jennyIndex], Vector2.Zero, Color.White);
                }
                else if (textIndex < 6)
                {
                    if (isHoldAnimation(35))
                    {
                        kennyIndex = random.Next(0, 3);
                    }
                    spriteBatch.Draw(Kenny[kennyIndex], Vector2.Zero, Color.White);
                }
                else if (textIndex < 7)
                {
                    if (isHoldAnimation(35))
                    {
                        jennyIndex = random.Next(0, 3);
                    }
                    spriteBatch.Draw(Jenny[jennyIndex], Vector2.Zero, Color.White);
                }
                else if (textIndex < 9)
                {
                    if (isHoldAnimation(35))
                    {
                        kennyIndex = random.Next(0, 3);
                    }
                    spriteBatch.Draw(Kenny[kennyIndex], Vector2.Zero, Color.White);
                }
            }
            else if (sceneIndex == 3)
            {
                if (isHoldText(35))
                {
                    makeCreditBackgroundBlink();
                }
                spriteBatch.Draw(CreditTexture, Vector2.Zero, Color.White);
                if (isHoldAnimation(350))
                {
                    if (creditIndex < Credits.Count)
                    {
                        creditIndex++;
                    }
                }
            }

            if (sceneIndex != 3)
            {
                spriteBatch.Draw(DialogBackground, Vector2.Zero, Color.White);
                spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            }
        }

    }
}
