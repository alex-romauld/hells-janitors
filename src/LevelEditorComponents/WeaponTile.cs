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

namespace SurvivalShooter.LevelEditorComponents
{
    class WeaponTile
    {
        public Tile _WeaponTile;
        public Color colorID;
        public int Price;
        public int AmmoPrice;

        public WeaponTile(Tile _WeaponTile, Color colorID, int Price, int AmmoPrice)
        {
            this._WeaponTile = _WeaponTile;
            this.colorID = colorID;
            this.Price = Price;
            this.AmmoPrice = AmmoPrice;
        }
    }
}
