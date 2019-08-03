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
    class Teleporter
    {
        public Texture2D texture;
        public Boolean sends;
        public int Channel;
        public int Price;
        public int CoolDownTime;
        public int CurCoolDownTime = 0;
        public Rectangle rect;
        public Boolean on = false;

        public Teleporter(Texture2D texture, Boolean sends, int Channel, int Price, Rectangle rect, int CoolDownTime)
        {
            this.texture = texture;
            this.sends = sends;
            this.Channel = Channel;
            this.Price = Price;
            this.CoolDownTime = CoolDownTime * 1000;
            this.rect = rect;
            on = false;
        }
        public void Update(GameTime gameTime)
        {
            if (CurCoolDownTime > 0)
                CurCoolDownTime -= gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
