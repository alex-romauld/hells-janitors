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

using System.IO;

namespace SurvivalShooter
{
    class GameDirector
    {
        GraphicsDevice graphics;
        SpriteFont font;
        SpriteFont Splitfont;
        SpriteFont GameInfoFont;
        SpriteFont secondaryFont;
        SpriteFont smallFont;

        Texture2D square;
        Texture2D GrassTile;

        //Setup Players
        Player player1 = new Player();
        Player player2 = new Player();
        Player player3 = new Player();
        Player player4 = new Player();
        public List<Player> players = new List<Player>();
        public int Player_paused = 0;

        private Vector2[] StimGasPos = new Vector2[4];
        private Boolean[] StimGasActive = new bool[4];
        private Vector2[] DamageBoostPos = new Vector2[4];
        private Boolean[] DamageBoostActive = new bool[4];
        private List<Rectangle> AmmoBagRect = new List<Rectangle>();
        private List<int> PlayersBag = new List<int>();

        //Setup Cameras
        Camera camera;//Fullscreen
        Camera camera_1;//Top
        Camera camera_2;//Bottom
        Camera camera_3;
        Camera camera_4;


        List<Projectile> bullets = new List<Projectile>();


        List<PointText> pointText = new List<PointText>();
        private int ADD_POINTS_Lastadded = 0;
        List<WorldText> DropText = new List<WorldText>();
        List<WorldText> PlayerPopupText = new List<WorldText>();

        //Setup ViewPorts
        Viewport DefaultViewPort;
        Viewport TopViewPort;
        Viewport BottomViewPort;

        Viewport ViewPort_TopRight;
        Viewport ViewPort_TopLeft;
        Viewport ViewPort_BottomRight;
        Viewport ViewPort_BottomLeft;

        //Setup Enemy Spawn System
        SurvivalEnemySpawner enemySpawner = new SurvivalEnemySpawner();

        //Setup the level Loader
        public LoadLevel levelLoader = new LoadLevel();

        public Options _options;
        
        public Boolean _doPlay = false;
        public Boolean playing = false;
        public Boolean paused = false;
        public Boolean quit = false;
        public Boolean InOptions = false;

        //Drops
        private List<Rectangle> Drops = new List<Rectangle>();
        private List<String> DropType = new List<string>();
        private List<Texture2D> DropTexture = new List<Texture2D>();
        private int PowerUpLifeTime = 15000;
        private List<int> Drop_Life = new List<int>();
        private int SpeedBoostEffect = 0;
            int SpeedBoostSec = 0;
        private int InstaKillEfect = 0;
            int InstaKillSec = 0;
        private int DeathTouchEffect = 0;
            int DeathTouchSec = 0;
        private int DoublePointsEffect = 0;
            int DoublePointsSec = 0;
        private Texture2D speedBoost;
        private Texture2D instaKill;
        private Texture2D healthKick;
        private Texture2D deathTouch;
        private Texture2D doublePoints;
        private Texture2D MaxAmmo;
        private Texture2D Discount;
        private List<String> activePowerUps = new List<string>();


        private Boolean PurchasedDoor = false;
        private int Purch_Door_Delay = 0;

        SpawnEnemies spawnEnemies;

        ParticleEffectManager partEffManager = new ParticleEffectManager();
        SoundEffect doorPurchase;


        private SoundEffect JukeBoxStart;
        private SoundEffect JukeBoxOff;
        List<Song> songs = new List<Song>();
        int CurSong = 0;
        int Prev_song = 0;
        Boolean changedSong = false;

        SoundEffect matchStart;
        SoundEffect ambience;

        public FileInfo lb_file;

        private int OnPlayer = 0;
        public Boolean GameOver = false;
        public int TimeTillGOfade = 0;
        public int GOfade = 0;
        private Boolean newHighScore = false;
        public Boolean BackOut = false;
        private int[] scores = new int[5];
        public String level_name = "";
        private int HighScorePlace = -1;

        List<Zapper> zapper = new List<Zapper>();
        Texture2D Electricity;


        SoundEffect glassCrack;
        SoundEffect glassShatter;

        SoundEffect drop;

        //Button[] PC_PauseButton = new Button[3];
        Rectangle[] PC_PauseButton = new Rectangle[3];

        private List<Teleporter> onTeleporters = new List<Teleporter>();


        public Boolean CinemaMode = false;
        //private ChallengeSystem challengeSystem;

        public GameDirector()
        {
            
        }
        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            this.graphics = graphics;

            DefaultViewPort = graphics.Viewport;

            TopViewPort = DefaultViewPort;
            TopViewPort.Height = TopViewPort.Height / 2;

            BottomViewPort = DefaultViewPort;
            BottomViewPort.Height = BottomViewPort.Height / 2 - 1;
            BottomViewPort.Y = TopViewPort.Height + 1;

            ViewPort_BottomLeft = BottomViewPort;
            ViewPort_BottomLeft.Width = DefaultViewPort.Width / 2 - 1;

            ViewPort_BottomRight = ViewPort_BottomLeft;
            ViewPort_BottomRight.X = DefaultViewPort.Width / 2 + 1;

            ViewPort_TopLeft = TopViewPort;
            ViewPort_TopLeft.Width = DefaultViewPort.Width / 2 - 1;

            ViewPort_TopRight = ViewPort_TopLeft;
            ViewPort_TopRight.X = DefaultViewPort.Width / 2 + 1;

            camera = new Camera();//DefaultViewPort);
            camera_1 = new Camera();//TopViewPort);
            camera_1.Zoom = 0.35f;
            camera_2 = new Camera();//BottomViewPort);
            camera_2.Zoom = 0.35f;
            camera_3 = new Camera();//ViewPort_BottomLeft);
            camera_3.Zoom = 0.35f;
            camera_4 = new Camera();//ViewPort_BottomRight);
            camera_4.Zoom = 0.35f;

            font = content.Load<SpriteFont>("standardfont");
            Splitfont = content.Load<SpriteFont>("Fonts/SplitFont");
            GameInfoFont = content.Load<SpriteFont>("Fonts/GameInfo");
            secondaryFont = content.Load<SpriteFont>("Fonts/SecondaryFont");
            smallFont = content.Load<SpriteFont>("Fonts/SmallFont");

            square = content.Load<Texture2D>("Sprites/square");
            GrassTile = content.Load<Texture2D>("Sprites/GrassTile");
            speedBoost = content.Load<Texture2D>("Sprites/Syringe");
            instaKill = content.Load<Texture2D>("Sprites/Instakill");
            healthKick = content.Load<Texture2D>("Sprites/HealthKick");
            deathTouch = content.Load<Texture2D>("Sprites/DeathTouch");
            doublePoints = content.Load<Texture2D>("Sprites/DoublePoints");
            MaxAmmo = content.Load<Texture2D>("Sprites/MaxAmmo");
            Discount = content.Load<Texture2D>("Sprites/Discount");

            doorPurchase = content.Load<SoundEffect>("SoundFX/WeaponPurchaseSE");

            //Load Level loader
            levelLoader.Load(content);

            //Load players
            player1.Load(new Vector2(600, 60), content, PlayerIndex.One);
            player2.Load(new Vector2(670, 60), content, PlayerIndex.Two);
            player3.Load(new Vector2(740, 60), content, PlayerIndex.Three);
            player4.Load(new Vector2(810, 60), content, PlayerIndex.Four);

            spawnEnemies = new SpawnEnemies(new Vector2(600, 60));
            spawnEnemies.Load(content);


            JukeBoxStart = content.Load<SoundEffect>("SoundFX/JukeboxCoinDropSE");
            JukeBoxOff = content.Load<SoundEffect>("SoundFX/SFX_RecordScratch");
            songs.Add(content.Load<Song>("Music/Shadow Of A Doubt-Another Empire"));
            songs.Add(content.Load<Song>("Music/Shadow Of A Doubt-Bleach"));
            songs.Add(content.Load<Song>("Music/Shadow Of A Doubt-Bloody Mary"));
            songs.Add(content.Load<Song>("Music/Shadow Of A Doubt-In Sickness and Greed"));
            songs.Add(content.Load<Song>("Music/Shadow Of A Doubt-The Mirror"));
            for (int i = 0; i < songs.Count; i++)
            {
                MediaPlayer.Play(songs[i]);
                MediaPlayer.Pause();
            }

            matchStart = content.Load<SoundEffect>("SoundFX/SFX_MatchStart");
            ambience = content.Load<SoundEffect>("SoundFX/SFX_MenuAmbience");

            partEffManager.Load(content);

            Electricity = content.Load<Texture2D>("Sprites/Misc/ElectricStreak");


            glassCrack = content.Load<SoundEffect>("SoundFX/Misc/Glass Crack");
            glassShatter = content.Load<SoundEffect>("SoundFX/Misc/Glass Shatter");

            drop = content.Load<SoundEffect>("SoundFX/Misc/Drop");

