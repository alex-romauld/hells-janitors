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

using SurvivalShooter.StandardGame;

namespace SurvivalShooter
{
    class WeaponManager
    {

        public String Cur_Weapon = "";

        public Texture2D Cur_WeaponTexture;
        public SoundEffect Cur_WeaponSound;
        public int Cur_SoundMinPitch;
        public int Cur_SoundMaxPitch;
        public Texture2D Cur_playerStance;
        public int Cur_WeaponShotTime = 5;
        public int BulletSpeed = 55;
        public int BulletDamage = 10;
        public int BulletLife = 250;
        public String BulletType = "";
        public SoundEffect BulletHitSound = null;
        public int HitPoints;
        public int KillPoints;

        private Weapon Pistol = new Weapon(400, 30, 60, 250, "Pistol", "Standard", 15, 100, 1000 , 1, -15, 15);//Standard 20 //317
        private Weapon Rifle = new Weapon(300, 35, 65, 250, "Rifle", "Standard", 10, 100, 250, 1, -7, 7);//25 //396

        private Weapon ShotGun = new Weapon(476, 50, 85, 150, "Shotgun", "ShotGun", 5, 100, 100, 1, -7, 7);
        private Weapon AutoShotGun = new Weapon(158, 10, 70, 150, "Auto-Shotty", "Auto-Shotty", 5, 50, 225, 1, -7, 7);

        private Weapon MachineGun = new Weapon(79, 10, 55, 250, "Machine Gun", "Standard", 5, 75, 450, 1, -6, 9);
        private Weapon DualUZIs = new Weapon(63, 6/*3*/, 55, 250, "Dual UZIs", "Dual", 5, 50, 900, 2, -7, 7);

        private Weapon GatlingGun = new Weapon(32, 8, 55, 250, "Gatling Gun", "Standard", 2, 50, 750, 1, -7, 7);

        private Weapon FreezeRay = new Weapon(190, 22, 65, 200, "Freeze Ray", "Freeze", 10, 100, 350, 1, -7, 7);

        private Weapon Flamethrower = new Weapon(47, 5, 14, 300, "Flamethrower", "_Standard", 5, 100, 500, 1, -7, 7);//Rosery // 30 life
        private Weapon PotatoGun = new Weapon(158, 5, 40, 200, "Potato Gun", "Standard", 5, 100, 250, 1, -7, 7);
        private Weapon RayGun = new Weapon(634, 125, 50, 250, "Ray Gun", "Standard", 10, 100, 75, 1, -7, 7);
        private Weapon Mines = new Weapon(1507, 200, 0, 5000, "Land Mines", "Explosive", 10, 100, 25, 1, -7, 7);
        private Weapon Bow = new Weapon(396, 40, 75, 30, "Bow and Arrow", "Standard", 10, 100, 100, 1, -7, 7);
        private Weapon RocketLauncher = new Weapon(1428, 200, 75, 250, "Rocket Launcher", "Rocket", 10, 100, 75, 1, -7, 7);
        private Weapon Tanker = new Weapon(111, 500, 40, 4550, "Tanker", "Rocket", 10, 100, 75, 1, -7, 7);//50 for speed ||  40  || 1

        private List<Weapon> RandomWeapons = new List<Weapon>();
        //Potato Gun
        //Ray's Gun

        public Weapon CurentWeapon;
        public Weapon SecondaryWeapon;
        public String SecondaryWeaponName = "";

        private Input pInput;
        public Boolean SwitchedWeapon = false;

        public Boolean updateText = false;

        public WeaponManager()
        {

        }

