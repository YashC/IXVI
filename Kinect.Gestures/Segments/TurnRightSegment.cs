using Microsoft.Kinect;

namespace Kinect.Gestures.Segments
    {
    /// <summary>
    /// The menu gesture segment
    /// </summary>
    public class TurnRightSegment: IRelativeGestureSegment
        {
        /// <summary>
        /// Checks the gesture.
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns>GesturePartResult based on if the gesture part has been completed</returns>
        public GesturePartResult CheckGesture (Skeleton skeleton, DepthImagePoint head, DepthImagePoint leftHand, DepthImagePoint rightHand)
            {
            // Left and right hands below hip
            if (skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.HipCenter].Position.Y && skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.HipCenter].Position.Y)
                {
                // left hand 0.3 to left of center hip
                if (skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.HipCenter].Position.X + 0.3)
                    {
                    // left hand 0.2 to left of left elbow
                    if (skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.ElbowRight].Position.X + 0.2)
                        {
                        return GesturePartResult.Succeed;
                        }
                    }

                return GesturePartResult.Pausing;
                }

            return GesturePartResult.Fail;
            }
        }
    }
