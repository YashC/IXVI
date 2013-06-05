using Microsoft.Kinect;

using System.IO;
using System;

namespace Kinect.Gestures.Segments
    {
    public class MoveForwardSegment: IRelativeGestureSegment
        {

        /// <summary>
        /// Checks the gesture.
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns>GesturePartResult based on if the gesture part has been completed</returns>
        public GesturePartResult CheckGesture (Skeleton skeleton, DepthImagePoint head, DepthImagePoint leftHand, DepthImagePoint rightHand)
            {

            if (leftHand.Depth < head.Depth - 200 && rightHand.Depth < head.Depth - 200)
                {
                // Left and right hands below hip
                if (skeleton.Joints[JointType.ElbowLeft].Position.Y > (skeleton.Joints[JointType.Spine].Position.Y + 0.1) && skeleton.Joints[JointType.ElbowRight].Position.Y > (skeleton.Joints[JointType.Spine].Position.Y + 0.1))
                    {
                    // left hand 0.3 to left of center hip
                    if (skeleton.Joints[JointType.WristLeft].Position.Y > skeleton.Joints[JointType.HipLeft].Position.Y + 0.05)
                        {
                        // left hand 0.2 to left of left elbow
                        if (skeleton.Joints[JointType.WristRight].Position.Y > skeleton.Joints[JointType.HipRight].Position.Y + 0.05)
                            {
                            return GesturePartResult.Succeed;
                            }
                        }

                    return GesturePartResult.Pausing;
                    }
                }
            return GesturePartResult.Fail;
            }

        }
    }
