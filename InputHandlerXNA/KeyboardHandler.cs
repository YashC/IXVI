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

namespace InputHandler
    {
   public class KeyboardHandler
        {
        //Rotation key mappings
        static Keys m_rotationLeft = Keys.Left ;
        static Keys m_rotationRight = Keys.Right;
        //static Keys m_rotationUp = Keys.S;
        //static Keys m_rotationDown = Keys.W;

        //Movement key mappings
       // static Keys m_moveLeft = Keys.Left;
       // static Keys m_moveRight = Keys.Right;
        static Keys m_moveBackward = Keys.Down;
        static Keys m_moveForward = Keys.Up;

        static Keys m_CameraState = Keys.C;

             
        public static void ProcessKeyboard (InputManager inputManager)
            {             
            if (inputManager.CurrentKeyboardState.IsKeyDown (m_rotationLeft))
                {
                // Rotate left.
                inputManager.GameState.AvatarYRotation += inputManager.GameState.TurningSpeed;
                }
            if (inputManager.CurrentKeyboardState.IsKeyDown (m_rotationRight))
                {
                // Rotate right.             
                inputManager.GameState.AvatarYRotation -= inputManager.GameState.TurningSpeed;
                }
            if (inputManager.CurrentKeyboardState.IsKeyDown (m_moveForward))
                {
                Matrix forwardMovement = Matrix.CreateRotationY (inputManager.GameState.AvatarYRotation);
                Vector3 v = new Vector3 (0, 0, inputManager.GameState.MovingSpeed);
                v = Vector3.Transform (v, forwardMovement);
                Vector3 avatarPosition = inputManager.GameState.AvatarPosition;
                avatarPosition.Z += v.Z;
                avatarPosition.X += v.X;
                inputManager.GameState.AvatarPosition = avatarPosition;
                }
            if (inputManager.CurrentKeyboardState.IsKeyDown (m_moveBackward))
                {
                Matrix forwardMovement = Matrix.CreateRotationY (inputManager.GameState.AvatarYRotation);
                Vector3 v = new Vector3 (0, 0, -inputManager.GameState.MovingSpeed);
                v = Vector3.Transform (v, forwardMovement);
                Vector3 avatarPosition = inputManager.GameState.AvatarPosition;
                avatarPosition.Z += v.Z;
                avatarPosition.X += v.X;
                inputManager.GameState.AvatarPosition = avatarPosition;
                }
            if (inputManager.CurrentKeyboardState.IsKeyDown (m_CameraState))
                {
                //inputManager.GameState.CameraState += 1;
                //inputManager.GameState.CameraState %= 3;        
                }  
            }

       
        }
    }
