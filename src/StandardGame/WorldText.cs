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
    class WorldText
    {
        public String Text;

        public Vector2 Pos;
        public int Life;

        private int VelocityX = 0;
        private int VelocityY;

        public int Fade = 255;

        public int PlayerIndex = 0;
        public Vector2 originPos = Vector2.Zero;
        public int YOffset = 0;

        public WorldText(Vector2 Pos, int Life, String Text)
        {
            this.Text = Text;
            Random random = new Random();
            this.Pos = Pos;
            this.Life = Life;
            originPos = Pos;
            //VelocityX = random.Next(-3, 4);
            VelocityY = -3;//-3, 4);

        }

        public void Update(GameTime gameTime)
        {
            Life += gameTime.ElapsedGameTime.Milliseconds;
            if(YOffset == 0)
                Pos = new Vector2(Pos.X + VelocityX, Pos.Y + VelocityY);
            else
                Pos = new Vector2(originPos.X + VelocityX, originPos.Y + VelocityY + YOffset);
            Fade -= 7;

        }
    }
}
