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
        }
    }