            //challengeSystem = new ChallengeSystem();
            //challengeSystem.Load(content);

        }
        private int PlayersAdded = 0;
        public void Update(GameTime gameTime, PlayerSelector playerSelector, Options options, Boolean SinglePlayer)
        {
            SoundEffect.MasterVolume = (float)options.SoundEffectsVolume / 100f;
            if(!InOptions)
            _options = options;
            partEffManager.Update(gameTime);
            if (_doPlay && !playing)
            {
                //if (playerSelector.Player1_Screen == 0  &&  playerSelector.Add_Player1)
                //{
                //    players.Add(player1);
                //}
                //else if (playerSelector.Player2_Screen == 0 && playerSelector.Add_Player2)
                //{
                //    players.Add(player2);
                //}
                for (int i = 0; i < levelLoader.ZapperRect.Count; i++)
                {
                    zapper.Add(new Zapper(levelLoader.ZapperRect[i], levelLoader.ZapperChannel[i], levelLoader.tileManager.Zapper.texture, Electricity, levelLoader.ZapDuration[i]));
                }
                while (PlayersAdded < 4)
                {
                    player1.Class = playerSelector.playerClass[0];
                    player2.Class = playerSelector.playerClass[1];
                    player3.Class = playerSelector.playerClass[2];
                    player4.Class = playerSelector.playerClass[3];
                    if (playerSelector.Add_Player1 && playerSelector.Player1_Screen == PlayersAdded)
                    {
                        players.Add(player1);
                        playerSelector.Add_Player1 = false;
                    }
                    if (playerSelector.Add_Player2 && playerSelector.Player2_Screen == PlayersAdded)
                    {
                        players.Add(player2);
                        playerSelector.Add_Player2 = false;
                    }
                    if (playerSelector.Add_Player3 && playerSelector.Player3_Screen == PlayersAdded)
                    {
                        players.Add(player3);
                        playerSelector.Add_Player3 = false;
                    }
                    if (playerSelector.Add_Player4 && playerSelector.Player4_Screen == PlayersAdded)
                    {
                        players.Add(player4);
                        playerSelector.Add_Player4 = false;
                    }
                    PlayersAdded++;
                }
                if (levelLoader.PlayerSpawns.Count > 0)
                {
                    spawnEnemies.P_Spawn = levelLoader.PlayerSpawns[0];
                    for (int i = 0; i < players.Count; i++)
                    {
                        //players[i].Class = playerSelector.playerClass[i];
                        players[i].UpdateToPos(levelLoader.PlayerSpawns[i]);
                        players[i].StartingPos = levelLoader.PlayerSpawns[i];
                        players[i].PlayerCount = players.Count;
                    }
                    if (SinglePlayer)
                        players[0].PC = !options.ControllerEnabled;
                    //levelLoader.Level_Load(playerSelector.CurrentLevel, playerSelector.CurrentLevelParts);
                    matchStart.Play();
               //     ambience.Play();
                    playing = true;
                }
            }
            if (!playerSelector.start)
            {
                playerSelector.Update(gameTime, SinglePlayer);
            }
            else
            {
                //challengeSystem.Update(gameTime);
                //if (challengeSystem.playingChallenge)
                //{
                //    if (challengeSystem.CurChallenge == "")
                //    {

                //    }
                //}
                if (!paused)
                    for (int p = 0; p < players.Count; p++)
                        if (SinglePlayer)
                            players[p].PC = !options.ControllerEnabled;
               // enemySpawner.Update(gameTime, players, levelLoader.EnemySpawnLoc, levelLoader.pathfindNode, new Vector2(601,61), PurchasedDoor);
                spawnEnemies.Update(gameTime, players, levelLoader.pathfindNode, PurchasedDoor, levelLoader.EnemySpawnLoc, GameOver);
                for (int i = 0; i < zapper.Count; i++)
                {
                    if (levelLoader.PurchasedChannelTraps.Contains(zapper[i].Channel))
                        zapper[i].active = true;
                    else
                        zapper[i].active = false;
                    zapper[i].Update(spawnEnemies.enemy, gameTime);
                    if (zapper[i].Fire)
                    {
                        Projectile temp = new Projectile(zapper[i].ShockPos, 0, Vector2.Zero, 0, 1, 0, zapper[i].Damage, 0, square, "trap", null, 0, 0);
                        bullets.Add(temp);
                        zapper[i].Fire = false;
                    }
                    if (zapper[i].CurTime >= zapper[i].Time)
                    {
                        if (levelLoader.PurchasedChannelTraps.Contains(zapper[i].Channel))
                            levelLoader.PurchasedChannelTraps.RemoveAt(levelLoader.PurchasedChannelTraps.IndexOf(zapper[i].Channel));
                        zapper[i].active = false;
                        zapper[i].CurTime = 0;
                    }
                }
                for (int p = 0; p < players.Count; p++)
                {
                   
                    //Sentry Gun Bullets
                    if (players[p].sentry.Fired)
                    {
                        Player P = players[p];
                        Projectile temp = new Projectile(P.sentry.BulletPos, P.sentry.BulletRotation, P.sentry.BulletDirection, 0, P.sentry.BulletLife, p, P.sentry.BulletDamage, P.sentry.BulletSpeed, P.sentry.BulletTexture, P.sentry.BulletType, null, 1, 2);
                        bullets.Add(temp);
                        players[p].sentry.Fired = false;
                    }
                    for (int s = 0; s < players[p].securityPlate.Count; s++)
                    {
                        if (players[p].securityPlate[s].fire)
                        {
                            Player P = players[p];
                            Projectile temp = new Projectile(P.securityPlate[s].pos, P.securityPlate[s].Rotation, P.securityPlate[s].fireDirection1, 0, P.securityPlate[s].Life, p, P.securityPlate[s].Damage, P.securityPlate[s].Speed, P.securityPlateBulletTexture, "trap", null, 0, 0);
                            Projectile temp2 = new Projectile(P.securityPlate[s].pos, P.securityPlate[s].Rotation + 180, P.securityPlate[s].fireDirection2, 0, P.securityPlate[s].Life, p, P.securityPlate[s].Damage, P.securityPlate[s].Speed, P.securityPlateBulletTexture, "trap", null, 0, 0);
                            bullets.Add(temp);
                            bullets.Add(temp2);
                            players[p].securityPlate[s].fire = false;
                        }
                    }
                    if (players[p].ammoBag.PlaceBag)
                    {
                        PlayersBag.Add(p);
                        AmmoBagRect.Add(players[p].ammoBag.BagRect);
                        players[p].ammoBag.PlaceBag = false;
                    }
                    for (int i = 0; i < AmmoBagRect.Count; i++)
                    {
                        if (!players[PlayersBag[i]].ammoBag.active)
                        {
                            PlayersBag.RemoveAt(i);
                            AmmoBagRect.RemoveAt(i);
                            break;
                        }
                        if (players[p].PlayerRect.Intersects(AmmoBagRect[i]))
                        {
                            if (players[p].weaponManager.CurentWeapon.Ammo < players[p].weaponManager.CurentWeapon.MaxAmmo && players[p].weaponManager.SecondaryWeapon == null)
                            {
                                players[p].weaponManager.BoughtAmmo(players[p].weaponManager.CurentWeapon.WeaponName);
                                players[p].weaponManager.BoughtAmmo(players[p].weaponManager.SecondaryWeaponName);
                                PlayersBag.RemoveAt(i);
                                AmmoBagRect.RemoveAt(i);
                                break;
                            }
                            else if (players[p].weaponManager.SecondaryWeapon != null)
                            {
                                if (players[p].weaponManager.CurentWeapon.Ammo < players[p].weaponManager.CurentWeapon.MaxAmmo ||
                                    players[p].weaponManager.SecondaryWeapon.Ammo < players[p].weaponManager.SecondaryWeapon.MaxAmmo)
                                {
                                    players[p].weaponManager.BoughtAmmo(players[p].weaponManager.CurentWeapon.WeaponName);
                                    players[p].weaponManager.BoughtAmmo(players[p].weaponManager.SecondaryWeaponName);
                                    PlayersBag.RemoveAt(i);
                                    AmmoBagRect.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                    }
                    if (players[p].Fired)
                    {
                        if (players[p].BulletType != "ShotGun" && players[p].BulletType != "Auto-Shotty" && players[p].BulletType != "Dual")
                        {
                            Player P = players[p];
                            Projectile temp = new Projectile(P.BulletPos, P.BulletRotation, P.BulletDirection, 0, P.BulletLife, p, P.BulletDamage, P.BulletSpeed, P.BulletTexture, P.BulletType, P.BulletHitSound, P.HitPoints, P.KillPoints);
                            bullets.Add(temp);
                        }
                        else if (players[p].BulletType == "ShotGun" || players[p].BulletType == "Auto-Shotty")
                        {//#fire
                            FireShotGun(players[p], p, players[p].BulletType);
                        }
                        else if (players[p].BulletType == "Dual")
                        {//#fire
                            FireDual(players[p], p, players[p].BulletType);
                        }
                        //players[p].
                        players[p].Fired = false;
                    }
                    if (players[p].weaponManager.updateText)
                    {
                        PlayerPopupText.Add(new WorldText(new Vector2((players[p].pos.X) - (font.MeasureString("" + players[p].weaponManager.CurentWeapon.WeaponName).X / 2), (players[p].pos.Y - 50)), 0, "" + players[p].weaponManager.CurentWeapon.WeaponName));
                        PlayerPopupText[PlayerPopupText.Count - 1].PlayerIndex = p;
                        players[p].weaponManager.updateText = false;
                    }
                    players[p].Update(gameTime, levelLoader.LevelCollisionObjects, levelLoader.LevelPiecesCollisionObject, levelLoader.Tiles, levelLoader.StoreRect, levelLoader.WeaponRects, levelLoader.WeaponType, levelLoader.WeaponCost, levelLoader.AmmoCost, spawnEnemies.Wave
                        ,spawnEnemies.enemy);
                    if (players[p].dead && spawnEnemies.newWave  &&  players.Count > 1)//!SinglePlayer)
                        players[p].ResetPlayer();
                    if (spawnEnemies.newWave && players[p].Health < players[p].MaxHealth)
                    {
                        //players[p].Health = players[p].MaxHealth;
                        //#phealth
                        if (players[p].Health < (players[p].MaxHealth / 2) - 60)
                            players[p].Health = players[p].MaxHealth / 2;
                        else
                            players[p].Health += 60;
                        if (players[p].Health >= players[p].MaxHealth)
                            players[p].Health = players[p].MaxHealth;
                        //players[p].CurinvulnerabilityTime = players[p].InvulnerabilityTime;
                    }
                    if (!players[p].GasMaskOn && spawnEnemies.Mutate && spawnEnemies.FinishedMutating && players[p].BreathTick <= 0  &&  !players[p].GasMaskPerk)
                        players[p].Health--;//Health Decrases too fast without gas mask!(#GASMASK)


                    StimGasPos[p] = players[p].stimGas.pos;
                    StimGasActive[p] = players[p].stimGas.active;
                    for (int s = 0; s < StimGasPos.Length; s++)
                    {
                        if (Vector2.Distance(players[p].pos, StimGasPos[s]) <= players[p].stimGas.HealDist && players[p].stimGas.CurrentHealtTick >= players[p].stimGas.HealTick
                            &&  StimGasActive[s]  &&  players[p].Health < players[p].MaxHealth  &&  players[p].Health > 0)
                        {
                            players[p].Health += players[p].stimGas.HealthGain;
                            players[p].stimGas.CurrentHealtTick = 0;
                        }
                    }
                    DamageBoostPos[p] = players[p].damageBoost.Pos;
                    DamageBoostActive[p] = players[p].damageBoost.active;
                    for (int d = 0; d < DamageBoostPos.Length; d++)
                    {
                        if (Vector2.Distance(players[p].pos, DamageBoostPos[d]) <= players[p].damageBoost.Range && DamageBoostActive[d])
                        {
                            players[p].damageBoost.underEffect = true;
                            d = 0;
                            break;
                        }
                        else
                            players[p].damageBoost.underEffect = false;
                    }

                    for (int d = 0; d < Drops.Count; d++)//Update Drops
                    {
                        if (players[p].PlayerRect.Intersects(Drops[d]))
                        {
                            if (!activePowerUps.Contains(DropType[d]) && DropType[d] != "Health Kick")
                                activePowerUps.Add(DropType[d]);

                            DropText.Add(new WorldText(new Vector2((Drops[d].X + Drops[d].Width / 2) - (font.MeasureString(DropType[d]).X / 2), (Drops[d].Y)), 0, DropType[d]));

                            if (DropType[d] == "Speed Boost")
                            {
                                for (int p2 = 0; p2 < players.Count; p2++)
                                {
                                    players[p2].PlayerSpeed = 17;
                                    players[p2].HasSpeedBost = true;
                                }
                                Drops.RemoveAt(d);
                                DropType.RemoveAt(d);
                                Drop_Life.RemoveAt(d);
                                DropTexture.RemoveAt(d);
                                SpeedBoostEffect = 20000;
                                break;
                            }
                            if (DropType[d] == "Insta Kill")
                            {
                                Drops.RemoveAt(d);
                                DropType.RemoveAt(d);
                                Drop_Life.RemoveAt(d);
                                DropTexture.RemoveAt(d);
                                InstaKillEfect = 20000;
                                break;
                            }
                            if (DropType[d] == "Health Kick")
                            {
                                Drops.RemoveAt(d);
                                DropType.RemoveAt(d);
                                Drop_Life.RemoveAt(d);
                                DropTexture.RemoveAt(d);
                                for (int i = 0; i < players.Count; i++)
                                {
                                   // if (players[i].Health <= 0)
                                        players[i].CurinvulnerabilityTime = players[i].InvulnerabilityTime;
                                  //  if (players[i].Health + 50 <= players[i].MaxHealth)
                                 //       players[i].Health += 50;
                                  //  else
                                        players[i].Health = players[i].MaxHealth;
                                }
                                break;
                            }
                            if (DropType[d] == "Death Touch")
                            {
                                Drops.RemoveAt(d);
                                DropType.RemoveAt(d);
                                Drop_Life.RemoveAt(d);
                                DropTexture.RemoveAt(d);
                                DeathTouchEffect = 15000;
                                break;
                            }
                            if (DropType[d] == "Double Points")
                            {
                                Drops.RemoveAt(d);
                                DropType.RemoveAt(d);
                                Drop_Life.RemoveAt(d);
                                DropTexture.RemoveAt(d);
                                DoublePointsEffect = 20000;
                                break;
                            }
                            if (DropType[d] == "Max Ammo")
                            {
                                Drops.RemoveAt(d);
                                DropType.RemoveAt(d);
                                Drop_Life.RemoveAt(d);
                                DropTexture.RemoveAt(d);
                                for (int i = 0; i < players.Count; i++)
                                {
                                    players[i].weaponManager.BoughtAmmo(players[i].weaponManager.CurentWeapon.WeaponName);
                                    players[i].weaponManager.BoughtAmmo(players[i].weaponManager.SecondaryWeaponName);
                                }
                                break;
                            }
                            if (DropType[d] == "Discount")
                            {
                                Drops.RemoveAt(d);
                                DropType.RemoveAt(d);
                                Drop_Life.RemoveAt(d);
                                DropTexture.RemoveAt(d);
                                for (int i = 0; i < players.Count; i++)
                                {
                                    players[i].StoreDiscount += 2;
                                }
                                break;
                            }
                        }
                    }
                }
                for (int d = 0; d < Drops.Count; d++)
                {
                    Drop_Life[d] += gameTime.ElapsedGameTime.Milliseconds;
                    if (Drop_Life[d] > PowerUpLifeTime)
                    {
                        Drops.RemoveAt(d);
                        DropType.RemoveAt(d);
                        Drop_Life.RemoveAt(d);
                        DropTexture.RemoveAt(d);
                        break;
                    }

                }
                if (SpeedBoostEffect > 0)
                {
                    SpeedBoostEffect -= gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    if (SpeedBoostEffect == 0)
                    {
                        for (int p2 = 0; p2 < players.Count; p2++)
                        {
                            players[p2].PlayerSpeed = players[p2].NormalSpeed;
                            players[p2].HasSpeedBost = false;
                        }
                        for (int i = 0; i < activePowerUps.Count; i++)
                        {
                            if (activePowerUps[i] == "Speed Boost")
                                activePowerUps.RemoveAt(i);
                        }
                    }
                    SpeedBoostEffect = -1;
                }
                SpeedBoostSec = SpeedBoostEffect / 100;
                /////////////
                if (InstaKillEfect > 0)
                {
                    InstaKillEfect -= gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    for (int i = 0; i < activePowerUps.Count; i++)
                    {
                        if (activePowerUps[i] == "Insta Kill")
                            activePowerUps.RemoveAt(i);
                    }
                    InstaKillEfect = -1;
                }
                InstaKillSec = InstaKillEfect / 100;
                /////////////////
                if (DeathTouchEffect > 0)
                {
                    DeathTouchEffect -= gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    for (int i = 0; i < activePowerUps.Count; i++)
                    {
                        if (activePowerUps[i] == "Death Touch")
                            activePowerUps.RemoveAt(i);
                    }
                    DeathTouchEffect = -1;
                }
                DeathTouchSec = DeathTouchEffect / 100;

                if (DoublePointsEffect > 0)
                {
                    DoublePointsEffect -= gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    for (int i = 0; i < activePowerUps.Count; i++)
                    {
                        if (activePowerUps[i] == "Double Points")
                            activePowerUps.RemoveAt(i);
                    }
                    DoublePointsEffect = -1;
                }
                DoublePointsSec = DoublePointsEffect / 100;

                //Update Bullets and check to see if they should be removed
                BulletLevelUpdate();
                PurchaseUpdate();
                PurchaseItemUpdate();
                PurchaseTeleporterUpdate(gameTime);
                BulletEnemyUpdate(gameTime);
                BulletGlassUpdate();
                MusicPlayerUpdate(gameTime);
                //camera.Update(gameTime, new Vector2(players[0].PlayerRect.X + players[0].PlayerRect.Width / 2, players[0].PlayerRect.Y + players[0].PlayerRect.Height / 2), graphics);
               // camera_3.Update(gameTime, new Vector2(players[0].PlayerRect.X + players[0].PlayerRect.Width / 2, players[0].PlayerRect.Y + players[0].PlayerRect.Height / 2), graphics);
                if (players.Count == 1)
                {
                    camera.Update(gameTime, players[0].CamCen, graphics);
                }
                if (players.Count == 2)
                {
                    camera_1.Update(gameTime, players[0].CamCen, graphics);
                    camera_2.Update(gameTime, players[1].CamCen, graphics);
                }
                if (players.Count == 3)
                {
                    camera_1.Update(gameTime, players[0].CamCen, graphics);
                    camera_3.Update(gameTime, players[1].CamCen, graphics);
                    camera_4.Update(gameTime, players[2].CamCen, graphics);
                }
                if (players.Count == 4)
                {
                    camera_1.Update(gameTime, players[0].CamCen, graphics);
                    camera_2.Update(gameTime, players[1].CamCen, graphics);
                    camera_3.Update(gameTime, players[2].CamCen, graphics);
                    camera_4.Update(gameTime, players[3].CamCen, graphics);
                }
            }


            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].dead)
                {
                    OnPlayer = 0;
                    break;
                }else
                    OnPlayer++;
                if (OnPlayer >= players.Count - 1 && players[players.Count - 1].dead)
                    GameOver = true;
            }
            //if (GameOver)
            //{
            //    UpdateHighScores();
            //    TimeTillGOfade += gameTime.ElapsedGameTime.Milliseconds;
            //    if (TimeTillGOfade >= 3000)
            //        GOfade += 2;
            //    paused = false;
            //    BackOut = true;
            //    for (int i = 0; i < players.Count; i++)
            //    {
            //        if (players[i].pInput.BackKey || players[i].kInput.GUI_Back)
            //        {
            //            quit = true;
            //            TimeTillGOfade = 0;
            //            GOfade = 0;
            //            paused = false;
            //            GameOver = false;
            //            newHighScore = false;
            //        }
            //    }
            //}
        }
        Boolean read = false;
        Boolean write = false;
        public void UpdateHighScores()
        {
            int Score1 = 0;
            int Score2 = 0;
            int Score3 = 0;
            int Score4 = 0;
            int Score5 = 0;

            if (!read)
            {
                FileStream fs = new FileStream(lb_file.FullName, FileMode.OpenOrCreate);
                StreamReader sr = new StreamReader(fs);

                int.TryParse(sr.ReadLine(), out Score1);
                int.TryParse(sr.ReadLine(), out Score2);
                int.TryParse(sr.ReadLine(), out Score3);
                int.TryParse(sr.ReadLine(), out Score4);
                int.TryParse(sr.ReadLine(), out Score5);

                int score = spawnEnemies.Wave;

                if (score > Score1)
                {
                    Score5 = Score4;
                    Score4 = Score3;
                    Score3 = Score2;
                    Score2 = Score1;
                    Score1 = score;
                    newHighScore = true;
                    HighScorePlace = 0;
                }
                else if (score > Score2)
                {
                    Score5 = Score4;
                    Score4 = Score3;
                    Score3 = Score2;
                    Score2 = score;
                    newHighScore = true;
                    HighScorePlace = 1;
                }
                else if (score > Score3)
                {
                    Score5 = Score4;
                    Score4 = Score3;
                    Score3 = score;
                    newHighScore = true;
                    HighScorePlace = 2;
                }
                else if (score > Score4)
                {
                    Score5 = Score4;
                    Score4 = score;
                    newHighScore = true;
                    HighScorePlace = 3;
                }
                else if (score > Score5)
                {
                    Score5 = score;
                    newHighScore = true;
                    HighScorePlace = 4;
                }

                scores[0] = Score1;
                scores[1] = Score2;
                scores[2] = Score3;
                scores[3] = Score4;
                scores[4] = Score5;

                sr.Close();
                fs.Close();



                File.WriteAllText(lb_file.FullName, String.Empty);
                read = true;
            }

            if (!write)
            {
                FileStream fs2 = new FileStream(lb_file.FullName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs2);

                sw.WriteLine(Score1); sw.WriteLine(Score2); sw.WriteLine(Score3); sw.WriteLine(Score4); sw.WriteLine(Score5);

                sw.Close();
                fs2.Close();
                write = true;
            }
            //System.Console.WriteLine("" + Score1 + "    " + Score2 + "    " + Score3 + "    " + Score4 + "    " + Score5);
        }


