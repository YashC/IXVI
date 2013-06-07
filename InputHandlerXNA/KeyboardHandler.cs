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
        static Keys m_rotationLeft = Keys.A ;
        static Keys m_rotationRight = Keys.D;
        //static Keys m_rotationUp = Keys.S;
        //static Keys m_rotationDown = Keys.W;

        //Movement key mappings
        static Keys m_moveUp = Keys.Up;
        static Keys m_moveDown = Keys.Down;
        static Keys m_moveBackward = Keys.S;
        static Keys m_moveForward = Keys.W;

        static Keys m_CameraState = Keys.C;

        static Keys m_switchToKinect = Keys.K;

             
        public static void ProcessKeyboard (InputManager inputManager)
            {
            if (inputManager.CurrentKeyboardState.GetPressedKeys ().Count () > 1)
                {
                if (inputManager.CurrentKeyboardState.GetPressedKeys ().Count () == 2 && inputManager.CurrentKeyboardState.IsKeyDown (m_switchToKinect))
                    {
                    inputManager.GameState.IsKinectActive = true;
                    inputManager.GameState.IsKeyboardActive = false;
                    inputManager.GameState.IsMouseActive = false;
                    }
                else
                    {
                    inputManager.GameState.IsKeyboardActive = true;
                    inputManager.GameState.IsKinectActive = false;
                    inputManager.GameState.IsMouseActive = true;
                    }
                }
            if (inputManager.GameState.IsKeyboardActive)
                {
            inputManager.GameState.IsInputActive = false;
            if (inputManager.CurrentKeyboardState.IsKeyDown (m_rotationLeft))
                {
                // Rotate left.
                inputManager.GameState.IsInputActive = true; 
                inputManager.GameState.AvatarYRotation += inputManager.GameState.TurningSpeed;
                } 

            if (inputManager.CurrentKeyboardState.IsKeyDown (m_rotationRight))
                {
                // Rotate right.
                inputManager.GameState.IsInputActive = true; 
                inputManager.GameState.AvatarYRotation -= inputManager.GameState.TurningSpeed;
                }
            if (inputManager.CurrentKeyboardState.IsKeyDown (m_moveForward))
                {
                inputManager.GameState.IsInputActive = true; 
                Matrix forwardMovement = Matrix.CreateRotationY (inputManager.GameState.AvatarYRotation);
                Vector3 v = new Vector3 (0, 0, -inputManager.GameState.MovingSpeed);
                v = Vector3.Transform (v, forwardMovement);
                Vector3 avatarPosition = inputManager.GameState.AvatarPosition;
                avatarPosition.Z += v.Z;
                avatarPosition.X += v.X;
                inputManager.GameState.AvatarPosition = avatarPosition;
               // Console.WriteLine ("Avatar Postion:{0}", avatarPosition.ToString ()); 
                }
            if (inputManager.CurrentKeyboardState.IsKeyDown (m_moveBackward))
                {
                inputManager.GameState.IsInputActive = true; 
                Matrix forwardMovement = Matrix.CreateRotationY (inputManager.GameState.AvatarYRotation);
                Vector3 v = new Vector3 (0, 0, inputManager.GameState.MovingSpeed);
                v = Vector3.Transform (v, forwardMovement);
                Vector3 avatarPosition = inputManager.GameState.AvatarPosition;
                avatarPosition.Z += v.Z;
                avatarPosition.X += v.X;
                inputManager.GameState.AvatarPosition = avatarPosition;
                }
            if (inputManager.CurrentKeyboardState.IsKeyDown (m_moveUp))
                {
                inputManager.GameState.IsInputActive = true; 
                Matrix forwardMovement = Matrix.CreateRotationY (inputManager.GameState.AvatarYRotation);
                Vector3 v = new Vector3 (0, inputManager.GameState.MovingSpeed, 0);
                v = Vector3.Transform (v, forwardMovement);
                Vector3 avatarPosition = inputManager.GameState.AvatarPosition;
                avatarPosition.Y += v.Y;
               
                inputManager.GameState.AvatarPosition = avatarPosition;
                }

            if (inputManager.CurrentKeyboardState.IsKeyDown (m_moveDown))
                {
                inputManager.GameState.IsInputActive = true; 
                Matrix forwardMovement = Matrix.CreateRotationY (inputManager.GameState.AvatarYRotation);
                Vector3 v = new Vector3 (0, -inputManager.GameState.MovingSpeed, 0);
                v = Vector3.Transform (v, forwardMovement);
                Vector3 avatarPosition = inputManager.GameState.AvatarPosition;
                avatarPosition.Y += v.Y;
                inputManager.GameState.AvatarPosition = avatarPosition;
                }   

            if (inputManager.CurrentKeyboardState.IsKeyDown (m_CameraState))
                {
                inputManager.GameState.CameraState += 1;
                inputManager.GameState.CameraState %= 3;        
                }  
            }
           }

       
        }
    }