        public void ResetWeapons()
        {
            CurentWeapon = Pistol;//Pistol
            SecondaryWeapon = null;// null;

            CurentWeapon.Ammo = CurentWeapon.MaxAmmo;
            if(SecondaryWeapon != null)
                SecondaryWeapon.Ammo = SecondaryWeapon.MaxAmmo;
        }
        public void Load(ContentManager content, PlayerIndex player)
        {
            Texture2D tempTexture = content.Load<Texture2D>("Sprites/Missile1");
            Cur_WeaponTexture = tempTexture;
            Pistol.Load(content, "Sprites/Missile1", "SoundFX/GunShots/RevolverGunSE", "GUI/Pistol", "Sprites/Player/PlayerPack1/player_Pistol", null);
            Rifle.Load(content, "Sprites/Missile1", "SoundFX/GunShots/RifleSE", "GUI/Rifle", "Sprites/Player/PlayerPack1/player_Rifle", null);

            ShotGun.Load(content, "Sprites/Missile1", "SoundFX/GunShots/ShotGun", "GUI/ShotGun", "Sprites/Player/PlayerPack1/player_Shotgun", null);
            AutoShotGun.Load(content, "Sprites/Missile1", "SoundFX/GunShots/ShotGun", "GUI/AutoShotty", "Sprites/Player/PlayerPack1/player_AutoShotty", null);

            MachineGun.Load(content, "Sprites/Missile1", /*"SoundFX/GunShots/MachineGunSE"*/"SoundFX/MachineGun_GunShotSE", "GUI/MachineGun", "Sprites/Player/PlayerPack1/player_MachineGun", null);
            DualUZIs.Load(content, "Sprites/Missile1", /*"SoundFX/GunShots/MachineGunSE"*/"SoundFX/GunShots/Uzi", "GUI/DualUzi", "Sprites/Player/PlayerPack1/player_Dual", null);

            GatlingGun.Load(content, "Sprites/Missile1", /*"SoundFX/GunShots/GatlingGunSE"*/"SoundFX/MachineGun_GunShotSE", "GUI/GatlingGun", "Sprites/Player/PlayerPack1/player_GatlingGun", null);

            FreezeRay.Load(content, "Sprites/Bullets/Ice1", "SoundFX/GunShots/FreezeRay", "GUI/FreezeRay", "Sprites/Player/PlayerPack1/player_FreezeRay", null);

            Flamethrower.Load(content, "Sprites/Flame", "AK_Gun_Sound", "GUI/FlameThrower", "Sprites/Player/PlayerPack1/player_FlameThrower", null);
            PotatoGun.Load(content, "Sprites/Potato", "Launcher", "GUI/PotatoLauncher", "Sprites/Player/PlayerPack1/player_PotatoLauncher", null);
            RayGun.Load(content, "Sprites/Bullet_RayGun", "SoundFX/GunShots/FreezeRay", "GUI/RayGun", "Sprites/Player/PlayerPack1/player_RayGun", null);
            Mines.Load(content, "Sprites/Bullets/LandMine", "AK_Gun_Sound", "GUI/LandMine", "Sprites/Player/PlayerPack1/player_LandMine", "SoundFX/Mine_ExplosionSFX");
            Bow.Load(content, "Sprites/Bullets/Arrow", "AK_Gun_Sound", "Sprites/Bullets/arrow", "Sprites/Player/PlayerPack1/player_Pistol", null);
            RocketLauncher.Load(content, "Sprites/Bullets/Rocket", "AK_Gun_Sound", "GUI/RocketLauncher", "Sprites/Player/PlayerPack1/player_RocketLauncher", "SoundFX/Mine_ExplosionSFX");
            Tanker.Load(content, "Sprites/Missile1", "AK_Gun_Sound", "GUI/Tanker", "Sprites/Player/PlayerPack1/player_Tanker", "SoundFX/Mine_ExplosionSFX");

            AddRandomWeapon(RocketLauncher, 2);
            AddRandomWeapon(PotatoGun, 3);
            //AddRandomWeapon(Bow, 3);
            AddRandomWeapon(RayGun, 1);
            AddRandomWeapon(Flamethrower, 2);
            AddRandomWeapon(Pistol, 3);
            AddRandomWeapon(Mines, 3);
            AddRandomWeapon(Tanker, 1);
            AddRandomWeapon(GatlingGun, 1);
            AddRandomWeapon(DualUZIs, 3);
            AddRandomWeapon(FreezeRay, 1);
            AddRandomWeapon(MachineGun, 1);
            AddRandomWeapon(ShotGun, 1);
            AddRandomWeapon(AutoShotGun, 3);


            ResetWeapons();
            UpdateToWeapon(CurentWeapon);

            pInput = new Input(player);
        }
        private void AddRandomWeapon(Weapon weapon, int Chance)
        {
            int AddedTimes = 0;
            while (AddedTimes < Chance)
            {
                RandomWeapons.Add(weapon);
                AddedTimes++;
            }
        }
        Boolean setUp = false;
        public void PerkBought()
        {
            if (!setUp)
            {
                AddRandomWeapon(FreezeRay, 3);
                AddRandomWeapon(GatlingGun, 3);
                AddRandomWeapon(RayGun, 3);
                AddRandomWeapon(AutoShotGun, 1);
                AddRandomWeapon(DualUZIs, 2);
                AddRandomWeapon(Tanker, 1);
                setUp = true;
            }
        }
        public void Update(Boolean SwitchWeaponKey, Boolean SwitchWeaponButton, Boolean PC)
        {
            pInput.Update(false, PC);
            if (SwitchWeaponButton || SwitchWeaponKey)
            {
                if (SecondaryWeapon != null)
                {
                    Weapon tempWeapon = CurentWeapon;
                    CurentWeapon = SecondaryWeapon;
                    SecondaryWeapon = tempWeapon;
                    SwitchedWeapon = true;
                    updateText = true;
                }
            }
            UpdateToWeapon(CurentWeapon);


            if (SecondaryWeapon == null)
            {
                SecondaryWeaponName = "";
            }
            else
            {
                SecondaryWeaponName = SecondaryWeapon.WeaponName;
            }
        }

