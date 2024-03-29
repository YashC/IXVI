﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using GameUtilities;


namespace InputHandler
    {
    class MouseHandler
        {
        TimeSpan m_lastScroll = TimeSpan.Zero;
        public MouseHandler()
            {

            }

        public void ProcessMouse(InputManager inputManager)
            {
            inputManager.CurrentMouseState = Mouse.GetState ();
            if(inputManager.CurrentMouseState.ScrollWheelValue != inputManager.PreviousMouseState.ScrollWheelValue)
                {
                TimeSpan timeDiff = TimeSpan.Zero;
                int steps = 1;

                if (LastScroll != null)
                    {
                    timeDiff = inputManager.UpdateGameTime.TotalGameTime - LastScroll;
                    if (timeDiff < TimeSpan.FromSeconds (3))
                        steps = steps * (600/(int)timeDiff.TotalMilliseconds);
                    else if (timeDiff < TimeSpan.FromSeconds (5))
                        steps = steps * 2;
                    }

                if (inputManager.CurrentMouseState.ScrollWheelValue > inputManager.PreviousMouseState.ScrollWheelValue)
                    {
                    for (int i = 0; i < steps; i++)
                        {
                        inputManager.GameState.IsInputActive = true;
                        Matrix forwardMovement = Matrix.CreateRotationY (inputManager.GameState.AvatarYRotation);
                        Vector3 v = new Vector3 (0, 0, -inputManager.GameState.MovingSpeed);
                        v = Vector3.Transform (v, forwardMovement);
                        Vector3 avatarPosition = inputManager.GameState.AvatarPosition;
                        avatarPosition.Z += v.Z;
                        avatarPosition.X += v.X;
                        inputManager.GameState.AvatarPosition = avatarPosition;
                        }
                    LastScroll = inputManager.UpdateGameTime.TotalGameTime;
                    }
                else if (inputManager.CurrentMouseState.ScrollWheelValue < inputManager.PreviousMouseState.ScrollWheelValue)
                    {
                    for (int i = 0; i < steps; i++)
                        {
                        inputManager.GameState.IsInputActive = true;
                        Matrix forwardMovement = Matrix.CreateRotationY (inputManager.GameState.AvatarYRotation);
                        Vector3 v = new Vector3 (0, 0, inputManager.GameState.MovingSpeed);
                        v = Vector3.Transform (v, forwardMovement);
                        Vector3 avatarPosition = inputManager.GameState.AvatarPosition;
                        avatarPosition.Z += v.Z;
                        avatarPosition.X += v.X;
                        inputManager.GameState.AvatarPosition = avatarPosition;
                        LastScroll = inputManager.UpdateGameTime.TotalGameTime;
                        }
                    }
                }
            inputManager.PreviousMouseState = inputManager.CurrentMouseState;
            }

        public TimeSpan LastScroll
            {
            get
                {
                return m_lastScroll;
                }
            set
                {
                m_lastScroll = value;
                }
            }
        }
    }
