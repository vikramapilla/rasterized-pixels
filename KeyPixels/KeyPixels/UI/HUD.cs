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
    class HUD : Component
    {
        Texture2D HUDBackground;
        Texture2D HUDBossBackground;
        Texture2D HUDPoint;
        Texture2D HUDPointBoss;
        List<Texture2D> KeyTexture = new List<Texture2D>();

        int healthCount;
        int bossHealthCount;

        List<Rectangle> healthBar = new List<Rectangle>();
        List<Rectangle> bossHealthBar = new List<Rectangle>();

        public void LoadContent(ContentManager Content)
        {
            HUDBackground = Content.Load<Texture2D>("HUD/hud_background");
            HUDBossBackground = Content.Load<Texture2D>("HUD/boss_hud_background");
            HUDPoint = Content.Load<Texture2D>("HUD/hud_point");
            HUDPointBoss = Content.Load<Texture2D>("HUD/hud_point1");
            KeyTexture.Add(Content.Load<Texture2D>("HUD/PickUps/key_1"));
            KeyTexture.Add(Content.Load<Texture2D>("HUD/PickUps/key_2"));

            for (int i = 0; i < 5; i++)
            {
                healthBar.Add(new Rectangle(160 + (i * 30), 50, HUDPoint.Width, HUDPoint.Height));
            }
            for (int i = 0; i < 5; i++)
            {
                healthBar.Add(new Rectangle(160 + (i * 30), 85, HUDPoint.Width, HUDPoint.Height));
            }


            for (int i = 0; i < 5; i++)
            {
                bossHealthBar.Add(new Rectangle(1733 - (i * 30), 50, HUDPoint.Width, HUDPoint.Height));
            }
            for (int i = 0; i < 5; i++)
            {
                bossHealthBar.Add(new Rectangle(1733 - (i * 30), 85, HUDPoint.Width, HUDPoint.Height));
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(HUDBackground, Vector2.Zero, Color.White);


            if (Game1.isKeyFoundIndexHUD == 1)
            {
                spriteBatch.Draw(KeyTexture[0], Vector2.Zero, Color.White);
            }
            else if (Game1.isKeyFoundIndexHUD == 2 && Game1.mapindex == 3)
            {
                spriteBatch.Draw(KeyTexture[1], Vector2.Zero, Color.White);
            }

            for (int i = 0; i < healthCount; i++)
            {
                spriteBatch.Draw(HUDPoint, healthBar[i], Color.White);
            }

            if (Game1.isBossFight)
            {
                spriteBatch.Draw(HUDBossBackground, Vector2.Zero, Color.White);
                for (int i = 0; i < bossHealthCount && i < 10; i++)
                {
                    spriteBatch.Draw(HUDPoint, bossHealthBar[i], Color.White);
                }
                for (int i = 10; i < bossHealthCount; i++)
                {
                    spriteBatch.Draw(HUDPointBoss, bossHealthBar[i - 10], Color.White);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            healthCount = Player.healthCounter;

            if (Game1.isBossFight)
            {
                bossHealthCount = Boss.healthCounter;
            }
        }
    }
}
