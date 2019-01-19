// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// %COPYRIGHT_END%
// ---------------------------------------------------------------------
// %BANNER_END%

using System.Collections;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace MagicLeap
{
    /// <summary>
    /// This class provides examples of how you can use haptics and LEDs
    /// on the Control.
    /// </summary>
    [RequireComponent(typeof(ControllerConnectionHandler))]
    public class ControllerFeedbackExample : MonoBehaviour
    {
        #region Private Variables
        private ControllerConnectionHandler _controllerConnectionHandler;

        private int _lastLEDindex = -1;
        [SerializeField] private bool isReadyToStart = false;
        private Rigidbody ballInstance;
        
        #endregion
        #region Public Variables
        public GameObject SelectedPrefab;
        public Rigidbody Ball;
        public GameObject EndModel;
        public Vector3 StartPoint;
        public Vector3 EndPoint;
        public bool StartPlaced;
        public bool EndPlaced;
        public bool Started = false;
        public TextMesh InfoText;
        public Vector3 ControllerLocation, ControllerRotation;

        
        #endregion
        #region Const Variables
        private const float TRIGGER_DOWN_MIN_VALUE = 0.2f;

        // UpdateLED - Constants
        private const float HALF_HOUR_IN_DEGREES = 15.0f;
        private const float DEGREES_PER_HOUR = 12.0f / 360.0f;

        private const int MIN_LED_INDEX = (int)(MLInputControllerFeedbackPatternLED.Clock12);
        private const int MAX_LED_INDEX = (int)(MLInputControllerFeedbackPatternLED.Clock6And12);
        private const int LED_INDEX_DELTA = MAX_LED_INDEX - MIN_LED_INDEX;

        private MLInputController controller;

        private bool showingGhost = false;
        private GameObject ghostObject;
        private bool prelim = true;
        #endregion

        #region Unity Methods
        /// <summary>
        /// Initialize variables, callbacks and check null references.
        /// </summary>
        void Start()
        {

            _controllerConnectionHandler = GetComponent<ControllerConnectionHandler>();
            controller = _controllerConnectionHandler.ConnectedController;

            MLInput.OnControllerButtonUp += HandleOnButtonUp;
            MLInput.OnControllerButtonDown += HandleOnButtonDown;
            MLInput.OnTriggerDown += HandleOnTriggerDown;

        }

        /// <summary>
        /// Update controller input based feedback.
        /// </summary>
        void Update()
        {
            MLInputController controller = _controllerConnectionHandler.ConnectedController;
            ControllerLocation = new Vector3(controller.Position.x, controller.Position.y, controller.Position.z);
            ControllerRotation = new Vector3(controller.Orientation.eulerAngles.x, controller.Orientation.eulerAngles.y, controller.Orientation.eulerAngles.z);
            UpdateLED();
            /*if(controller.TriggerValue >= TRIGGER_DOWN_MIN_VALUE)
            {
                Vector3 newPieceLocation = new Vector3(controller.Position.x, controller.Position.y, controller.Position.z);
                Vector3 newPieceRotation = new Vector3(controller.Orientation.x, controller.Orientation.eulerAngles.y, controller.Orientation.eulerAngles.z);
                Instantiate(TrackPrefab, newPieceLocation, Quaternion.Euler(newPieceRotation));
                if ((controller.Position.x > (newPieceLocation.x + (newPieceLocation.x / 2))) || (controller.Position.x < (newPieceLocation.x + (newPieceLocation.x / 2))))
                {
                    newPieceLocation.x *= 2;
                    Instantiate(TrackPrefab, newPieceLocation, Quaternion.Euler(newPieceRotation));
                }
            }*/
            if(EndPlaced)
            {
                if (!showingGhost)
                {
                    ghostObject = Instantiate(SelectedPrefab, ControllerLocation, Quaternion.Euler(ControllerRotation));
                    showingGhost = true;
                }
                else
                {
                    ghostObject.transform.position = ControllerLocation;
                    ghostObject.transform.rotation = Quaternion.Euler(ControllerRotation);
                }
            }
           
            



            //selectedobject is the prefab
            /*Transform SelectedObject;
            SelectedObject.position = controllerLocation;
            SelectedObject.rotation = Quaternion.Euler(newRotation);*/

        }

        /// <summary>
        /// Stop input api and unregister callbacks.
        /// </summary>
        void OnDestroy()
        {
            if (MLInput.IsStarted)
            {
                MLInput.OnTriggerDown -= HandleOnTriggerDown;
                MLInput.OnControllerButtonDown -= HandleOnButtonDown;
                MLInput.OnControllerButtonUp -= HandleOnButtonUp;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Updates LED on the physical controller based on touch pad input.
        /// </summary>
        private void UpdateLED()
        {
            if (!_controllerConnectionHandler.IsControllerValid())
            {
                return;
            }

            MLInputController controller = _controllerConnectionHandler.ConnectedController;
            if (controller.Touch1Active)
            {
                // Get angle of touchpad position.
                float angle = -Vector2.SignedAngle(Vector2.up, controller.Touch1PosAndForce);
                if (angle < 0.0f)
                {
                    angle += 360.0f;
                }

                // Get the correct hour and map it to [0,6]
                int index = (int)((angle + HALF_HOUR_IN_DEGREES) * DEGREES_PER_HOUR) % LED_INDEX_DELTA;

                // Pass from hour to MLInputControllerFeedbackPatternLED index  [0,6] -> [MAX_LED_INDEX, MIN_LED_INDEX + 1, ..., MAX_LED_INDEX - 1]
                index = (MAX_LED_INDEX + index > MAX_LED_INDEX) ? MIN_LED_INDEX + index : MAX_LED_INDEX;

                if (_lastLEDindex != index)
                {
                    // a duration of 0 means leave it on indefinitely
                    controller.StartFeedbackPatternLED((MLInputControllerFeedbackPatternLED)index, MLInputControllerFeedbackColorLED.BrightCosmicPurple, 0);
                    _lastLEDindex = index;
                }
            }
            else if (_lastLEDindex != -1)
            {
                controller.StopFeedbackPatternLED();
                _lastLEDindex = -1;
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles the event for button down.
        /// </summary>
        /// <param name="controller_id">The id of the controller.</param>
        /// <param name="button">The button that is being pressed.</param>
        private void HandleOnButtonDown(byte controllerId, MLInputControllerButton button)
        {

            if (controller != null && controller.Id == controllerId &&
                button == MLInputControllerButton.Bumper)
            {
                // Demonstrate haptics using callbacks.
                controller.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.ForceDown, MLInputControllerFeedbackIntensity.Medium);
                // Toggle UseCFUIDTransforms
                controller.UseCFUIDTransforms = !controller.UseCFUIDTransforms;
                /*if ((isReadyToStart))
                {*/
                   
                //}
            }
        }

        /// <summary>
        /// Handles the event for button up.
        /// </summary>
        /// <param name="controller_id">The id of the controller.</param>
        /// <param name="button">The button that is being released.</param>
        private void HandleOnButtonUp(byte controllerId, MLInputControllerButton button)
        {
            MLInputController controller = _controllerConnectionHandler.ConnectedController;
            if (controller != null && controller.Id == controllerId &&
                button == MLInputControllerButton.Bumper)
            {
                // Demonstrate haptics using callbacks.
                controller.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.ForceUp, MLInputControllerFeedbackIntensity.Medium);

                if(!Started)
                {
                    Started = true;
                    ballInstance.GetComponent<Rigidbody>().useGravity = true;
                    InfoText.text = "Press Bumper to restart";
                }
                else
                {
                    InfoText.text = "Place your track down, and press the bumper to start.";
                    ballInstance.GetComponent<Rigidbody>().useGravity = true;
                    ballInstance.position = StartPoint;
                    Started = false;
                }
                
            }
            
        }

        /// <summary>
        /// Handles the event for trigger down.
        /// </summary>
        /// <param name="controller_id">The id of the controller.</param>
        /// <param name="value">The value of the trigger button.</param>
        private bool touchingObject = false;
        private void HandleOnTriggerDown(byte controllerId, float value)
        {
            MLInputController controller = _controllerConnectionHandler.ConnectedController;
            if (controller != null && controller.Id == controllerId)
            {

                MLInputControllerFeedbackIntensity intensity = (MLInputControllerFeedbackIntensity)((int)(value * 2.0f));
                controller.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.Buzz, intensity);
                Vector3 newPieceLocation = ControllerLocation;
                Vector3 newPieceRotation = ControllerRotation;
                //Vector3 newPieceLocation = ControllerLocation;
                //Vector3 newPieceRotation = ControllerRotation;
                if (!StartPlaced && !touchingObject)
                {
                    StartPoint = newPieceLocation;
                    ballInstance = Instantiate(Ball, newPieceLocation, Quaternion.identity);
                    StartPlaced = true;
                    InfoText.text = "Set end point";
                }
                else if(!EndPlaced && !touchingObject)
                {
                    EndPoint = newPieceLocation;
                    Instantiate(EndModel, newPieceLocation, Quaternion.identity);
                    EndPlaced = true;
                    InfoText.text = "Place your track down, and press the bumper to start.";
                    isReadyToStart = true;
                }
                else if(!Started && !touchingObject)
                {
                    Instantiate(SelectedPrefab, newPieceLocation, Quaternion.Euler(newPieceRotation));
                    
                }
                



            }
        }
        /*private void OnTriggerEnter(Collider other)
        {
            MLInputController controller = _controllerConnectionHandler.ConnectedController;
            if(other != ghostObject)
            {
                touchingObject = true;
            }

            if (controller.TriggerValue == 1)
            {
                SelectedPrefab = other.GetComponent<GameObject>();
                Destroy(other.gameObject);
                touchingObject = false;
                showingGhost = false;
            }
        }*/

        #endregion
    }
}
