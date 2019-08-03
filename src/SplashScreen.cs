using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SurvivalShooter
{
    class SplashScreen
    {
        private Texture2D Music_splashScreen;
        private Rectangle viewPortRect;

        private int Fade = 0;
        private int Timer = 0;
        private int SplashTime = 6000;

        public Boolean active = false;//true;

        private Input input;

        public SplashScreen()
        {
        }

        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            Music_splashScreen = content.Load<Texture2D>("Splash Screens/ShadowOfADoubt");
            viewPortRect = new Rectangle(graphics.Viewport.X, graphics.Viewport.Y, graphics.Viewport.Width, graphics.Viewport.Height);

            input = new Input(PlayerIndex.One);
        }

        public void Update(GameTime gameTime)
        {
            if (active)
            {
                

                Timer += gameTime.ElapsedGameTime.Milliseconds;
                input.Update(false, false);
                if (input.SelectKey)
                {
                    Fade = 0;
                    Timer = SplashTime - 100;
                }
                if (Fade < 255 &&  Timer < SplashTime / 2)
                    Fade += 2;
                if (Fade > 0 && Timer >= SplashTime)
                    Fade -= 2;
                if (Timer >= SplashTime && Fade <= 0)
                    active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Music_splashScreen, viewPortRect, new Color(Fade, Fade, Fade, Fade));
            spriteBatch.End();
        }
    }
}
