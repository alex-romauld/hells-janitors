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

namespace SurvivalShooter.Items
{
    class Slower
    {
        private Rectangle rect;
        private Texture2D texture;
        public Vector2 pos;
        private float rotation;

        private int LifeTime = 0;
        public Boolean Remove = false;


        public int AttackRadius = 500;

        private int RingRadius = 0;
        private Texture2D RingTexture;
        private int Fade = 255;

        public Slower(Texture2D texture, Texture2D RingTexture, Rectangle rect)
        {
            this.texture = texture;
            this.rect = rect;
            this.RingTexture = RingTexture;
        }

        public void Update(GameTime gameTime)
        {
            pos = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            rotation += 0.5f;

            LifeTime += gameTime.ElapsedGameTime.Milliseconds;
            if (RingRadius >= AttackRadius)
            {
                RingRadius = 0;
                Fade = 255;
            }
            else
            {
                RingRadius += 6;
                Fade -= 3;
            }


            

            if (LifeTime >= 25000)
            {
                Remove = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (LifeTime <= 20000)
                spriteBatch.Draw(texture, pos, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
            else if (LifeTime % 10 < 5)
             spriteBatch.Draw(texture, pos, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
            //if(LifeTime <= 20000)
            //    spriteBatch.Draw(texture, rect, Color.Green);
            //else if (LifeTime % 10 < 5)
            //    spriteBatch.Draw(texture, rect, Color.Green);
            spriteBatch.Draw(RingTexture, new Rectangle((int)pos.X - RingRadius, (int)pos.Y - RingRadius, RingRadius * 2, RingRadius * 2), new Color(Fade, Fade, Fade, Fade));
        }
    }
}
