using Microsoft.Kinect;
using System.Collections.Generic;
using System;


namespace Kinect.Gestures.Segments
{
    public class RightHandSelectSegment : IRelativeGestureSegment
    {
        public static List<SkeletonPoint> sPoints;
        public static int count = 0;
        /// <summary>
        /// Checks the gesture.
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns>GesturePartResult based on if the gesture part has been completed</returns>
        public GesturePartResult CheckGesture (Skeleton skeleton, DepthImagePoint head, DepthImagePoint leftHand, DepthImagePoint rightHand)
        {

        if (rightHand.Depth >= head.Depth)
            {
            //Console.WriteLine ("Skipping because of head");
            if(sPoints != null)
                sPoints.Clear ();
            return GesturePartResult.Fail;
            }

        if (Math.Abs (leftHand.Depth - head.Depth) > 170)
            {
            //Console.WriteLine ("Skipping bcause of Left hand" + leftHand.Depth + ";" + head.Depth);
            if (sPoints != null)
                sPoints.Clear ();
            return GesturePartResult.Fail;
            }

        if (sPoints == null)
            {         
            sPoints = new List<SkeletonPoint> ();
            }

         SkeletonPoint sp = skeleton.Joints[JointType.HandRight].Position;
         if (sPoints.Count > 0)
             {
             if (ComparePoints (sp, sPoints[sPoints.Count - 1]))
                 {
                 sPoints.Add (sp);
                 //Console.WriteLine (sPoints.Count);
                 if (sPoints.Count > 15)
                     {
                     //Console.WriteLine ("*************************");
                     //foreach (SkeletonPoint point in sPoints)
                     //    {
                     //    Console.WriteLine ("{0},{1},{2}", point.X, point.Y, point.Z);
                     //    }                       
                     //Console.WriteLine ("*************************");
                     sPoints.Clear ();
                     return GesturePartResult.Succeed;
                     }
                 else
                     {
                     return GesturePartResult.Pausing;
                     }
                 }
             else
                 {
                 //Console.WriteLine ("Skipping bcause of Movement");
                 sPoints.Clear ();
                 return GesturePartResult.Fail;
                 }
             }
         else
             {
             sPoints.Add (sp);
             return GesturePartResult.Pausing;
             }
            }  

        public bool ComparePoints (SkeletonPoint point1, SkeletonPoint point2)
            {
            bool valid = true;
            if (Math.Abs(point1.X - point2.X) > 3)
                valid = false;
            if (Math.Abs(point1.Y - point2.Y) > 3)
                valid = false;
            if (Math.Abs(point1.Z - point2.Y) > 3)
                valid = false;
            return valid;
            }


    }      
}
