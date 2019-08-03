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
    class DamageBoost
    {
        public Boolean active = false;
        public Vector2 Pos;

        public int CurOperationTime;
        public int OperationTime = 20000;
        public int CurRechargeTime;
        public int RechargeTime = 70000;
        public int Price = 0;//1000;

        public Boolean UsedDamageBoost = false;

        public Texture2D icon;
        private Texture2D circle;
        private Texture2D circle2;


        public int Range = 375;
        private int scale = 0;
        public float DamageMultiplier = 3.25f;
        public Boolean underEffect = false;


        public int OperationTime_P20;
        public float DamageMultiplier_P20;
        public int Range_P20;

        public int RechargeTime_P20;
        


        public DamageBoost()
        {

        }

        public void Load(ContentManager content)
        {
            icon = content.Load<Texture2D>("Sprites/DamageBoost");
            circle = content.Load<Texture2D>("Sprites/Ring");
            circle2 = content.Load<Texture2D>("Sprites/Circle");

            OperationTime_P20 = (int)(OperationTime * 1.2f);
            DamageMultiplier_P20 = DamageMultiplier * 1.2f;
            Range_P20 = (int)(Range * 1.2f);

            RechargeTime_P20 = (int)(RechargeTime * 0.8f);
        }

        public void Update(GameTime gameTime, Input input, Vector2 playerPos, int Score, Boolean ItemKey, Boolean ItemButton, Boolean Usable)
        {
            if (CurRechargeTime > RechargeTime)
                CurRechargeTime = RechargeTime;
            if (active)
            {
                scale++;
                if (scale >= Range)
                    scale = 0;
                CurOperationTime -= gameTime.ElapsedGameTime.Milliseconds;
                CurRechargeTime = RechargeTime;
                Pos = playerPos;
                if (CurOperationTime <= 0)
                {
                    CurRechargeTime = RechargeTime;
                    active = false;
                }
            }
            else
            {

                if ((ItemButton || ItemKey) && Score >= Price && CurRechargeTime <= 0 && Usable)
                {
                    CurOperationTime = OperationTime;
                    Pos = playerPos;
                    scale = 0;
                    active = true;
                    UsedDamageBoost = true;
                }
            }

        }
        public void UpdateRecharge(GameTime gameTime)
        {
            if (!active)
                if (CurRechargeTime > 0)
                    CurRechargeTime -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(circle, new Rectangle((int)Pos.X - scale, (int)Pos.Y - scale, scale * 2, scale * 2), new Color(52, 23, 23, 25));//Color.IndianRed);
                spriteBatch.Draw(circle, new Rectangle((int)Pos.X - (Range - scale), (int)Pos.Y - (Range - scale), (Range - scale) * 2, (Range - scale) * 2), new Color(85,0,0,25));//Color.Red);
                //spriteBatch.Draw(circle, new Rectangle((int)Pos.X - Range / 2, (int)Pos.Y - Range / 2,Range, Range), new Color(85, 0, 0, 10));//Color.Red);
                spriteBatch.Draw(circle2, new Rectangle((int)Pos.X - Range, (int)Pos.Y - Range, Range * 2, Range * 2), new Color(45, 0, 0, 10));//Color.Red);
            }
        }
    }
}
