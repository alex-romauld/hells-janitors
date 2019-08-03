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
    class AmmoBag
    {
        public Boolean active = false;
        public Vector2 Pos;

        public int CurOperationTime;
        public int OperationTime = 20000;
        public int CurRechargeTime;
        public int RechargeTime = 90000;
        public int Price = 0;//1000;

        public int BagTimeCost = 5000;

        public Texture2D icon;


        public int OperationTime_P20;
        public int RechargeTime_P20;
        public int BagCost_P20;

        public Boolean PlaceBag = false;
        public Rectangle BagRect;
        public Boolean RefillAmmo = false;

        public AmmoBag()
        {
        }

        public void Load(ContentManager content)
        {
            icon = content.Load<Texture2D>("Sprites/Misc/AmmoBag");
            OperationTime_P20 = (int)(OperationTime * 1.2f);
            RechargeTime_P20 = (int)(RechargeTime * 0.8f);
            BagCost_P20 = (int)(BagTimeCost * 0.5f);
        }

        public void Update(GameTime gameTime, Input input, Vector2 playerPos, int Score, Boolean ItemKey, Boolean ItemButton, Boolean Usable)
        {
            if (CurRechargeTime > RechargeTime)
                CurRechargeTime = RechargeTime;
            if (active)
            {

                CurOperationTime -= gameTime.ElapsedGameTime.Milliseconds;
                CurRechargeTime = RechargeTime;
                Pos = playerPos;
                if (CurOperationTime <= 0)
                {
                    CurRechargeTime = RechargeTime;
                    active = false;
                }
                if ((ItemButton || ItemKey) && CurOperationTime >= BagTimeCost && Usable)
                {
                    RefillAmmo = true;
                    PlaceAmmoBag(playerPos);
                    CurOperationTime -= BagTimeCost;
                }
            }
            else
            {

                if ((ItemButton || ItemKey) && Score >= Price && CurRechargeTime <= 0 && Usable)
                {
                    CurOperationTime = OperationTime;
                    Pos = playerPos;
                    active = true;
                    RefillAmmo = true;
                    PlaceAmmoBag(playerPos);
                }
            }
        }
        private void PlaceAmmoBag(Vector2 playerPos)
        {
            BagRect = new Rectangle((int)playerPos.X - 40, (int)playerPos.Y - 40, 80, 80);
            PlaceBag = true;
        }
        public void UpdateRecharge(GameTime gameTime)
        {
            if (!active)
                if (CurRechargeTime > 0)
                    CurRechargeTime -= gameTime.ElapsedGameTime.Milliseconds;
        }
        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
