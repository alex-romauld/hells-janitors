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
    class StoreEffect
    {
        public String name;
        public String description;
        public int price;
        public int life;

        public Boolean Purchased = false;
        public int LifeLeft;

        public int FullPrice;

        public StoreEffect(String name, String description, int price, int life)
        {
            this.name = name;
            this.description = description;
            this.life = life;
            this.price = price;
            FullPrice = price;
        }
    }
}
