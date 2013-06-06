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


namespace GameUtilities
    {
    public static class Extensions
        {
        static public BoundingBox GetBoundingBox(this Model model)
            {
            return BoundingBox.CreateFromSphere(GetSceneSphere(model));
            }

        static public BoundingSphere GetSceneSphere(this Model model)
            {
            BoundingSphere sceneSphere = new BoundingSphere();
            foreach (ModelMesh mesh in model.Meshes)
                {
                BoundingSphere boundingSphere = mesh.BoundingSphere;
                sceneSphere = BoundingSphere.CreateMerged(sceneSphere, boundingSphere);
                }
            return sceneSphere;
            }

        public static string GetSelectedObjectName (GameState gameState)
            {
            string objectName = string.Empty;

            float shortestDiff = 10000000000.0f;
            Vector2 cursorPos = gameState.CursorScreenLocation;
            //Ray ray = new Ray (gameState.CameraPosition, gameState.CameraTarget);
            BoundingSphere boundingSphere;
            foreach (ModelMesh mesh in gameState.Model.Meshes)
                {
                boundingSphere = mesh.BoundingSphere.Transform (gameState.BoneTransforms[mesh.ParentBone.Index] * Matrix.CreateRotationY (gameState.ModelRotation) * Matrix.CreateTranslation (gameState.ModelPosition));

                Vector3 screenSpace = gameState.GraphicsDevice.Viewport.Project (boundingSphere.Center, gameState.ProjectionMatrix, gameState.ViewMatrix, Matrix.Identity);
                Vector2 positionOnScreen = new Vector2 (screenSpace.X, screenSpace.Y);
                Vector2 diff = positionOnScreen - cursorPos;
                float diffLength = diff.Length ();
                if (diffLength < shortestDiff)
                    {
                    shortestDiff = diffLength;
                    if (shortestDiff <= gameState.SelectionRadius)
                        objectName = mesh.Name;
                    }
                }

            //Console.WriteLine (objectName);
            return objectName;
            }
        }
    }
