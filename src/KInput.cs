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
    class KInput
    {

        public KInput(){}

        private KeyboardState oldState;
        private MouseState oldMouseState;

        public Boolean LMB = false;

        public Boolean UseKey = false;
        public Boolean ItemKey = false;
        public Boolean PlaceItemKey = false;
        public Boolean PlaceItem2Key = false;
        public Boolean GasMaskKey = false;
        public Boolean SwitchedWeapon = false;
        public Boolean ScoreBoardKey = false;

        public Boolean FowardKey = false;
        public Boolean BackwardsKey = false;
        public Boolean LeftKey = false;
        public Boolean RightKey = false;

        public Boolean GUI_Back = false;
        public Boolean GUI_Ready = false;
        public Boolean GUI_ChangleClass = false;
        public Boolean GUI_Select = false;
        public Boolean GUI_Enter = false;
        public Boolean GUI_Up = false;
        public Boolean GUI_Down = false;
        public Boolean GUI_Left = false;
        public Boolean GUI_Right = false;
        public Boolean GUI_Left_Hold = false;
        public Boolean GUI_Right_Hold = false;

        public Texture2D UseKeyTexture;
        public Texture2D SwitchWeaponKeyTexture;
        public Texture2D GasMaskKeyTexture;
        public Texture2D AbilityKeyTexture;
        public Texture2D Fire1;
        public Texture2D Fire2;
        public Texture2D EscapeKeyTexture;
        public Texture2D Item1KeyTexture;
        public Texture2D Item2KeyTexture;
        public Texture2D ScrollTexture;

        public void Load(ContentManager content)
        {
            UseKeyTexture = content.Load<Texture2D>("KeyBoard Keys/Key_E");
            SwitchWeaponKeyTexture = content.Load<Texture2D>("KeyBoard Keys/Key_Tab");
            GasMaskKeyTexture = content.Load<Texture2D>("KeyBoard Keys/Key_Q");
            AbilityKeyTexture = content.Load<Texture2D>("KeyBoard Keys/Key_1");
            Fire1 = content.Load<Texture2D>("Keyboard Keys/Mouse_Left");
            Fire2 = content.Load<Texture2D>("Keyboard Keys/Mouse_Right");
            EscapeKeyTexture = content.Load<Texture2D>("KeyBoard Keys/Key_Esc");
            Item1KeyTexture = content.Load<Texture2D>("KeyBoard Keys/Key_2");
            Item2KeyTexture = content.Load<Texture2D>("KeyBoard Keys/Key_3");
            ScrollTexture = content.Load<Texture2D>("KeyBoard Keys/Mouse_Scroll");
        }

        public void Update(Boolean PC)
        {
            UpdateGUIKeys();
            if (PC)
            {
               // UpdateGUIKeys();
                
                UseKey = KeyPressed(Keys.E);
                ItemKey = KeyPressed(Keys.D1);
                PlaceItemKey = KeyPressed(Keys.D2);
                PlaceItem2Key = KeyPressed(Keys.D3);
                GasMaskKey = KeyPressed(Keys.Q);
                SwitchedWeapon = KeyPressed(Keys.Tab);
                ScoreBoardKey = KeyPressed(Keys.OemTilde);

                FowardKey = KeyHeld(Keys.W);
                BackwardsKey = KeyHeld(Keys.S);
                LeftKey = KeyHeld(Keys.A);
                RightKey = KeyHeld(Keys.D);
            }
        }
        public Rectangle MouseRect;
        private void UpdateGUIKeys()
        {
            LMB = LMBPressed();
            GUI_Enter = KeyPressed(Keys.Space) || KeyPressed(Keys.Enter);
            GUI_Select = GUI_Enter || _MousePressed();
            GUI_Up = KeyPressed(Keys.Up);
            GUI_Down = KeyPressed(Keys.Down);
            GUI_Left = KeyPressed(Keys.Left);
            GUI_Right = KeyPressed(Keys.Right);

            GUI_Left_Hold = KeyHeld(Keys.Left);
            GUI_Right_Hold = KeyHeld(Keys.Right);

            GUI_Ready = KeyPressed(Keys.F);
            GUI_ChangleClass = KeyPressed(Keys.X);

            GUI_Back = KeyPressed(Keys.Escape);
            MouseRect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);
        }
        Boolean MouseUp = true;
        Boolean MousePressed = false;
        public Boolean _MousePressed()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (MouseUp)
                {
                    MousePressed = true;
                    MouseUp = false;
                }
                else
                    MousePressed = false;
            }
            else
            {
                MousePressed = false;
                MouseUp = true;
            }
            return MousePressed;
        }

        public Boolean LMBPressed()
        {
            Boolean pressed = false;
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (oldMouseState.LeftButton != ButtonState.Pressed)
                {
                    pressed = true;
                    oldMouseState = mouseState;
                }
            }
            else
            {
                if (oldMouseState.LeftButton == ButtonState.Pressed)
                {
                    pressed = false;
                    oldMouseState = mouseState;
                }
            }
            return pressed;
        }
        public Boolean KeyPressed(Keys key)
        {
            Boolean pressed = false;
            KeyboardState newState = Keyboard.GetState();
            if (newState.IsKeyDown(key))
            {
                if (!oldState.IsKeyDown(key))
                {
                    pressed = true;
                    oldState = newState;
                }
            }
            else
            {
                if (oldState.IsKeyDown(key))
                {
                    pressed = false;
                    oldState = newState;
                }
            }

            return pressed;
        }
        public Boolean KeyHeld(Keys key)
        {
            Boolean held = false;
            if (Keyboard.GetState().IsKeyDown(key))
                held = true;
            else
                held = false;
            return held;
        }
    }
}
