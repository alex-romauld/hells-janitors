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

using SurvivalShooter.Survival;
using SurvivalShooter.StandardGame;
using SurvivalShooter.Items;

namespace SurvivalShooter
{
    class Player
    {
        public Boolean PC = false;
        //PC
        private Texture2D Cursor;
        private float Cursor_rotation;
        private Vector2 Cursor_Pos;

        private SpriteFont font;

        public PlayerIndex player;
        public int PlayerSpeed = 10;//Speed Boost = 20
        public int NormalSpeed = 10;
        public Texture2D playerTexture;
        public Texture2D PlayerTexture;
        public Rectangle PlayerRect;
        public Vector2 pos;

        public Vector2 SpectatorPos;
        private int SpectateSpeed;
        public Boolean DisplayHud = true;
        public Vector2 CamCen;

        private int FireTick = 0;


        

        public int BulletDamage = 10;
        public int BulletLife;
        public int BulletSpeed = 55;


        private float LookDirection = 0f;//#lookDir
        public float BulletRotation;
        public Vector2 BulletDirection;
        public Vector2 BulletPos;
        public Texture2D BulletTexture;
        public Boolean Fired = false;
        public String BulletType;
        public SoundEffect BulletHitSound;
        public int HitPoints;
        public int KillPoints;

        private float WalkRotation;


        public Boolean Active = false;
        public Boolean GameOver = false;

     //   List<Rectangle> CollisionObjects = new List<Rectangle>();

        private Rectangle BottomRect;
        private Rectangle Top_Rect;
        private Rectangle Left_Rect;
        private Rectangle Right_Rect;

        public int Score = 1250;


        public int Cur_Node = 0;


        public WeaponManager weaponManager = new WeaponManager();
        public Boolean PurchasedWeapon = false;
        private SoundEffect PurchaseWeaponSound;

        //Health
        public Rectangle HealthBar;
        public int Health = 250;
        public int MaxHealth = 250;
        public Boolean dead = false;
        private int DiedOnWave = -1;
        public Vector2 StartingPos;
        Vector2 PlayerSize;
        public int InvulnerabilityTime = 850;
        public int CurinvulnerabilityTime = 0;

        private int DamageFlashTick = 0;


        public Input pInput;
        public KInput kInput;

        public int Class = 0;
        public Sentry sentry = new Sentry();
        public StimGas stimGas = new StimGas();
        public DamageBoost damageBoost = new DamageBoost();
        public AmmoBag ammoBag = new AmmoBag();

        public Boolean GasMaskOn = false;
        private Texture2D GasMask;
        public Texture2D GasMaskEffect;
        public int BreathTick = 0;

        private Rectangle DpadRect;


        public Boolean OnMusicSpace = false;

        public ParticleEffect hitEffect = new ParticleEffect();
        public Vector2 pointEffect;


        private Boolean ShowScoreBoard = false;
        public int Kills;
        public int Deaths;

        private Boolean PrevDead = false;

        public Boolean usingShop = false;
        private int ShopUsing = 0;
        private int StoreOption = 0;
        private List<StoreEffect> perks = new List<StoreEffect>();
        private List<StoreEffect> avaliablePerks = new List<StoreEffect>();
        private List<StoreEffect> PurchasedPerks = new List<StoreEffect>();
        public Boolean GasMaskPerk = false;
        private List<Rectangle> perkbuttons = new List<Rectangle>();

        public int StoreDiscount = 0;

        public Texture2D UseKeyTexture;
        public Texture2D SwitchKeyTexture;
        public Texture2D GasMaskKeyTexture;
        public Texture2D AbilityKeyTexture;


        public Texture2D securityPlateBulletTexture;
        private Texture2D securityPlateTexture;
        public String Item = "";//Sonic Boom";//Security Plate";//Sonic Boom";
        public String Item2 = "";
        public Boolean HasExtraSlot = false;
        private Texture2D SonicBoomTexture;
        private Texture2D SonicBoomExplosionTexture;
        private SoundEffect SonicBoomExplosion;
        private SoundEffect SonicBoomBeep;
        public List<SonicBoom> sonicBoom = new List<SonicBoom>();
        public List<SecurityPlate> securityPlate = new List<SecurityPlate>();
        private Texture2D SlowerTexture;
        private Texture2D SlowerRingTexture;
        public List<Slower> Slower = new List<Slower>();

        public Boolean HasSpeedBost = true;
        public List<Rectangle> SpeedBoostTrail = new List<Rectangle>();
        private Texture2D circle;

        public SoundEffect TeleportSound;
        public int TeleportFade = 0;
        
        public int PlayerCount = 1;

        public Player()
        {

        }

