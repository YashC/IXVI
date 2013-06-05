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
using System.Windows.Media;
using Kinect.Sensor;


namespace GameUtilities
    {
    public class GameState: GameComponent
        {

        Vector3 m_avatarPosition = new Vector3(0.0f, 50.0f,1000.0f);
        Vector3 m_thirdPersonReference = new Vector3 (0, 50, 200);
        // Set the direction the camera points without rotation.
        Vector3 m_cameraReference = new Vector3 (0, 0, -1);
        Vector3 m_cameraAvatarOffset = new Vector3 (0.0f, 0.0f, 200.0f);
        Vector3 m_avatarHeadOffset = new Vector3 (0, 70, -10);
       
        Vector3 m_cameraPosition;
        //direction to which Camera Points to. 
        Vector3 m_cameraTarget;
        Vector3 m_cameraUpVector;
        Vector3 m_world;
        // Set the position of the model in world space
        Vector3 m_modelPosition = Vector3.Zero;
        //default model rotation
        float m_modelRotation = 0.0f;

        Vector3 m_cameraSpeed = Vector3.Zero;

        int m_cameraState = 2;
        float m_avatarYRotation = MathHelper.ToRadians (0.0f);            
        Quaternion m_cameraRotation = Quaternion.Identity;

        float m_aspectRatio;
        float m_turningSpeed = 1.0f / 60.0f;
        float m_movingSpeed = 2.0f;
        float m_moveStep = 0.1f;
        bool m_isInputActive = false;

        Vector2 m_cursorScreenLocation = new Vector2 (0.0f, 0.0f);
		bool m_isKinectConnected = false;  
        ColorImageData m_kinectVideoColors; 
		
        bool m_showCursor = false;
        bool m_showInfo = false;

        public Vector2 CursorScreenLocation
            {
            get
                {
                return m_cursorScreenLocation;
                }
            set
                {
                m_cursorScreenLocation = value;
                }
            }

        public bool ShowCursor
            {
            get
                {
                return m_showCursor;
                }
            set
                {
                m_showCursor = value;
                }
            }

        public bool ShowInfo
            {
            get
                {
                return m_showInfo;
                }
            set
                {
                m_showInfo = value;
                }
            }

        public bool IsInputActive
            {
            get
                {
                return m_isInputActive;
                }
            set
                {
                m_isInputActive = value;
                }
            }

        public float MoveStep
            {
            get
                {
                return m_moveStep;
                }
            set
                {
                m_moveStep = value;
                }
            }
        // float m_cameraSpeed = 10.0f;
        float m_direction = 1.0f;
        float m_changeElevation = 0.0f;
        float m_changePanLeftRight = 0.0f;
        
        float m_rotationInDegree = 1;
        float m_nearClip = 1.0f;
        float m_farClip = 10000.0f;
        float m_viewAngle = MathHelper.PiOver4;
        Matrix m_viewMatrix;
        Matrix m_projectionMatrix;


        public GameState (Game game)
            : base (game)
            {          
            
            }


        public Vector3 ModelPosition
            {
            get
                {
                return m_modelPosition;
                }
            set
                {
                m_modelPosition = value;
                }
            }

        public Vector3 CameraTarget
            {
            get
                {
                return m_cameraTarget;
                }
            set
                {
                m_cameraTarget = value;
                }
            }

        public Vector3 CameraPosition
            {
            get
                {
                return m_cameraPosition;
                }
            set
                {
                m_cameraPosition = value;
                }
            }

        public Vector3 CameraUpVector
            {
            get
                {
                return m_cameraUpVector;
                }
            set
                {
                m_cameraUpVector = value;
                }
            }

        public float AspectRatio
            {
            get
                {
                return m_aspectRatio;
                }
            set
                {
                m_aspectRatio = value;
                }
            }

        public Matrix ViewMatrix
            {
            get
                {
                return m_viewMatrix;
                }
            set
                {
                m_viewMatrix = value;
                }
            }

        public Matrix ProjectionMatrix
            {
            get
                {
                return m_projectionMatrix;
                }
            set
                {
                m_projectionMatrix = value;
                }
            }

        public float RotationInDegree
            {
            get
                {
                return m_rotationInDegree;
                }
            set
                {
                m_rotationInDegree = value;
                }
            }

        public float NearClip
            {
            get
                {
                return m_nearClip;
                }
            set
                {
                m_nearClip = value;
                }
            }

        public float FarClip
            {
            get
                {
                return m_farClip;
                }
            set
                {
                m_farClip = value;
                }
            }

        public float ViewAngle
            {
            get
                {
                return m_viewAngle;
                }
            set
                {
                m_viewAngle = value;
                }
            }

        public float ModelRotation
            {
            get
                {
                return m_modelRotation;
                }
            set
                {
                m_modelRotation = value;
                }
            }

        public float TurningSpeed
            {
            get
                {
                return m_turningSpeed;
                }
            set
                {
                m_turningSpeed = value;
                }
            }

        public float MovingSpeed
            {
            get
                {
                return m_movingSpeed;
                }
            set
                {
                m_movingSpeed = value;
                }
            }

        public Vector3 AvatarHeadOffset
            {
            get
                {
                return m_avatarHeadOffset;
                }
            set
                {
                m_avatarHeadOffset = value;
                }
            }
        public Vector3 AvatarPosition
            {
            get
                {
                return m_avatarPosition;
                }
            set
                {
                m_avatarPosition = value;
                }
            }
        public float AvatarYRotation
            {
            get
                {
                return m_avatarYRotation;
                }
            set
                {
                m_avatarYRotation = value;
                }
            }
        public Vector3 ThirdPersonReference
            {
            get
                {
                return m_thirdPersonReference;
                }
            set
                {
                m_thirdPersonReference = value;
                }
            }
       
        public Quaternion CameraRotation
            {
            get
                {
                return m_cameraRotation;
                }
            set
                {
                m_cameraRotation = value;
                }
            }
        
        public Vector3 CameraReference
            {
            get
                {
                return m_cameraReference;
                }
            set
                {
                m_cameraReference = value;
                }
            }

        public int CameraState
            {
            get
                {
                return m_cameraState;
                }
            set
                {
                m_cameraState = value;
                }
            }


        public bool IsKinectConnected
            {
            get
                {
                return m_isKinectConnected;
                }
            set
                {
                m_isKinectConnected = value;
                }
            }

        public ColorImageData KinectVideoColors
            {
            get
                {
                return m_kinectVideoColors;
                }
            set
                {
                m_kinectVideoColors = value;
                }
            }
        }
    }
