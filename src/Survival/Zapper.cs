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
    class Zapper
    {
        private Texture2D zapperTexture, Electricity;

        public Rectangle rect;
        public int Channel;
        public int Time = 30000;
        public int CurTime = 0;

        public Boolean active = false;
        private int ShockDistance = 300;

        public Vector2 ShockPos;
        public Boolean Fire = false;
        public int Damage;

        public int CoolDown = 0;
        public int CurCoolDown = 0;

        private List<Vector2> zapPoints = new List<Vector2>();
        private List<int> zapLife = new List<int>();

        Random random;

        public Zapper(Rectangle rect, int Channel, Texture2D zapperTexture, Texture2D Electricity, int Duration)
        {
            this.zapperTexture = zapperTexture;
            this.Electricity = Electricity;

            this.rect = rect;
            this.Channel = Channel;
            this.Time = Duration * 1000;
        }
        public void Update(List<Enemy> enemy, GameTime gameTime)
        {
            random = new Random();
            if (active)
            {
                if (CurCoolDown >= CoolDown)
                {
                    Zap(enemy);
                    CurCoolDown = 0;
                    CoolDown = random.Next(300, 1100);
                }
                CurCoolDown += gameTime.ElapsedGameTime.Milliseconds;
                CurTime += gameTime.ElapsedGameTime.Milliseconds;
               // zapPoints.Clear();
            }
            else
            {
                CurTime = 0;
                CurCoolDown = 0;
                CoolDown = 0;
                zapPoints.Clear();
                zapLife.Clear();
            }
            for (int i = 0; i < zapLife.Count; i++)
            {
                zapLife[i]++;
                if (zapLife[i] >= 4)
                {
                    zapLife.RemoveAt(i);
                    zapPoints.RemoveAt(i);
                }
            }
        }
        private void Zap(List<Enemy> enemy)
        {
            for (int i = 0; i < enemy.Count; i++)
            {
                //if (enemy[i].EnemyRect.Intersects(rect))
                if (Vector2.Distance(enemy[i].pos, new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height)) <= rect.Width * 2)
                {
                    ShockPos = enemy[i].pos;
                    Fire = true;
                    Damage = enemy[i].Health;
                    zapPoints.Add(enemy[i].pos);
                    zapLife.Add(0);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if ((CurCoolDown >= CoolDown || CurCoolDown == 0) && active)
                spriteBatch.Draw(zapperTexture, rect, Color.SkyBlue);
            else
                spriteBatch.Draw(zapperTexture, rect, Color.White);
        }
        public void DrawElectricity(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < zapPoints.Count; i++)
                DrawLine(spriteBatch, Electricity, new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2), zapPoints[i]);
        }

        private void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end)
        {
            spriteBatch.Draw(texture, start, null, Color.White,
                             (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
                             new Vector2(0f, (float)texture.Height / 2),
                             new Vector2((Vector2.Distance(start, end) / texture.Width), 1f),
                             SpriteEffects.None, 0f);
        }

    }
}