        public void Load(Vector2 StartingPos, ContentManager content, PlayerIndex player)
        {
            Cursor = content.Load<Texture2D>("Sprites/Cursor");
            font = content.Load<SpriteFont>("standardfont");

            PlayerRect = new Rectangle((int)StartingPos.X, (int)StartingPos.Y, 50, 50);
            this.StartingPos = StartingPos;
            PlayerSize.X = 50;
            PlayerSize.Y = 50;

            circle = content.Load<Texture2D>("Sprites/Circle");
            playerTexture = content.Load<Texture2D>("Sprites/Square");
            PlayerTexture = content.Load<Texture2D>("Sprites/player_Pistol");//player_gatlinggun
            weaponManager.Load(content, player);
            this.player = player;

            pInput = new Input(player);
            pInput.Load(content);
            kInput = new KInput();
            kInput.Load(content);

            PC = pInput.PC;
           

            HealthBar = new Rectangle(10,140,Health,10);

            sentry.Load(content);
            stimGas.Load(content);
            damageBoost.Load(content);
            ammoBag.Load(content);

            GasMask = content.Load<Texture2D>("GUI/GasMask");
            GasMaskEffect = content.Load<Texture2D>("Sprites/GasMaskEffect");
            PurchaseWeaponSound = content.Load<SoundEffect>("SoundFX/WeaponPurchaseSE");

            hitEffect.Load(content, 0 ,0, 10, 20, 5, 1, 5, "Sprites/Flame", Color.Orange, new Vector2(50,50));

            securityPlateTexture = content.Load<Texture2D>("Sprites/Items/SecurityPlate");
            securityPlateBulletTexture = content.Load<Texture2D>("Sprites/Flame");
            SonicBoomTexture = content.Load<Texture2D>("Sprites/Items/SonicBoom");
            SonicBoomExplosionTexture = content.Load<Texture2D>("Sprites/square");
            SonicBoomBeep = content.Load<SoundEffect>("SoundFX/Items/SonicBoomBeep");
            SonicBoomExplosion = content.Load<SoundEffect>("SoundFX/Items/SonicBoomExplosion");
            SlowerTexture = content.Load<Texture2D>("Sprites/Items/Slower");
            SlowerRingTexture = content.Load<Texture2D>("Sprites/Ring");
            SetupPerks();

            TeleportSound = content.Load<SoundEffect>("SoundFX/Misc/Teleport");

        }
        private void SetupPerks()
        {
            //perks.Add(new StoreEffect("IRON LUNGS", "Gas Mask is no longer required during mutation waves", 0, 0));

            perks.Add(new StoreEffect("Ability Duration", "Ability Duration\nIncreased By 20%", 4000, 0));
            perks.Add(new StoreEffect("Ability Strength", "Ability Stength\nIncreased By 20%", 4000, 0));
            perks.Add(new StoreEffect("Ability Cooldown", "Ability Recharge Time\nDecreased By 20%", 4000, 0));
            perks.Add(new StoreEffect("Gas Mask", "No Gas Mask Required", 1500, 0));
            perks.Add(new StoreEffect("Recovery Time", "Longer Recovery Time\nAfter Taking Damage", 3000, 0));
            perks.Add(new StoreEffect("Extra Item Slot", "Carry an Extra Item ", 4000, 0));
            perks.Add(new StoreEffect("Random Luck", "At Random Weapon Stations\nRare Weapons Become Less Rare", 7000, 0));
            //perks.Add(new StoreEffect("asdS 2 ", "gasdasdas?", 0, 0));
            avaliablePerks = perks;
           // perkbuttons = new Rectangle[avaliablePerks.Count];
        }
        private void EstablishTextures()
        {
            if (PC)
            {
                UseKeyTexture = kInput.UseKeyTexture;
                SwitchKeyTexture = kInput.SwitchWeaponKeyTexture;
                GasMaskKeyTexture = kInput.GasMaskKeyTexture;
                AbilityKeyTexture = kInput.AbilityKeyTexture;
            }
            else
            {
                UseKeyTexture = pInput.ButtonX;
                SwitchKeyTexture = pInput.ButtonY;
                GasMaskKeyTexture = pInput.ButtonDpad;
                AbilityKeyTexture = pInput.ButtonDpad;
            }
        }
        public void Update(GameTime gameTime,
            List<Rectangle> CollisionObjects, List<Rectangle> LevelPiecesCollisionObject, List<Rectangle> WalkableTiles, List<Rectangle> StoreRect,
            List<Rectangle> WeaponRects, List<String> WeaponTypes, List<int> WeaponCost, List<int> WeaponAmmo
            , int Wave,
            List<Enemy> enemies)
        {
            //if (!GamePad.GetState(pInput.player).IsConnected  && !pInput.PC)
            //{

            //}
            EstablishTextures();
            //pInput.Update();
            //kInput.Update(PC);
            pos = new Vector2(PlayerRect.X + PlayerRect.Width / 2, PlayerRect.Y + PlayerRect.Height / 2);
            HealthBar = new Rectangle(10,80,Health,10);//140
            

            if (Health <= 0)
            {
                //PlayerRect.Width = 0;
                //PlayerRect.Height = 0;
                if (!PrevDead)
                {
                    Deaths++;
                    PrevDead = true;
                }
                dead = true;
                Health = 0;
            }
            if (Health > 0)
            {
                PrevDead = false;
                PlayerRect.Width = (int)PlayerSize.X;
                PlayerRect.Height = (int)PlayerSize.Y;
                dead = false;
                DiedOnWave = -1;
            }

            if (DiedOnWave == -1 && dead)
                DiedOnWave = Wave;
            if (DiedOnWave > -1 && DiedOnWave != Wave && dead)
                ResetPlayer();

            if (!dead)
            {
                UpdatePlaceItem();
                UpdateShooting(gameTime);
                weaponManager.Update(kInput.SwitchedWeapon, pInput.SwitchWeaponKey, PC);
                UpdateMovement();
                DetectCollisions(CollisionObjects, PlayerSpeed);
                DetectCollisions(LevelPiecesCollisionObject, PlayerSpeed);
                DisplayHud = true;

                if (Class == 0)
                {
                    sentry.Update(gameTime, enemies, pos, pInput, CollisionObjects, LevelPiecesCollisionObject, Score, kInput.ItemKey && PC, pInput.ItemKey && !PC, !usingShop);
                    if (sentry.PlaceSentry)
                    {
                        Score -= sentry.SentryPrice;
                        sentry.PlaceSentry = false;
                    }
                }
                if (Class == 1)
                {
                    stimGas.Update(gameTime, pos, pInput, Score, kInput.ItemKey  &&  PC, pInput.ItemKey  &&  !PC, !usingShop);
                    if (stimGas.PlacedStimGas)
                    {
                        Score -= stimGas.StimGasPrice;
                        stimGas.PlacedStimGas = false;
                    }
                }
                if (Class == 2)
                {
                    damageBoost.Update(gameTime, pInput, pos, Score, kInput.ItemKey && PC, pInput.ItemKey && !PC, !usingShop);
                    if (damageBoost.UsedDamageBoost)
                    {
                        Score -= damageBoost.Price;
                        damageBoost.UsedDamageBoost = false;
                    }
                }
                if (Class == 3)
                {
                    ammoBag.Update(gameTime, pInput, pos, Score, kInput.ItemKey && PC, pInput.ItemKey && !PC, !usingShop);
                    if (ammoBag.RefillAmmo)
                    {
                        weaponManager.BoughtAmmo(weaponManager.CurentWeapon.WeaponName);
                        weaponManager.BoughtAmmo(weaponManager.SecondaryWeaponName);
                        ammoBag.RefillAmmo = false;
                    }
                }
                stimGas.AddHealthTick(gameTime);


                SpectatorPos = pos;
                CamCen = pos;
                for (int i = 0; i < PurchasedPerks.Count; i++)
                {
                    if (PurchasedPerks[i].name == "Ability Duration")
                    {
                        sentry.OPERATION_TIME = sentry.OperationTime_P20;
                        damageBoost.OperationTime = damageBoost.OperationTime_P20;
                        stimGas.OPERATION_TIME = stimGas.OperationTime_P20;
                        ammoBag.OperationTime = ammoBag.OperationTime_P20;
                    }
                    if (PurchasedPerks[i].name == "Ability Strength")
                    {
                        sentry.BulletDamage = sentry.BulletDamage_P20;
                        damageBoost.DamageMultiplier = damageBoost.DamageMultiplier_P20;
                        damageBoost.Range = damageBoost.Range_P20;
                        stimGas.HealDist = stimGas.HealDist_P20;
                        stimGas.HealTick = stimGas.HealthTick_P20;
                        ammoBag.BagTimeCost = ammoBag.BagCost_P20;
                    }
                    if (PurchasedPerks[i].name == "Ability Cooldown")
                    {
                        sentry.Sentry_RechargeTime = sentry.RechargeTime_P20;
                        stimGas.StimGas_RechargeTime = stimGas.RechargeTime_P20;
                        damageBoost.RechargeTime = damageBoost.RechargeTime_P20;
                        ammoBag.RechargeTime = ammoBag.RechargeTime_P20;
                    }
                    if (PurchasedPerks[i].name == "Gas Mask")
                    {
                        GasMaskPerk = true;
                    }
                    if (PurchasedPerks[i].name == "Random Luck")
                    {
                        weaponManager.PerkBought();
                    }
                    if (PurchasedPerks[i].name == "Recovery Time")
                    {
                        InvulnerabilityTime = 1500;
                    }
                    if (PurchasedPerks[i].name == "Extra Item Slot")
                    {
                        HasExtraSlot = true;
                    }

                }
            }
            else
            {
                stimGas.active = false;
                sentry.active = false;
                damageBoost.active = false;
                UpdateSpectate();
                CamCen = SpectatorPos;
            }
            
            for (int i = 0; i < perks.Count; i++)
            {
                perks[i].price = (int)(perks[i].FullPrice - (perks[i].FullPrice * (StoreDiscount / 100.0f)));
            }
            sentry.UpdateRecharge(gameTime);
            stimGas.UpdateRecharge(gameTime);
            damageBoost.UpdateRecharge(gameTime);
            ammoBag.UpdateRecharge(gameTime);
            UpdateSpeedBoostTrail();
            if (!dead)
            {
                for (int i = 0; i < WeaponRects.Count; i++)
                {
                    if (PlayerRect.Intersects(WeaponRects[i]))
                    {
                        int Price = 0;
                        Boolean buy = false;
                        Boolean buyAmmo = false;
                        if (weaponManager.CurentWeapon.WeaponName != WeaponTypes[i]
                            && weaponManager.SecondaryWeaponName != WeaponTypes[i])
                        {
                            Price = WeaponCost[i];
                            buyAmmo = false;
                            buy = true;
                        }
                        else if (weaponManager.CurentWeapon.WeaponName == WeaponTypes[i] && weaponManager.CurentWeapon.Ammo < weaponManager.CurentWeapon.MaxAmmo)
                        {
                            Price = WeaponAmmo[i];
                            buyAmmo = true;
                            buy = true;
                        }
                        else if (weaponManager.SecondaryWeapon != null)
                        {
                            if (weaponManager.SecondaryWeapon.WeaponName == WeaponTypes[i] && weaponManager.SecondaryWeapon.Ammo < weaponManager.SecondaryWeapon.MaxAmmo)
                            {
                                Price = WeaponAmmo[i];
                                buyAmmo = true;
                                buy = true;
                            }
                        }
                        else
                            buy = false;
                        if(buy)
                            if ((pInput.UseKey || kInput.UseKey) && Score >= Price)
                            {

                                if (!buyAmmo)
                                {
                                    weaponManager.BoughtWeapon(WeaponTypes[i]);
                                    PurchaseWeaponSound.Play();
                                    PurchasedWeapon = true;
                                    Score -= Price;
                                }
                                else if (buyAmmo)
                                {
                                    weaponManager.BoughtAmmo(WeaponTypes[i]);
                                    PurchaseWeaponSound.Play();
                                    PurchasedWeapon = true;
                                    Score -= Price;
                                }
                               
                            }
                    }
                }
                for (int i = 0; i < StoreRect.Count; i++)
                {
                    if(PlayerRect.Intersects(StoreRect[i]))
                    if (!usingShop)
                        if ((pInput.UseKey && !PC) || (kInput.UseKey && PC))
                        {
                            usingShop = true;
                            ShopUsing = i;
                        }
                }
            }
            if (dead)
            {
                usingShop = false;
            }

            if (!dead)
                DamageFlashTick += gameTime.ElapsedGameTime.Milliseconds;
            else
                DamageFlashTick = -1;


            if (CurinvulnerabilityTime > 0)
                CurinvulnerabilityTime -= gameTime.ElapsedGameTime.Milliseconds;
             
            PlayerTexture = weaponManager.Cur_playerStance;
            hitEffect.Update();

            UpdateNode(WalkableTiles);
            
            //Scout = 17
            //Heavy = 7
            //Soldier = 10;
            if (Health >= MaxHealth)
            {
                Health = MaxHealth;
            }



            if (((pInput.GasMaskKey &&  !PC) || (kInput.GasMaskKey  &&  PC))  &&  !GameOver)
                GasMaskOn = !GasMaskOn;
            if (BreathTick <= 0)
                BreathTick = 35;
            else
                BreathTick -= gameTime.ElapsedGameTime.Milliseconds;

            if ((pInput.scoreBoardKey  &&  !PC) || (kInput.ScoreBoardKey && PC))
                ShowScoreBoard = !ShowScoreBoard;
            if (usingShop) Shop(StoreRect);
                
            //temp = new Vector2((Mouse.GetState().X + pos.X) / 2 , Mouse.GetState().Y + pos.Y);
            
            for (int i = 0; i < sonicBoom.Count; i++)
            {
                sonicBoom[i].Update(gameTime);
                if (sonicBoom[i].Remove)
                    sonicBoom.RemoveAt(i);
            }
            for (int i = 0; i < securityPlate.Count; i++)
            {
                securityPlate[i].Update(gameTime);
                if (securityPlate[i].Remove)
                    securityPlate.RemoveAt(i);
            }
            for (int i = 0; i < Slower.Count; i++)
            {
                Slower[i].Update(gameTime);
                if (Slower[i].Remove)
                    Slower.RemoveAt(i);
            }
        }
        public void ResetPlayer()
        {
            PlayerRect.X = (int)StartingPos.X - PlayerRect.Width / 2;
            PlayerRect.Y = (int)StartingPos.Y - PlayerRect.Height / 2;
            Health = MaxHealth / 2;
            weaponManager.ResetWeapons();
            Item = "";
            Item2 = "";
            for (int i = 0; i < PurchasedPerks.Count; i++)
            {
                if (PurchasedPerks[i].life != -1)
                {
                    avaliablePerks.Add(PurchasedPerks[i]);
                    PurchasedPerks.RemoveAt(i);
                }
            }


            dead = false;
            DiedOnWave = -1;
        }

