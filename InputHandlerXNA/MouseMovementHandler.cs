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
        public static void ProcessMouse (MovementManager movementManager)
            {
            MouseState mouseState = movementManager.CurrentMouseState;
            Vector2 mousePosition = new Vector2 (mouseState.X, mouseState.Y);
            movementManager.CursorPosition = mousePosition;
            movementManager.GameState.CursorScreenLocation = mousePosition;
            }
        }
    }
