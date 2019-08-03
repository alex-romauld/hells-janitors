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
    class PointText
    {
        public String points;
        public int Points;

        public Vector2 Pos;
        public int Life;
        public int Player;

        private int VelocityX;
        private int VelocityY;


        private int GUIxVelocity;
        private int GUIyVelocity;
        public Vector2 GUIpos;
        private int Delay;
        private int CurDelay;
        


        public PointText(Vector2 Pos, int Life, int Player, int Points, Vector2 GUIpos)
        {
            this.Points = Points;
            Random random = new Random();
            points = "+" + Points;
            this.Pos = Pos;
            this.Life = Life;
            this.Player = Player;

            VelocityX = random.Next(-3, 4);
            VelocityY = random.Next(-3, 4);

            this.GUIpos = GUIpos;
            GUIxVelocity = random.Next(1, 3);
            GUIyVelocity = random.Next(-1, 2);

            Delay = random.Next(0, 2);
        }

        public void Update(GameTime gameTime)
        {
            Life += gameTime.ElapsedGameTime.Milliseconds;
            Pos = new Vector2(Pos.X + VelocityX, Pos.Y + VelocityY);
            if (CurDelay >= Delay)
            {
                GUIpos = new Vector2(GUIpos.X + GUIxVelocity, GUIpos.Y + GUIyVelocity);
                CurDelay = 0;
            }
            else
                CurDelay++;


        }
    }
}
