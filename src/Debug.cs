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

namespace SurvivalShooter
{
    class Debug
    {
        SpriteFont _spr_font;
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        public int _fps = 0;

        public Debug()
        {

        }

        public void Load(ContentManager content)
        {
            _spr_font = content.Load<SpriteFont>("standardfont");
        }

        public void Update(GameTime gameTime)
        {
            _elapsed_time += gameTime.ElapsedGameTime.Milliseconds;
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }

        }

        public void UpdateDraw()
        {
            _total_frames++;
        }

    }
}
