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
        private Vector2 m_infoDialogLocation;
        private Vector2 m_nameLocation;
        private Vector2 m_propertyLocation;
        private Vector2 m_propertyValueLocation;

        private bool m_infoShown = false;
        
        private Model m_model;
        private Matrix[] m_transforms;

        Texture2D m_kinectVideo;

        List<string> insideBoundingSpheres = new List<string>();
        string pickedModelName;
        string pickedMeshName;

        // Vertex array that stores exactly which triangle was picked.
        VertexPositionColor[] pickedTriangle =
        {
            new VertexPositionColor(Vector3.Zero, Color.Magenta),
            new VertexPositionColor(Vector3.Zero, Color.Magenta),
            new VertexPositionColor(Vector3.Zero, Color.Magenta),
        };

        // Effect and vertex declaration for drawing the picked triangle.
        BasicEffect lineEffect;

        // Custom rasterizer state for drawing in wireframe.
        static RasterizerState WireFrame = new RasterizerState
        {
            FillMode = FillMode.WireFrame,
            CullMode = CullMode.None
        };

        #region DataDisplay
        private Texture2D m_popupRetical = null;
        private Texture2D m_popupInfoBoxes = null;
        private string m_displayInfo = string.Empty;
        private Color m_infoColor = Color.White;
        private SpriteFont m_infoFont;
        private bool m_showRectical = false;
        private bool m_showInfoBoxes = false;

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
                if (obj.GetType() == typeof(GameState))
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
            m_popupRetical = m_game.Content.Load<Texture2D> (@"Sprites\Retical");
            m_gameState.SelectionRadius = m_popupRetical.Height * 0.5f;
            m_popupInfoBoxes = m_game.Content.Load<Texture2D> (@"Sprites\Dialog Left");
            m_infoFont = m_game.Content.Load<SpriteFont> (@"Sprites\\default");
            m_displayInfo = "This is a Test";

            lineEffect = new BasicEffect(m_game.GraphicsDevice);
            lineEffect.VertexColorEnabled = true;

            base.LoadContent ();
            }

        public override void Initialize ()
            {
            base.Initialize ();
            }

        public override void Draw (GameTime gameTime)
            {
            m_gameBatch.Begin ();            
            Vector2 mouseLocation = m_gameState.CursorScreenLocation;

            if (m_gameState.ShowInfo)
                {
                string selectedObjName = Extensions.GetSelectedObjectName (m_gameState);
                }

            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
                {
                UpdatePicking();

                // Draw the outline of the triangle under the cursor.
                DrawPickedTriangle();

                if (!String.IsNullOrEmpty(pickedMeshName))
                    {
                    string text = "Mesh selected: " + pickedMeshName;

                    m_gameBatch.DrawString(m_infoFont, text,
                                           m_nameLocation, Color.Black);

                    m_gameBatch.DrawString(m_infoFont, text,
                                           m_propertyLocation, Color.White);
                    }
                }
            if (m_infoShown && m_gameState.ShowInfo && m_infoDialogLocation != null)
                {
                InfoDialog (mouseLocation, false);
                m_gameBatch.End ();
                base.Draw (gameTime);
                return;
                }
            
            Vector2 reticalLocation = m_gameState.CursorScreenLocation;

            reticalLocation.X -= m_popupRetical.Width * 0.5f;            
            reticalLocation.Y -= m_popupRetical.Height * 0.5f;
            m_showRectical = m_gameState.ShowCursor;
            m_showInfoBoxes = m_gameState.ShowInfo;
            if (m_showInfoBoxes)
                {
                m_infoShown = true;
                InfoDialog (mouseLocation, true);
                }
            else if (m_showRectical)
                {
                m_infoShown = false;
                m_gameBatch.Draw (m_popupRetical, reticalLocation, Color.White);
                }

            KinectVideo ();
            
            m_gameBatch.End ();
            base.Draw (gameTime);
            }
        // Draws the Info Dialog
        private void InfoDialog (Vector2 mouseLocation, bool updateLocation)
            {
            if (updateLocation)
                {
                // Set locations and which sprite to use
                if (mouseLocation.X < m_game.Window.ClientBounds.Width / 2 &&
                    mouseLocation.Y < m_game.Window.ClientBounds.Height / 2)
                    {
                    // Top Left
                    // Dialog locations
                    m_infoDialogLocation.X = mouseLocation.X - 75;
                    m_infoDialogLocation.Y = mouseLocation.Y - 142;
                    m_popupInfoBoxes = m_game.Content.Load<Texture2D> (@"Sprites\Dialog Left");

                    // Text locations
                    m_nameLocation.X = mouseLocation.X + 130;
                    m_nameLocation.Y = mouseLocation.Y - 40;

                    m_propertyLocation.X = mouseLocation.X + 50;
                    m_propertyLocation.Y = mouseLocation.Y + 70;

                    m_propertyValueLocation.X = mouseLocation.X + 250;
                    m_propertyValueLocation.Y = mouseLocation.Y + 70;
                    }
                else if (mouseLocation.X < m_game.Window.ClientBounds.Width / 2 &&
                        mouseLocation.Y >= m_game.Window.ClientBounds.Height / 2)
                    {
                    // Bottom Left
                    m_infoDialogLocation.X = mouseLocation.X - 72;
                    m_infoDialogLocation.Y = mouseLocation.Y - 406;
                    m_popupInfoBoxes = m_game.Content.Load<Texture2D> (@"Sprites\Dialog Bottom Left");

                    // Text locations
                    m_nameLocation.X = mouseLocation.X + 130;
                    m_nameLocation.Y = mouseLocation.Y + 10;

                    m_propertyLocation.X = mouseLocation.X + 50;
                    m_propertyLocation.Y = mouseLocation.Y - 90;

                    m_propertyValueLocation.X = mouseLocation.X + 250;
                    m_propertyValueLocation.Y = mouseLocation.Y - 90;
                    }
                else if (mouseLocation.X >= m_game.Window.ClientBounds.Width / 2 &&
                        mouseLocation.Y < m_game.Window.ClientBounds.Height / 2)
                    {
                    // Top Right
                    m_infoDialogLocation.X = mouseLocation.X - 406;
                    m_infoDialogLocation.Y = mouseLocation.Y - 141;
                    m_popupInfoBoxes = m_game.Content.Load<Texture2D> (@"Sprites\Dialog Right");

                    // Text locations
                    m_nameLocation.X = mouseLocation.X - 250;
                    m_nameLocation.Y = mouseLocation.Y - 40;

                    m_propertyLocation.X = mouseLocation.X - 370;
                    m_propertyLocation.Y = mouseLocation.Y + 70;

                    m_propertyValueLocation.X = mouseLocation.X - 170;
                    m_propertyValueLocation.Y = mouseLocation.Y + 70;
                    }
                else if (mouseLocation.X >= m_game.Window.ClientBounds.Width / 2 &&
                        mouseLocation.Y >= m_game.Window.ClientBounds.Height / 2)
                    {
                    // Bottom Right
                    m_infoDialogLocation.X = mouseLocation.X - 406;
                    m_infoDialogLocation.Y = mouseLocation.Y - 406;
                    m_popupInfoBoxes = m_game.Content.Load<Texture2D> (@"Sprites\Dialog Bottom Right");

                    // Text locations
                    m_nameLocation.X = mouseLocation.X - 250;
                    m_nameLocation.Y = mouseLocation.Y + 10;

                    m_propertyLocation.X = mouseLocation.X - 370;
                    m_propertyLocation.Y = mouseLocation.Y - 90;

                    m_propertyValueLocation.X = mouseLocation.X - 170;
                    m_propertyValueLocation.Y = mouseLocation.Y - 90;
                    }
                }
            // Draw the info box and its text
            m_gameBatch.Draw (m_popupInfoBoxes, m_infoDialogLocation, Color.White);
            m_gameBatch.DrawString (m_infoFont, m_gameState.ComponentName, m_nameLocation, m_infoColor);
            m_gameBatch.DrawString (m_infoFont, m_gameState.Property, m_propertyLocation, m_infoColor);
            m_gameBatch.DrawString (m_infoFont, m_gameState.PropertyValue, m_propertyValueLocation, m_infoColor);
            }

        // Draws the Kinect Video display if the Kinect is available
        private void KinectVideo ()
            {
            if (!m_gameState.IsKinectConnected && m_gameState.KinectVideoColors.ColorImage == null)
                return;

            m_kinectVideo.SetData (m_gameState.KinectVideoColors.ColorImage);
            m_gameBatch.Draw (m_kinectVideo, new Rectangle (0, 0, 128, 128), Color.White);                
            }

        public Model Model
            {
            get
                {
                return m_model;
                }
            set
                {
                m_model = value;
                }
            }

        public Matrix[] Transforms
            {
            get
                {
                return m_transforms;
                }
            set
                {
                m_transforms = value;
                }
            }


        /// <summary>
        /// Helper for drawing the outline of the triangle currently under the cursor.
        /// </summary>
        void DrawPickedTriangle()
            {
            if (pickedModelName != null)
                {
                GraphicsDevice device = m_game.GraphicsDevice;

                // Set line drawing renderstates. We disable backface culling
                // and turn off the depth buffer because we want to be able to
                // see the picked triangle outline regardless of which way it is
                // facing, and even if there is other geometry in front of it.
                device.RasterizerState = WireFrame;
                device.DepthStencilState = DepthStencilState.None;

                // Activate the line drawing BasicEffect.
                lineEffect.Projection = m_gameState.ProjectionMatrix;
                lineEffect.View = m_gameState.ViewMatrix;

                lineEffect.CurrentTechnique.Passes[0].Apply();

                // Draw the triangle.
                device.DrawUserPrimitives(PrimitiveType.TriangleList,
                                          pickedTriangle, 0, 1);

                // Reset renderstates to their default values.
                device.RasterizerState = RasterizerState.CullCounterClockwise;
                device.DepthStencilState = DepthStencilState.Default;
                }
            }



        /// <summary>
        /// Runs a per-triangle picking algorithm over all the models in the scene,
        /// storing which triangle is currently under the cursor.
        /// </summary>
        void UpdatePicking()
            {
            // Look up a collision ray based on the current cursor position. See the
            // Picking Sample documentation for a detailed explanation of this.
            Ray cursorRay = CalculateCursorRay(m_gameState.ProjectionMatrix, m_gameState.ViewMatrix);

            // Clear the previous picking results.
            insideBoundingSpheres.Clear();

            pickedModelName = null;

            // Keep track of the closest object we have seen so far, so we can
            // choose the closest one if there are several models under the cursor.
            float closestIntersection = float.MaxValue;


            bool insideBoundingSphere;
            Vector3 vertex1, vertex2, vertex3;
            string selectedMeshName;

            // Perform the ray to model intersection test.
            float? intersection = RayIntersectsModel(cursorRay, m_model,
                                                        m_transforms[m_model.Meshes[0].ParentBone.Index], 
                                                        out insideBoundingSphere,
                                                        out vertex1, out vertex2,
                                                        out vertex3, out selectedMeshName);

            // If this model passed the initial bounding sphere test, remember
            // that so we can display it at the top of the screen.
            if (insideBoundingSphere)
                insideBoundingSpheres.Add("main");

            // Do we have a per-triangle intersection with this model?
            if (intersection != null)
                {
                // If so, is it closer than any other model we might have
                // previously intersected?
                if (intersection < closestIntersection)
                    {
                    // Store information about this model.
                    closestIntersection = intersection.Value;

                    pickedModelName = "main";

                    // Store vertex positions so we can display the picked triangle.
                    pickedTriangle[0].Position = vertex1;
                    pickedTriangle[1].Position = vertex2;
                    pickedTriangle[2].Position = vertex3;

                    pickedMeshName = selectedMeshName;
                    }
                }
            }


        /// <summary>
        /// Checks whether a ray intersects a model. This method needs to access
        /// the model vertex data, so the model must have been built using the
        /// custom TrianglePickingProcessor provided as part of this sample.
        /// Returns the distance along the ray to the point of intersection, or null
        /// if there is no intersection.
        /// </summary>
        static float? RayIntersectsModel(Ray ray, Model model, Matrix modelTransform,
                                         out bool insideBoundingSphere,
                                         out Vector3 vertex1, out Vector3 vertex2,
                                         out Vector3 vertex3, out string selectedMeshName)
            {
            vertex1 = vertex2 = vertex3 = Vector3.Zero;
            selectedMeshName = "";

            // The input ray is in world space, but our model data is stored in object
            // space. We would normally have to transform all the model data by the
            // modelTransform matrix, moving it into world space before we test it
            // against the ray. That transform can be slow if there are a lot of
            // triangles in the model, however, so instead we do the opposite.
            // Transforming our ray by the inverse modelTransform moves it into object
            // space, where we can test it directly against our model data. Since there
            // is only one ray but typically many triangles, doing things this way
            // around can be much faster.

            Matrix inverseTransform = Matrix.Invert(modelTransform);

            ray.Position = Vector3.Transform(ray.Position, inverseTransform);
            ray.Direction = Vector3.TransformNormal(ray.Direction, inverseTransform);

            // Look up our custom collision data from the Tag property of the model.
            Dictionary<string, object> tagData = (Dictionary<string, object>)model.Tag;

            if (tagData == null)
                {
                throw new InvalidOperationException(
                    "Model.Tag is not set correctly. Make sure your model " +
                    "was built using the custom TrianglePickingProcessor.");
                }

            // Start off with a fast bounding sphere test.
            BoundingSphere boundingSphere = (BoundingSphere)tagData["BoundingSphere"];

            if (boundingSphere.Intersects(ray) == null)
                {
                // If the ray does not intersect the bounding sphere, we cannot
                // possibly have picked this model, so there is no need to even
                // bother looking at the individual triangle data.
                insideBoundingSphere = false;

                return null;
                }
            else
                {
                // The bounding sphere test passed, so we need to do a full
                // triangle picking test.
                insideBoundingSphere = true;

                // Keep track of the closest triangle we found so far,
                // so we can always return the closest one.
                float? closestIntersection = null;

                // Loop over the vertex data, 3 at a time (3 vertices = 1 triangle).
                Vector3[] vertices = (Vector3[])tagData["Vertices"];
                string[] verticesMeshes = (String[])tagData["VerticesMeshes"];

                for (int i = 0; i < vertices.Length; i += 3)
                    {
                    // Perform a ray to triangle intersection test.
                    float? intersection;

                    RayIntersectsTriangle(ref ray,
                                          ref vertices[i],
                                          ref vertices[i + 1],
                                          ref vertices[i + 2],
                                          out intersection);

                    // Does the ray intersect this triangle?
                    if (intersection != null)
                        {
                        // If so, is it closer than any other previous triangle?
                        if ((closestIntersection == null) ||
                            (intersection < closestIntersection))
                            {
                            // Store the distance to this triangle.
                            closestIntersection = intersection;

                            // Transform the three vertex positions into world space,
                            // and store them into the output vertex parameters.
                            Vector3.Transform(ref vertices[i],
                                              ref modelTransform, out vertex1);

                            Vector3.Transform(ref vertices[i + 1],
                                              ref modelTransform, out vertex2);

                            Vector3.Transform(ref vertices[i + 2],
                                              ref modelTransform, out vertex3);

                            selectedMeshName = verticesMeshes[i];
                            }
                        }
                    }

                return closestIntersection;
                }
            }


        /// <summary>
        /// Checks whether a ray intersects a triangle. This uses the algorithm
        /// developed by Tomas Moller and Ben Trumbore, which was published in the
        /// Journal of Graphics Tools, volume 2, "Fast, Minimum Storage Ray-Triangle
        /// Intersection".
        /// 
        /// This method is implemented using the pass-by-reference versions of the
        /// XNA math functions. Using these overloads is generally not recommended,
        /// because they make the code less readable than the normal pass-by-value
        /// versions. This method can be called very frequently in a tight inner loop,
        /// however, so in this particular case the performance benefits from passing
        /// everything by reference outweigh the loss of readability.
        /// </summary>
        static void RayIntersectsTriangle(ref Ray ray,
                                          ref Vector3 vertex1,
                                          ref Vector3 vertex2,
                                          ref Vector3 vertex3, out float? result)
            {
            // Compute vectors along two edges of the triangle.
            Vector3 edge1, edge2;

            Vector3.Subtract(ref vertex2, ref vertex1, out edge1);
            Vector3.Subtract(ref vertex3, ref vertex1, out edge2);

            // Compute the determinant.
            Vector3 directionCrossEdge2;
            Vector3.Cross(ref ray.Direction, ref edge2, out directionCrossEdge2);

            float determinant;
            Vector3.Dot(ref edge1, ref directionCrossEdge2, out determinant);

            // If the ray is parallel to the triangle plane, there is no collision.
            if (determinant > -float.Epsilon && determinant < float.Epsilon)
                {
                result = null;
                return;
                }

            float inverseDeterminant = 1.0f / determinant;

            // Calculate the U parameter of the intersection point.
            Vector3 distanceVector;
            Vector3.Subtract(ref ray.Position, ref vertex1, out distanceVector);

            float triangleU;
            Vector3.Dot(ref distanceVector, ref directionCrossEdge2, out triangleU);
            triangleU *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (triangleU < 0 || triangleU > 1)
                {
                result = null;
                return;
                }

            // Calculate the V parameter of the intersection point.
            Vector3 distanceCrossEdge1;
            Vector3.Cross(ref distanceVector, ref edge1, out distanceCrossEdge1);

            float triangleV;
            Vector3.Dot(ref ray.Direction, ref distanceCrossEdge1, out triangleV);
            triangleV *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (triangleV < 0 || triangleU + triangleV > 1)
                {
                result = null;
                return;
                }

            // Compute the distance along the ray to the triangle.
            float rayDistance;
            Vector3.Dot(ref edge2, ref distanceCrossEdge1, out rayDistance);
            rayDistance *= inverseDeterminant;

            // Is the triangle behind the ray origin?
            if (rayDistance < 0)
                {
                result = null;
                return;
                }

            result = rayDistance;
            }


        // CalculateCursorRay Calculates a world space ray starting at the camera's
        // "eye" and pointing in the direction of the cursor. Viewport.Unproject is used
        // to accomplish this. see the accompanying documentation for more explanation
        // of the math behind this function.
        public Ray CalculateCursorRay(Matrix projectionMatrix, Matrix viewMatrix)
            {
            // create 2 positions in screenspace using the cursor position. 0 is as
            // close as possible to the camera, 1 is as far away as possible.
            Vector3 nearSource = new Vector3(m_gameState.CursorScreenLocation, 0f);
            Vector3 farSource = new Vector3(m_gameState.CursorScreenLocation, 1f);

            // use Viewport.Unproject to tell what those two screen space positions
            // would be in world space. we'll need the projection matrix and view
            // matrix, which we have saved as member variables. We also need a world
            // matrix, which can just be identity.
            Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(nearSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 farPoint = GraphicsDevice.Viewport.Unproject(farSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // and then create a new ray using nearPoint as the source.
            return new Ray(nearPoint, direction);
            }

        }
    }
