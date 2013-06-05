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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using GameUtilities;

namespace HUD
    {   
    public class HUDComponent : DrawableGameComponent
        {
        // Game Sprite Batch
        private SpriteBatch m_gameBatch;

        // Game 
        private Game m_game;
        private GameState m_gameState;

        #region DataDisplay
        private Texture2D m_popupRectical = null;
        private Texture2D m_popupInfoBoxes = null;
        private string m_displayInfo = string.Empty;
        private Color m_infoColor = Color.White;
        private SpriteFont m_infoFont;
        public string DisplayInfo
            {
            get
                {
                return m_displayInfo;
                }
            set
                {
                m_displayInfo = value;
                }
            }
        #endregion

        public HUDComponent (Game game) : base(game)
            {
            // Set game content manager create new game batch
            m_game = game;
            m_gameBatch = new SpriteBatch (game.GraphicsDevice);

            foreach (object obj in game.Components)
                {
                if (obj.GetType () == typeof (GameState))
                    {
                    m_gameState = obj as GameState;
                    }
                }


            // Only allows one instance, this is just here to ensure this
            if (Instance == null)
                m_instance = this;
            else
                throw new Exception ("");
            }

        // Instance of the HUD Component
        private static HUDComponent m_instance;
        public static HUDComponent Instance
            {
            get
                {
                return m_instance;
                }
            }

        protected override void LoadContent ()
            {
            m_popupRectical = m_game.Content.Load<Texture2D>(@"Sprites\Rectical");
            m_popupInfoBoxes = m_game.Content.Load<Texture2D> (@"Sprites\InfoBoxes");
            m_infoFont = m_game.Content.Load<SpriteFont> ("Sprites\\default");
            m_displayInfo = "This is a Test";
            base.LoadContent ();
            }

        public override void Initialize ()
            {
            base.Initialize ();
            }

        public override void Draw (GameTime gameTime)
            {
            m_gameBatch.Begin ();            
            //m_gameBatch.Draw (m_popupImage, new Vector2 (5, 100), Color.White);
            Vector2 screenLocation = m_gameState.CursorScreenLocation;
            screenLocation.X -= m_popupRectical.Width * 0.5f;
            screenLocation.Y -= m_popupRectical.Height * 0.5f;
            if (m_gameState.CursorSelected)
                {
                screenLocation.Y -= 47.0f;
                m_gameBatch.Draw (m_popupInfoBoxes, screenLocation, Color.White);
                }
            else
                {
                m_gameBatch.Draw (m_popupRectical, screenLocation, Color.White);
                }
            //m_gameBatch.DrawString (m_infoFont, m_displayInfo, new Vector2 (37, 200), m_infoColor);
            m_gameBatch.End ();
            base.Draw (gameTime);
            }
        }
    }
