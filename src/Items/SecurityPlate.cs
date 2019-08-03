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
namespace SurvivalShooter.Items
{
    class SecurityPlate
    {
        private Rectangle rect;
        private Texture2D texture;
        public Vector2 pos;

        public float Rotation = 0.0f;
        public Vector2 fireDirection1;
        public Vector2 fireDirection2;
        public Boolean fire = false;

        private int fireRate = 0;

        public int Damage = 10;
        public int Speed = 10;
        public int Life = 30;

        private int LifeTime = 0;
        public Boolean Remove = false;

        public SecurityPlate(Texture2D texture, Rectangle rect)
        {
            this.texture = texture;
            this.rect = rect;
        }

        public void Update(GameTime gameTime)
        {
            pos = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

                Rotation += 0.1f;

            if (fireRate >= 1)
            {
                fire = true;
                fireDirection1 = RotateAboutOrigin(fireDirection1, pos, Rotation);
                fireDirection1 = pos - fireDirection1;
                fireDirection1.Normalize();
                

                fireDirection2 = RotateAboutOrigin(fireDirection1, pos, Rotation + 180);
                fireDirection2 = pos - fireDirection2;
                fireDirection2.Normalize();
                fireRate = 0;
            }
            else
                fireRate++;

            LifeTime += gameTime.ElapsedGameTime.Milliseconds;
            if (LifeTime >= 30000)
            {
                Remove = true;
            }
        }
        public Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin, float rotation)
        {
            return Vector2.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
        } 
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, pos, null, Color.White, Rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
            //spriteBatch.Draw(texture, rect, Color.White);
        }
    }
}
