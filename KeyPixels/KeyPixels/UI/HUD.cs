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
        Texture2D HUDPoint;

        int healthCount;
        int shotsCount;

        int healthXStartPoint = 120;
        int shotsStartPoint = 650;

        List<Rectangle> healthBar = new List<Rectangle>();
        List<Rectangle> shotsBar = new List<Rectangle>();

        public void LoadContent(ContentManager Content)
        {
            HUDBackground = Content.Load<Texture2D>("HUD/hud_background");
            HUDPoint = Content.Load<Texture2D>("HUD/hud_point");

            for (int i = 0; i < 5; i++)
            {
                healthBar.Add(new Rectangle(120 + (i * 35), 85, HUDPoint.Width, HUDPoint.Height));
            }

            for (int i = 0; i < 10; i++)
            {
                shotsBar.Add(new Rectangle(50, 650 - (i * 20), HUDPoint.Width, HUDPoint.Height));
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(HUDBackground, Vector2.Zero, Color.White);
            for(int i=0; i<healthCount; i++)
            {
                spriteBatch.Draw(HUDPoint, healthBar[i], Color.White);
            }

            for (int i = 0; i < shotsCount; i++)
            {
                spriteBatch.Draw(HUDPoint, shotsBar[i], Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {
            shotsCount = Game1.numberOfShots();
            healthCount = 5;



        }
    }
}
