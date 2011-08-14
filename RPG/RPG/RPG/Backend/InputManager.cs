using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace RPG
{
    static class InputManager
    {
        public static bool JapaneseButtonSwap = false;
        public static bool inputSuspended = false;

        public static bool Up;
        public static bool Down;
        public static bool Left;
        public static bool Right;
        public static bool Select;
        public static bool Start;
        public static bool Confirm;
        public static bool Cancel;
        public static bool Y;
        public static bool X;

        public static void Update()
        {
            if (inputSuspended != true)
            {
                KeyboardState CurrentKeyboardState = Keyboard.GetState();

                
                //Directions
                if ((GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed) || (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up)))
                    Up = true;
                else Up = false;
                if ((GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed) || (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down)))
                    Down = true;
                else Down = false;
                if ((GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed) || (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left)))
                    Left = true;
                else Left = false;

                if ((GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed) || (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right)))
                    Right = true;
                else Right = false;

                //Menu Buttons
                if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) || (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape)))
                    Select = true;
                else Select = false;
                if ((GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed) || (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter)))
                    Start = true;
                else Start = false;
                
                //Face Buttons
                if ((GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed) || (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.A)))
                    Y = true;
                else Y = false;
                if ((GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed) || (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.S)))
                    X = true;
                else X = false;
                if (!JapaneseButtonSwap)
                {
                    if ((GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed) || (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.X)))
                        Confirm = true;
                    else Confirm = false;
                    if ((GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed) || (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Z)))
                        Cancel = true;
                    else Cancel = false;
                }
                else
                {
                    if ((GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed) || (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.X)))
                        Cancel = true;
                    else Cancel = false;
                    if ((GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed) || (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Z)))
                        Confirm = true;
                    else Confirm = false;
                }
            }
        }
    }
}
