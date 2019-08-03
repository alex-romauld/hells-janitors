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
    class SonicBoom
    {
        private Rectangle rect;
        private Texture2D texture;
        public Boolean Remove = false;

        private Texture2D ExplosionTexture;

        private int BlowTimer = 0;
        public Boolean BlowUp = false;

        public Rectangle ExplosionRect;
        private int ExplosionRate = 50;
        private int Fade = 255;

        private SoundEffect soundEffect;
        int Prev_BlowTimer = 0;

        private SoundEffect Explosion;



        public SonicBoom(Rectangle rect, Texture2D texture, Texture2D ExplosionTexture, SoundEffect soundEffect, SoundEffect Explosion)
        {
            this.rect = rect;
            this.texture = texture;
            this.ExplosionTexture = ExplosionTexture;
            this.soundEffect = soundEffect;
            this.Explosion = Explosion;
        }

        public void Update(GameTime gameTime)
        {
            if (!BlowUp)
            {
                if (Prev_BlowTimer < 2000 && BlowTimer > 2000)
                {
                    soundEffect.Play();
                    Prev_BlowTimer = BlowTimer;
                }
                if (Prev_BlowTimer < 3000 && BlowTimer > 3000)
                {
                    soundEffect.Play();
                    Prev_BlowTimer = BlowTimer;
                }
                if (Prev_BlowTimer < 4000 && BlowTimer > 4000)
                {
                    soundEffect.Play();
                    Prev_BlowTimer = BlowTimer;
                }
                if (BlowTimer >= 5000)
                {
                    BlowUp = true;
                    Explosion.Play();
                }
                else
                    BlowTimer += gameTime.ElapsedGameTime.Milliseconds;
                ExplosionRect = rect;
            }
            else
            {
                ExplosionRect.X -= ExplosionRate;
                ExplosionRect.Y -= ExplosionRate;
                ExplosionRect.Width += ExplosionRate * 2;
                ExplosionRect.Height += ExplosionRate * 2;
                if (ExplosionRect.Width - rect.Width > 500)
                    Fade -= 5;
            }
            if (Fade <= 0)
                Remove = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!BlowUp)
                spriteBatch.Draw(texture, rect, Color.White);
            else
                spriteBatch.Draw(ExplosionTexture, ExplosionRect, new Color(Fade, Fade, Fade, Fade));
        }
    }
}
