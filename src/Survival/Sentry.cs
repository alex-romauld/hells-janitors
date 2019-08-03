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
    class Sentry
    {

        public Boolean active = false;

        public Texture2D SentryTexture;
        public Texture2D SentryStand;
        Vector2 Direction;
        public Vector2 pos;
        public float Rot;

        private SoundEffect ShotSound;
        public float BulletRotation;
        public Vector2 BulletDirection;
        public Vector2 BulletPos;
        public Texture2D BulletTexture;
        public int BulletLife = 500;
        public int BulletDamage = 7;//2
        public int BulletSpeed = 50;//55
        public String BulletType = "Sentry";
        public Boolean Fired = false;
        

        private int FireTick = 0;
        private int FireRate = 90;//100;//25

        private Vector2 Check_Dir;
        private Vector2 Check_Pos;
        private float CheckRot;
        private int CheckSpeed = 75;
        private Boolean Safe_fire = false;

        public int OperationTime = 0;
        public int OPERATION_TIME = 25000;
        public int SentryPrice = 0;//1000;
        public int Sentry_RechargeTime = 90000;//90000;
        public int Sentry_Recharge = 0;
        private SoundEffect SentryOffline;


        public Boolean PlaceSentry = false;


        public int OperationTime_P20;
        public int BulletDamage_P20;

        public int RechargeTime_P20;

        public Sentry()
        {
        }
        public void Load(ContentManager content)
        {
            SentryTexture = content.Load<Texture2D>("Sprites/SentryGun");
            SentryStand = content.Load<Texture2D>("Sprites/SentryGunStand");
            BulletTexture = content.Load<Texture2D>("Sprites/Missile1");
            SentryOffline = content.Load<SoundEffect>("SoundFX/SentryGunOfflineSE");
            ShotSound = content.Load<SoundEffect>("SoundFX/GunShots/SentryGunSE");
            pos = new Vector2(425, 175);

            OperationTime_P20 = (int)(OPERATION_TIME * 1.2f);
            BulletDamage_P20 = (int)(BulletDamage * 1.2f);

            RechargeTime_P20 = (int)(Sentry_RechargeTime * 0.8f);
        }

        int ClosestEnemy = 0;
        public void Update(GameTime gameTime , List<Enemy> enemies, Vector2 playerPos, Input input, List<Rectangle> CollisionObjects, List<Rectangle> CollisionPieces,
            int Score, Boolean ItemKey, Boolean ItemButton, Boolean Usable)
        {
            if (Sentry_Recharge > Sentry_RechargeTime)
                Sentry_Recharge = Sentry_RechargeTime;
            if ((ItemButton || ItemKey) && Score >= SentryPrice && Sentry_Recharge <= 0 && Usable)
            {
                Check_Pos = pos;
                PlaceSentry = true;
                active = true;
            }
            if (!active)
            {
                pos = playerPos;
                OperationTime = OPERATION_TIME;
               
            }

            if (active)
            {
                Sentry_Recharge = Sentry_RechargeTime;
                OperationTime -= gameTime.ElapsedGameTime.Milliseconds;
                if (OperationTime <= 0)
                {
                    SentryOffline.Play();
                    OperationTime = OPERATION_TIME;
                    active = false;
                }
                if (FireTick < FireRate)
                {
                    FireTick += gameTime.ElapsedGameTime.Milliseconds;
                }

                if (enemies.Count > 0)
                {
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (i != ClosestEnemy && ClosestEnemy < enemies.Count)
                        {
                            if (Vector2.Distance(pos, enemies[i].pos) < Vector2.Distance(pos, enemies[ClosestEnemy].pos))
                                ClosestEnemy = i;
                        }
                        if (ClosestEnemy >= enemies.Count)
                        {
                            ClosestEnemy = 0;
                        }
                    }
                    Check_Dir = enemies[ClosestEnemy].pos - pos;
                    Check_Dir.Normalize();
                    CheckRot = (float)Math.Atan2((double)Check_Dir.Y, (double)Check_Dir.X);
                    Check_Pos += Check_Dir * CheckSpeed;
                    if (Vector2.Distance(Check_Pos, enemies[ClosestEnemy].pos) < enemies[ClosestEnemy].EnemyTexture.Width)
                    {
                        Check_Pos = pos;
                        Safe_fire = true;
                    }
                    for (int i = 0; i < CollisionObjects.Count; i++)
                    {
                        if (Vector2.Distance(Check_Pos, new Vector2(CollisionObjects[i].X + CollisionObjects[i].Width / 2, CollisionObjects[i].Y + CollisionObjects[i].Height / 2))
                            < CollisionObjects[i].Width / 2)
                        {
                            Check_Pos = pos;
                            Safe_fire = false;
                        }
                    }
                    for (int i = 0; i < CollisionPieces.Count; i++)
                    {
                        if (Vector2.Distance(Check_Pos, new Vector2(CollisionPieces[i].X + CollisionPieces[i].Width / 2, CollisionPieces[i].Y + CollisionPieces[i].Height / 2))
                            < CollisionPieces[i].Width / 2)
                        {
                            Check_Pos = pos;
                            Safe_fire = false;
                        }
                    }
                    Direction = enemies[ClosestEnemy].pos - pos;
                    Direction.Normalize();
                    Rot = (float)Math.Atan2((double)Direction.Y, (double)Direction.X);




                    if (FireTick >= FireRate && Safe_fire)
                    {
                        //Fire
                        ShotSound.Play();
                        BulletRotation = Rot;
                        BulletPos = pos;
                        BulletDirection = Direction;
                        Fired = true;
                        FireTick = 0;
                    }
                }
                
            }
        }

        public void UpdateRecharge(GameTime gameTime)
        {
            if (!active)
                if (Sentry_Recharge > 0)
                    Sentry_Recharge -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(SentryStand, pos, null, Color.White, 0f, new Vector2(SentryTexture.Width / 2, SentryTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                spriteBatch.Draw(SentryTexture, pos, null, Color.White, Rot, new Vector2(SentryTexture.Width / 2, SentryTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
            }
        }
    }
}