        private void Shop(List<Rectangle> ShopRect)
        {
            if (pInput.BackKey && !PC)//  || pInput.UseKey || kInput.UseKey)
                usingShop = false;
            ShowScoreBoard = false;
            if(!PlayerRect.Intersects(ShopRect[ShopUsing]))
                usingShop = false;

            if (StoreOption > 0)
                if ((pInput.DPad_Up && !PC) || (kInput.GUI_Up && PC))//Change to DPAD UP
                    StoreOption--;
            if (StoreOption < perks.Count - 1)
                if ((pInput.DPad_Down && !PC) || (kInput.GUI_Down && PC))
                    StoreOption++;
            if (avaliablePerks.Count > StoreOption  &&  avaliablePerks.Count > 0)
            if (((pInput.SelectKey && !PC) || (kInput.GUI_Select && PC)) && Score >= avaliablePerks[StoreOption].price)
            {
               // perks[StoreOption].Purchased = true;
                Score -= avaliablePerks[StoreOption].price;
                PurchasedPerks.Add(avaliablePerks[StoreOption]);
                PurchasedPerks[PurchasedPerks.Count - 1].Purchased = true;
                avaliablePerks.RemoveAt(StoreOption);//this won't work 
                if(StoreOption >= avaliablePerks.Count)
                    StoreOption = avaliablePerks.Count - 1;

                PurchaseWeaponSound.Play();
               
            }

            if (PC)
            {
                for (int i = 0; i < perkbuttons.Count; i++)
                {
                    if (i < avaliablePerks.Count)
                    {
                        Rectangle mouseRect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);
                        if (mouseRect.Intersects(perkbuttons[i]))
                        {
                            
                                StoreOption = i;
                        }
                    }
                }
            }
        }
        private void DrawShop(SpriteBatch spriteBatch, Viewport viewPort, SpriteFont FONT)
        {
            int x = Scaled(viewPort.Width / 4);
            Rectangle background = new Rectangle(x,//Scaled(320),//Do this without viewPort and just SpriteScale
                   Scaled(120),
                   viewPort.Width - (x * 2), Scaled(90));// (int)(150 * SpriteScale)
            spriteBatch.Draw(playerTexture, new Rectangle(background.X - Scaled(5), Scaled(background.Y - 5), background.Width + Scaled(10), Scaled(102) + Scaled(perks.Count * 40)), Color.Black);
            spriteBatch.Draw(playerTexture, new Rectangle(background.X, Scaled(background.Y), background.Width, Scaled(background.Height)), Color.Gray);

            spriteBatch.Draw(playerTexture, new Rectangle(background.X, Scaled(background.Y), background.Width, Scaled(50)), Color.DarkGray);
            spriteBatch.DrawString(FONT, "STORE", new Vector2((background.X + background.Width / 2 - (FONT.MeasureString("STORE").X / 2)), Scaled(background.Y + 5)), Color.White);

            spriteBatch.Draw(playerTexture, new Rectangle(background.X, Scaled(background.Y + 50), background.Width, Scaled(40)), Color.Gray);
            spriteBatch.DrawString(FONT, "Item", new Vector2(background.X + Scaled(5), Scaled(background.Y + 52)), Color.White);
            spriteBatch.DrawString(FONT, "Price", new Vector2(background.X + background.Width - Scaled(165), Scaled(background.Y + 52)), Color.White);

            //spriteBatch.Draw(playerTexture, new Rectangle(viewPort.X+ 5, viewPort.Y, viewPort.Width - 10, viewPort.Height), Color.Black);
            for (int i = 0; i < avaliablePerks.Count; i++)
            {
                if (!avaliablePerks[i].Purchased)
                {
                    Color BackColor = Color.Cyan;
                    Color TextColor = Color.White;
                    if (i == StoreOption)
                        BackColor = Color.Black;
                   // if (perks[i].Purchased)
                        //BackColor = Color.Green;
                    if (Score < avaliablePerks[i].price)
                        TextColor = Color.Red;

                    spriteBatch.Draw(playerTexture, new Rectangle(background.X,Scaled(background.Y + 50 + (40 * (i + 1))), background.Width, Scaled(40)), BackColor);//new Rectangle(background.X, background.Y + 50 + (40 * (i + 1)), background.Width, 40)
                    spriteBatch.DrawString(FONT, avaliablePerks[i].name, new Vector2(background.X + Scaled(5), Scaled(background.Y + 50 + (40 * (i + 1)) + 5)), TextColor);//new Vector2(background.X + 5, background.Y + 50 + (40 * (i + 1)) + 5)
                    spriteBatch.DrawString(FONT, "" + avaliablePerks[i].price, new Vector2(background.X + background.Width - Scaled(165), Scaled(background.Y + 50 + (40 * (i + 1)) + 5)), TextColor);//new Vector2(background.X + background.Width - 165, background.Y + 50 + (40 * (i + 1)) + 5)
                    if (!perkbuttons.Contains(new Rectangle(background.X, Scaled(background.Y + 50 + (40 * (i + 1))), background.Width, Scaled(40))))
                    perkbuttons.Add(new Rectangle(background.X, Scaled(background.Y + 50 + (40 * (i + 1))), background.Width, Scaled(40)));
                }
            }

            if (PlayerCount == 1)
            {
                Rectangle infoBackground = new Rectangle(x,//Scaled(320),//Do this without viewPort and just SpriteScale
                   Scaled(500),
                   viewPort.Width - (x * 2), Scaled(90));// (int)(150 * SpriteScale)
                spriteBatch.Draw(playerTexture, new Rectangle(infoBackground.X - Scaled(5), Scaled(infoBackground.Y - 5), infoBackground.Width + Scaled(10), infoBackground.Height + Scaled(10)), Color.Black);
                spriteBatch.Draw(playerTexture, new Rectangle(infoBackground.X, Scaled(infoBackground.Y), infoBackground.Width, Scaled(infoBackground.Height)), Color.Gray);

                if (avaliablePerks.Count > StoreOption && avaliablePerks.Count > 0)
                    spriteBatch.DrawString(FONT, perks[StoreOption].description, new Vector2(infoBackground.X + 10, infoBackground.Y), Color.White);

               // spriteBatch.Draw(playerTexture, new Rectangle(background.X - Scaled(5), Scaled(background.Y - 5), background.Width + Scaled(10), Scaled(102) + Scaled(perks.Count * 40)), Color.Black);
            }

            if (!PC)
            {
                //draw dpad
                if (avaliablePerks.Count > StoreOption && avaliablePerks.Count > 0)
                    spriteBatch.Draw(pInput.ButtonDpad, new Rectangle( background.X - 40, Scaled(background.Y + 55 + (40 * (StoreOption + 1))) ,30,30), Color.White);
            }
            // if (avaliablePerks.Count > StoreOption  &&  avaliablePerks.Count > 0)
            //spriteBatch.Draw(playerTexture, new Rectangle(background.X - Scaled(5), Scaled(background.Y - 5), background.Width + Scaled(10), Scaled(102) + Scaled(perks.Count * 40)), Color.Black);
            //spriteBatch.Draw(playerTexture, new Rectangle(background.X - Scaled(5), Scaled(background.Y - 5), background.Width + Scaled(10), Scaled(102) + Scaled(perks.Count * 40)), Color.Black);
           // spriteBatch.Draw(playerTexture, new Rectangle(background.X - 5, background.Y + background.Height + 145, background.Width + 10, background.Height + 10), Color.Black);
           // if(avaliablePerks.Count > 0)
           // spriteBatch.DrawString(FONT, avaliablePerks[StoreOption].description, new Vector2(background.X, background.Y + background.Height + 150), Color.White);

        }
        private void UpdateSpeedBoostTrail()
        {
           // if(SpeedBoostTrail.Count < 15)
            if(!dead  &&  HasSpeedBost)
                SpeedBoostTrail.Add(new Rectangle(PlayerRect.X + 5, PlayerRect.Y + 5, PlayerRect.Width - 10, PlayerRect.Height - 10));
            for (int i = 0; i < SpeedBoostTrail.Count; i++)
            {
                SpeedBoostTrail[i] = new Rectangle(SpeedBoostTrail[i].X + 1, SpeedBoostTrail[i].Y + 1,
                SpeedBoostTrail[i].Width - 2,
                SpeedBoostTrail[i].Height - 2);
                if (SpeedBoostTrail[i].Width < 20)
                    SpeedBoostTrail.RemoveAt(i);
            }
            if(!dead  && HasSpeedBost)
                SpeedBoostTrail.Add(new Rectangle(PlayerRect.X + 5, PlayerRect.Y + 5, PlayerRect.Width - 10, PlayerRect.Height - 10));
        }
        private void UpdatePlaceItem()
        {
           
            if(pInput.ChangeClass_right  ||  kInput.PlaceItemKey)
            {
                if (Item == "Sonic Boom")
                {
                    sonicBoom.Add(new SonicBoom(new Rectangle((int)pos.X - 20, (int)pos.Y - 20, 40, 40), SonicBoomTexture, SonicBoomExplosionTexture, SonicBoomBeep, SonicBoomExplosion));
                    if (HasExtraSlot)
                    {
                        Item = Item2;
                        Item2 = "";
                    }
                    else
                        Item = "";
                }
                else if (Item == "Security Plate")
                {
                    securityPlate.Add(new SecurityPlate(securityPlateTexture, new Rectangle((int)pos.X - 20, (int)pos.Y - 20, 40, 40)));
                    if (HasExtraSlot)
                    {
                        Item = Item2;
                        Item2 = "";
                    }
                    else
                        Item = "";
                }
                else if (Item == "Slower")
                {
                    Slower.Add(new Slower(SlowerTexture,SlowerRingTexture, new Rectangle((int)pos.X - 20, (int)pos.Y - 20, 40, 40)));
                    if (HasExtraSlot)
                    {
                        Item = Item2;
                        Item2 = "";
                    }
                    else
                        Item = "";
                }
            }
            if (pInput.ChangeClass_left || kInput.PlaceItem2Key)
            {
                if (Item2 == "Sonic Boom")
                {
                    sonicBoom.Add(new SonicBoom(new Rectangle((int)pos.X - 20, (int)pos.Y - 20, 40, 40), SonicBoomTexture, SonicBoomExplosionTexture, SonicBoomBeep, SonicBoomExplosion));
                    Item2 = "";
                }
                else if (Item2 == "Security Plate")
                {
                    securityPlate.Add(new SecurityPlate(securityPlateTexture, new Rectangle((int)pos.X - 20, (int)pos.Y - 20, 40, 40)));
                    Item2 = "";
                }
                else if (Item2 == "Slower")
                {
                    Slower.Add(new Slower(SlowerTexture, SlowerRingTexture, new Rectangle((int)pos.X - 20, (int)pos.Y - 20, 40, 40)));
                    Item2 = "";
                }
            }
        }
        private void UpdateNode(List<Rectangle> WalkableTiles)
        {
            for (int i = 0; i < WalkableTiles.Count; i++)
            {
                Rectangle cen = new Rectangle((int)pos.X, (int)pos.Y, 1, 1);
                if (cen.Intersects(WalkableTiles[i]))
                {
                    Cur_Node = i;
                }
            }
        }
        Vector2 AnalogPos;
        Vector2 AnalogPos2;
        private void UpdateSpectate()
        {
            if (!GameOver)
            {
                if (!PC)
                {
                    SpectatorPos.X += (int)(GamePad.GetState(player).ThumbSticks.Left.X * SpectateSpeed);
                    SpectatorPos.Y -= (int)(GamePad.GetState(player).ThumbSticks.Left.Y * SpectateSpeed);

                    if (pInput.HoldA)
                        SpectateSpeed = 25;
                    else
                        SpectateSpeed = 12;

                    if (pInput.BackKey)
                        DisplayHud = !DisplayHud;
                }
            }
        }
        private void UpdateMovement()
        {
            if (!PC)
            {
                PlayerRect.X += (int)(GamePad.GetState(player).ThumbSticks.Left.X * PlayerSpeed);
                PlayerRect.Y -= (int)(GamePad.GetState(player).ThumbSticks.Left.Y * PlayerSpeed);
                
                AnalogPos2.X = (int)((PlayerRect.X + PlayerRect.Width / 2) + (GamePad.GetState(player).ThumbSticks.Left.X * 25));
                AnalogPos2.Y = (int)((PlayerRect.Y + PlayerRect.Height / 2) - (GamePad.GetState(player).ThumbSticks.Left.Y * 25));
                if (Math.Abs(GamePad.GetState(player).ThumbSticks.Left.X) > 0.1 || Math.Abs(GamePad.GetState(player).ThumbSticks.Left.Y) > 0.1)
                {
                    Vector2 WalkDir = AnalogPos2 - pos;
                    WalkDir.Normalize();
                    WalkRotation = (float)Math.Atan2((double)WalkDir.Y, (double)WalkDir.X);
                    if(Math.Abs(GamePad.GetState(player).ThumbSticks.Right.X) <= 0.1 && (Math.Abs(GamePad.GetState(player).ThumbSticks.Right.Y) <= 0.1))
                        LookDirection = WalkRotation;
                }
            }
            if (PC)
            {
                if (kInput.FowardKey)
                {
                    if (!kInput.LeftKey && !kInput.RightKey)
                        PlayerRect.Y -= PlayerSpeed;
                    else
                        PlayerRect.Y -= PlayerSpeed / 2 + PlayerSpeed / 4;//3;
                }
                else if (kInput.BackwardsKey)
                {
                    if (!kInput.LeftKey && !kInput.RightKey)
                        PlayerRect.Y += PlayerSpeed;
                    else
                        PlayerRect.Y += PlayerSpeed / 2 + PlayerSpeed / 4;
                }
                if (kInput.LeftKey)
                {
                    if (!kInput.FowardKey && !kInput.BackwardsKey)
                        PlayerRect.X -= PlayerSpeed;
                    else
                        PlayerRect.X -= PlayerSpeed / 2 + PlayerSpeed / 4;
                }
                else if (kInput.RightKey)
                {
                    if (!kInput.FowardKey && !kInput.BackwardsKey)
                        PlayerRect.X += PlayerSpeed;
                    else
                        PlayerRect.X += PlayerSpeed / 2 + PlayerSpeed / 4;
                }

                //if (Math.Abs(new Vector2(pos.X + (Mouse.GetState().X - 640) * 2).X) > 0 && Math.Abs(new Vector2(pos.Y + (Mouse.GetState().Y - 360) * 2).Y) > 0 && Cursor_Pos - pos != Vector2.Zero)
                    Cursor_Pos = new Vector2(pos.X + (Mouse.GetState().X - 640) * 2, pos.Y + (Mouse.GetState().Y - 360) * 2);
                    //graphics.viewport.widht/height / 2 #mouse
                Vector2 WalkDir2 = new Vector2(pos.X + (Mouse.GetState().X - 640) * 2, pos.Y + (Mouse.GetState().Y - 360) * 2) - pos;
                WalkDir2.Normalize();
                if (Math.Abs((float)Math.Atan2((double)WalkDir2.Y, (double)WalkDir2.X)) >= 0)
                {
                    WalkRotation = (float)Math.Atan2((double)WalkDir2.Y, (double)WalkDir2.X);
                }


                Vector2 CursorDir = pos - Cursor_Pos;
                CursorDir.Normalize();
                if (Math.Abs((float)Math.Atan2((double)CursorDir.Y, (double)CursorDir.X)) >= 0)
                {
                    Cursor_rotation = (float)Math.Atan2((double)CursorDir.Y, (double)CursorDir.X);
                }
                

            }
            if (PlayerRect.X <= -200)
                PlayerRect.X = -200;
            if (PlayerRect.Y <= -200)
                PlayerRect.Y = -200;
            if (PlayerRect.X + PlayerRect.Width >= 5800)
                PlayerRect.X = 5800 - PlayerRect.Width;
            if (PlayerRect.Y + PlayerRect.Height >= 5800)
                PlayerRect.Y = 5800 - PlayerRect.Height;
        }
        public void UpdateToPos(Vector2 newPos)
        {
            PlayerRect.X = (int)newPos.X - PlayerRect.Width / 2;
            PlayerRect.Y = (int)newPos.Y - PlayerRect.Height / 2;
            pos = newPos;

        }
        private void UpdateShooting(GameTime gameTime)
        {
            if (weaponManager.SwitchedWeapon)
            {
                FireTick = 0;
                weaponManager.SwitchedWeapon = false;
            }
            if(weaponManager.CurentWeapon.Ammo <= 0)
                FireTick = 0;

            
            if (!PC)
            {
                AnalogPos.X = (int)((PlayerRect.X + PlayerRect.Width / 2) + (GamePad.GetState(player).ThumbSticks.Right.X * 150));
                AnalogPos.Y = (int)((PlayerRect.Y + PlayerRect.Height / 2) - (GamePad.GetState(player).ThumbSticks.Right.Y * 150));
            }
            FireTick += gameTime.ElapsedGameTime.Milliseconds;// 1;
            if (((Math.Abs(GamePad.GetState(player).ThumbSticks.Right.X) > 0.1  &&  !PC) || (Math.Abs(GamePad.GetState(player).ThumbSticks.Right.Y) > 0.1  &&  !PC))
                ||  (PC  &&  Mouse.GetState().LeftButton ==  ButtonState.Pressed  && !usingShop))
            {
                if (FireTick > weaponManager.Cur_WeaponShotTime  &&  weaponManager.CurentWeapon.Ammo > 0)//  && GamePad.GetState(player).Triggers.Right >= 0.6)
                {
                    //SoundEffectInstance si = weaponManager.Cur_WeaponSound.CreateInstance();
                    Random random = new Random();
                    //si.Pitch = (float)(random.Next(weaponManager.Cur_SoundMinPitch, weaponManager.Cur_SoundMaxPitch)) / 100f;
                    //si.Play();
                    //weaponManager.Cur_WeaponSound.Play();
                    weaponManager.Cur_WeaponSound.Play(1, (float)(random.Next(weaponManager.Cur_SoundMinPitch, weaponManager.Cur_SoundMaxPitch)) / 100f, 0);
                    BulletSpeed = weaponManager.BulletSpeed;
                    BulletDamage = weaponManager.BulletDamage;
                    BulletLife = weaponManager.BulletLife;
                    BulletType = weaponManager.BulletType;
                    BulletHitSound = weaponManager.BulletHitSound;
                    HitPoints = weaponManager.HitPoints;
                    KillPoints = weaponManager.KillPoints;
                    //pInput.ViberateController(3, 0.09f);
                    weaponManager.CurentWeapon.Ammo -=  weaponManager.CurentWeapon.AmmoLossRate;
                    Fired = true;
                    FireTick = 0;
                }
                BulletTexture = weaponManager.Cur_WeaponTexture;
                Vector2 spawnpos = new Vector2(PlayerRect.X + PlayerRect.Width / 2, PlayerRect.Y + PlayerRect.Height / 2);// - (BulletTexture.Height / 2));
                BulletPos = spawnpos;
                Vector2 Direction;
                if (!PC)
                {
                    Direction = AnalogPos - spawnpos;
                }
                else
                {
                    Direction = /*Cursor_Pos*/new Vector2(pos.X + (Mouse.GetState().X - 640) * 2, pos.Y + (Mouse.GetState().Y - 360) * 2) - pos;//new Vector2(CamCen.X + Mouse.GetState().X, CamCen.Y + Mouse.GetState().Y)
                }
                Direction.Normalize();
                this.BulletDirection = Direction;
                BulletRotation = (float)Math.Atan2((double)Direction.Y, (double)Direction.X);
                LookDirection = BulletRotation;
            }
        }
        private void DetectCollisions(List<Rectangle> CollisionObjects , int PlayerSpeed)
        {
            BottomRect = new Rectangle(PlayerRect.X + 1, PlayerRect.Y + PlayerRect.Height - 1, PlayerRect.Width - 2, 2);
            Top_Rect = new Rectangle(PlayerRect.X + 1, PlayerRect.Y - 1, PlayerRect.Width - 2, 2);
            Left_Rect = new Rectangle(PlayerRect.X - 1, PlayerRect.Y + 1, 2, PlayerRect.Height - 2);
            Right_Rect = new Rectangle(PlayerRect.X + PlayerRect.Width - 1, PlayerRect.Y + 1, 2, PlayerRect.Height - 2);

            for (int i = 0; i < CollisionObjects.Count; i++)
            {
                if (PlayerRect.Y + PlayerRect.Height <= CollisionObjects[i].Y + PlayerSpeed || PlayerRect.Y >= CollisionObjects[i].Y + CollisionObjects[i].Height - PlayerSpeed)
                {
                    if (Top_Rect.Intersects(CollisionObjects[i]))
                    {
                        if (PlayerRect.X < CollisionObjects[i].X + CollisionObjects[i].Width - PlayerSpeed &&
                            PlayerRect.X + PlayerRect.Width > CollisionObjects[i].X + PlayerSpeed)
                        {
                            PlayerRect.Y = CollisionObjects[i].Y + CollisionObjects[i].Height;
                        }
                    }
                    else if (BottomRect.Intersects(CollisionObjects[i]))
                    {
                        PlayerRect.Y = CollisionObjects[i].Y - PlayerRect.Height;
                    }
                    BottomRect = new Rectangle(PlayerRect.X + 1, PlayerRect.Y + PlayerRect.Height - 1, PlayerRect.Width - 2, 2);
                    Top_Rect = new Rectangle(PlayerRect.X + 1, PlayerRect.Y - 1, PlayerRect.Width - 2, 2);
                    Left_Rect = new Rectangle(PlayerRect.X - 1, PlayerRect.Y + 1, 2, PlayerRect.Height - 2);
                    Right_Rect = new Rectangle(PlayerRect.X + PlayerRect.Width - 1, PlayerRect.Y + 1, 2, PlayerRect.Height - 2);

                }
                else
                {
                    if (Right_Rect.Intersects(CollisionObjects[i]))
                    {
                        PlayerRect.X = CollisionObjects[i].X - PlayerRect.Width;
                    }
                    else if (Left_Rect.Intersects(CollisionObjects[i]))
                    {
                        PlayerRect.X = CollisionObjects[i].X + CollisionObjects[i].Width;
                    }
                    BottomRect = new Rectangle(PlayerRect.X + 1, PlayerRect.Y + PlayerRect.Height - 1, PlayerRect.Width - 2, 2);
                    Top_Rect = new Rectangle(PlayerRect.X + 1, PlayerRect.Y - 1, PlayerRect.Width - 2, 2);
                    Left_Rect = new Rectangle(PlayerRect.X - 1, PlayerRect.Y + 1, 2, PlayerRect.Height - 2);
                    Right_Rect = new Rectangle(PlayerRect.X + PlayerRect.Width - 1, PlayerRect.Y + 1, 2, PlayerRect.Height - 2);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            //if (player == PlayerIndex.One)//Determine class instead of player to chose color tint
            //{
            //    color = Color.DarkOrange;
            //}
            //if (player == PlayerIndex.Two)
            //{
            //    color = Color.Blue;
            //}
            //spriteBatch.Draw(playerTexture, PlayerRect, color);
            sentry.Draw(spriteBatch);
            for (int i = 0; i < SpeedBoostTrail.Count; i++)
            {
                if (i < SpeedBoostTrail.Count - 1  &&  !SpeedBoostTrail[i].Intersects(new Rectangle((int)pos.X - 1, (int)pos.Y - 1, 2, 2)))
                {
                    Color _color = new Color((int)(255f * ((float)(SpeedBoostTrail[i].Width - 19) / (float)PlayerRect.Width)),
                        (int)(255f * ((float)(SpeedBoostTrail[i].Width - 19) / (float)PlayerRect.Width)),
                        (int)(255f * ((float)(SpeedBoostTrail[i].Width - 19) / (float)PlayerRect.Width)),
                        (int)(255f * ((float)(SpeedBoostTrail[i].Width - 19) / (float)PlayerRect.Width)));
                    spriteBatch.Draw(circle, SpeedBoostTrail[i], _color);
                }
            }
            if (!dead)
            {
                if (!PC)// || (PC && player != PlayerIndex.One))#pcChange
                {
                    //if (Math.Abs(GamePad.GetState(player).ThumbSticks.Right.X) > 0.1 || Math.Abs(GamePad.GetState(player).ThumbSticks.Right.Y) > 0.1)
                    //{
                    //    spriteBatch.Draw(PlayerTexture, pos, null, Color.White, BulletRotation, new Vector2(PlayerTexture.Width / 2, PlayerTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                    //}
                    //else
                    //{
                    //    spriteBatch.Draw(PlayerTexture, pos, null, Color.White, WalkRotation, new Vector2(PlayerTexture.Width / 2, PlayerTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                    //}
                    if(CurinvulnerabilityTime <= 0)
                        spriteBatch.Draw(PlayerTexture, pos, null, Color.White, LookDirection, new Vector2(PlayerTexture.Width / 2, PlayerTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                    else
                        if(CurinvulnerabilityTime % 10 < 5)
                            spriteBatch.Draw(PlayerTexture, pos, null, Color.White, LookDirection, new Vector2(PlayerTexture.Width / 2, PlayerTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
               }
                if (PC)//  &&  player == PlayerIndex.One)#pcChange
                {
                    if (CurinvulnerabilityTime <= 0)
                        spriteBatch.Draw(PlayerTexture, pos, null, Color.White, WalkRotation, new Vector2(PlayerTexture.Width / 2, PlayerTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                    else
                        if (CurinvulnerabilityTime % 10 < 5)
                            spriteBatch.Draw(PlayerTexture, pos, null, Color.White, WalkRotation, new Vector2(PlayerTexture.Width / 2, PlayerTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);

                    if(!usingShop)
                    spriteBatch.Draw(Cursor, Cursor_Pos, null, Color.White, Cursor_rotation, new Vector2(Cursor.Width / 2, Cursor.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                }
            }
            hitEffect.Draw(spriteBatch);
        }
        public void DrawItems(SpriteBatch spriteBatch)
        {
            stimGas.Draw(spriteBatch);
            damageBoost.Draw(spriteBatch);
            for (int i = 0; i < sonicBoom.Count; i++)
            {
                sonicBoom[i].Draw(spriteBatch);
            }
            for (int i = 0; i < securityPlate.Count; i++)
                securityPlate[i].Draw(spriteBatch);
            for (int i = 0; i < Slower.Count; i++)
                Slower[i].Draw(spriteBatch);

        }
        private float SpriteScale;
        public void DrawGUI(SpriteBatch spriteBatch, SpriteFont _font, float SpriteScale, Viewport viewPort, List<Player> players, SpriteFont smallFont)
        {
            this.SpriteScale = SpriteScale;
            //SpriteScale = 1;
            /*Background*/
            if (!dead || (dead && DisplayHud))
            {
                Rectangle BackgroundRect = new Rectangle((int)((HealthBar.X - 2) * SpriteScale), (int)((HealthBar.Y - 4) * SpriteScale), (int)((MaxHealth + 8) * SpriteScale), (int)(155 * SpriteScale));//Height = 115
                spriteBatch.Draw(playerTexture, BackgroundRect, new Color(64, 64, 64, 128));//128 128 128 255

                _DrawItems(spriteBatch, BackgroundRect);

                DrawHealthBar(spriteBatch);
                DrawFireRateBar(spriteBatch);
                DrawWeaponHUD(spriteBatch);
                
                //Score

                spriteBatch.DrawString(_font, "" + Score, new Vector2((HealthBar.X + 160) * SpriteScale, (HealthBar.Y + 15) * SpriteScale), Color.Yellow);
                pointEffect = new Vector2((HealthBar.X + 160 + _font.MeasureString("" + Score).X) * SpriteScale, (HealthBar.Y + 16 + (_font.MeasureString("" + Score).Y / 2)) * SpriteScale);

                DpadRect = new Rectangle(BackgroundRect.X + BackgroundRect.Width / 2 - Scaled(30), BackgroundRect.Y + BackgroundRect.Height + Scaled(20), Scaled(60), Scaled(60));
                if (PC)
                {
                    spriteBatch.Draw(GasMaskKeyTexture, new Rectangle(DpadRect.X, DpadRect.Y + Scaled(15), Scaled(30), Scaled(30)), Color.White);
                    spriteBatch.Draw(AbilityKeyTexture, new Rectangle(DpadRect.X + DpadRect.Width - Scaled(20), DpadRect.Y + Scaled(15), Scaled(30), Scaled(30)), Color.White);
                }
                else
                    spriteBatch.Draw(pInput.ButtonDpad, DpadRect, Color.White);

                if (Class == 0)
                    DrawItemIcon(85, 15, spriteBatch, _font, sentry.OperationTime, sentry.OPERATION_TIME, sentry.Sentry_Recharge, sentry.Sentry_RechargeTime, sentry.SentryPrice
                        , sentry.SentryStand, sentry.SentryTexture);
                if (Class == 1)
                    DrawItemIcon(85, 15, spriteBatch, _font, stimGas.OperationTime, stimGas.OPERATION_TIME, stimGas.StimGas_Recharge, stimGas.StimGas_RechargeTime, stimGas.StimGasPrice, stimGas.icon, null);
                if (Class == 2)
                    DrawItemIcon(85, 15, spriteBatch, _font, damageBoost.CurOperationTime, damageBoost.OperationTime, damageBoost.CurRechargeTime, damageBoost.RechargeTime, damageBoost.Price, damageBoost.icon, null);
                if (Class == 3)
                    DrawItemIcon(85, 15, spriteBatch, _font, ammoBag.CurOperationTime, ammoBag.OperationTime, ammoBag.CurRechargeTime, ammoBag.RechargeTime, ammoBag.Price, ammoBag.icon, null);


                if (!GasMaskOn)
                    spriteBatch.Draw(GasMask, new Rectangle(DpadRect.X - Scaled(75/*130*/), DpadRect.Y, Scaled(60), Scaled(60)), Color.White);
                else
                    spriteBatch.Draw(GasMask, new Rectangle(DpadRect.X - Scaled(75/*130*/), DpadRect.Y, Scaled(60), Scaled(60)), Color.Aqua);

                Rectangle ammoRect = new Rectangle(BackgroundRect.X + BackgroundRect.Width + 10,
                    BackgroundRect.Y + BackgroundRect.Height - 50, 100, 50);
                Vector2 ammoPos = new Vector2((int)((HealthBar.X + 75 - (_font.MeasureString("" + weaponManager.CurentWeapon.Ammo).X / 2)) * SpriteScale), (int)((HealthBar.Y + 115) * SpriteScale));
               // spriteBatch.Draw(playerTexture, ammoRect, new Color(64, 64, 64, 128));//128 128 128 255
                spriteBatch.DrawString(_font, "" + weaponManager.CurentWeapon.Ammo, ammoPos, Color.White);

                if (dead  &&  !GameOver)
                {
                    spriteBatch.DrawString(font, "Spectating", new Vector2(10, viewPort.Height - font.MeasureString(".").Y * 3), Color.White);
                    spriteBatch.DrawString(font, "Use 'A' To Move Faster", new Vector2(10, viewPort.Height - font.MeasureString(".").Y * 2), Color.White);
                    spriteBatch.DrawString(font, "Use 'B' To Toggle HUD", new Vector2(10, viewPort.Height - font.MeasureString(".").Y), Color.White);
                }
            }
            SpriteFont FONT;
            if (players.Count <= 2)
                FONT = _font;
            else
                FONT = smallFont;
            if (usingShop) DrawShop(spriteBatch, viewPort, FONT);//_font);
            if (ShowScoreBoard)
            {
                Rectangle scoreBoardRect = new Rectangle(viewPort.Width / 4,
                    viewPort.Height / 6,
                    viewPort.Width - (viewPort.Width / 4) * 2, (int)(40 * SpriteScale));
                   // viewPort.Height - (viewPort.Height / 6) * 2);// new Rectangle(viewPort.Width / 2 - 200, viewPort.Height - 100, viewPort.Width / 2 + 400, viewPort.Height / 2 + 00);
                SpriteFont ScoreBoardFont;
                if (players.Count <= 2)
                    ScoreBoardFont = _font;
                else
                    ScoreBoardFont = smallFont;


                spriteBatch.Draw(playerTexture, scoreBoardRect, new Color(200, 50, 50, 100));

                Vector2 KillsScorePos = new Vector2(scoreBoardRect.X + ScoreBoardFont.MeasureString("player . " + (100 * SpriteScale)).X, scoreBoardRect.Y + scoreBoardRect.Height / 2 - ScoreBoardFont.MeasureString(".").Y / 2);
                spriteBatch.DrawString(ScoreBoardFont, "Kills", KillsScorePos, Color.White);

                Vector2 DeathsScorePos = new Vector2(scoreBoardRect.X + ScoreBoardFont.MeasureString("player . " + (100 * SpriteScale)).X + ScoreBoardFont.MeasureString("Kills ").X + (40 * SpriteScale), scoreBoardRect.Y + scoreBoardRect.Height / 2 - ScoreBoardFont.MeasureString(".").Y / 2);
                spriteBatch.DrawString(ScoreBoardFont, "Deaths", DeathsScorePos, Color.White);

                for (int i = 0; i < players.Count; i++)
                {
                    Rectangle playerScoreRect = new Rectangle(scoreBoardRect.X, scoreBoardRect.Y + (int)((45 * (i + 1)) * SpriteScale), scoreBoardRect.Width, scoreBoardRect.Height);

                    spriteBatch.Draw(playerTexture, playerScoreRect, new Color(200, 50, 50, 100));

                    float StatY = playerScoreRect.Y + playerScoreRect.Height / 2 - ScoreBoardFont.MeasureString(".").Y / 2;


                    spriteBatch.DrawString(ScoreBoardFont, "Player " + (i + 1), new Vector2(playerScoreRect.X + 10, StatY), Color.White); 

                    spriteBatch.DrawString(ScoreBoardFont, "" + players[i].Kills, new Vector2(KillsScorePos.X
                        + ScoreBoardFont.MeasureString("Kills").X / 2 - ScoreBoardFont.MeasureString("" + players[i].Kills).X / 2,
                        StatY), Color.White);

                    spriteBatch.DrawString(ScoreBoardFont, "" + players[i].Deaths, new Vector2(DeathsScorePos.X +
                        ScoreBoardFont.MeasureString("Deaths").X / 2 - ScoreBoardFont.MeasureString("" + players[i].Deaths).X / 2, StatY), Color.White);
                }
                
            }

        }
        private void _DrawItems(SpriteBatch spriteBatch, Rectangle BackgroundRect)
        {
            if (Item != "")
            {
                Rectangle itemRect1 = new Rectangle(BackgroundRect.X + BackgroundRect.Width + 20, BackgroundRect.Y, 50, 50);
                Texture2D Item1Texture = null;
                if (Item == "Sonic Boom")
                    Item1Texture = SonicBoomTexture;
                if (Item == "Security Plate")
                    Item1Texture = securityPlateTexture;
                if (Item == "Slower")
                    Item1Texture = SlowerTexture;
                
                spriteBatch.Draw(playerTexture, itemRect1, Color.Black);
                spriteBatch.Draw(playerTexture, new Rectangle(itemRect1.X + 4, itemRect1.Y + 4, itemRect1.Width - 8, itemRect1.Height - 8), Color.LightGray);
                if (Item1Texture != null)
                {
                    spriteBatch.Draw(Item1Texture, new Rectangle(itemRect1.X + 6, itemRect1.Y + 6, itemRect1.Width - 12, itemRect1.Height - 12), Color.White);//#Addkeyhere
                    if(PC)
                        spriteBatch.Draw(kInput.Item1KeyTexture, new Rectangle(itemRect1.X + itemRect1.Width / 2 - 10, itemRect1.Y + itemRect1.Height + 10, 20, 20), Color.White);
                    else
                        spriteBatch.Draw(pInput.ButtonRB, new Rectangle(itemRect1.X + itemRect1.Width / 2 - 25, itemRect1.Y + itemRect1.Height + 10, 50, 25), Color.White);
                }

                if (HasExtraSlot)
                {
                    Rectangle itemRect2 = new Rectangle(BackgroundRect.X + BackgroundRect.Width + 80, BackgroundRect.Y, 50, 50);
                    spriteBatch.Draw(playerTexture, itemRect2, Color.Black);
                    spriteBatch.Draw(playerTexture, new Rectangle(itemRect2.X + 4, itemRect2.Y + 4, itemRect2.Width - 8, itemRect2.Height - 8), Color.LightGray);

                    Texture2D Item2Texture = null;
                    if (Item2 == "Sonic Boom")
                        Item2Texture = SonicBoomTexture;
                    if (Item2 == "Security Plate")
                        Item2Texture = securityPlateTexture;
                    if (Item2 == "Slower")
                        Item2Texture = SlowerTexture;

                    if (Item2Texture != null)
                    {
                        spriteBatch.Draw(Item2Texture, new Rectangle(itemRect2.X + 6, itemRect2.Y + 6, itemRect2.Width - 12, itemRect2.Height - 12), Color.White);
                        if (PC)
                            spriteBatch.Draw(kInput.Item2KeyTexture, new Rectangle(itemRect2.X + itemRect2.Width / 2 - 10, itemRect2.Y + itemRect2.Height + 10, 20, 20), Color.White);
                        else
                            spriteBatch.Draw(pInput.ButtonLB, new Rectangle(itemRect2.X + itemRect2.Width / 2 - 25, itemRect2.Y + itemRect2.Height + 10, 50, 25), Color.White);
                    }
                }
            }
        }
        private void DrawHealthBar(SpriteBatch spriteBatch)
        {
            //BlackBackground
            spriteBatch.Draw(playerTexture, new Rectangle((int)((HealthBar.X) * SpriteScale), (int)((HealthBar.Y - 2) * SpriteScale), (int)((MaxHealth + 4) * SpriteScale), (int)((HealthBar.Height + 4) * SpriteScale)), Color.Black);
            //Red
            
            if (DamageFlashTick != -1)
            {
                if (DamageFlashTick >= 500)
                {
                    spriteBatch.Draw(playerTexture, new Rectangle((int)((HealthBar.X + 2) * SpriteScale), (int)((HealthBar.Y) * SpriteScale), (int)((MaxHealth) * SpriteScale), (int)((HealthBar.Height) * SpriteScale)), Color.OrangeRed);
                    if (DamageFlashTick >= 1000)
                        DamageFlashTick = 0;
                }
                else
                    spriteBatch.Draw(playerTexture, new Rectangle((int)((HealthBar.X + 2) * SpriteScale), (int)((HealthBar.Y) * SpriteScale), (int)((MaxHealth) * SpriteScale), (int)((HealthBar.Height) * SpriteScale)), Color.DarkRed);
            }
            else
                spriteBatch.Draw(playerTexture, new Rectangle((int)((HealthBar.X + 2) * SpriteScale), (int)((HealthBar.Y) * SpriteScale), (int)((MaxHealth) * SpriteScale), (int)((HealthBar.Height) * SpriteScale)), Color.Red);
            spriteBatch.Draw(playerTexture, new Rectangle((int)((HealthBar.X + 2) * SpriteScale), (int)((HealthBar.Y) * SpriteScale), (int)(HealthBar.Width * SpriteScale), (int)(HealthBar.Height * SpriteScale)), Color.LimeGreen);

            spriteBatch.Draw(playerTexture, new Rectangle((int)((HealthBar.X + 2) * SpriteScale), (int)((HealthBar.Y) * SpriteScale), 
                (int)(((int)(MaxHealth * ((float)CurinvulnerabilityTime / (float)InvulnerabilityTime)) * SpriteScale)),
                (int)(HealthBar.Height * SpriteScale)), Color.Aquamarine);
        }
        private void DrawWeaponHUD(SpriteBatch spriteBatch)
        {
            /*CurWeaponTexture*/
            spriteBatch.Draw(weaponManager.CurentWeapon.GUITexture, new Rectangle((int)(HealthBar.X * SpriteScale), (int)((HealthBar.Y + 35) * SpriteScale), (int)(150 * SpriteScale), (int)(75 * SpriteScale)), Color.White);
            /*SecWeaponTexture*/
            if (weaponManager.SecondaryWeapon != null)
                spriteBatch.Draw(weaponManager.SecondaryWeapon.GUITexture, new Rectangle((int)((HealthBar.X + 180) * SpriteScale), (int)((HealthBar.Y + 65) * SpriteScale), (int)((64) * SpriteScale), (int)((32) * SpriteScale)), Color.White);
            if (weaponManager.SecondaryWeapon != null)
                if(PC)
                spriteBatch.Draw(SwitchKeyTexture, new Rectangle(
                    (int)((HealthBar.X + 145) * SpriteScale),
                    (int)((HealthBar.Y + 60) * SpriteScale),
                    (int)(40 * SpriteScale),
                    (int)(20 * SpriteScale)), Color.White);
                else
                    spriteBatch.Draw(SwitchKeyTexture, new Rectangle(
                    (int)((HealthBar.X + 165) * SpriteScale),
                    (int)((HealthBar.Y + 60) * SpriteScale),
                    (int)(25 * SpriteScale),
                    (int)(25 * SpriteScale)), Color.White);
        }
        private void DrawFireRateBar(SpriteBatch spriteBatch)
        {
            int FireBarHeight = 10;
            int FireBarY = HealthBar.Y + 15;
            int FireBarWidth = 150;
            /*FireTick Background*/
            spriteBatch.Draw(playerTexture, new Rectangle((int)((HealthBar.X) * SpriteScale), (int)((FireBarY - 2) * SpriteScale), (int)((FireBarWidth + 4) * SpriteScale), (int)((FireBarHeight + 4) * SpriteScale)), Color.Black);//DarkGray

            if (weaponManager.CurentWeapon.Ammo > 0)
            {
                if ((int)(((float)FireTick / (float)weaponManager.CurentWeapon.ShotTime)) < 1)

                    spriteBatch.Draw(playerTexture, new Rectangle((int)((HealthBar.X + 2) * SpriteScale), (int)((FireBarY) * SpriteScale),
                        (int)(((int)(FireBarWidth * ((float)FireTick / (float)weaponManager.CurentWeapon.ShotTime)) * SpriteScale))
                        , (int)((FireBarHeight) * SpriteScale)), Color.LightSkyBlue);
                else
                    spriteBatch.Draw(playerTexture, new Rectangle((int)((HealthBar.X + 2) * SpriteScale), (int)((FireBarY) * SpriteScale), (int)((FireBarWidth) * SpriteScale), (int)((FireBarHeight) * SpriteScale)), Color.DeepSkyBlue);
            }
        }
        private void DrawItemIcon(int x, int y, SpriteBatch spriteBatch, SpriteFont _font, int curOperationTime, int OperationTime, int CurRechargeTime, int RechargeTime
            ,int price, Texture2D img1, Texture2D img2)
        {
            spriteBatch.Draw(playerTexture, new Rectangle(DpadRect.X + Scaled((x - 2)), DpadRect.Y + Scaled((y - 2)), Scaled(34), Scaled(34)), Color.Black);
            spriteBatch.Draw(playerTexture, new Rectangle(DpadRect.X + Scaled(x), DpadRect.Y + Scaled(y), Scaled(30), Scaled(30)), Color.RoyalBlue);
            spriteBatch.Draw(img1, new Rectangle(DpadRect.X + Scaled(x + 1), DpadRect.Y + Scaled(y + 1), Scaled(28), Scaled(28)), Color.White);
            if(img2 != null)
                spriteBatch.Draw(img2, new Rectangle(DpadRect.X + Scaled(x + 1), DpadRect.Y + Scaled(y + 1), Scaled(28), Scaled(28)), Color.White);
            if (CurRechargeTime > 0)
            {
                if ((30 * CurRechargeTime) / RechargeTime < 31 || sentry.active)//Recharge Time 
                {
                    spriteBatch.Draw(playerTexture, new Rectangle(
                        DpadRect.X + Scaled((x) + (30 - (30 * CurRechargeTime) / RechargeTime)),
                        DpadRect.Y + Scaled(y),
                        Scaled((int)((30 * CurRechargeTime) / RechargeTime) + 1),
                        Scaled(30)), new Color(125, 25, 25, 150));
                }
            }
            if (curOperationTime > 0)
            {
                if ((30 * curOperationTime) / OperationTime != 30) //Operation Time
                {
                    spriteBatch.Draw(playerTexture, new Rectangle(
                        DpadRect.X + Scaled((x) + (30 - (30 * curOperationTime) / OperationTime)),
                        DpadRect.Y + Scaled(y),
                        Scaled((int)((30 * curOperationTime) / OperationTime) + 1),
                        Scaled(30)), new Color(25, 125, 25, 150));
                }
            }
            if(price > 0)
            spriteBatch.DrawString(_font, "($" + price + ")", new Vector2(DpadRect.X + Scaled(x + 15)
                - _font.MeasureString("($" + price + ")").X / 2, DpadRect.Y + Scaled(60)), Color.White);
        }
        private int Scaled(int NUM)
        {
            int Num = (int)(NUM * SpriteScale);
            return Num;
        }

    }
}
