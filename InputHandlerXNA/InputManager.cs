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
using GameUtilities;

namespace InputHandler
    {

    //public interface IInputService
    //    {
    //    bool IsRotate
    //        {
    //        get;
    //        set;
    //        }
    //    bool IsMove
    //        {
    //        get;
    //        set;
    //        }
    //    float DeltaTime
    //        {
    //        get;
    //        set;
    //        }
    //    RotationDirections RotateDirection
    //        {
    //        get;
    //        set;
    //        }
    //    MoveDirections MoveDirection
    //        {
    //        get;
    //        set;
    //        }
    //    }

    public enum InputDeviceType
        {
        KeyBoard,
        Mouse,
        GamePad,
        Kinect
        }

    public enum RotationDirections
        {
        None,
        Left,
        Right,
        Up,
        Down
        }

    public enum MoveDirections
        {
        None,
        Left,
        Right,
        Forward,
        Backward
        }
    
    public class InputManager : GameComponent
        {
        KeyboardState m_currentKeyboardState;
        KeyboardState m_previousKeyboardState;
        MouseState m_currentMouseState;
        MouseState m_previousMouseState;
        Game m_game;
        GameState m_gameState = null;
        KinectInputHandler kinectHandler;
        MouseHandler m_mouseHandler;
        GameTime m_updateGameTime;

        public GameState GameState
            {
            get
                {
                return m_gameState;
                }
            set
                {
                m_gameState = value;
                }
            }
        
        public InputManager (Game game) : base (game)
            {
            m_game = game;
            foreach (object obj in game.Components)
                {
                if (obj.GetType() == typeof (GameState))
                    {
                    m_gameState = obj as GameState;
                    }
                }

            kinectHandler = new KinectInputHandler (this);
            m_mouseHandler = new MouseHandler ();
            }

        public void DisposeHandlers()
            {
            if(kinectHandler != null)
                kinectHandler.StopKinectSensor ();
            }

        public override void Initialize ()
            {
            base.Initialize ();
            }

        /// <summary>
        /// Updates the input states. This method must be called once per frame.
        /// </summary>
        /// <param name="deltaTime">The elapsed time since the last update.</param>
        public override void Update (GameTime gameTime)
            {
            UpdateGameTime = gameTime;

            CurrentKeyboardState =Keyboard.GetState ();                
            KeyboardHandler.ProcessKeyboard (this);

            m_mouseHandler.ProcessMouse (this);

            if(m_gameState.IsKinectConnected)
                kinectHandler.ProcessKinectCommands (this);
            base.Update (gameTime);
            }

        public GameTime UpdateGameTime
            {
            get
                {
                return m_updateGameTime;
                }
            set
                {
                m_updateGameTime = value;
                }
            }

        public KeyboardState CurrentKeyboardState
            {
            get
                {
                return m_currentKeyboardState;
                }
            set
                {
                m_currentKeyboardState = value;
                }
            }

        public KeyboardState PreviousKeyboardState
            {
            get
                {
                return m_previousKeyboardState;
                }
            set
                {
                m_previousKeyboardState = value;
                }
            }


        public MouseState CurrentMouseState
            {
            get
                {
                return m_currentMouseState;
                }
            set
                {
                m_currentMouseState = value;
                }
            }

        public MouseState PreviousMouseState
            {
            get
                {
                return m_previousMouseState;
                }
            set
                {
                m_previousMouseState = value;
                }
            }
        }
    }
