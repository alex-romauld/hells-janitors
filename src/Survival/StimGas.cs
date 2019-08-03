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


namespace SurvivalShooter.Survival
{
    class StimGas
    {
        public Boolean active = false;

        public Texture2D icon;
        private Texture2D circle;
        private int scale = 0;
        public Vector2 pos;


        public int HealDist = 220;
        public int HealthGain = 2;

        public int OperationTime = 0;
        public int OPERATION_TIME = 30000;
        public int StimGasPrice = 0;//1000;
        public int StimGas_RechargeTime = 150000;//82500;
        public int StimGas_Recharge = 0;

        public Boolean PlacedStimGas = false;
        private SoundEffect StimGasOffline;


        public int HealTick = 500;//550
        public int CurrentHealtTick = 0;


        public int OperationTime_P20;
        public int HealthTick_P20;
        public int HealDist_P20;

        public int RechargeTime_P20;


        public StimGas()
        {
        }

        public void Load(ContentManager content)
        {
            icon = content.Load<Texture2D>("Sprites/HealthKick");
            circle = content.Load<Texture2D>("Sprites/Player/player_Circle");
            StimGasOffline = content.Load<SoundEffect>("SoundFX/SentryGunOfflineSE");

            OperationTime_P20 = (int)(OPERATION_TIME * 1.2f);
            HealthTick_P20 = (int)(HealTick * 0.8f);// / 1.1f
            HealDist_P20 = (int)(HealDist * 1.2f);

            RechargeTime_P20 = (int)(StimGas_RechargeTime * 0.8);
        }

        public void Update(GameTime gameTime, Vector2 playerPos, Input input, int Score, Boolean ItemKey, Boolean ItemButton, Boolean Usable)
        {
            if (StimGas_Recharge > StimGas_RechargeTime)
                StimGas_Recharge = StimGas_RechargeTime;
            if (active)
            {
                scale++;
                if (scale >= HealDist)
                    scale = 0;
                StimGas_Recharge = StimGas_RechargeTime;
                OperationTime -= gameTime.ElapsedGameTime.Milliseconds;
                if (OperationTime <= 0)
                {
                    active = false;
                    StimGasOffline.Play();
                }
            }
            else
            {

                if ((ItemButton || ItemKey) && StimGas_Recharge <= 0 && Score >= StimGasPrice && Usable)
                {
                    pos = playerPos;
                    OperationTime = OPERATION_TIME;
                    scale = 0;
                    active = true;
                    PlacedStimGas = true;
                }
            }
         
        }
        public void UpdateRecharge(GameTime gameTime)
        {
            if (!active)
                if (StimGas_Recharge > 0)
                    StimGas_Recharge -= gameTime.ElapsedGameTime.Milliseconds;
        }
        public void AddHealthTick(GameTime gameTime)
        {
            CurrentHealtTick += gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(circle, new Rectangle((int)pos.X - scale, (int)pos.Y - scale, scale * 2, scale * 2), Color.SkyBlue);
                spriteBatch.Draw(circle, new Rectangle((int)pos.X - (HealDist - scale), (int)pos.Y - (HealDist - scale), (HealDist - scale) * 2, (HealDist - scale) * 2), Color.SkyBlue);
                spriteBatch.Draw(circle, new Rectangle((int)pos.X - HealDist, (int)pos.Y - HealDist, HealDist * 2, HealDist * 2), new Color(135, 206, 235, 50));
            }
        }
    }
}
