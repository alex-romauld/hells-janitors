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
    class Weapon
    {
        public Texture2D BulletTexture;
        public Texture2D GUITexture;
        public Texture2D PlayerStanceTexture;
        public SoundEffect shotSound;
        public int ShotTime;
        public int BulletDamage;
        public int BulletSpeed;
        public int BulletLife;
        public String WeaponName;
        public String BulletType;
        public ParticleEffect hitEffect = null;
        public SoundEffect HitSound = null;
        public int HitPoints;
        public int KillPoints;
        public int Ammo;
        public int MaxAmmo;
        public int AmmoLossRate;
        public int MinPitch;
        public int MaxPitch;

        public Weapon(int shotTime, int damage, int speed, int life, String WeaponName, String BulletType, int HitPoints, int KillPoints, int Ammo, int AmmoLossRate, int MinPitch, int MaxPitch)
        {
            this.ShotTime = shotTime;
            this.BulletDamage = damage;
            this.BulletSpeed = speed;
            this.BulletLife = life;
            this.WeaponName = WeaponName;
            this.BulletType = BulletType;
            this.HitPoints = HitPoints;
            this.KillPoints = KillPoints;
            this.Ammo = Ammo;
            this.MaxAmmo = Ammo;
            this.AmmoLossRate = AmmoLossRate;
            this.MinPitch = MinPitch;
            this.MaxPitch = MaxPitch;
        }

        public void Load(ContentManager content, String TexturePath, String SoundPath, String GUIPath, String PlayerStancePath, String HitSoundPath)
        {
            this.BulletTexture = content.Load<Texture2D>(TexturePath);
            this.shotSound = content.Load<SoundEffect>(SoundPath);
            this.GUITexture = content.Load<Texture2D>(GUIPath);
            PlayerStanceTexture = content.Load<Texture2D>(PlayerStancePath);
            if(HitSoundPath != null)
                this.HitSound = content.Load<SoundEffect>(HitSoundPath);
        }
    }
}
