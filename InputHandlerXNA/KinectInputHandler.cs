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
using Microsoft.Kinect;
using Kinect.Gestures;
using Kinect.Sensor;

namespace InputHandler
    {
    class KinectInputHandler
        {
        KinectHandler kinectHandler;
        //InputManager m_inputManager;
        public KinectInputHandler (InputManager inputManager)
            {
            //this.m_inputManager = inputManager;
            kinectHandler = new KinectHandler ();
            kinectHandler.StartKinectSensor ();

            if (kinectHandler.SensorStatus == KinectSensorStatus.Started)
                {
                kinectHandler.StartKinectServices ();
                inputManager.GameState.IsKinectConnected = true;               
                } 
            //kinectHandler.onGestureChanged += new EventHandler (KinectHandlerOnGestureChanged);
            }

        public void ProcessKinectCommands(InputManager inputManager)
            {                                                                   
            inputManager.GameState.KinectVideoColors = kinectHandler.ColorImage;
            if (inputManager.GameState.IsKinectActive)
                {
                inputManager.GameState.IsInputActive = false;

                if (kinectHandler.Gesture == "TurnLeft")
                    {
                    // Rotate left.
                    inputManager.GameState.IsInputActive = true;
                    inputManager.GameState.AvatarYRotation += inputManager.GameState.TurningSpeed / 2;
                    }

                if (kinectHandler.Gesture == "TurnRight")
                    {
                    // Rotate right.
                    inputManager.GameState.IsInputActive = true;
                    inputManager.GameState.AvatarYRotation -= inputManager.GameState.TurningSpeed / 2;
                    }
                if (kinectHandler.Gesture == "MoveForward")
                    {
                    inputManager.GameState.IsInputActive = true;
                    Matrix forwardMovement = Matrix.CreateRotationY (inputManager.GameState.AvatarYRotation);
                    Vector3 v = new Vector3 (0, 0, -inputManager.GameState.MovingSpeed);
                    v = Vector3.Transform (v, forwardMovement);
                    Vector3 avatarPosition = inputManager.GameState.AvatarPosition;
                    avatarPosition.Z += v.Z;
                    avatarPosition.X += v.X;
                    inputManager.GameState.AvatarPosition = avatarPosition;
                    //Console.WriteLine ("Avatar Postion:{0}", avatarPosition.ToString ());
                    }
                if (kinectHandler.Gesture == "MoveBackward")
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
                //if (kinectHandler.Gesture == "Swipe Up")
                //    {
                //    inputManager.GameState.IsInputActive = true;
                //    Matrix forwardMovement = Matrix.CreateRotationY (inputManager.GameState.AvatarYRotation);
                //    Vector3 v = new Vector3 (0, inputManager.GameState.MovingSpeed, 0);
                //    v = Vector3.Transform (v, forwardMovement);
                //    Vector3 avatarPosition = inputManager.GameState.AvatarPosition;
                //    avatarPosition.Y += v.Y;

                //    inputManager.GameState.AvatarPosition = avatarPosition;
                //    }

                //if (kinectHandler.Gesture == "Swipe Down")
                //    {
                //    inputManager.GameState.IsInputActive = true;
                //    Matrix forwardMovement = Matrix.CreateRotationY (inputManager.GameState.AvatarYRotation);
                //    Vector3 v = new Vector3 (0, -inputManager.GameState.MovingSpeed, 0);
                //    v = Vector3.Transform (v, forwardMovement);
                //    Vector3 avatarPosition = inputManager.GameState.AvatarPosition;
                //    avatarPosition.Y += v.Y;
                //    inputManager.GameState.AvatarPosition = avatarPosition;
                //    }

                //if (kinectHandler.Gesture == "Zoom In")
                //    {
                //    inputManager.GameState.CameraState += 1;
                //    inputManager.GameState.CameraState %= 3;
                //    }



                if (kinectHandler.Gesture == "Clear")
                    {
                    inputManager.GameState.IsInputActive = false;
                    if (inputManager.GameState.ShowInfo)
                        inputManager.GameState.ShowInfo = false;
                    if (inputManager.GameState.ShowCursor)
                        inputManager.GameState.ShowCursor = false;
                    }

                if (kinectHandler.Gesture == "Select")
                    {
                    // Console.WriteLine (inputManager.GameState.ShowCursor);                      
                    if (inputManager.GameState.ShowCursor)
                        {
                        inputManager.GameState.ShowInfo = true;
                        }
                    }

                if (kinectHandler.Gesture == "Wave Right")
                    {                    
                    inputManager.GameState.ShowCursor = true;
                    }

                if (inputManager.GameState.ShowCursor)
                    { 
                    inputManager.GameState.CursorScreenLocation = new Vector2 (kinectHandler.ScaledRightHand.Position.X, kinectHandler.ScaledRightHand.Position.Y);                  
                    }
                }
            }


        void KinectHandlerOnGestureChanged (object sender, EventArgs e)
            {
          
            }

            public void StopKinectSensor()
                {
                if(kinectHandler != null)
                 kinectHandler.StopKinectSensor ();
                }
              
        }
    }
