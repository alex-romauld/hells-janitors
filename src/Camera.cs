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
    class Camera
    {
        public Matrix transform;
        //Viewport view;
        Vector2 center;

        public float Zoom = .5f;//5f;

        public Camera()//Viewport newView)
        {
           // view = newView;
        }

        public void Update(GameTime gameTime, Vector2 CameraCenter, GraphicsDevice graphics)
        {
            center = new Vector2(CameraCenter.X - (graphics.Viewport.Width / 2) * (1 / Zoom), CameraCenter.Y - (graphics.Viewport.Height / 2) * (1 / Zoom));
            transform = Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) * Matrix.CreateTranslation(new Vector3(-center.X / (1 / Zoom), -center.Y / (1 / Zoom), 0));
            //if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightShoulder))
            //    Zoom += 0.02f;
            //if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftShoulder))
            //    Zoom -= 0.02f;
        }

    }
}

