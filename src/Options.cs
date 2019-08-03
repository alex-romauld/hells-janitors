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
    class Options
    {
        public Boolean ControllerEnabled = false;

        public Boolean DrawPlayerCircles = false;
        public Texture2D _playerCircle;
        public int _PlayerCircleSize = 85;

        public int MusicVolume = 100;
        public int SoundEffectsVolume = 100;

        public Boolean ControllerVibration = true;

        public Boolean FullScreen = false;

        public int GraphicsSettings = 0;

        public Options()
        {
            
        }

        public void Load(ContentManager content)
        {
            _playerCircle = content.Load<Texture2D>("Sprites/Player/player_Circle");
        }
        public void ToggleDrawPlayerCircles()
        {
            DrawPlayerCircles = !DrawPlayerCircles;
        }
        public void ToggleVibrateController()
        {
            ControllerVibration = !ControllerVibration;
        }
        public void ToggleFullScreen()
        {
            FullScreen = !FullScreen;
        }
    }
}