        public void BoughtWeapon(String weapon)
        {
            if (SecondaryWeapon == null)
            {
                SecondaryWeapon = CurentWeapon;

                if (weapon == "Machine Gun")
                {
                    CurentWeapon = MachineGun;
                }
                if (weapon == "Gatling Gun")
                {
                    CurentWeapon = GatlingGun;
                }
                if (weapon == "Rifle")
                {
                    CurentWeapon = Rifle;
                }
                if (weapon == "Shotgun")
                {
                    CurentWeapon = ShotGun;
                }
                if (weapon == "Auto-Shotty")
                {
                    CurentWeapon = AutoShotGun;
                }
                if (weapon == "Pistol")
                {
                    CurentWeapon = Pistol;
                }
                if (weapon == "Dual UZIs")
                {
                    CurentWeapon = DualUZIs;
                }
                if (weapon == "Rocket Launcher")
                {
                    CurentWeapon = RocketLauncher;
                }
                if (weapon == "Tanker")
                {
                    CurentWeapon = Tanker;
                }
                if (weapon == "Land Mines")
                    CurentWeapon = Mines;
                if (weapon == "Flamethrower")
                    CurentWeapon = Flamethrower;
                if (weapon == "Ray Gun")
                    CurentWeapon = RayGun;
                if (weapon == "Freeze Ray")
                    CurentWeapon = FreezeRay;
                if (weapon == "Bow and Arrow")
                    CurentWeapon = Bow;
                if (weapon == "Potato Gun")
                    CurentWeapon = PotatoGun;

                if (weapon == "Random Weapon")
                {
                    Random random = new Random();
                    int _randweapon = random.Next(0, RandomWeapons.Count);
                    while (CurentWeapon == RandomWeapons[_randweapon])
                    {
                        random = new Random();
                        _randweapon = random.Next(0, RandomWeapons.Count);
                    }
                    CurentWeapon = RandomWeapons[_randweapon];
                }
            }
            else if(SecondaryWeapon.WeaponName != weapon)
            {
                if (weapon == "Machine Gun")
                {
                    CurentWeapon = MachineGun;
                }
                if (weapon == "Gatling Gun")
                {
                    CurentWeapon = GatlingGun;
                }
                if (weapon == "Rifle")
                {
                    CurentWeapon = Rifle;
                }
                if (weapon == "Shotgun")
                {
                    CurentWeapon = ShotGun;
                }
                if (weapon == "Auto-Shotty")
                {
                    CurentWeapon = AutoShotGun;
                }
                if (weapon == "Pistol")
                {
                    CurentWeapon = Pistol;
                }
                if (weapon == "Dual UZIs")
                {
                    CurentWeapon = DualUZIs;
                }
                if (weapon == "Rocket Launcher")
                {
                    CurentWeapon = RocketLauncher;
                }
                if (weapon == "Tanker")
                {
                    CurentWeapon = Tanker;
                }
                if (weapon == "Land Mines")
                    CurentWeapon = Mines;
                if (weapon == "Flamethrower")
                    CurentWeapon = Flamethrower;
                if (weapon == "Ray Gun")
                    CurentWeapon = RayGun;
                if (weapon == "Freeze Ray")
                    CurentWeapon = FreezeRay;
                if (weapon == "Bow and Arrow")
                    CurentWeapon = Bow;
                if (weapon == "Potato Gun")
                    CurentWeapon = PotatoGun;
                if (weapon == "Random Weapon")
                {
                    Random random = new Random();
                    int _randweapon = random.Next(0, RandomWeapons.Count);
                    while (CurentWeapon == RandomWeapons[_randweapon] || SecondaryWeapon == RandomWeapons[_randweapon])
                    {
                        random = new Random();
                        _randweapon = random.Next(0, RandomWeapons.Count);
                    }
                    CurentWeapon = RandomWeapons[_randweapon];
                }
            }
            UpdateToWeapon(CurentWeapon);
            CurentWeapon.Ammo = CurentWeapon.MaxAmmo;
            updateText = true;
        }
        public void BoughtAmmo(String weapon)
        {
            if (CurentWeapon.WeaponName == weapon)
                CurentWeapon.Ammo = CurentWeapon.MaxAmmo;
            if (SecondaryWeaponName == weapon  &&  SecondaryWeapon != null)
                SecondaryWeapon.Ammo = SecondaryWeapon.MaxAmmo;
        }
        private void UpdateToWeapon(Weapon cur_weapon)
        {
            Cur_WeaponTexture = cur_weapon.BulletTexture;
            Cur_WeaponSound = cur_weapon.shotSound;
            Cur_SoundMinPitch = cur_weapon.MinPitch;
            Cur_SoundMaxPitch = cur_weapon.MaxPitch;
            Cur_WeaponShotTime = cur_weapon.ShotTime;
            BulletSpeed = cur_weapon.BulletSpeed;
            BulletDamage = cur_weapon.BulletDamage;
            BulletLife = cur_weapon.BulletLife;
            BulletType = cur_weapon.BulletType;
            BulletHitSound = cur_weapon.HitSound;
            Cur_Weapon = cur_weapon.WeaponName;
            Cur_playerStance = cur_weapon.PlayerStanceTexture;
            HitPoints = cur_weapon.HitPoints;
            KillPoints = cur_weapon.KillPoints;
        }
    }
}
