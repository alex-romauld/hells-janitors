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
    class Button
    {
        public Rectangle ButtonRect;
        private Texture2D ButtonTexture, HighlightButtonTexture, PressButtonTexture;

        public Boolean Pressed = false;


        public Button()
        {

        }

        public void Load(Rectangle ButtonRect, Texture2D ButtonTexture, Texture2D HighlightButtonTexture, Texture2D PressButtonTexture)
        {
            this.ButtonRect = ButtonRect;
            this.ButtonTexture = ButtonTexture;
            this.HighlightButtonTexture = HighlightButtonTexture;
            this.PressButtonTexture = PressButtonTexture;
        }

        public void Update(KInput input)
        {
            if (Pressed)
                Pressed = false;
            if (input.MouseRect.Intersects(ButtonRect))
                if (input._MousePressed())
                    Pressed = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D cur_texture = ButtonTexture;
            if (Pressed)
                cur_texture = PressButtonTexture;
            if(cur_texture != null)
                spriteBatch.Draw(cur_texture, ButtonRect, Color.White);
        }
    }
}
