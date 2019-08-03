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
    class Tile
    {
        public Color colorID;
        public Texture2D texture;
        public String name = "";
        public Boolean collision = false;

        public Tile(Texture2D texture, Color colorID, String name, Boolean collision)
        {
            this.colorID = colorID;
            this.texture = texture;
            this.name = name;
            this.collision = collision;
        }


    }
}
