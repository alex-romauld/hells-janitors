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
    class Input
    {
        public PlayerIndex player;

        //public Boolean Buttons_Press_A = false;
        //public Boolean Buttons_Hold_A = false;

        //public Boolean Buttons_Press_B = false;
        //public Boolean Buttons_Hold_B = false;

        //public Boolean Buttons_Press_X = false;
        //public Boolean Buttons_Hold_X = false;

        //public Boolean Buttons_Press_Y = false;
        //public Boolean Buttons_Hold_Y = false;

        //public Boolean Buttons_Press_Start = false;
        //public Boolean Buttons_Hold_Start = false;

        //public Boolean Buttons_Press_Dpad_Up = false;
        //public Boolean Buttons_Hold_Dpad_Up = false;
        //public Boolean Buttons_Press_Dpad_Down = false;
        //public Boolean Buttons_Hold_Dpad_Down = false;
        //public Boolean Buttons_Press_Dpad_Left = false;
        //public Boolean Buttons_Hold_Dpad_Left = false;
        //public Boolean Buttons_Press_Dpad_Right = false;
        //public Boolean Buttons_Hold_Dpad_Right = false;

        //private Boolean Released_Button_A = true;
        //private Boolean Released_Button_B = true;
        //private Boolean Released_Button_X = true;
        //private Boolean Released_Button_Y = true;
        //private Boolean Released_Button_Start = true;

        //private Boolean Released_Buttons_Dpad_Up = true;
        //private Boolean Released_Buttons_Dpad_Down = true;
        //private Boolean Released_Buttons_Dpad_Left = true;
        //private Boolean Released_Buttons_Dpad_Right = true;



        //public Boolean Buttons_Press_RightBumper = false;
        //public Boolean Buttons_Hold_RightBumper = false;
        //public Boolean Buttons_Press_LeftBumper = false;
        //public Boolean Buttons_Hold_LeftBumper = false;

        //private Boolean Released_Button_RightBumper = true;
        //private Boolean Released_Button_LeftBumper = true;


        public Texture2D ButtonA;
        public Texture2D ButtonB;
        public Texture2D ButtonX;
        public Texture2D ButtonY;
        public Texture2D ButtonStart;
        public Texture2D ButtonDpad;
        public Texture2D ButtonRB;
        public Texture2D ButtonLB;


        public Boolean GUI_Up = false;
        public Boolean GUI_Down = false;
        public Boolean GUI_Left = false;
        public Boolean GUI_Right = false;
        public Boolean GUI_Hold_Left = false;
        public Boolean GUI_Hold_Right = false;
        private Boolean GUI_Released_Up = true;
        private Boolean GUI_Released_Down = true;
        private Boolean GUI_Released_Left = true;
        private Boolean GUI_Released_Right = true;


        public Boolean PC = false;

        public Input(PlayerIndex player)
        {
            this.player = player;
        }
        public void Load(ContentManager content)
        {
            ButtonA = content.Load<Texture2D>("ControllerButtons/xboxControllerButtonA");
            ButtonB = content.Load<Texture2D>("ControllerButtons/xboxControllerButtonB");
            ButtonX = content.Load<Texture2D>("ControllerButtons/xboxControllerButtonX");
            ButtonY = content.Load<Texture2D>("ControllerButtons/xboxControllerButtonY");
            ButtonDpad = content.Load<Texture2D>("ControllerButtons/xboxControllerDPad");
            ButtonRB = content.Load<Texture2D>("ControllerButtons/xboxControllerRightShoulder");
            ButtonLB = content.Load<Texture2D>("ControllerButtons/xboxControllerLeftShoulder");
            ButtonStart = content.Load<Texture2D>("ControllerButtons/xboxControllerStart");
        }

        Boolean _Viberate;
        public void Update(Boolean _Viberate, Boolean PC)
        {
            this._Viberate = _Viberate;
            //if (GamePad.GetState(player).IsButtonDown(Buttons.A) || (PC && Keyboard.GetState().IsKeyDown(Keys.Enter) && player == PlayerIndex.One))
            //{
            //    if (Released_Button_A)
            //        Buttons_Press_A = true;
            //    else
            //        Buttons_Press_A = false;
            //    Released_Button_A = false;
            //    Buttons_Hold_A = true;
            //}
            //else
            //{
            //    Buttons_Press_A = false;
            //    Released_Button_A = true;
            //    Buttons_Hold_A = false;
            //}

            //if (GamePad.GetState(player).IsButtonDown(Buttons.B) || (PC && Keyboard.GetState().IsKeyDown(Keys.Back) && player == PlayerIndex.One))
            //{
            //    if (Released_Button_B)
            //        Buttons_Press_B = true;
            //    else
            //        Buttons_Press_B = false;
            //    Released_Button_B = false;
            //    Buttons_Hold_B = true;
            //}
            //else
            //{
            //    Buttons_Press_B = false;
            //    Released_Button_B = true;
            //    Buttons_Hold_B = false;
            //}



            //if (GamePad.GetState(player).IsButtonDown(Buttons.X) || (PC && Keyboard.GetState().IsKeyDown(Keys.E) && player == PlayerIndex.One))
            //{
            //    if (Released_Button_X)
            //        Buttons_Press_X = true;
            //    else
            //        Buttons_Press_X = false;
            //    Released_Button_X = false;
            //    Buttons_Hold_X = true;
            //}
            //else
            //{
            //    Buttons_Press_X = false;
            //    Released_Button_X = true;
            //    Buttons_Hold_X = false;
            //}



            //if (GamePad.GetState(player).IsButtonDown(Buttons.Y) || (PC && Keyboard.GetState().IsKeyDown(Keys.Q) && player == PlayerIndex.One))
            //{
            //    if (Released_Button_Y)
            //        Buttons_Press_Y = true;
            //    else
            //        Buttons_Press_Y = false;
            //    Released_Button_Y = false;
            //    Buttons_Hold_Y = true;
            //}
            //else
            //{
            //    Buttons_Press_Y = false;
            //    Released_Button_Y = true;
            //    Buttons_Hold_Y = false;
            //}





            //if (GamePad.GetState(player).IsButtonDown(Buttons.Start) || (PC && Keyboard.GetState().IsKeyDown(Keys.Escape) && player == PlayerIndex.One))
            //{
            //    if (Released_Button_Start)
            //        Buttons_Press_Start = true;
            //    else
            //        Buttons_Press_Start = false;
            //    Released_Button_Start = false;
            //    Buttons_Hold_Start = true;
            //}
            //else
            //{
            //    Buttons_Press_Start = false;
            //    Released_Button_Start = true;
            //    Buttons_Hold_Start = false;
            //}



            //if (GamePad.GetState(player).IsButtonDown(Buttons.DPadUp) || (PC && Keyboard.GetState().IsKeyDown(Keys.D1) && player == PlayerIndex.One))
            //{
            //    if (Released_Buttons_Dpad_Up)
            //        Buttons_Press_Dpad_Up = true;
            //    else
            //        Buttons_Press_Dpad_Up = false;
            //    Released_Buttons_Dpad_Up = false;
            //    Buttons_Hold_Dpad_Up = true;
            //}
            //else
            //{
            //    Buttons_Press_Dpad_Up = false;
            //    Released_Buttons_Dpad_Up = true;
            //    Buttons_Hold_Dpad_Up = false;
            //}
            //if (GamePad.GetState(player).IsButtonDown(Buttons.DPadDown) || (PC && Keyboard.GetState().IsKeyDown(Keys.D2) && player == PlayerIndex.One))
            //{
            //    if (Released_Buttons_Dpad_Down)
            //        Buttons_Press_Dpad_Down = true;
            //    else
            //        Buttons_Press_Dpad_Down = false;
            //    Released_Buttons_Dpad_Down = false;
            //    Buttons_Hold_Dpad_Down = true;
            //}
            //else
            //{
            //    Buttons_Press_Dpad_Down = false;
            //    Released_Buttons_Dpad_Down = true;
            //    Buttons_Hold_Dpad_Down = false;
            //}
            //if (GamePad.GetState(player).IsButtonDown(Buttons.DPadLeft) || (PC && Keyboard.GetState().IsKeyDown(Keys.D3) && player == PlayerIndex.One))
            //{
            //    if (Released_Buttons_Dpad_Left)
            //        Buttons_Press_Dpad_Left = true;
            //    else
            //        Buttons_Press_Dpad_Left = false;
            //    Released_Buttons_Dpad_Left = false;
            //    Buttons_Hold_Dpad_Left = true;
            //}
            //else
            //{
            //    Buttons_Press_Dpad_Left = false;
            //    Released_Buttons_Dpad_Left = true;
            //    Buttons_Hold_Dpad_Left = false;
            //}
            //if (GamePad.GetState(player).IsButtonDown(Buttons.DPadRight) || (PC && Keyboard.GetState().IsKeyDown(Keys.D4) && player == PlayerIndex.One))
            //{
            //    if (Released_Buttons_Dpad_Right)
            //        Buttons_Press_Dpad_Right = true;
            //    else
            //        Buttons_Press_Dpad_Right = false;
            //    Released_Buttons_Dpad_Right = false;
            //    Buttons_Hold_Dpad_Right = true;
            //}
            //else
            //{
            //    Buttons_Press_Dpad_Right = false;
            //    Released_Buttons_Dpad_Right = true;
            //    Buttons_Hold_Dpad_Right = false;
            //}





           
            //if (GamePad.GetState(player).IsButtonDown(Buttons.RightShoulder))
            //{
            //    if (Released_Button_RightBumper)
            //        Buttons_Press_RightBumper = true;
            //    else
            //        Buttons_Press_RightBumper = false;
            //    Released_Button_RightBumper = false;
            //    Buttons_Hold_RightBumper = true;
            //}
            //else
            //{
            //    Buttons_Press_RightBumper = false;
            //    Released_Button_RightBumper = true;
            //    Buttons_Hold_RightBumper = false;
            //}

            //if (GamePad.GetState(player).IsButtonDown(Buttons.LeftShoulder))
            //{
            //    if (Released_Button_LeftBumper)
            //        Buttons_Press_LeftBumper = true;
            //    else
            //        Buttons_Press_LeftBumper = false;
            //    Released_Button_LeftBumper = false;
            //    Buttons_Hold_LeftBumper = true;
            //}
            //else
            //{
            //    Buttons_Press_LeftBumper = false;
            //    Released_Button_LeftBumper = true;
            //    Buttons_Hold_LeftBumper = false;
            //}

            if (!PC)
            {
                UpdateGUIButtons();

                if (Vibrate)
                {
                    Delay++;
                    if (Delay > DelayTime)
                    {
                        GamePad.SetVibration(player, 0f, 0f);
                        Vibrate = false;
                        Delay = 0;
                    }
                    else
                    {
                        GamePad.SetVibration(player, Intensity, Intensity);
                    }
                }
                HoldA = GamePad.GetState(player).IsButtonDown(Buttons.A);
                SelectKey = ButtonPressed(Buttons.A);
                BackKey = ButtonPressed(Buttons.B);
                PauseKey = ButtonPressed(Buttons.Start);

                ChangeClass_left = ButtonPressed(Buttons.LeftShoulder);
                ChangeClass_right = ButtonPressed(Buttons.RightShoulder);

                UseKey = ButtonPressed(Buttons.X);
                ItemKey = ButtonPressed(Buttons.DPadRight);
                GasMaskKey = ButtonPressed(Buttons.DPadLeft);
                SwitchWeaponKey = ButtonPressed(Buttons.Y);
                scoreBoardKey = ButtonPressed(Buttons.Back);

                DPad_Up = ButtonPressed(Buttons.DPadUp);
                DPad_Down = ButtonPressed(Buttons.DPadDown);

             //   Item1Key = ButtonPressed(Buttons.RightShoulder);
            }
        }


        private GamePadState oldState;

        public Boolean HoldA = false;
        public Boolean SelectKey = false;
        public Boolean BackKey = false;
        public Boolean PauseKey = false;

        public Boolean ChangeClass_left = false;
        public Boolean ChangeClass_right = false;

        public Boolean UseKey = false;
        public Boolean ItemKey = false;
        public Boolean GasMaskKey = false;
        public Boolean SwitchWeaponKey = false;
        public Boolean scoreBoardKey = false;
        public Boolean Item1Key = false;

        public Boolean DPad_Down = false;
        public Boolean DPad_Up = false;

        public Boolean ButtonPressed(Buttons button)
        {
            Boolean pressed = false;
            GamePadState newState = GamePad.GetState(player);
            if (newState.IsButtonDown(button))
            {
                if (!oldState.IsButtonDown(button))
                {
                    pressed = true;
                    oldState = newState;
                }
            }
            else
            {
                if (oldState.IsButtonDown(button))
                {
                    pressed = false;
                    oldState = newState;
                }
            }

            return pressed;
        }



        private float ThumbstickMin = 0.6f;
        private void UpdateGUIButtons()
        {
            if (GamePad.GetState(player).IsButtonDown(Buttons.DPadUp) || GamePad.GetState(player).ThumbSticks.Left.Y > ThumbstickMin)
            {
                if (GUI_Released_Up)
                    GUI_Up = true;
                else
                    GUI_Up = false;
                GUI_Released_Up = false;
            }
            else
            {
                GUI_Up = false;
                GUI_Released_Up = true;
            }

            if (GamePad.GetState(player).IsButtonDown(Buttons.DPadDown) || GamePad.GetState(player).ThumbSticks.Left.Y < -ThumbstickMin)
            {
                if (GUI_Released_Down)
                    GUI_Down = true;
                else
                    GUI_Down = false;
                GUI_Released_Down = false;
            }
            else
            {
                GUI_Down = false;
                GUI_Released_Down = true;
            }


            if (GamePad.GetState(player).IsButtonDown(Buttons.DPadLeft) || GamePad.GetState(player).ThumbSticks.Left.X < -ThumbstickMin)
            {
                if (GUI_Released_Left)
                    GUI_Left = true;
                else
                    GUI_Left = false;
                GUI_Released_Left = false;
                GUI_Hold_Left = true;
            }
            else
            {
                GUI_Left = false;
                GUI_Released_Left = true;
                GUI_Hold_Left = false;
            }

            if (GamePad.GetState(player).IsButtonDown(Buttons.DPadRight) || GamePad.GetState(player).ThumbSticks.Left.X > ThumbstickMin)
            {
                if (GUI_Released_Right)
                    GUI_Right = true;
                else
                    GUI_Right = false;
                GUI_Released_Right = false;
                GUI_Hold_Right = true;
            }
            else
            {
                GUI_Right = false;
                GUI_Released_Right = true;
                GUI_Hold_Right = false;
            }
        }
        
        
        private int Delay = 0;
        private int DelayTime = 3;
        private float Intensity = 0.1f;
        private Boolean Vibrate = false;

        public void ViberateController(int DelayTime, float Intensity)
        {
            if (_Viberate)
            {
                if (!PC)
                {
                    this.DelayTime = DelayTime;
                    this.Intensity = Intensity;
                    Vibrate = true;
                    Delay = 0;
                }
            }
        }

        

    }
}
