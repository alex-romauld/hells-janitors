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
    class Projectile
    {
        public Vector2 Pos;
        public float Rot;
        public Vector2 Dir;
        public int Life;
        public int LifeTime;
        public int PlayerFired;
        public int Damage;
        public int Speed;
        public Texture2D texture;
        public String type;
        public Rectangle Rect;
        public SoundEffect hitSound;
        public int HitPoints;
        public int KillPoints;

        public Projectile(Vector2 Pos, float Rot, Vector2 Dir, int Life, int LifeTime, int PlayerFired, int Damage, int Speed, Texture2D texture, String type, SoundEffect hitSound, int HitPoints, int KillPoints)
        {
            this.Pos = Pos;
            this.Rot = Rot;
            this.Dir = Dir;
            this.Life = Life;
            this.LifeTime = LifeTime;
            this.PlayerFired = PlayerFired;
            this.Damage = Damage;
            this.Speed = Speed;
            this.texture = texture;
            this.type = type;
            this.Rect = new Rectangle((int)Pos.X - (texture.Height / 2) + 1,
                            (int)Pos.Y - (texture.Height / 2) + 1,
                            (int)texture.Height - 2,
                            (int)texture.Height - 2);
            this.hitSound = hitSound;
            this.HitPoints = HitPoints;
            this.KillPoints = KillPoints;
        }       

        public void Update()
        {
            Pos += Dir * Speed;
            Life += 1;
            Rect = new Rectangle((int)Pos.X - (texture.Height / 2) + 1,
                            (int)Pos.Y - (texture.Height / 2) + 1,
                            (int)texture.Height - 2,
                            (int)texture.Height - 2);
        }

        //public void Draw(SpriteBatch spriteBatch)
        //{

        //}
    }
}
