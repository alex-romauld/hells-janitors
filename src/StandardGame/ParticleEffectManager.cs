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

namespace SurvivalShooter.StandardGame
{
    class ParticleEffectManager
    {

        public ParticleEffect LandMineExplosion = new ParticleEffect();
        public ParticleEffect RocketExplosion = new ParticleEffect();
        public ParticleEffect Freeze = new ParticleEffect();
        public ParticleEffect Blood = new ParticleEffect();
        public ParticleEffect Blood2 = new ParticleEffect();

        public ParticleEffect GlassShatter = new ParticleEffect();


        public ParticleEffect Purchase = new ParticleEffect();


        public ParticleEffectManager()
        {

        }

        public void Load(ContentManager content)
        {
            LandMineExplosion.Load(content, 4, 4, 7, 10, 8, 1, 20, "Sprites/Misc/Explosion", Color.Orange, new Vector2(30, 30));
            RocketExplosion.Load(content, 4, 4, 7, 10, 8, 1, 20, "Sprites/Misc/Explosion", Color.Yellow, new Vector2(30, 30));
            Freeze.Load(content, 4, 4, 7, 10, 8, 1, 20, "Sprites/Player/player_Circle", Color.LightSkyBlue, new Vector2(30, 30));
            Blood.Load(content, 4, 4, 7, 10, 15, 10, 1, "Sprites/Flame", new Color(200, 50, 0, 70), new Vector2(30, 30));//Color.Orange
            Blood2.Load(content, 4, 4, 12, 20, 20, 10, 1, "Sprites/Misc/BloodSplatter", Color.Red, new Vector2(30, 30));

            GlassShatter.Load(content, 4, 4, 7, 10, 12, 5, -1, "Sprites/Misc/GlassShards", Color.White, new Vector2(30, 30));

            Purchase.Load(content, 4, 4, 17, 25, 28, 2, -5, "Sprites/Misc/Purchase", Color.White, new Vector2(75,75));

        }

        public void Update(GameTime gameTime)
        {
            LandMineExplosion.Update();
            RocketExplosion.Update();
            Freeze.Update();
            Blood.Update();
            Blood2.Update();

            GlassShatter.Update();

            Purchase.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            LandMineExplosion.Draw(spriteBatch);

            Blood2.Draw(spriteBatch);
            Blood.Draw(spriteBatch);

            RocketExplosion.Draw(spriteBatch);
            Freeze.Draw(spriteBatch);

            GlassShatter.Draw(spriteBatch);

            Purchase.Draw(spriteBatch);
        }
    }
}