        private int music_CurrentDurationPlayed = 0;
        private int music_songDuration;
        private int Cur_StartDur = 0;
        private Boolean IsOnMusicSpace(int p)
        {
            for(int i = 0; i < levelLoader.MusicPlayerRect.Count; i++)
            {
                if(players[p].PlayerRect.Intersects(levelLoader.MusicPlayerRect[i]))
                    return true;
            }
            return false;
        }
        private void MusicPlayerUpdate(GameTime gameTime)
        {
            if(levelLoader.MusicPlayerRect.Count > 0)// levelLoader.MusicPlayer.Width > 0  &&  levelLoader.MusicPlayer.Height > 0)
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].dead)
                {
                    //if (players[i].PlayerRect.Intersects(levelLoader.MusicPlayer) && !players[i].dead)
                    //    players[i].OnMusicSpace = true;
                    //else
                    //    players[i].OnMusicSpace = false;
                    players[i].OnMusicSpace = IsOnMusicSpace(i);
                    if (players[i].OnMusicSpace && (players[i].pInput.UseKey || players[i].kInput.UseKey) && !changedSong)
                    {
                        if (CurSong == 0)
                        {
                            CurSong = Prev_song + 1;
                            if (CurSong > songs.Count)
                            {
                                CurSong = 1;
                            }
                            changedSong = true;
                        }
                        else
                        {
                            Prev_song = CurSong;
                            CurSong = 0;
                            if (music_CurrentDurationPlayed >= 500)
                            JukeBoxOff.Play();
                        }
                    }
                }
            }
            
            if (changedSong)
            {

                if (Cur_StartDur == 0)
                    JukeBoxStart.Play();
                if (Cur_StartDur <= JukeBoxStart.Duration.Milliseconds + 100)
                    Cur_StartDur += gameTime.ElapsedGameTime.Milliseconds;
                if (Cur_StartDur >= JukeBoxStart.Duration.Milliseconds + 100)
                {
                    MediaPlayer.Play(songs[CurSong - 1]);
                    music_songDuration = (int)MediaPlayer.Queue.ActiveSong.Duration.TotalMilliseconds;
                    music_CurrentDurationPlayed = 0;
                    Cur_StartDur = 0;
                    changedSong = false;
                }          
            }
            if (CurSong != 0  &&  !changedSong)
            {
                if (music_CurrentDurationPlayed < music_songDuration)
                    music_CurrentDurationPlayed += gameTime.ElapsedGameTime.Milliseconds;
                if (music_CurrentDurationPlayed >= music_songDuration)
                {
                    Prev_song = CurSong;
                    CurSong = 0;
                }
            }
            if (CurSong == 0)
                MediaPlayer.Stop();
        }
        private void BulletLevelUpdate()
        {
            for (int a = 0; a < bullets.Count; a++)//Bullet Loop
            {
                bullets[a].Update();
                for (int i = 0; i < levelLoader.LevelCollisionObjects.Count; i++)
                {
                    if (i < levelLoader.LevelCollisionObjects.Count && a < bullets.Count)
                    {
                        if (bullets[a].Rect.Intersects(levelLoader.LevelCollisionObjects[i]))
                        {
                            //if (Vector2.Distance(AB_Pos[a], new Vector2(levelLoader.LevelCollisionObjects[i].X + levelLoader.LevelCollisionObjects[i].Width / 2,
                            //    levelLoader.LevelCollisionObjects[i].Y + levelLoader.LevelCollisionObjects[i].Height / 2)) < /*(missile.Height * 2) + (missile.Height / 2)*/ AB_Texture[a].Width / 2 + levelLoader.LevelCollisionObjects[i].Width / 2)
                            //{
                            if (bullets[a].type == "Rocket")
                            {
                                partEffManager.RocketExplosion.InstantiateEffect(bullets[a].Pos);
                                int Area = 155;
                                SplashDamage(Area, bullets[a].Damage, bullets[a].Pos, bullets[a].PlayerFired, -1);
                            }
                            if (bullets[a].type == "Explosive")
                            {
                                partEffManager.LandMineExplosion.InstantiateEffect(bullets[a].Pos);
                                int Area = 155;
                                SplashDamage(Area,  bullets[a].Damage, bullets[a].Pos, bullets[a].PlayerFired, -1);
                            }
                            if (bullets[a].hitSound != null)
                                bullets[a].hitSound.Play();

                            bullets.RemoveAt(a);
                            break;
                            //  }
                        }
                    }
                }
                for (int b = 0; b < levelLoader.LevelPiecesCollisionObject.Count; b++)
                {
                    if (levelLoader.DoorChannel[b] != -2)
                    {
                        if (b < levelLoader.LevelPiecesCollisionObject.Count && a < bullets.Count)
                        {
                            if (bullets[a].Rect.Intersects(levelLoader.LevelPiecesCollisionObject[b]))
                            {
                                //if (Vector2.Distance(AB_Pos[a], new Vector2(levelLoader.LevelPiecesCollisionObject[b].X + levelLoader.LevelPiecesCollisionObject[b].Width / 2,
                                //    levelLoader.LevelPiecesCollisionObject[b].Y + levelLoader.LevelPiecesCollisionObject[b].Height / 2)) < AB_Texture[a].Width / 2 + levelLoader.LevelPiecesCollisionObject[b].Width / 2)
                                //{
                                if (bullets[a].type == "Rocket")
                                {
                                    partEffManager.RocketExplosion.InstantiateEffect(bullets[a].Pos);
                                    int Area = 155;
                                    SplashDamage(Area, bullets[a].Damage, bullets[a].Pos, bullets[a].PlayerFired, -1);
                                }
                                if (bullets[a].type == "Explosive")
                                {
                                    partEffManager.LandMineExplosion.InstantiateEffect(bullets[a].Pos);
                                    int Area = 155;
                                    SplashDamage(Area, bullets[a].Damage, bullets[a].Pos, bullets[a].PlayerFired, -1);
                                }//#added splashdamage
                                if (bullets[a].hitSound != null)
                                    bullets[a].hitSound.Play();

                                bullets.RemoveAt(a);
                                break;
                                //   }
                            }
                        }
                    }
                }
                if (a < bullets.Count)
                    if (bullets[a].Life > bullets[a].LifeTime)
                        bullets.RemoveAt(a);

            }
        }
        private void PurchaseTeleporterUpdate(GameTime gameTime)
        {
            for (int i = 0; i < onTeleporters.Count; i++)
                onTeleporters[i].Update(gameTime);
            for (int p = 0; p < players.Count; p++)
            {
                if (!players[p].dead)
                {
                    
                    for (int i = 0; i < onTeleporters.Count; i++)
                    {
                        if (players[p].PlayerRect.Intersects(onTeleporters[i].rect))
                            if (players[p].pInput.UseKey || players[p].kInput.UseKey)
                            {
                                if (onTeleporters[i].sends  &&  players[p].Score >= onTeleporters[i].Price  && onTeleporters[i].CurCoolDownTime <= 0)
                                {
                                    List<Teleporter> avaliableRecievers = new List<Teleporter>();
                                    for (int a = 0; a < onTeleporters.Count; a++)
                                    {
                                        if (!onTeleporters[a].sends && onTeleporters[a].Channel == onTeleporters[i].Channel)
                                            avaliableRecievers.Add(onTeleporters[a]);
                                    }
                                    if (avaliableRecievers.Count > 0)
                                    {
                                        Random random = new Random();
                                        int _t = random.Next(0, avaliableRecievers.Count);
                                        for (int a = 0; a < players.Count; a++)
                                        {
                                            if (players[a].PlayerRect.Intersects(onTeleporters[i].rect))
                                            {
                                                players[a].UpdateToPos(new Vector2(avaliableRecievers[_t].rect.X + avaliableRecievers[_t].rect.Width / 2, avaliableRecievers[_t].rect.Y + avaliableRecievers[_t].rect.Height / 2));
                                                players[a].TeleportFade = 255;
                                            }
                                        }
                                        //players[p].UpdateToPos(new Vector2(avaliableRecievers[_t].rect.X + avaliableRecievers[_t].rect.Width / 2, avaliableRecievers[_t].rect.Y + avaliableRecievers[_t].rect.Height / 2));
                                        players[p].Score -= onTeleporters[i].Price;
                                        onTeleporters[i].CurCoolDownTime = onTeleporters[i].CoolDownTime;
                                        //players[p].TeleportFade = 255;
                                        players[p].TeleportSound.Play();
                                    }
                                }
                            }
                    }
                    for (int i = 0; i < levelLoader.teleporter.Count; i++)
                    {
                        if (players[p].PlayerRect.Intersects(levelLoader.teleporter[i].rect))
                            if (players[p].pInput.UseKey || players[p].kInput.UseKey)
                            {
                                if (!levelLoader.teleporter[i].on)
                                {
                                    levelLoader.teleporter[i].on = true;
                                    if(levelLoader.teleporter[i].sends)
                                        levelLoader.teleporter[i].texture = levelLoader.tileManager.Teleporter_SenderOn;
                                    else
                                        levelLoader.teleporter[i].texture = levelLoader.tileManager.Teleporter_ReceiverOn;
                                    onTeleporters.Add(levelLoader.teleporter[i]);
                                }
                            }
                    }
                }
            }
        }
        private void PurchaseItemUpdate()
        {
            for (int p = 0; p < players.Count; p++)
            {
                if (!players[p].dead)
                {
                    for(int i = 0; i < levelLoader.ItemRect.Count; i++)
                    {
                        if (players[p].PlayerRect.Intersects(levelLoader.ItemRect[i]))
                        {
                            if ((players[p].pInput.UseKey || players[p].kInput.UseKey) && players[p].Score >= levelLoader.ItemPrice[i] && ((players[p].Item == "" || players[p].Item != levelLoader.ItemName[i]) || ((players[p].Item2 == "" || players[p].Item2 != levelLoader.ItemName[i]) && players[p].HasExtraSlot)))
                            {
                                if (players[p].HasExtraSlot)
                                    players[p].Item2 = players[p].Item;
                                players[p].Item = levelLoader.ItemName[i];
                                players[p].Score -= levelLoader.ItemPrice[i];
                                partEffManager.Purchase.InstantiateEffect(players[p].pos);
                                doorPurchase.Play();
                            }
                        }
                    }
                }
            }
        }
        private void PurchaseUpdate()
        {
            for (int p = 0; p < players.Count; p++)
            {
                if (!players[p].dead)
                {
                    for (int a = 0; a < levelLoader.LevelPiecesCollisionObject.Count; a++)
                    {
                        if (Vector2.Distance(players[p].pos, new Vector2(levelLoader.LevelPiecesCollisionObject[a].X + levelLoader.LevelPiecesCollisionObject[a].Width / 2,
                        levelLoader.LevelPiecesCollisionObject[a].Y + levelLoader.LevelPiecesCollisionObject[a].Height / 2)) < levelLoader.InteractionDist)
                        {

                            if ((players[p].pInput.UseKey || players[p].kInput.UseKey) && players[p].Score >= levelLoader.DoorPrices[a] && levelLoader.DoorChannel[a] == -1)
                            {
                                partEffManager.Purchase.InstantiateEffect(players[p].pos);
                                levelLoader.LevelPiecesCollisionObject.RemoveAt(a);
                                levelLoader.pathfindNode[levelLoader.DoorIndex[a]].walkable = true;
                                levelLoader.DoorIndex.RemoveAt(a);
                                levelLoader.DoorChannel.RemoveAt(a);
                                levelLoader.DoorHealth.RemoveAt(a);
                                players[p].Score -= levelLoader.DoorPrices[a];
                                doorPurchase.Play();
                                levelLoader.DoorPrices.RemoveAt(a);
                                PurchasedDoor = true;
                                break;
                            }

                        }

                       // for (int b = 0; b < levelLoader.PurchasedChannel.Count; b++)
                       //{

                            if (levelLoader.PurchasedChannel.Contains(levelLoader.DoorChannel[a]))//  && PurchasedChannel >= 0)
                            {
                              //  partEffManager.Purchase.InstantiateEffect(players[p].pos);//#PFglitch
                                levelLoader.LevelPiecesCollisionObject.RemoveAt(a);
                                levelLoader.pathfindNode[levelLoader.DoorIndex[a]].walkable = true;
                                levelLoader.DoorIndex.RemoveAt(a);
                                levelLoader.DoorChannel.RemoveAt(a);
                                levelLoader.DoorPrices.RemoveAt(a);
                                levelLoader.DoorHealth.RemoveAt(a);
                                //doorPurchase.Play();
                                PurchasedDoor = true;
                                break;
                            }
                       // }
                    }
                    for (int a = 0; a < levelLoader.DoorSwitches.Count; a++)
                    {
                        if (Vector2.Distance(players[p].pos, new Vector2(levelLoader.DoorSwitches[a].X + levelLoader.DoorSwitches[a].Width / 2,
                        levelLoader.DoorSwitches[a].Y + levelLoader.DoorSwitches[a].Height / 2)) < levelLoader.InteractionDist)
                        {
                            if ((players[p].pInput.UseKey || players[p].kInput.UseKey) && players[p].Score >= levelLoader.SwitchPrice[a] && !levelLoader.PurchasedChannelTraps.Contains(levelLoader.SwitchChannel[a]))
                            {
                                //for (int i = 0; i < levelLoader.DoorChannel.Count; i++)
                                //{
                                //    if(levelLoader.DoorChannel[a] 
                                //}
                                //PurchasedChannel = levelLoader.SwitchChannel[a];
                                if (!levelLoader.PurchasedChannel.Contains(levelLoader.SwitchChannel[a]))
                                    levelLoader.PurchasedChannel.Add(levelLoader.SwitchChannel[a]);
                                if (!levelLoader.PurchasedChannelTraps.Contains(levelLoader.SwitchChannel[a]))
                                    levelLoader.PurchasedChannelTraps.Add(levelLoader.SwitchChannel[a]);

                                players[p].Score -= levelLoader.SwitchPrice[a];
                                partEffManager.Purchase.InstantiateEffect(players[p].pos);
                                doorPurchase.Play();
                            }
                        }
                    }
                }
                if (players[p].PurchasedWeapon)
                {
                    partEffManager.Purchase.InstantiateEffect(players[p].pos);
                    players[p].PurchasedWeapon = false;
                }
            }
        }
        private void BulletGlassUpdate()
        {
            for (int i = 0; i < levelLoader.LevelPiecesCollisionObject.Count; i++)
            {
                if (levelLoader.DoorChannel[i] == -2)
                {
                    for (int a = 0; a < bullets.Count; a++)
                    {
                        if (bullets[a].Rect.Intersects(levelLoader.LevelPiecesCollisionObject[i]))
                        {
                            int damage = bullets[a].Damage;
                            if(players[bullets[a].PlayerFired].damageBoost.underEffect)
                                damage = (int)(damage * players[bullets[a].PlayerFired].damageBoost.DamageMultiplier);
                            if (levelLoader.DoorHealth[i] - damage <= 0)
                            {
                                glassShatter.Play();
                                levelLoader.LevelPiecesCollisionObject.RemoveAt(i);
                                levelLoader.pathfindNode[levelLoader.DoorIndex[i]].walkable = true;
                                levelLoader.DoorIndex.RemoveAt(i);
                                levelLoader.DoorChannel.RemoveAt(i);
                                partEffManager.GlassShatter.InstantiateEffect(bullets[a].Pos);
                                levelLoader.DoorPrices.RemoveAt(i);
                                levelLoader.DoorHealth.RemoveAt(i);
                                PurchasedDoor = true;


                                if (bullets[a].type == "Rocket")
                                {
                                    partEffManager.RocketExplosion.InstantiateEffect(bullets[a].Pos);
                                    int Area = 155;
                                    SplashDamage(Area, bullets[a].Damage, bullets[a].Pos, bullets[a].PlayerFired, -1);
                                }
                                if (bullets[a].type == "Explosive")
                                {
                                    partEffManager.LandMineExplosion.InstantiateEffect(bullets[a].Pos);
                                    int Area = 155;
                                    SplashDamage(Area, bullets[a].Damage, bullets[a].Pos, bullets[a].PlayerFired, -1);
                                }
                                if (bullets[a].hitSound != null)
                                    bullets[a].hitSound.Play();
                                bullets.RemoveAt(a);
                                break;
                            }
                            else
                            {
                                glassCrack.Play();
                                partEffManager.GlassShatter.InstantiateEffect(bullets[a].Pos);
                                levelLoader.DoorHealth[i] -= damage;
                                if (bullets[a].type == "Rocket")
                                {
                                    partEffManager.RocketExplosion.InstantiateEffect(bullets[a].Pos);
                                    int Area = 155;
                                    SplashDamage(Area, bullets[a].Damage, bullets[a].Pos, bullets[a].PlayerFired, -1);
                                }
                                if (bullets[a].type == "Explosive")
                                {
                                    partEffManager.LandMineExplosion.InstantiateEffect(bullets[a].Pos);
                                    int Area = 155;
                                    SplashDamage(Area, bullets[a].Damage, bullets[a].Pos, bullets[a].PlayerFired, -1);
                                }
                                if (bullets[a].hitSound != null)
                                    bullets[a].hitSound.Play();
                                bullets.RemoveAt(a);
                                break;
                            }
                        }
                    }
                }
            }
        }
        private void BulletEnemyUpdate(GameTime gameTime)
        {

            if (spawnEnemies.newWave && spawnEnemies.ColIndexToRemove.Count > 0)
            {
                spawnEnemies.ColIndexToRemove.Reverse();
                while (spawnEnemies.ColIndexToRemove.Count > 0)
                {
                        levelLoader.AllCollisionObjects.RemoveAt(spawnEnemies.ColIndexToRemove[0]);
                        spawnEnemies.ColIndexToRemove.RemoveAt(0);
                }
                spawnEnemies.newWave = false;
            }
            for (int i = 0; i < spawnEnemies.enemy.Count; i++)
            {
                 //Items
                for (int p = 0; p < players.Count; p++)
                {
                    for (int s = 0; s < players[p].sonicBoom.Count; s++)
                    {
                        if (players[p].sonicBoom[s].BlowUp)
                            if (spawnEnemies.enemy[i].EnemyRect.Intersects(players[p].sonicBoom[s].ExplosionRect))
                            {
                                Projectile temp = new Projectile(spawnEnemies.enemy[i].pos, 0, Vector2.Zero, 0, 1, 0, spawnEnemies.enemy[i].Health, 0, square, "trap", null, 0, 0);
                                bullets.Add(temp);
                            }
                    }
                    //for (int s = 0; s < players[p].Slower.Count; s++)
                    //{
                    //    EnemyInSlower(spawnEnemies.enemy[i]);
                    //    if (Vector2.Distance(spawnEnemies.enemy[i].pos, players[p].Slower[s].pos) < players[p].Slower[s].AttackRadius)
                    //    {
                    //        if (spawnEnemies.enemy[i].Speed > 2)
                    //            spawnEnemies.enemy[i].Speed--;a
                    //    }
                    //}
                }
                if (EnemyInSlower(spawnEnemies.enemy[i]))
                {
                    if (spawnEnemies.enemy[i].Speed > 2)
                        spawnEnemies.enemy[i].Speed--;
                    if (spawnEnemies.Mutate)
                        if (spawnEnemies.enemy[i].Speed > 2)
                            spawnEnemies.enemy[i].Speed = 2;
                }
                else
                {
                    if (/*!spawnEnemies.Mutate && */spawnEnemies.enemy[i].StunTime <= 0)
                        spawnEnemies.enemy[i].Speed = spawnEnemies.enemy[i].NormalSpeed;
                }
                if (spawnEnemies.enemy[i].StunHurt)
                {
                    Projectile temp = new Projectile(spawnEnemies.enemy[i].pos, 0, Vector2.Zero, 0, 1, spawnEnemies.enemy[i].PlayerStunnedBy, 1,
                            0, square, "invis", null, 10, 50);
                    bullets.Add(temp);
                    spawnEnemies.enemy[i].StunHurt = false;
                }
                if (!spawnEnemies.enemy[i].AddedToCollision)//Add To Collision List
                {
                        levelLoader.AllCollisionObjects.Add(spawnEnemies.enemy[i].EnemyRect);
                        spawnEnemies.enemy[i].CollisionIndex = levelLoader.AllCollisionObjects.Count - 1;
                        spawnEnemies.ColIndexToRemove.Add(spawnEnemies.enemy[i].CollisionIndex);
                        spawnEnemies.enemy[i].AddedToCollision = true;                    
                }
                
                //Update Enemies
                    spawnEnemies.enemy[i].Update(bullets, levelLoader.AllCollisionObjects, levelLoader.LevelPiecesCollisionObject/**//*,levelLoader.pathfindNode*/, players, gameTime);//, levelLoader.LevelPiecesCollisionObject
                    spawnEnemies.enemy[i].CheckCollisionSafe(spawnEnemies.enemy, spawnEnemies.OpenSpawns);
                Random _random = new Random();
                if (spawnEnemies.enemy[i].HitBullet >= 0)
                {
                    //#HitEffects
                    if (bullets[spawnEnemies.enemy[i].HitBullet].hitSound != null)
                        bullets[spawnEnemies.enemy[i].HitBullet].hitSound.Play();
                    if (bullets[spawnEnemies.enemy[i].HitBullet].type == "Explosive")
                    {
                        partEffManager.LandMineExplosion.InstantiateEffect(spawnEnemies.enemy[i].pos);

                        Vector2 hitPos = spawnEnemies.enemy[i].pos;
                        Projectile _mine = bullets[spawnEnemies.enemy[i].HitBullet];

                        int Area = 155;
                        SplashDamage(Area, _mine.Damage, hitPos, _mine.PlayerFired, i);
                    }
                    if (bullets[spawnEnemies.enemy[i].HitBullet].type == "Rocket")
                    {
                        partEffManager.RocketExplosion.InstantiateEffect(spawnEnemies.enemy[i].pos);
                        
                        //splash damage

                        Vector2 hitPos = spawnEnemies.enemy[i].pos;
                        Projectile _rocket = bullets[spawnEnemies.enemy[i].HitBullet];
                        //Projectile temp = new Projectile(hitPos, 0, Vector2.Zero, 0, 1, _rocket.PlayerFired, _rocket.Damage * 2,
                        //    0, square, "invis", null, 10, 50);
                        //bullets.Add(temp);

                        int Area = 155;
                        SplashDamage(Area, _rocket.Damage, hitPos, _rocket.PlayerFired, i);
                    }
                    if (bullets[spawnEnemies.enemy[i].HitBullet].type == "Freeze")
                    {
                        spawnEnemies.enemy[i].StunTime = 7500;
                        spawnEnemies.enemy[i].PlayerStunnedBy = bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired;
                        partEffManager.Freeze.InstantiateEffect(spawnEnemies.enemy[i].pos);
                    }
                    partEffManager.Blood.InstantiateEffect(spawnEnemies.enemy[i].pos);
                    partEffManager.Blood2.InstantiateEffect(spawnEnemies.enemy[i].pos);

                    if (((spawnEnemies.enemy[i].Health - bullets[spawnEnemies.enemy[i].HitBullet].Damage > 0
                        &&  !players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].damageBoost.underEffect)


                        || (spawnEnemies.enemy[i].Health - bullets[spawnEnemies.enemy[i].HitBullet].Damage *
                        players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].damageBoost.DamageMultiplier > 0
                        && players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].damageBoost.underEffect)) 

                        
                        && InstaKillEfect <= 0)
                    {
                        if (bullets[spawnEnemies.enemy[i].HitBullet].type != "Sentry")
                            players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].pInput.ViberateController(3, 0.09f);

                        if(!players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].damageBoost.underEffect)
                            spawnEnemies.enemy[i].Health -= bullets[spawnEnemies.enemy[i].HitBullet].Damage;
                        else
                            spawnEnemies.enemy[i].Health -= (int) (bullets[spawnEnemies.enemy[i].HitBullet].Damage
                                 * players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].damageBoost.DamageMultiplier);

                        //if (bullets[spawnEnemies.enemy[i].HitBullet].type != "Sentry")
                        //{
                        PointText temp;


                        if (DoublePointsEffect <= 0)
                        {
                            players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].Score += bullets[spawnEnemies.enemy[i].HitBullet].HitPoints;
                            temp = new PointText(new Vector2(spawnEnemies.enemy[i].EnemyRect.X + 10, spawnEnemies.enemy[i].EnemyRect.Y - 10), 0, bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired, bullets[spawnEnemies.enemy[i].HitBullet].HitPoints, players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].pointEffect);
                        }
                        else
                        {
                            players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].Score += bullets[spawnEnemies.enemy[i].HitBullet].HitPoints * 2;
                            temp = new PointText(new Vector2(spawnEnemies.enemy[i].EnemyRect.X + 10, spawnEnemies.enemy[i].EnemyRect.Y - 10), 0, bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired, bullets[spawnEnemies.enemy[i].HitBullet].HitPoints * 2, players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].pointEffect);
                        }
                        if(temp.Points != 0)//#ptext
                            pointText.Add(temp);
                        //}
                        //else
                        //{
                        //    players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].Score += 1;
                        //    PointText temp = new PointText(new Vector2(spawnEnemies.enemy[i].EnemyRect.X + 10, spawnEnemies.enemy[i].EnemyRect.Y - 10), 0, bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired, 1);
                        //    pointText.Add(temp);
                        //}
                        ADD_POINTS_Lastadded = 0;
                    }
                    else
                    {


                        PointText temp;
                        if (DoublePointsEffect <= 0)
                        {
                            players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].Score += bullets[spawnEnemies.enemy[i].HitBullet].KillPoints;
                            temp = new PointText(new Vector2(spawnEnemies.enemy[i].EnemyRect.X + 10, spawnEnemies.enemy[i].EnemyRect.Y - 10), 0, bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired, bullets[spawnEnemies.enemy[i].HitBullet].KillPoints, players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].pointEffect);
                        }
                        else
                        {
                            players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].Score += bullets[spawnEnemies.enemy[i].HitBullet].KillPoints * 2;
                            temp = new PointText(new Vector2(spawnEnemies.enemy[i].EnemyRect.X + 10, spawnEnemies.enemy[i].EnemyRect.Y - 10), 0, bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired, bullets[spawnEnemies.enemy[i].HitBullet].KillPoints * 2, players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].pointEffect);
                        }
                        if (temp.Points != 0)//#ptext
                            pointText.Add(temp);

                        ADD_POINTS_Lastadded = 0;

                        if(bullets[spawnEnemies.enemy[i].HitBullet].type != "trap")
                        players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].Kills++;

                        spawnEnemies.enemy[i].hit.Play();
                        if(!players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].damageBoost.underEffect)
                            spawnEnemies.enemy[i].Health -= bullets[spawnEnemies.enemy[i].HitBullet].Damage;
                        else
                            spawnEnemies.enemy[i].Health -= (int)(bullets[spawnEnemies.enemy[i].HitBullet].Damage *
                                players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].damageBoost.DamageMultiplier);
                        int _drop = spawnEnemies.enemy[i].Drop;//_random.Next(1, 150);//70);//6);

                        if (!GameOver)
                        {
                            if (_drop == 1 || _drop == 2)
                            {
                                //  Rectangle spawnRect = new Rectangle(square[spawnEnemies.enemy[i].EnemyRect.X,
                                // spawnEnemies.enemy[i].EnemyRect.Y,

                                Drops.Add(spawnEnemies.enemy[i].EnemyRect);
                                DropType.Add("Speed Boost");
                                DropTexture.Add(speedBoost);
                                Drop_Life.Add(0);
                                drop.Play();
                            }
                            if ((_drop == 3 || _drop == 4)  &&  !spawnEnemies.Mutate)
                            {
                                Drops.Add(spawnEnemies.enemy[i].EnemyRect);
                                DropType.Add("Insta Kill");
                                DropTexture.Add(instaKill);
                                Drop_Life.Add(0);
                                drop.Play();
                            }
                            if (_drop == 5)// || _drop == 6)
                            {
                                Drops.Add(spawnEnemies.enemy[i].EnemyRect);
                                DropType.Add("Health Kick");
                                DropTexture.Add(healthKick);
                                Drop_Life.Add(0);
                                drop.Play();
                            }
                            if (_drop == 7)
                            {
                                Drops.Add(spawnEnemies.enemy[i].EnemyRect);
                                DropType.Add("Death Touch");
                                DropTexture.Add(deathTouch);
                                Drop_Life.Add(0);
                                drop.Play();
                            }
                            if (_drop == 8)
                            {
                                Drops.Add(spawnEnemies.enemy[i].EnemyRect);
                                DropType.Add("Double Points");
                                DropTexture.Add(doublePoints);
                                Drop_Life.Add(0);
                                drop.Play();
                            }
                            if (_drop == 9)
                            {
                                Drops.Add(spawnEnemies.enemy[i].EnemyRect);
                                DropType.Add("Max Ammo");
                                DropTexture.Add(MaxAmmo);
                                Drop_Life.Add(0);
                                drop.Play();
                            }
                            if (_drop == 10 && players[0].StoreDiscount < 50 && levelLoader.StoreRect.Count > 0)
                            {
                                Drops.Add(spawnEnemies.enemy[i].EnemyRect);
                                DropType.Add("Discount");
                                DropTexture.Add(Discount);
                                Drop_Life.Add(0);
                                drop.Play();
                            }
                        }
                    }
                    if (InstaKillEfect > 0)
                    {
                        players[bullets[spawnEnemies.enemy[i].HitBullet].PlayerFired].pInput.ViberateController(3, 0.15f);
                    }
                    bullets.RemoveAt(spawnEnemies.enemy[i].HitBullet);
                    spawnEnemies.enemy[i].HitBullet = -1;
                    if (InstaKillEfect > 0)
                    {
                        spawnEnemies.enemy[i].Health = 0;
                    }
                    
                }
                if (spawnEnemies.enemy[i].CollisionSafe)//Add/Push Aside enemy collision Boundaries
                    levelLoader.AllCollisionObjects[spawnEnemies.enemy[i].CollisionIndex] = spawnEnemies.enemy[i].CollisionRect;
                else
                    levelLoader.AllCollisionObjects[spawnEnemies.enemy[i].CollisionIndex] = new Rectangle(0, 0, 0, 0);
                for (int p = 0; p < players.Count; p++)//Remove Enemy/Damage Player
                {
                    if (spawnEnemies.enemy[i].Health <= 0 || spawnEnemies.enemy[i].EnemyRect.Intersects(players[p].PlayerRect) && players[p].Health > 0)
                    {
                        if (spawnEnemies.enemy[i].StunTime <= 0 || (spawnEnemies.enemy[i].StunTime > 0 && DeathTouchEffect > 0) || spawnEnemies.enemy[i].Health <= 0)
                        {
                            if (spawnEnemies.enemy[i].EnemyRect.Intersects(players[p].PlayerRect)  &&  spawnEnemies.enemy[i].Health > 0)
                            {
                                if (DeathTouchEffect <= 0 && players[p].CurinvulnerabilityTime <= 0)//#inv
                                {
                                    players[p].Health -= (45 + spawnEnemies.Wave);
                                    players[p].hitEffect.InstantiateEffect(players[p].pos);
                                    partEffManager.Blood2.InstantiateEffect(players[p].pos);
                                    players[p].CurinvulnerabilityTime = players[p].InvulnerabilityTime;
                                    spawnEnemies.SpawnedEnemies--;
                                }
                                if (DeathTouchEffect > 0)
                                {
                                    partEffManager.RocketExplosion.InstantiateEffect(spawnEnemies.enemy[i].pos);
                                    partEffManager.Blood.InstantiateEffect(spawnEnemies.enemy[i].pos);
                                    partEffManager.Blood2.InstantiateEffect(spawnEnemies.enemy[i].pos);
                                }
                            }
                            levelLoader.AllCollisionObjects[spawnEnemies.enemy[i].CollisionIndex] = new Rectangle(0, 0, 0, 0);
                            spawnEnemies.enemy.RemoveAt(i);
                        }
                        break;
                    }
                }

               
            }

            if (PurchasedDoor)
                Purch_Door_Delay++;

            if (Purch_Door_Delay > 1)
            {
                PurchasedDoor = false;
                Purch_Door_Delay = 0;
            }

            ADD_POINTS_Lastadded += gameTime.ElapsedGameTime.Milliseconds;
            for (int i = 0; i < pointText.Count; i++)
            {
                pointText[i].Update(gameTime);
                if (pointText[i].Life > 500)
                    pointText.RemoveAt(i);
            }
            for (int i = 0; i < DropText.Count; i++)
            {
                DropText[i].Update(gameTime);
                if (DropText[i].Life > 650)
                    DropText.RemoveAt(i);
            }
            for (int i = 0; i < PlayerPopupText.Count; i++)
            {
                PlayerPopupText[i].Update(gameTime);
                PlayerPopupText[i].originPos = new Vector2((players[PlayerPopupText[i].PlayerIndex].pos.X) - (font.MeasureString("" + PlayerPopupText[i].Text).X / 2), (players[PlayerPopupText[i].PlayerIndex].pos.Y - 50));
                PlayerPopupText[i].YOffset -= 2;
                if (PlayerPopupText[i].Life > 550)
                    PlayerPopupText.RemoveAt(i);
                
            }
        }
        private Boolean EnemyInSlower(Enemy enemy)
        {
            for (int p = 0; p < players.Count; p++)
                if (players[p].Slower.Count > 0)
                {
                    for (int i = 0; i < players[p].Slower.Count; i++)
                    {

                        if (Vector2.Distance(enemy.pos, players[p].Slower[i].pos) < players[p].Slower[i].AttackRadius)
                            return true;
                    }
                }
            return false;
        }
        private void FireStandard(Player player, int _playerIndex)
        {
        }
        private void FireShotGun(Player player, int _playerIndex, String ShotGunType)
        {//#fire
            
            if (ShotGunType == "ShotGun")
            {
                AngleBullet(player, _playerIndex, player.BulletRotation);

                AngleBullet(player, _playerIndex, player.BulletRotation + 0.10f);
                AngleBullet(player, _playerIndex, player.BulletRotation - 0.10f);
                //float dirX = player.BulletDirection.X;
                //float dirY = player.BulletDirection.Y;
                //float angle = player.BulletRotation + 0.12f;
                //float angle2 = player.BulletRotation - 0.12f;
                //int r = 1;
                //float x = (float)(r * Math.Cos(angle));
                //float y = (float)(r * Math.Sin(angle));

                //float x2 = (float)(r * Math.Cos(angle2));
                //float y2 = (float)(r * Math.Sin(angle2));
                ////System.Console.WriteLine(new Vector2(x, y));
                //Projectile temp = new Projectile(player.BulletPos, angle,

                //    new Vector2(x, y),//player.BulletDirection,

                //    0, player.BulletLife, _playerIndex, player.BulletDamage
                //    , player.BulletSpeed, player.BulletTexture, player.BulletType, player.BulletHitSound, player.HitPoints, player.KillPoints);

                //bullets.Add(temp);

                //Projectile temp2 = new Projectile(player.BulletPos, player.BulletRotation, player.BulletDirection, 0, player.BulletLife, _playerIndex, player.BulletDamage
                //    , player.BulletSpeed, player.BulletTexture, player.BulletType, player.BulletHitSound, player.HitPoints, player.KillPoints);
                //bullets.Add(temp2);

                //Projectile temp3 = new Projectile(player.BulletPos, angle2,

                //   new Vector2(x2, y2),//player.BulletDirection,

                //   0, player.BulletLife, _playerIndex, player.BulletDamage
                //   , player.BulletSpeed, player.BulletTexture, player.BulletType, player.BulletHitSound, player.HitPoints, player.KillPoints);

                //bullets.Add(temp3);
           }
            else if (ShotGunType == "Auto-Shotty")
            {
                AngleBullet(player, _playerIndex, player.BulletRotation);

                AngleBullet(player, _playerIndex, player.BulletRotation + 0.14f);
                AngleBullet(player, _playerIndex, player.BulletRotation - 0.14f);

                AngleBullet(player, _playerIndex, player.BulletRotation + 0.07f);
                AngleBullet(player, _playerIndex, player.BulletRotation - 0.07f);
            }
        }

        private void FireDual(Player player, int _playerIndex, String Type)
        {
            MovedBullet(player, _playerIndex, 90, 9);
            MovedBullet(player, _playerIndex, -90, 9);
        }

        private void MovedBullet(Player player, int _playerIndex, float angle, int R)
        {
            float x = player.BulletPos.X + R * (float)Math.Cos(player.BulletRotation + angle);
            float y = player.BulletPos.Y + R * (float)Math.Sin(player.BulletRotation + angle);

            Projectile temp = new Projectile(new Vector2(x,y), player.BulletRotation, player.BulletDirection, 0, player.BulletLife, _playerIndex, player.BulletDamage, player.BulletSpeed,
                player.BulletTexture, player.BulletType, player.BulletHitSound, player.HitPoints, player.KillPoints);
            bullets.Add(temp);
        }
        private void AngleBullet(Player player, int _playerIndex, float angle)
        {
            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            Projectile temp = new Projectile(player.BulletPos, angle, new Vector2(x, y), 0, player.BulletLife, _playerIndex, player.BulletDamage, player.BulletSpeed,
                player.BulletTexture, player.BulletType, player.BulletHitSound, player.HitPoints, player.KillPoints);
            bullets.Add(temp);
        }

        private void SplashDamage(int Area, int damage, Vector2 hitPos, int playerFired, int HitZombie)
        {
            if (players[playerFired].damageBoost.underEffect)
                damage = (int)(damage * players[playerFired].damageBoost.DamageMultiplier);
            for (int a = 0; a < spawnEnemies.enemy.Count; a++)
            {
                if (a != HitZombie)
                    if (Vector2.Distance(hitPos, spawnEnemies.enemy[a].pos) <= Area)
                    {
                        Projectile temp = new Projectile(spawnEnemies.enemy[a].pos, 0, Vector2.Zero, 0, 1, playerFired, (int)(damage * (Vector2.Distance(hitPos, spawnEnemies.enemy[a].pos) / Area)),
                            0, square, "invis", null, 10, 50);
                        bullets.Add(temp);
                    }
            }
            for (int a = 0; a < levelLoader.LevelPiecesCollisionObject.Count; a++)
            {
                if (levelLoader.DoorChannel[a] == -2)
                {
                    Vector2 tempVec = new Vector2(levelLoader.LevelPiecesCollisionObject[a].X + levelLoader.LevelPiecesCollisionObject[a].Width / 2
                        , levelLoader.LevelPiecesCollisionObject[a].Y + levelLoader.LevelPiecesCollisionObject[a].Height / 2);
                    if (Vector2.Distance(hitPos, tempVec) <= Area)
                    {
                        Projectile temp = new Projectile(tempVec, 0, Vector2.Zero, 0, 1, playerFired, (int)(damage * (Vector2.Distance(hitPos, tempVec) / Area)),
                            0, square, "invis", null, 10, 50);
                        bullets.Add(temp);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_doPlay)
            {
                if (players.Count == 1)
                {
                    DrawGame(spriteBatch, DefaultViewPort, camera, 0, false);
                    DrawGUIs(spriteBatch, DefaultViewPort, DefaultViewPort);
                }
                if (players.Count == 2)
                {
                    DrawGame(spriteBatch, TopViewPort, camera_1, 0, true);
                    DrawGUIs(spriteBatch, DefaultViewPort, TopViewPort);

                    DrawGame(spriteBatch, BottomViewPort, camera_2, 1, true);
                    DrawGUIs(spriteBatch, DefaultViewPort, BottomViewPort);
                }
                if (players.Count == 3)
                {

                    DrawGame(spriteBatch, ViewPort_TopLeft, camera_1, 0, false);
                    DrawGUIs(spriteBatch, DefaultViewPort, ViewPort_TopLeft);

                    DrawGame(spriteBatch, ViewPort_BottomLeft, camera_3, 1, false);
                    DrawGUIs(spriteBatch, DefaultViewPort, ViewPort_BottomLeft);

                    DrawGame(spriteBatch, ViewPort_BottomRight, camera_4, 2, false);
                    DrawGUIs(spriteBatch, DefaultViewPort, ViewPort_BottomRight);
                   
                }
                if (players.Count == 4)
                {

                    DrawGame(spriteBatch, ViewPort_TopLeft, camera_1, 0, false);
                    DrawGUIs(spriteBatch, DefaultViewPort, ViewPort_TopLeft);

                    DrawGame(spriteBatch, ViewPort_TopRight, camera_2, 1, false);
                    DrawGUIs(spriteBatch, DefaultViewPort, ViewPort_TopRight);

                    DrawGame(spriteBatch, ViewPort_BottomLeft, camera_3, 2, false);
                    DrawGUIs(spriteBatch, DefaultViewPort, ViewPort_BottomLeft);

                    DrawGame(spriteBatch, ViewPort_BottomRight, camera_4, 3, false);
                    DrawGUIs(spriteBatch, DefaultViewPort, ViewPort_BottomRight);

                }
                //if (paused  && !GameOver)
                //{
                //   // UpdatePauseMenu();
                //    spriteBatch.Begin();
                //    //DrawPauseScreen(spriteBatch);
                //    spriteBatch.End();
                //}
                if (GameOver)
                {
                    Viewport curViewPort = graphics.Viewport;
                    graphics.Viewport = DefaultViewPort;
                    spriteBatch.Begin();
                    spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(GOfade, GOfade, GOfade, GOfade));

                    if (GOfade >= 200)
                    {
                        Rectangle Background = new Rectangle(graphics.Viewport.X + graphics.Viewport.Width / 4, graphics.Viewport.Y, graphics.Viewport.Width / 2, graphics.Viewport.Height);
                        //spriteBatch.Draw(square, Background, Color.Red);
                        spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(255, 65, 48, 100));

                        //spriteBatch.DrawString(font, "Game Over", new Vector2(Background.X + 5, Background.Y + 5), Color.Black);
                        spriteBatch.DrawString(font, "Game Over", new Vector2(Background.X + Background.Width / 2 - font.MeasureString("Game Over").X / 2, Background.Y + 5), Color.Black);
                        if (newHighScore)
                            //spriteBatch.DrawString(font, "NEW HIGHSCORE", new Vector2(Background.X + 5, Background.Y + 30), Color.Yellow);
                            spriteBatch.DrawString(font, "NEW HIGHSCORE", new Vector2(Background.X + Background.Width / 2 - font.MeasureString("NEW HIGHSCORE").X / 2, Background.Y + 30), Color.Yellow);
                        String rounds = "Rounds";
                        if (spawnEnemies.Wave == 1)
                            rounds = "Round";
                        spriteBatch.DrawString(font, "You Survived " + spawnEnemies.Wave + " " + rounds,
                            new Vector2(Background.X + Background.Width / 2 - font.MeasureString("You Survived " + spawnEnemies.Wave + " " + rounds).X / 2, Background.Y + 110)
                            , Color.Black);

                        if(players[0].PC)
                            spriteBatch.Draw(players[0].kInput.EscapeKeyTexture, new Rectangle(Background.X + Background.Width - 60, Background.Y + Background.Height - 60, 50, 50), Color.White);
                        else
                            spriteBatch.Draw(players[0].pInput.ButtonB, new Rectangle(Background.X + Background.Width - 60, Background.Y + Background.Height - 60, 50, 50), Color.White);

                        spriteBatch.DrawString(font, level_name, new Vector2(Background.X + Background.Width / 2 - font.MeasureString(level_name).X / 2, Background.Y + 140), Color.Black);
                        int lineWidth = (int)font.MeasureString(level_name).X + 10;
                        spriteBatch.Draw(square, new Rectangle(Background.X + Background.Width / 2 - (lineWidth / 2), Background.Y + 171, lineWidth, 4), Color.Black);
                        for (int i = 0; i < scores.Length; i++)
                        {
                            Color color = Color.Black;
                            if (HighScorePlace == i)
                                color = Color.SkyBlue;
                            spriteBatch.DrawString(font, "" + scores[i], new Vector2(Background.X + Background.Width / 2 - font.MeasureString("" + scores[i]).X / 2, Background.Y + 175 + (25 * i)), color);
                        }

                        Rectangle scoreBoardRect = new Rectangle(Background.X + Background.Width / 2 - (Background.Width - 50) / 2,
                  Background.Y + Background.Height / 2,
                  Background.Width - 50, 50);
                        // viewPort.Height - (viewPort.Height / 6) * 2);// new Rectangle(viewPort.Width / 2 - 200, viewPort.Height - 100, viewPort.Width / 2 + 400, viewPort.Height / 2 + 00);
                        SpriteFont ScoreBoardFont = font;


                        
                        spriteBatch.Draw(square, scoreBoardRect, new Color(200, 50, 50, 100));

                        spriteBatch.Draw(square, new Rectangle(scoreBoardRect.X, scoreBoardRect.Y + scoreBoardRect.Height - 10, scoreBoardRect.Width, 10), Color.Black);

                        Vector2 KillsScorePos = new Vector2(scoreBoardRect.X + ScoreBoardFont.MeasureString("player . " + (100 )).X, scoreBoardRect.Y + scoreBoardRect.Height / 2 - ScoreBoardFont.MeasureString(".").Y / 2);
                        spriteBatch.DrawString(ScoreBoardFont, "Kills", KillsScorePos, Color.White);

                        Vector2 DeathsScorePos = new Vector2(scoreBoardRect.X + ScoreBoardFont.MeasureString("player . " + (100 )).X + ScoreBoardFont.MeasureString("Kills ").X + (40), scoreBoardRect.Y + scoreBoardRect.Height / 2 - ScoreBoardFont.MeasureString(".").Y / 2);
                        spriteBatch.DrawString(ScoreBoardFont, "Deaths", DeathsScorePos, Color.White);

                        for (int i = 0; i < players.Count; i++)
                        {
                            Rectangle playerScoreRect = new Rectangle(scoreBoardRect.X, scoreBoardRect.Y + (int)((45 * (i + 1))), scoreBoardRect.Width, scoreBoardRect.Height);

                            spriteBatch.Draw(square, playerScoreRect, new Color(200, 50, 50, 100));

                            float StatY = playerScoreRect.Y + playerScoreRect.Height / 2 - ScoreBoardFont.MeasureString(".").Y / 2;


                            spriteBatch.DrawString(ScoreBoardFont, "Player " + (i + 1), new Vector2(playerScoreRect.X + 10, StatY), Color.White);

                            spriteBatch.DrawString(ScoreBoardFont, "" + players[i].Kills, new Vector2(KillsScorePos.X
                                + ScoreBoardFont.MeasureString("Kills").X / 2 - ScoreBoardFont.MeasureString("" + players[i].Kills).X / 2,
                                StatY), Color.White);

                            spriteBatch.DrawString(ScoreBoardFont, "" + players[i].Deaths, new Vector2(DeathsScorePos.X +
                                ScoreBoardFont.MeasureString("Deaths").X / 2 - ScoreBoardFont.MeasureString("" + players[i].Deaths).X / 2, StatY), Color.White);
                        }
                    }
                    //spriteBatch.Draw(GrassTile, new Vector2(100, 100), null, Color.White, MathHelper.ToRadians(300), new Vector2(GrassTile.Width / 2, GrassTile.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                    spriteBatch.End();
                    graphics.Viewport = curViewPort;
                }
            }
        }


        public int GameFadeValue = 255;
        private void DrawGame(SpriteBatch spriteBatch, Viewport viewport, Camera camera, int playerIndex, Boolean DrawBorders)
        {
           
            graphics.Viewport = viewport;
            float SpriteScale = 1f;// = ((float)viewport.Height / (float)720);
            SpriteFont _font;
            if (players.Count == 2)
            {
                _font = Splitfont;
                SpriteScale = 0.75f;
            }
            else if (players.Count >= 3)
            {
                _font = Splitfont;
                SpriteScale = 0.75f;
            }
            else
            {
                _font = font;
                SpriteScale = 1f;
            }
            spriteBatch.Begin();
                spriteBatch.Draw(GrassTile, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(100, 100, 100));//OuterGrass
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);
                levelLoader.Draw(spriteBatch, players[playerIndex].CamCen);//Level
                for (int i = 0; i < levelLoader.WeaponRects.Count; i++)//Weapon Purchase Stations
                {
                    if (Vector2.Distance(players[playerIndex].CamCen, new Vector2(levelLoader.WeaponRects[i].X + levelLoader.WeaponRects[i].Width / 2,
                        levelLoader.WeaponRects[i].Y + levelLoader.WeaponRects[i].Height / 2)) < 1550)
                    {
                        if ((players[playerIndex].Score >= levelLoader.WeaponCost[i] 
                            &&  players[playerIndex].weaponManager.CurentWeapon.WeaponName != levelLoader.WeaponType[i]
                            &&  players[playerIndex].weaponManager.SecondaryWeaponName != levelLoader.WeaponType[i])
                            ||  (players[playerIndex].Score >= levelLoader.AmmoCost[i]  &&  
                            (players[playerIndex].weaponManager.CurentWeapon.WeaponName == levelLoader.WeaponType[i]  ||  
                            players[playerIndex].weaponManager.SecondaryWeaponName == levelLoader.WeaponType[i])))
                            spriteBatch.Draw(levelLoader.WeaponTileTexture[i], levelLoader.WeaponRects[i], Color.White);                        
                        else
                            spriteBatch.Draw(levelLoader.WeaponTileTexture[i], levelLoader.WeaponRects[i], new Color(75, 75, 75, 35));
                    }
                }
                for (int i = 0; i < levelLoader.MusicPlayerRect.Count; i++)
                    spriteBatch.Draw(levelLoader.tileManager.JukeBox.texture, levelLoader.MusicPlayerRect[i], Color.White);
                for (int i = 0; i < levelLoader.StoreRect.Count; i++)
                {
                    spriteBatch.Draw(levelLoader.tileManager.Store.texture, levelLoader.StoreRect[i], Color.White);
                }
                for (int i = 0; i < levelLoader.ItemName.Count; i++)
                {
                     if (players[playerIndex].Score >= levelLoader.ItemPrice[i]) 
                        spriteBatch.Draw(levelLoader.ItemTexture[i], levelLoader.ItemRect[i], Color.White);
                     else
                         spriteBatch.Draw(levelLoader.ItemTexture[i], levelLoader.ItemRect[i], new Color(75, 75, 75, 35));

                }
                for (int i = 0; i < levelLoader.teleporter.Count; i++)
                {
                    spriteBatch.Draw(levelLoader.teleporter[i].texture, levelLoader.teleporter[i].rect, Color.White);
                }
                for (int i = 0; i < levelLoader.DoorSwitches.Count; i++)
                {
                    if (levelLoader.PurchasedChannelTraps.Contains(levelLoader.SwitchChannel[i]))
                        spriteBatch.Draw(levelLoader.tileManager.DoorSwitchOn, levelLoader.DoorSwitches[i], Color.White);//#switch
                    else
                        spriteBatch.Draw(levelLoader.tileManager.DoorSwitch.texture, levelLoader.DoorSwitches[i], Color.White);
                }
                for (int i = 0; i < zapper.Count; i++) zapper[i].Draw(spriteBatch);


                spawnEnemies.DrawSpawns(spriteBatch, players[playerIndex].CamCen);

                for (int i = 0; i < AmmoBagRect.Count; i++)
                    spriteBatch.Draw(players[0].ammoBag.icon, AmmoBagRect[i], Color.White);

                for (int i = 0; i < bullets.Count; i++)//Active Bullets
                    if (!bullets[i].Rect.Intersects(players[bullets[i].PlayerFired].PlayerRect) || (bullets[i].type != "Standard" && bullets[i].type != "ShotGun" && bullets[i].type != "Auto-Shotty" && bullets[i].type != "Dual" && bullets[i].type != "Freeze") && bullets[i].type != "invis" && bullets[i].type != "trap")
                        spriteBatch.Draw(bullets[i].texture, bullets[i].Pos, null, Color.White, bullets[i].Rot, new Vector2(bullets[i].texture.Width / 2, bullets[i].texture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);

                levelLoader.DrawColObjects(spriteBatch, players[playerIndex].CamCen);

                for (int p = 0; p < players.Count; p++)
                    players[p].DrawItems(spriteBatch);
               

                spawnEnemies.Draw(spriteBatch, players[playerIndex].CamCen);//Enemies

                partEffManager.Draw(spriteBatch);

                for (int i = 0; i < zapper.Count; i++) zapper[i].DrawElectricity(spriteBatch);

                for (int p = 0; p < players.Count; p++)//Players
                {
                    if (_options.DrawPlayerCircles)
                    {
                        if (p == 0) spriteBatch.Draw(_options._playerCircle, new Rectangle((int)players[p].pos.X - _options._PlayerCircleSize / 2, (int)players[p].pos.Y - _options._PlayerCircleSize / 2, _options._PlayerCircleSize, _options._PlayerCircleSize), new Color(100, 100, 100, 100));
                        if (p == 1) spriteBatch.Draw(_options._playerCircle, new Rectangle((int)players[p].pos.X - _options._PlayerCircleSize / 2, (int)players[p].pos.Y - _options._PlayerCircleSize / 2, _options._PlayerCircleSize, _options._PlayerCircleSize), new Color(0, 0, 100, 100));
                        if (p == 2) spriteBatch.Draw(_options._playerCircle, new Rectangle((int)players[p].pos.X - _options._PlayerCircleSize / 2, (int)players[p].pos.Y - _options._PlayerCircleSize / 2, _options._PlayerCircleSize, _options._PlayerCircleSize), new Color(100, 0, 0, 100));
                        if (p == 3) spriteBatch.Draw(_options._playerCircle, new Rectangle((int)players[p].pos.X - _options._PlayerCircleSize / 2, (int)players[p].pos.Y - _options._PlayerCircleSize / 2, _options._PlayerCircleSize, _options._PlayerCircleSize), new Color(0, 100, 0, 100));
                    }
                    players[p].Draw(spriteBatch);
                }
                for (int d = 0; d < Drops.Count; d++)//Drops
                    DrawDrop(spriteBatch, Drops[d], DropTexture[d], Drop_Life[d], 8, 400);
                if(!CinemaMode)
                for (int i = 0; i < pointText.Count; i++)//+point Text
                    if (pointText[i].Player == playerIndex)
                        spriteBatch.DrawString(font, pointText[i].points, pointText[i].Pos, Color.White);

                for (int i = 0; i < DropText.Count; i++)
                    spriteBatch.DrawString(font, DropText[i].Text, DropText[i].Pos, new Color(DropText[i].Fade, DropText[i].Fade, DropText[i].Fade, DropText[i].Fade));
                for (int i = 0; i < PlayerPopupText.Count; i++)
                    spriteBatch.DrawString(font, PlayerPopupText[i].Text, PlayerPopupText[i].Pos, new Color(PlayerPopupText[i].Fade, PlayerPopupText[i].Fade, PlayerPopupText[i].Fade, PlayerPopupText[i].Fade));

                if (!players[playerIndex].dead)
                {
                    for (int a = 0; a < levelLoader.LevelPiecesCollisionObject.Count; a++)
                    {
                        if (Vector2.Distance(players[playerIndex].pos, new Vector2(levelLoader.LevelPiecesCollisionObject[a].X + levelLoader.LevelPiecesCollisionObject[a].Width / 2,
                            levelLoader.LevelPiecesCollisionObject[a].Y + levelLoader.LevelPiecesCollisionObject[a].Height / 2)) < levelLoader.InteractionDist  &&  levelLoader.DoorChannel[a] == -1)
                        {
                            spriteBatch.Draw(players[playerIndex].UseKeyTexture, new Rectangle((int)players[playerIndex].pos.X - 22, (int)players[playerIndex].pos.Y - 160, 44, 44), Color.White);
                            if (levelLoader.DoorPrices[a] != 0)
                                spriteBatch.DrawString(GameInfoFont, "Cost | " + levelLoader.DoorPrices[a], new Vector2(players[playerIndex].pos.X -
                                    GameInfoFont.MeasureString("Cost |").X + GameInfoFont.MeasureString("|").X / 2 /*132*/, players[playerIndex].pos.Y - 110), Color.White);
                            else
                                spriteBatch.DrawString(GameInfoFont, "Open Door", new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString("Open Door").X / 2 /*132*/, players[playerIndex].pos.Y - 110), Color.White);
                        }
                    }
                    for (int a = 0; a < levelLoader.DoorSwitches.Count; a++)
                    {
                        if (Vector2.Distance(players[playerIndex].pos, new Vector2(levelLoader.DoorSwitches[a].X + levelLoader.DoorSwitches[a].Width / 2,
                           levelLoader.DoorSwitches[a].Y + levelLoader.DoorSwitches[a].Height / 2)) < levelLoader.InteractionDist)
                        {
                            if (!levelLoader.PurchasedChannelTraps.Contains(levelLoader.SwitchChannel[a]))
                            {
                                spriteBatch.Draw(players[playerIndex].UseKeyTexture, new Rectangle((int)players[playerIndex].pos.X - 22, (int)players[playerIndex].pos.Y - 160, 44, 44), Color.White);
                                if (levelLoader.SwitchPrice[a] == 0)
                                    spriteBatch.DrawString(GameInfoFont, "Use Switch", new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString("Use Switch").X / 2 /*132*/, players[playerIndex].pos.Y - 110), Color.White);
                                else
                                    spriteBatch.DrawString(GameInfoFont, "Cost | " + levelLoader.SwitchPrice[a], new Vector2(players[playerIndex].pos.X -
                                   GameInfoFont.MeasureString("Cost |").X + GameInfoFont.MeasureString("|").X / 2 /*132*/, players[playerIndex].pos.Y - 110), Color.White);
                            }
                        }
                    }
                    for (int i = 0; i < levelLoader.WeaponRects.Count; i++)
                    {
                        if (players[playerIndex].PlayerRect.Intersects(levelLoader.WeaponRects[i]))
                        {
                            spriteBatch.Draw(players[playerIndex].UseKeyTexture, new Rectangle((int)players[playerIndex].pos.X - 22, (int)players[playerIndex].pos.Y - 190, 44, 44), Color.White);
                            String Text;
                            int Price;
                            if (players[playerIndex].weaponManager.CurentWeapon.WeaponName != levelLoader.WeaponType[i]
                                && players[playerIndex].weaponManager.SecondaryWeaponName != levelLoader.WeaponType[i])
                            {
                                Text = levelLoader.WeaponType[i];
                                Price = levelLoader.WeaponCost[i];
                            }
                            else
                            {
                                Text = levelLoader.WeaponType[i] + " " + "Ammo";
                                Price = levelLoader.AmmoCost[i];
                            }
                            spriteBatch.DrawString(GameInfoFont, "" + Text,
                                new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString("" + Text).X / 2, players[playerIndex].pos.Y - 140),
                                Color.White);
                            if(Price != 0)
                            spriteBatch.DrawString(GameInfoFont, "\n" + "Cost | " + Price,
                                new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString("Cost |").X + GameInfoFont.MeasureString("|").X / 2/*132*/, players[playerIndex].pos.Y - 140),
                                Color.White);
                        }
                    }
                   
                    for (int i = 0; i < levelLoader.ItemRect.Count; i++)
                    {
                        if (players[playerIndex].PlayerRect.Intersects(levelLoader.ItemRect[i]))
                        {
                            spriteBatch.Draw(players[playerIndex].UseKeyTexture, new Rectangle((int)players[playerIndex].pos.X - 22, (int)players[playerIndex].pos.Y - 190, 44, 44), Color.White);
                            String Text;
                            int Price;
                            
                                Text = levelLoader.ItemName[i];
                                Price = levelLoader.ItemPrice[i];
                            
                            spriteBatch.DrawString(GameInfoFont, "" + Text,
                                new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString("" + Text).X / 2, players[playerIndex].pos.Y - 140),
                                Color.White);
                            if (Price != 0)
                                spriteBatch.DrawString(GameInfoFont, "\n" + "Cost | " + Price,
                                    new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString("Cost |").X + GameInfoFont.MeasureString("|").X / 2/*132*/, players[playerIndex].pos.Y - 140),
                                    Color.White);
                        }
                    }
                    for (int i = 0; i < onTeleporters.Count; i++)
                    {
                        if (players[playerIndex].PlayerRect.Intersects(onTeleporters[i].rect)  &&  onTeleporters[i].sends)
                        {
                            if (onTeleporters[i].CurCoolDownTime > 0)
                            {
                                spriteBatch.DrawString(GameInfoFont, "\n" + "Cool Down | " + ((onTeleporters[i].CurCoolDownTime / 1000) + 1),
                                            new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString("Cool Down |").X + GameInfoFont.MeasureString("|").X / 2/*132*/, players[playerIndex].pos.Y - 140),
                                            Color.White);
                            }
                            else
                            {
                                int Price = onTeleporters[i].Price;
                                if (Price != 0)
                                {
                                    spriteBatch.DrawString(GameInfoFont, "\n" + "Cost | " + Price,
                                    new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString("Cost |").X + GameInfoFont.MeasureString("|").X / 2/*132*/, players[playerIndex].pos.Y - 140),
                                    Color.White);
                                }
                                else
                                {
                                    spriteBatch.DrawString(GameInfoFont, "\n" + "Teleport",
                                    new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString("Teleport").X / 2/*132*/, players[playerIndex].pos.Y - 140),
                                    Color.White);
                                }
                            }
                        }
                    }
                    for (int i = 0; i < levelLoader.teleporter.Count; i++)
                    {
                        if (players[playerIndex].PlayerRect.Intersects(levelLoader.teleporter[i].rect))
                        {

                            String Text;
                            String Text2;


                            if (levelLoader.teleporter[i].sends)
                            {
                                spriteBatch.Draw(players[playerIndex].UseKeyTexture, new Rectangle((int)players[playerIndex].pos.X - 22, (int)players[playerIndex].pos.Y - 190, 44, 44), Color.White);
                                Text = "Sender [CH " + levelLoader.teleporter[i].Channel + "]";
                               
                                //if (levelLoader.teleporter[i].on)
                                //{
                                //    Text = "Teleport [CH " + levelLoader.teleporter[i].Channel + "]";
                                //    int Price = levelLoader.teleporter[i].Price;
                                //    if(levelLoader.teleporter[i].CoolDownTime <= 0)
                                //        if (levelLoader.teleporter[i].Price != 0)
                                //        {
                                //            Text2 = "Cost | " + Price;
                                //            spriteBatch.DrawString(GameInfoFont, "\n" + "Cost | " + Price,
                                //            new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString("Cost |").X + GameInfoFont.MeasureString("|").X / 2/*132*/, players[playerIndex].pos.Y - 140),
                                //            Color.White);
                                //        }
                                //        else
                                //        {
                                //            spriteBatch.DrawString(GameInfoFont, "\n" + "Cool Down | " + levelLoader.teleporter[i].CurCoolDownTime,
                                //         new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString("Cool Down |").X + GameInfoFont.MeasureString("|").X / 2/*132*/, players[playerIndex].pos.Y - 140),
                                //         Color.White);
                                //        }
                                //}
                                spriteBatch.DrawString(GameInfoFont, "" + Text,
                           new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString(Text).X / 2, players[playerIndex].pos.Y - 140),
                           Color.White);
                            }
                            else
                            {
                                Text = "Receiver [CH " + levelLoader.teleporter[i].Channel + "]";
                                spriteBatch.DrawString(GameInfoFont, "" + Text,
                            new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString(Text).X / 2, players[playerIndex].pos.Y - 140),
                            Color.White);
                            }
                                if (!levelLoader.teleporter[i].on)
                                {
                                    if(!levelLoader.teleporter[i].sends)
                                        spriteBatch.Draw(players[playerIndex].UseKeyTexture, new Rectangle((int)players[playerIndex].pos.X - 22, (int)players[playerIndex].pos.Y - 190, 44, 44), Color.White);
                                    Text2 = "Turn On";
                                    spriteBatch.DrawString(GameInfoFont, "\n" + Text2,
                                new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString(Text2).X / 2/*132*/, players[playerIndex].pos.Y - 140),
                                Color.White);
                                }
                        }
                    }
                    if (players[playerIndex].OnMusicSpace)
                    {
                        spriteBatch.Draw(players[playerIndex].UseKeyTexture, new Rectangle((int)players[playerIndex].pos.X - 22, (int)players[playerIndex].pos.Y - 190, 44, 44), Color.White);
                        spriteBatch.DrawString(GameInfoFont, "Use Jukebox", new Vector2(players[playerIndex].pos.X -
                            GameInfoFont.MeasureString("Use Jukebox").X / 2/*132*/, players[playerIndex].pos.Y - 140), Color.White);
                    }
                    for (int i = 0; i < levelLoader.StoreRect.Count; i++)
                    {
                        if (players[playerIndex].PlayerRect.Intersects(levelLoader.StoreRect[i])  &&  !players[playerIndex].usingShop)
                        {
                            spriteBatch.Draw(players[playerIndex].UseKeyTexture, new Rectangle((int)players[playerIndex].pos.X - 22, (int)players[playerIndex].pos.Y - 190, 44, 44), Color.White);
                            spriteBatch.DrawString(GameInfoFont, "Shop",
                                new Vector2(players[playerIndex].pos.X - GameInfoFont.MeasureString("Shop").X / 2, players[playerIndex].pos.Y - 140),
                                Color.White);
                        }
                    }
                }
                spriteBatch.End();
    
                spriteBatch.Begin();
                    spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color((int)Math.Abs(spawnEnemies.SirenAlpha), 0, 0, (int)Math.Abs(spawnEnemies.SirenAlpha)));
                    players[playerIndex].TeleportFade = spawnEnemies.CoolDownNumber(1, 0, players[playerIndex].TeleportFade);
                    spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(players[playerIndex].TeleportFade,
                        players[playerIndex].TeleportFade,
                        players[playerIndex].TeleportFade,
                        players[playerIndex].TeleportFade));
                    if (players[playerIndex].GasMaskOn)
                    {
                        spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(20, 20, 20, 15));
                        if (!DrawBorders)
                            spriteBatch.Draw(players[playerIndex].GasMaskEffect, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(20, 20, 20, 150));
                        else
                            spriteBatch.Draw(players[playerIndex].GasMaskEffect, new Rectangle(graphics.Viewport.Width / 6, 0, graphics.Viewport.Width - (graphics.Viewport.Width / 6) - (graphics.Viewport.Width / 6), graphics.Viewport.Height), new Color(20, 20, 20, 150));
                    }
                
                    if (DrawBorders)
                    {
                        spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width / 6, graphics.Viewport.Height), Color.Black);
                        spriteBatch.Draw(square, new Rectangle(graphics.Viewport.Width - graphics.Viewport.Width / 6, 0, graphics.Viewport.Width / 6, graphics.Viewport.Height), Color.Black);
                    }

                    //#Cinema
                    if (!CinemaMode)
                    {
                        if (!players[playerIndex].dead || (players[playerIndex].dead && players[playerIndex].DisplayHud))
                            spriteBatch.DrawString(_font, "Wave: " + spawnEnemies.Wave, new Vector2(10 * SpriteScale, 30 * SpriteScale), Color.White);
                        spriteBatch.DrawString(font, "Wave " + (spawnEnemies.Wave - 1) + " Cleared",
                            new Vector2(graphics.Viewport.Width / 2 - (font.MeasureString("Wave " + (spawnEnemies.Wave - 1) + " Cleared").X / 2), 50),
                            new Color(spawnEnemies.NewWaveFade, spawnEnemies.NewWaveFade, spawnEnemies.NewWaveFade, spawnEnemies.NewWaveFade));

                        if (spawnEnemies.Mutate && !spawnEnemies.FinishedMutating && !players[playerIndex].GasMaskOn && !players[playerIndex].GasMaskPerk)
                            spriteBatch.DrawString(font, "GAS MASK ON!", new Vector2(graphics.Viewport.Width / 2
                                - ((font.MeasureString("GAS MASK ON!")).X / 2), 100), new Color(255, 255, 255));

                        players[playerIndex].DrawGUI(spriteBatch, _font, SpriteScale, viewport, players, smallFont);
                        for (int i = 0; i < pointText.Count; i++)
                            if (pointText[i].Player == playerIndex)
                                spriteBatch.DrawString(secondaryFont, pointText[i].points, new Vector2(pointText[i].GUIpos.X, pointText[i].GUIpos.Y - (secondaryFont.MeasureString(pointText[i].points).Y / 2)), new Color(125, 125, 50, 10));
                    }
                    if (_doPlay)
                    {
                        if (GameFadeValue > 0)
                        {
                            spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(0, 0, 0, GameFadeValue));
                            GameFadeValue--;
                        }
                    }
                    else
                        GameFadeValue = 255;
                if(paused && playerIndex == Player_paused  &&  !GameOver)
                    DrawPauseScreen(spriteBatch);
            spriteBatch.End();

        }
        private void DrawGUIs(SpriteBatch spriteBatch, Viewport viewPort, Viewport playerViewPort)
        {
            graphics.Viewport = viewPort;
            spriteBatch.Begin();
            for (int a = 0; a < activePowerUps.Count; a++)
            {
                if (activePowerUps[a] == "Speed Boost")
                    DrawPowerUp(spriteBatch, speedBoost, SpeedBoostSec, a);
                if (activePowerUps[a] == "Insta Kill")
                    DrawPowerUp(spriteBatch, instaKill, InstaKillSec, a);
                if (activePowerUps[a] == "Death Touch")
                    DrawPowerUp(spriteBatch, deathTouch, DeathTouchSec, a);
                if (activePowerUps[a] == "Double Points")
                    DrawPowerUp(spriteBatch, doublePoints, DoublePointsSec, a);
            }
            //challengeSystem.Draw(spriteBatch, graphics.Viewport);//, font);
            spriteBatch.End();
            graphics.Viewport = playerViewPort;
        }
        private void DrawPowerUp(SpriteBatch spriteBatch, Texture2D effectT, int effectSec, int a)
        {
            if (effectSec > 0)
            {
                int StartY = 275;
                int x = graphics.Viewport.Width - 75;
                if (effectSec > 100)
                    spriteBatch.Draw(effectT, new Rectangle(x, StartY + (a * 60), 50, 50), Color.White);
                else
                    if (effectSec % 10 < 5)
                        spriteBatch.Draw(effectT, new Rectangle(x, StartY + (a * 60), 50, 50), Color.White);
                spriteBatch.DrawString(font, "" + (1 + (effectSec / 10)), new Vector2(x - 20, StartY + (a * 60)), Color.White);
            }
        }
        private void DrawDrop(SpriteBatch spriteBatch, Rectangle rect, Texture2D texure, int Time, int TimeTillBlink, int BlinkTime)
        {
            int Time_Second = (int)(Time / 1000);
            if (Time > 0)
            {
                int x = graphics.Viewport.Width - 75;
                if (Time_Second < TimeTillBlink)
                    spriteBatch.Draw(texure, rect, Color.White);
                else
                    if (Time % BlinkTime < (BlinkTime / 2))
                        spriteBatch.Draw(texure, rect, Color.White);
            }
        }


        public int PauseMenu_SelectedOption = 0;
        Vector2 Prev_MousePos;
        public void UpdatePauseMenu()
        {
            if (players.Count > 0)
            {
                //if (!InOptions)
                //{
                //    if (PauseMenu_SelectedOption <= 0)
                //        PauseMenu_SelectedOption = 0;
                //    if (PauseMenu_SelectedOption > 2)
                //        PauseMenu_SelectedOption = 2;
                //}
                if (!GameOver)
                {
                    if (players.Count == 1)//players[Player_paused].PC)
                    {

                        if (Prev_MousePos != new Vector2(Mouse.GetState().X, Mouse.GetState().Y))
                        {
                            for (int i = 0; i < PC_PauseButton.Length; i++)
                            {
                                if (players[Player_paused].kInput.MouseRect.Intersects(PC_PauseButton[i]))
                                    PauseMenu_SelectedOption = i;
                            }
                            Prev_MousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                        }

                    }
                    if (!InOptions)
                    {
                        if ((players[Player_paused].pInput.GUI_Up || players[Player_paused].kInput.GUI_Up) && PauseMenu_SelectedOption > 0)
                            PauseMenu_SelectedOption--;
                        if ((players[Player_paused].pInput.GUI_Down || players[Player_paused].kInput.GUI_Down) && PauseMenu_SelectedOption < 2)
                            PauseMenu_SelectedOption++;

                        if (players[Player_paused].pInput.BackKey)
                        {
                            paused = false;
                        }

                        if ((players[Player_paused].pInput.SelectKey || players[Player_paused].kInput.GUI_Select))
                        {
                            Boolean go = true;
                            if (players[Player_paused].kInput.GUI_Select && !players[Player_paused].kInput.GUI_Enter /*&& players[Player_paused].PC*/ &&  players.Count == 1)
                                if (!players[Player_paused].kInput.MouseRect.Intersects(PC_PauseButton[PauseMenu_SelectedOption]))
                                    go = false;
                            if (go)
                            {
                                if (PauseMenu_SelectedOption == 0)
                                {
                                    paused = false;
                                }
                                if (PauseMenu_SelectedOption == 1)
                                {
                                    InOptions = true;
                                    PauseMenu_SelectedOption = 0;
                                }
                                if (PauseMenu_SelectedOption == 2)
                                {
                                    quit = true;
                                }
                            }

                        }

                        
                    }
                    else
                    {
                        if ((players[Player_paused].pInput.GUI_Up || players[Player_paused].kInput.GUI_Up) && PauseMenu_SelectedOption > 0)
                            PauseMenu_SelectedOption--;
                        if ((players[Player_paused].pInput.GUI_Down || players[Player_paused].kInput.GUI_Down) && PauseMenu_SelectedOption < 6)
                            PauseMenu_SelectedOption++;

                        if (players[Player_paused].pInput.BackKey || players[Player_paused].kInput.BackwardsKey)
                        {
                            InOptions = false;
                            PauseMenu_SelectedOption = 0;
                        }

                        if ((players[Player_paused].pInput.SelectKey || players[Player_paused].kInput.GUI_Select))
                        {
                            Boolean go = true;
                            if (players[Player_paused].kInput.GUI_Select && !players[Player_paused].kInput.GUI_Enter &&  players.Count == 1/* && players[Player_paused].PC*/)
                                if (!players[Player_paused].kInput.MouseRect.Intersects(PC_PauseButton[PauseMenu_SelectedOption]))
                                    go = false;
                            if (go)
                            {
                                if (PauseMenu_SelectedOption == 0)
                                {
                                    _options.ToggleDrawPlayerCircles();
                                }
                                if (PauseMenu_SelectedOption == 1)
                                {
                                    _options.ToggleFullScreen();
                                }
                                if (PauseMenu_SelectedOption == 2)
                                {
                                    _options.ToggleVibrateController();
                                }
                                if (PauseMenu_SelectedOption == 3)
                                {
                                    _options.ControllerEnabled = !_options.ControllerEnabled;
                                }
                                if (PauseMenu_SelectedOption == 6)
                                {
                                    InOptions = false;
                                    PauseMenu_SelectedOption = 0;
                                }

                            }
                        }
                            if (PauseMenu_SelectedOption == 4)
                            {
                                if ((players[Player_paused].pInput.GUI_Hold_Left || players[Player_paused].kInput.GUI_Left_Hold) && _options.MusicVolume > 0)
                                    _options.MusicVolume--;
                                if ((players[Player_paused].pInput.GUI_Hold_Right || players[Player_paused].kInput.GUI_Right_Hold) && _options.MusicVolume < 100)
                                    _options.MusicVolume++;
                            }
                            if (PauseMenu_SelectedOption == 5)
                            {
                                if ((players[Player_paused].pInput.GUI_Hold_Left || players[Player_paused].kInput.GUI_Left_Hold) && _options.SoundEffectsVolume > 0)
                                    _options.SoundEffectsVolume--;
                                if ((players[Player_paused].pInput.GUI_Hold_Right || players[Player_paused].kInput.GUI_Right_Hold) && _options.SoundEffectsVolume < 100)
                                    _options.SoundEffectsVolume++;
                            }
                        
                    }
                }
                //if (InOptions)
                //{
                //    PauseMenu_SelectedOption = 0;
                //    if(players[Player_paused].pInput.BackKey ||  players[Player_paused].kInput.GUI_Back)
                //        InOptions = false;
                //}
            }
        }
        private void DrawPauseScreen(SpriteBatch spriteBatch)
        {

            //72, 61, 139, 210
            spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(25, 25, 25, 175));//Color.DarkSlateBlue);//72 61, 139, 255
            spriteBatch.DrawString(font, "Game Paused", new Vector2(50, 50), Color.White);
            Color SelectedColor = Color.LightGray;
            if (!InOptions)
            {
                PC_PauseButton = new Rectangle[3];
                PC_PauseButton[0] = new Rectangle(50, 65 + (int)font.MeasureString(".").Y, 450, (int)font.MeasureString(".").Y);
                if (PauseMenu_SelectedOption == 0)
                    spriteBatch.DrawString(font, "> Resume", new Vector2(50, 65 + font.MeasureString(".").Y), SelectedColor);//Yellowy
                else
                    spriteBatch.DrawString(font, "Resume", new Vector2(50, 65 + font.MeasureString(".").Y), Color.White);

                PC_PauseButton[1] = new Rectangle(50, 65 + (int)font.MeasureString(".").Y * 2, 450, (int)font.MeasureString(".").Y);
                if (PauseMenu_SelectedOption == 1)
                    spriteBatch.DrawString(font, "> Options", new Vector2(50, 65 + font.MeasureString(".").Y * 2), SelectedColor);
                else
                    spriteBatch.DrawString(font, "Options", new Vector2(50, 65 + font.MeasureString(".").Y * 2), Color.White);

                PC_PauseButton[2] = new Rectangle(50, 65 + (int)font.MeasureString(".").Y * 3, 450, (int)font.MeasureString(".").Y);
                if (PauseMenu_SelectedOption == 2)
                    spriteBatch.DrawString(font, "> Quit", new Vector2(50, 65 + font.MeasureString(".").Y * 3), SelectedColor);
                else
                    spriteBatch.DrawString(font, "Quit", new Vector2(50, 65 + font.MeasureString(".").Y * 3), Color.White);
            }
            else
            {
                PC_PauseButton = new Rectangle[7];
                for (int i = 0; i < PC_PauseButton.Length; i++)
                {
                    PC_PauseButton[i] = new Rectangle(50, 65 + (int)font.MeasureString(".").Y * (i + 1), 450, (int)font.MeasureString(".").Y);
                }


                if (PauseMenu_SelectedOption == 0)
                {
                    spriteBatch.DrawString(font, "> Player Circles", new Vector2(50, 65 + font.MeasureString(".").Y * (PauseMenu_SelectedOption + 1)), SelectedColor);
                    spriteBatch.DrawString(font, TF_Label(_options.DrawPlayerCircles, "Disabled", "Enabled"), new Vector2(500, 65 + font.MeasureString(".").Y * (PauseMenu_SelectedOption + 1)), SelectedColor);
                }
                else
                    spriteBatch.DrawString(font, "Player Circles", new Vector2(50, 65 + font.MeasureString(".").Y * 1), Color.White);


                if (PauseMenu_SelectedOption == 1)
                {
                    spriteBatch.DrawString(font, "> Full Screen", new Vector2(50, 65 + font.MeasureString(".").Y * (PauseMenu_SelectedOption + 1)), SelectedColor);
                    spriteBatch.DrawString(font, TF_Label(_options.FullScreen, "Disabled", "Enabled"), new Vector2(500, 65 + font.MeasureString(".").Y * (PauseMenu_SelectedOption + 1)), SelectedColor);
                }
                else
                    spriteBatch.DrawString(font, "Full Screen", new Vector2(50, 65 + font.MeasureString(".").Y * 2), Color.White);

                if (PauseMenu_SelectedOption == 2)
                {
                    spriteBatch.DrawString(font, "> Controller Vibration", new Vector2(50, 65 + font.MeasureString(".").Y * (PauseMenu_SelectedOption + 1)), SelectedColor);
                    spriteBatch.DrawString(font, TF_Label(_options.ControllerVibration, "Disabled", "Enabled"), new Vector2(500, 65 + font.MeasureString(".").Y * (PauseMenu_SelectedOption + 1)), SelectedColor);
                }
                else
                    spriteBatch.DrawString(font, "Controller Vibration", new Vector2(50, 65 + font.MeasureString(".").Y * 3), Color.White);


                if (PauseMenu_SelectedOption == 3)
                {
                    spriteBatch.DrawString(font, "> GamePad", new Vector2(50, 65 + font.MeasureString(".").Y * (PauseMenu_SelectedOption + 1)), SelectedColor);
                    spriteBatch.DrawString(font, TF_Label(_options.ControllerEnabled, "Disabled", "Enabled"), new Vector2(500, 65 + font.MeasureString(".").Y * (PauseMenu_SelectedOption + 1)), SelectedColor);
                }
                else
                    spriteBatch.DrawString(font, "GamePad", new Vector2(50, 65 + font.MeasureString(".").Y * 4), Color.White);

                if (PauseMenu_SelectedOption == 4)
                {
                    spriteBatch.DrawString(font, "> Music Volume", new Vector2(50, 65 + font.MeasureString(".").Y * (PauseMenu_SelectedOption + 1)), SelectedColor);
                    spriteBatch.DrawString(font, _options.MusicVolume + "%", new Vector2(500, 65 + font.MeasureString(".").Y * (PauseMenu_SelectedOption + 1)), SelectedColor);
                }
                else
                    spriteBatch.DrawString(font, "Music Volume", new Vector2(50, 65 + font.MeasureString(".").Y * 5), Color.White);

                if (PauseMenu_SelectedOption == 5)
                {
                    spriteBatch.DrawString(font, "> SFX Volume", new Vector2(50, 65 + font.MeasureString(".").Y * (PauseMenu_SelectedOption + 1)), SelectedColor);
                    spriteBatch.DrawString(font, _options.SoundEffectsVolume + "%", new Vector2(500, 65 + font.MeasureString(".").Y * (PauseMenu_SelectedOption + 1)), SelectedColor);
                }
                else
                    spriteBatch.DrawString(font, "SFX Volume", new Vector2(50, 65 + font.MeasureString(".").Y * 6), Color.White);

                if (players.Count == 1)
                    if (PauseMenu_SelectedOption == 6)
                        spriteBatch.DrawString(font, "> Back", new Vector2(50, 65 + font.MeasureString(".").Y * (PauseMenu_SelectedOption + 1)), SelectedColor);
                    else
                        spriteBatch.DrawString(font, "Back", new Vector2(50, 65 + font.MeasureString(".").Y * 7), Color.White);
            }
        }


        private String TF_Label(Boolean toggle, String falseLabel, String trueLabel)
        {
            if (toggle)
                return trueLabel;
            else
                return falseLabel;
        }

        public void ResetGame()
        {
            while (spawnEnemies.enemy.Count > 0) spawnEnemies.enemy.RemoveAt(0);
            if(spawnEnemies.enemy.Count <= 0)
                while (players.Count > 0) players.RemoveAt(0);
           
        }
    }
}
