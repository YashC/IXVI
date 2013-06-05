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
        public static void ProcessMouse (MovementManager movementManager)
            {
            MouseState mouseState = movementManager.CurrentMouseState;
            Vector2 mousePosition = new Vector2 (mouseState.X, mouseState.Y);
            movementManager.CursorPosition = mousePosition;
            if (mouseState.LeftButton == ButtonState.Pressed)
                m_prevLeftButtonPressed = true;
            if (mouseState.RightButton == ButtonState.Pressed)
                m_prevRightButtonPressed = true;
            if (mouseState.LeftButton == ButtonState.Released && m_prevLeftButtonPressed)
                {
                movementManager.ShowCursor = !movementManager.ShowCursor;
                m_prevLeftButtonPressed = false;
                }
            else if (mouseState.RightButton == ButtonState.Released && m_prevRightButtonPressed)
                {
                movementManager.ShowInfo = !movementManager.ShowInfo;
                m_prevRightButtonPressed = false;
                }
            }
        }
    }
