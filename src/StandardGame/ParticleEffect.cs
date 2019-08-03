using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SurvivalShooter.StandardGame
{
    class ParticleEffect
    {
        private Texture2D EffectTexture;
        private int KillTime;
        private int MaxXVelocity;
        private int MaxYVelocity;
        private int MaxEmmision;
        private int MinEmmision;
        private int AvgLifeTime;
        private Rectangle InitRect;


        private List<Rectangle> Particles = new List<Rectangle>();
        private List<int> ParticleLifeTime = new List<int>();
        private List<int> ParticleXV = new List<int>();
        private List<int> ParticleYV = new List<int>();

        private Color tint;

        private int GrowTimer;
        private int CurrentGrowTime;
        private int GrowRate = 0;
        private int Growth;
        private Vector2 WIDTH_HEIGHT;

        public ParticleEffect()
        {

        }

        public void Load(ContentManager content, int MaxXVelocity, int MaxYVelocity, int MinEmmision, int MaxEmmision, int AvgLifeTime, int GrowTimer, int Growth, String TexturePath, Color tint, Vector2 WIDTH_HEIGHT)
        {
            this.MaxXVelocity = MaxXVelocity;
            this.MaxYVelocity = MaxYVelocity;

            this.MinEmmision = MinEmmision;
            this.MaxEmmision = MaxEmmision;

            this.AvgLifeTime = AvgLifeTime;
            this.tint = tint;

            //this.GrowRate = GrowRate;
            this.GrowTimer = GrowTimer;
            this.Growth = Growth;
            this.WIDTH_HEIGHT = WIDTH_HEIGHT;

            EffectTexture = content.Load<Texture2D>(TexturePath);
        }

        public void Update()
        {
            CurrentGrowTime++;
            if (CurrentGrowTime >= GrowTimer && GrowTimer != 0)
            {
                GrowRate = Growth;
            }
            
            for (int i = 0; i < Particles.Count; i++)
            {
                ParticleLifeTime[i]++;
                if (Particles[i].Width > 0 && Particles[i].Height > 0)
                {
                    Particles[i] = new Rectangle(Particles[i].X + ParticleXV[i], Particles[i].Y - GrowRate / 2, Particles[i].Width + GrowRate, Particles[i].Height);//.X ;
                    Particles[i] = new Rectangle(Particles[i].X - GrowRate / 2, Particles[i].Y + ParticleYV[i], Particles[i].Width, Particles[i].Height + GrowRate);//.X ;
                }
                if (ParticleLifeTime[i] >= AvgLifeTime)
                {
                    Particles.RemoveAt(i);
                    ParticleXV.RemoveAt(i);
                    ParticleYV.RemoveAt(i);
                    ParticleLifeTime.RemoveAt(i);
                }

            }
            if (GrowRate == Growth)
            {
                GrowRate = 0;
                CurrentGrowTime = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                spriteBatch.Draw(EffectTexture, Particles[i], tint);
            }
        }


        public void InstantiateEffect(Vector2 pos)
        {
            this.InitRect = new Rectangle((int)(pos.X - (WIDTH_HEIGHT.X / 2)), (int)(pos.Y - (WIDTH_HEIGHT.Y / 2)), (int)WIDTH_HEIGHT.X, (int)WIDTH_HEIGHT.Y);
            Random random = new Random();
            int CurrentEmmision = random.Next(MinEmmision, MaxEmmision);
            for (int i = 0; i < CurrentEmmision; i++)
            {
                Particles.Add(InitRect);
                ParticleXV.Add(random.Next(-MaxXVelocity, MaxXVelocity));
                ParticleYV.Add(random.Next(-MaxYVelocity, MaxYVelocity));
                ParticleLifeTime.Add(0);
            }
        }
    }
}