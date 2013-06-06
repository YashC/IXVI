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
using InputHandler;
using GameUtilities;
using SkinnedModel;
using HUD;

namespace ModelViewer
    {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game: Microsoft.Xna.Framework.Game
        {
        GraphicsDeviceManager m_graphics;
        SpriteBatch m_spriteBatch;
        Model m_model;
        Model m_avatar;
        AnimationPlayer animationPlayer;
        BoundingBox m_modelExtents;
        HUDComponent headsUpDisplay;

        // Array holding all the bone transform matrices for the entire model.
        // We could just allocate this locally inside the Draw method, but it
        // is more efficient to reuse a single array, as this avoids creating
        // unnecessary garbage.
        Matrix[] m_boneTransforms;
        AnimationClip m_clip;

        Quad m_quad;
        Texture2D m_texture;
        BasicEffect m_quadEffect;

        InputManager m_inputManager;
        MovementManager m_movementManager;
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

        public Game ()
            {
            m_graphics = new GraphicsDeviceManager (this);
            Content.RootDirectory = "Content";
            m_graphics.PreferredBackBufferWidth = 1028;
            m_graphics.PreferredBackBufferHeight = 720;
            }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize ()
            {
            // Makes Input Manager as a service and adds as a component 
            m_gameState = new GameState (this);
            Components.Add (m_gameState);
            m_inputManager = new InputManager (this);
            Services.AddService (typeof (InputManager), m_inputManager);
            Components.Add (m_inputManager);

            m_movementManager = new MovementManager (this);
            Services.AddService (typeof (MovementManager), m_movementManager);
            Components.Add (m_movementManager);

            // Setting up HUD. Must follow setting up MovementManager!
            headsUpDisplay = new HUDComponent (this);
            Components.Add (headsUpDisplay);

            RasterizerState state = new RasterizerState ();
            state.CullMode = CullMode.None;
            m_graphics.GraphicsDevice.RasterizerState = state;
            m_gameState.AspectRatio = m_graphics.GraphicsDevice.Viewport.AspectRatio;

            base.Initialize ();
            }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent ()
            {  
            // Create a new SpriteBatch, which can be used to draw textures.
            m_spriteBatch = new SpriteBatch (GraphicsDevice); 
            m_model = Content.Load<Model> ("Models\\testdgnimodel");
            m_modelExtents = m_model.GetBoundingBox();

            float groundWidth = 10.0f * (m_modelExtents.Max.X - m_modelExtents.Min.X);
            float groundDepth = 10.0f * (m_modelExtents.Max.Z - m_modelExtents.Min.Z);
            float groundOriginX = m_modelExtents.Min.X + (groundWidth * 0.5f);
            float groundOriginZ = m_modelExtents.Min.Z + (groundDepth * 0.5f);
            float groundOriginY = m_modelExtents.Min.Y;
            Vector3 groundOrigin = new Vector3 (0.0f, 0.0f, 0.0f);

            //Copy model and transform to HUDComponent
            headsUpDisplay.Model = m_model;
            headsUpDisplay.Transforms = new Matrix[m_model.Bones.Count];
            m_model.CopyAbsoluteBoneTransformsTo(headsUpDisplay.Transforms);


            m_quad = new Quad (groundOrigin, Vector3.Up, Vector3.Forward, m_gameState.FarClip, m_gameState.FarClip);

            // Allocate the transform matrix array.
            m_boneTransforms = new Matrix[m_model.Bones.Count];

            //Avatar
            m_avatar = Content.Load<Model> ("Avatar\\dude");

            // Look up our custom skinning information.
            SkinningData skinningData = m_avatar.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer (skinningData);

            m_clip = skinningData.AnimationClips["Take 001"];
            animationPlayer.StartClip (m_clip);

            // TODO: Load any ResourceManagementMode.Automatic content
            m_texture = Content.Load<Texture2D> (@"Textures\GreenOnBlackGrid");
            m_quadEffect = new BasicEffect (m_graphics.GraphicsDevice);
            m_quadEffect.EnableDefaultLighting ();
           

            m_quadEffect.World = Matrix.CreateTranslation(Vector3.Zero);
            m_quadEffect.TextureEnabled = true;
            m_quadEffect.Texture = m_texture;
            }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent ()
            {
            // Unload any non ContentManager content here
            }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update (GameTime gameTime)
            {
            // Allows the game to exit
            if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit ();

            animationPlayer.Update (gameTime.ElapsedGameTime, true, Matrix.Identity);

            base.Update (gameTime);
            //FitCameraToScene ();
            }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw (GameTime gameTime)
            {

            m_graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            m_graphics.GraphicsDevice.BlendState = BlendState.Opaque;
            RasterizerState rasterizeState = new RasterizerState ();
            rasterizeState.CullMode = CullMode.None;
            m_graphics.GraphicsDevice.RasterizerState = rasterizeState;
            GraphicsDevice.Clear (Color.CornflowerBlue);

            switch (m_gameState.CameraState)
                {
                default:
                case 0:
                    UpdateCamera ();
                    break;
                case 1:
                    UpdateCameraFirstPerson ();
                    break;
                case 2:
                    UpdateCameraThirdPerson ();
                    break;
                }

            DrawAvatar ();
            if (m_gameState.CameraState == 2 && m_gameState.IsInputActive)
                {
                //animationPlayer.Update (gameTime.ElapsedGameTime, true, Matrix.Identity);
                if (animationPlayer.ClipIsPaused)
                    animationPlayer.ResumeClip ();
                }
            else
                {
                //animationPlayer.Update (new TimeSpan (0, 0, 0), true, Matrix.Identity);
                animationPlayer.PauseClip();
                }
            DrawModel ();
            DrawQuad ();
            base.Draw (gameTime);
            }

        private void DrawAvatar ()
            {
            Matrix[] bones = animationPlayer.GetSkinTransforms (); 

            // Render the skinned mesh.
            foreach (ModelMesh mesh in m_avatar.Meshes)
                {
                foreach (SkinnedEffect effect in mesh.Effects)
                    {
                    effect.SetBoneTransforms (bones);
                    effect.World =  Matrix.CreateRotationY (m_gameState.AvatarYRotation) * Matrix.CreateTranslation (m_gameState.AvatarPosition);
                    effect.View = m_gameState.ViewMatrix;
                    effect.Projection = m_gameState.ProjectionMatrix;

                    effect.EnableDefaultLighting ();

                    effect.SpecularColor = new Vector3 (0.25f);
                    effect.SpecularPower = 16;
                    }

                mesh.Draw ();
                }
            }

        private void DrawQuad ()
            {
            m_quadEffect.World = 
            m_quadEffect.View = m_gameState.ViewMatrix;
            m_quadEffect.Projection = m_gameState.ProjectionMatrix;

            foreach (EffectPass pass in m_quadEffect.CurrentTechnique.Passes)
                { 
                pass.Apply ();
                m_graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture> (
                    PrimitiveType.TriangleList, m_quad.Vertices, 0, 4, m_quad.Indices, 0, 2);
                }

            //Vector3 origin = m_quad.Origin;
            //origin.X -= (m_quad.UpperRight.X - m_quad.UpperLeft.X);
            //m_quad = new Quad (origin, Vector3.Up, Vector3.Forward, m_quad.Width, m_quad.Height);

            //foreach (EffectPass pass in m_quadEffect.CurrentTechnique.Passes)
            //    {
            //    pass.Apply ();
            //    m_graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture> (
            //        PrimitiveType.TriangleList, m_quad.Vertices, 0, 4, m_quad.Indices, 0, 2);
            //    }

            }

        void UpdateCamera ()
            {            
            // Calculate the camera's current position.
            Vector3 cameraPosition = m_gameState.AvatarPosition;
            
            Matrix rotationMatrix = Matrix.CreateRotationY (m_gameState.AvatarYRotation);

            // Create a vector pointing the direction the camera is facing.
            Vector3 transformedReference = Vector3.Transform (m_gameState.CameraReference, rotationMatrix);

            // Calculate the position the camera is looking at.
            Vector3 cameraLookat = cameraPosition +  transformedReference;

            // Set up the view matrix and projection matrix.
            
            m_gameState.ViewMatrix = Matrix.CreateLookAt (cameraPosition, cameraLookat, new Vector3 (0.0f, 1.0f, 0.0f));
                   
            m_gameState.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView (m_gameState.ViewAngle, m_gameState.AspectRatio, m_gameState.NearClip, m_gameState.FarClip);
            }
        
        void UpdateCameraFirstPerson ()
            {
            Matrix rotationMatrix = Matrix.CreateRotationY (m_gameState.AvatarYRotation);


            // Transform the head offset so the camera is positioned properly relative to the avatar.
            Vector3 headOffset = Vector3.Transform (m_gameState.AvatarHeadOffset, rotationMatrix);

            // Calculate the camera's current position.
            Vector3 cameraPosition = m_gameState.AvatarPosition + headOffset;

            // Create a vector pointing the direction the camera is facing.
            Vector3 transformedReference = Vector3.Transform (m_gameState.CameraReference, rotationMatrix);

            // Calculate the position the camera is looking at.
            Vector3 cameraLookat = transformedReference + cameraPosition;

            // Set up the view matrix and projection matrix.

            m_gameState.ViewMatrix = Matrix.CreateLookAt (cameraPosition, cameraLookat, new Vector3 (0.0f, 1.0f, 0.0f)); 

            m_gameState.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView (m_gameState.ViewAngle, m_gameState.AspectRatio, m_gameState.NearClip, m_gameState.FarClip);

            }

        void UpdateCameraThirdPerson ()
            {
            Matrix rotationMatrix = Matrix.CreateRotationY (m_gameState.AvatarYRotation);

            // Create a vector pointing the direction the camera is facing.
            Vector3 transformedReference = Vector3.Transform (m_gameState.ThirdPersonReference, rotationMatrix);

            // Calculate the position the camera is looking from.
            Vector3 cameraPosition = m_gameState.AvatarPosition + transformedReference;
            
            

            // Set up the view matrix and projection matrix.
            Vector3 lookAt = m_gameState.AvatarPosition;
            lookAt.Y += 50.0f;
            m_gameState.ViewMatrix = Matrix.CreateLookAt (cameraPosition, lookAt, new Vector3 (0.0f, 1.0f, 0.0f));

            m_gameState.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView (m_gameState.ViewAngle, m_gameState.AspectRatio, m_gameState.NearClip, m_gameState.FarClip);

            }
        
        private void DrawModel ()
            {
            // Copy any parent transforms.
            m_model.CopyAbsoluteBoneTransformsTo (m_boneTransforms);
            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in m_model.Meshes)
                {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                    {
                    effect.EnableDefaultLighting ();
                    effect.World = m_boneTransforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(m_gameState.ModelRotation)  * Matrix.CreateTranslation(m_gameState.ModelPosition);
                    effect.View = m_gameState.ViewMatrix;
                    effect.Projection = m_gameState.ProjectionMatrix;
                    }
                // Draw the mesh, using the effects set above.
                mesh.Draw ();
                }
            }

        void FitCameraToScene ()
            {
            BoundingSphere sceneSphere = m_model.GetSceneSphere ();

            m_gameState.AvatarPosition = sceneSphere.Center;

            float distanceToCenter = sceneSphere.Radius / (float)Math.Sin (m_gameState.ViewAngle / 2);


            Vector3 back = m_gameState.ViewMatrix.Backward;
            back.X = -back.X; //flip x's sign

            m_gameState.AvatarPosition += (back * distanceToCenter);

            }
        }
    }
