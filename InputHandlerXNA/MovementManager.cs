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
using GameUtilities;


namespace InputHandler
    {
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MovementManager : Microsoft.Xna.Framework.GameComponent
        {
        private MouseState m_mouseState;
        Game m_game;
        GameState m_gameState;

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

        public MovementManager (Game game)
            : base (game)
            {
            m_game = game;
            foreach (object obj in game.Components)
                {
                if (obj.GetType () == typeof (GameState))
                    {
                    m_gameState = obj as GameState;
                    }
                }
            }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize ()
            {
            // TODO: Add your initialization code here

            base.Initialize ();
            }

        public MouseState CurrentMouseState
            {
            get
                {
                return m_mouseState;
                }
            set
                {
                m_mouseState = value;
                }
            }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update (GameTime gameTime)
            {
            CurrentMouseState = Mouse.GetState ();
            MouseMovementHandler.ProcessMouse (this);
            base.Update (gameTime);
            }

        public Vector2 CursorPosition
            {
            get
                {
                return GameState.CursorScreenLocation;
                }
            set
                {
                GameState.CursorScreenLocation = value;
                }
            }

        public bool CursorSelected
            {
            get
                {
                return GameState.CursorSelected;
                }
            set
                {
                GameState.CursorSelected = value;
                }
            }
        }
    }
