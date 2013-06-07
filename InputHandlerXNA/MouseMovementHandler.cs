using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace InputHandler
    {
    class MouseMovementHandler
        {
        static bool m_prevLeftButtonPressed = false;
        static bool m_prevRightButtonPressed = false;
        public static void ProcessMouse (InputManager inputManager)
            {
             MouseState mouseState = Mouse.GetState ();           
            if (inputManager.GameState.ShowCursor && inputManager.GameState.IsMouseActive)
                {
                Vector2 mousePosition = new Vector2 (mouseState.X, mouseState.Y);                
                inputManager.GameState.CursorScreenLocation = mousePosition;
                }  

            if (mouseState.LeftButton == ButtonState.Pressed)
                {
                inputManager.GameState.IsMouseActive = true;
                inputManager.GameState.IsKeyboardActive = true;
                inputManager.GameState.IsKinectActive = false;
                m_prevLeftButtonPressed = true;
                }
            if (mouseState.RightButton == ButtonState.Pressed)
                {
                inputManager.GameState.IsMouseActive = true;
                inputManager.GameState.IsKeyboardActive = true;
                inputManager.GameState.IsKinectActive = false;
                m_prevRightButtonPressed = true;
                }				
             if (mouseState.LeftButton == ButtonState.Released && m_prevLeftButtonPressed)
                {
                inputManager.GameState.ShowCursor = !inputManager.GameState.ShowCursor;
                m_prevLeftButtonPressed = false;
                }
            else if (mouseState.RightButton == ButtonState.Released && m_prevRightButtonPressed)
                {
                inputManager.GameState.ShowInfo = !inputManager.GameState.ShowInfo;
                m_prevRightButtonPressed = false;
                }
            }
        }
    }
