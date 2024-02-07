using Assets.Codebase.Gameplay.Input;
using Assets.Codebase.Infrastructure.ServicesManagment.ModelAccess;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Models.Gameplay.Data;
using Assets.Codebase.Views.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Codebase.Gameplay.Tutorial;

namespace DavidJalbert.TinyCarControllerAdvance
{
    public class TCCAMobileInput : MonoBehaviour
    {
        public TCCAPlayer carController;

        [Tooltip("Whether the mouse can be used as input for the on-screen controls.")]
        public bool simulateTouchWithMouse = true;
        [Tooltip("The color of the UI element when idle.")]
        public Color colorIdle = new Color(1f, 1f, 1f, 0.5f);
        [Tooltip("The color of the UI element when touched.")]
        public Color colorTouched = new Color(1f, 1f, 1f, 1f);

        [Tooltip("The UI graphic container for the steering wheel.")]
        public RectTransform steeringWheel;
        [Tooltip("The UI area of the steering wheel that will be checked for touches.")]
        public RectTransform steeringWheelTouchArea;
        [Tooltip("The value by which to multiply the value of the steering. Useful if you want to clamp the steering to its min/max value.")]
        public float steeringWheelMultiplier = 2f;

        public RectTransform rightCorner;
        public RectTransform leftCorner;
        [Tooltip("The UI graphic container and touch area for the gas pedal.")]
        public RectTransform gasPedal;
        [Tooltip("The UI graphic container and touch area for the brake pedal.")]
        public RectTransform brakePedal;
        [Tooltip("The UI graphic container and touch area for the respawn button.")]
        public RectTransform respawnButton;
        [Tooltip("The UI graphic container and touch area for the pause button.")]
        public RectTransform pauseButton;
        [Tooltip("Object which is clicked to trigger boost.")]
        public ClickHandler boostClickableObject;
        [Tooltip("Object which is clicked to trigger respawn.")]
        public ClickHandler respawnClickableObject;
        [Tooltip("Object which is clicked to trigger pause.")]
        public ClickHandler pauseClickableObject;
        [Tooltip("Tutorial reference")]
        public MobileTutorial tutorialObject;

        private GraphicRaycaster raycaster;
        private Graphic steeringWheelGraphic;
        private Graphic gasPedalGraphic;
        private Graphic brakePedalGraphic;
        private Graphic respawnButtonGraphic;
        private Graphic pauseButtonGraphic;

        private int _boostClickCounter = 0;
        private Coroutine _boostClickResetter;

        void Start()
        {
            raycaster = GetComponent<GraphicRaycaster>();
            if (raycaster == null)
            {
                raycaster = gameObject.AddComponent<GraphicRaycaster>();
            }

            steeringWheelGraphic = steeringWheel.GetComponentInChildren<Graphic>();
            gasPedalGraphic = gasPedal.GetComponentInChildren<Graphic>();
            brakePedalGraphic = brakePedal.GetComponentInChildren<Graphic>();
            respawnButtonGraphic = respawnButton.GetComponentInChildren<Graphic>();
            pauseButtonGraphic = pauseButton.GetComponentInChildren<Graphic>();

            if (pauseButtonGraphic != null) pauseButtonGraphic.color = colorIdle;
            if (respawnButtonGraphic != null) respawnButtonGraphic.color = colorIdle;
        }

        private void OnEnable()
        {
            boostClickableObject.OnClick += BoostClicked;
            respawnClickableObject.OnClick += RespawnClicked;
            pauseClickableObject.OnClick += PauseClicked;
        }

        private void OnDisable()
        {
            boostClickableObject.OnClick -= BoostClicked;
            respawnClickableObject.OnClick -= RespawnClicked;
            pauseClickableObject.OnClick -= PauseClicked;
        }

        void Update()
        {
            bool steeringWheelTouched = false;
            float steeringWheelDelta = 0;
            bool gasPedalTouched = false;
            bool brakePedalTouched = false;
            bool boostButtonTouched = false;

            List<PointerEventData> pointers = new List<PointerEventData>();

            foreach (Touch touch in Input.touches)
            {
                PointerEventData pointer = new PointerEventData(EventSystem.current);
                pointer.position = touch.position;
                pointers.Add(pointer);
            }

            if (simulateTouchWithMouse && Input.GetMouseButton(0))
            {
                PointerEventData pointer = new PointerEventData(EventSystem.current);
                pointer.position = Input.mousePosition;
                pointers.Add(pointer);
            }

            foreach (PointerEventData pointer in pointers)
            {
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointer, results);
                foreach (RaycastResult result in results)
                {
                    Graphic graphic = result.gameObject.GetComponent<Graphic>();
                    if (graphic != null)
                    {
                        Vector2 uiScreenPosition = RectTransformUtility.PixelAdjustPoint(graphic.transform.position, graphic.transform, graphic.canvas);
                        Vector2 rayScreenPosition = result.screenPosition;
                        Vector2 relativePosition = rayScreenPosition - uiScreenPosition;
                        Vector2 positionDelta = new Vector2(relativePosition.x / (graphic.rectTransform.rect.width * graphic.rectTransform.lossyScale.x), relativePosition.y / (graphic.rectTransform.rect.height * graphic.rectTransform.lossyScale.y)) * 2f;

                        if (steeringWheelTouchArea != null && result.gameObject == steeringWheelTouchArea.gameObject)
                        {
                            steeringWheelTouched = true;
                            steeringWheelDelta = Mathf.Clamp(positionDelta.x * steeringWheelMultiplier, -1, 1);
                        }
                        else if (gasPedal != null && result.gameObject == gasPedal.gameObject)
                        {
                            gasPedalTouched = true;
                        }
                        else if (brakePedal != null && result.gameObject == brakePedal.gameObject)
                        {
                            brakePedalTouched = true;
                        }
                        else if (respawnButton != null && result.gameObject == respawnButton.gameObject)
                        {
                            boostButtonTouched = true;
                        }
                    }
                }
            }

            if (carController != null)
            {
                if (steeringWheelTouched)
                {
                    if (steeringWheelGraphic != null) steeringWheelGraphic.color = colorTouched;
                    steeringWheel.localRotation = Quaternion.Euler(0, 0, -steeringWheelDelta * 90);
                    carController.setSteering(steeringWheelDelta);
                }
                else
                {
                    if (steeringWheelGraphic != null) steeringWheelGraphic.color = colorIdle;
                    steeringWheel.localRotation = Quaternion.identity;
                    carController.setSteering(0);
                }

                if (gasPedalTouched)
                {
                    if (gasPedalGraphic != null) gasPedalGraphic.color = colorTouched;
                    carController.setMotor(1);
                }
                else
                {
                    if (gasPedalGraphic != null) gasPedalGraphic.color = colorIdle;
                    carController.setMotor(0);
                }

                if (brakePedalTouched)
                {
                    if (brakePedalGraphic != null) brakePedalGraphic.color = colorTouched;
                    carController.setHandbrake(true);
                }
                else
                {
                    if (brakePedalGraphic != null) brakePedalGraphic.color = colorIdle;
                    carController.setHandbrake(false);
                }

                //if (boostButtonTouched)
                //{
                //    if (boostButtonGraphic != null) boostButtonGraphic.color = colorTouched;
                //    carController.setBoost(1);
                //    carController.setMotor(1);
                //}
                //else
                //{
                //    if (boostButtonGraphic != null) boostButtonGraphic.color = colorIdle;
                //    carController.setBoost(0);
                //}
            }
        }

        private void PauseClicked()
        {
            var gameplayModel = ServiceLocator.Container.Single<IModelAccessService>().GameplayModel;
            if (gameplayModel.State.Value != GameState.Pause)
            {
                gameplayModel.PauseGame();
                gameplayModel.ActivateView(ViewId.Pause);
            }
            else
            {
                gameplayModel.UnPauseGame(GameState.Race);
                gameplayModel.ActivateView(ViewId.Ingame);
            }
        }

        private void RespawnClicked()
        {
            carController.immobilize();
            carController.setPosition(carController.getRespawnPosition() + Vector3.up);
            carController.setRotation(carController.getRespawnRotation());

            foreach (TrailRenderer t in carController.GetComponentsInChildren<TrailRenderer>())
            {
                t.Clear();
            }
        }

        private void BoostClicked()
        {
            _boostClickCounter++;
            if (_boostClickCounter >= 2) 
            {
                carController.setBoost(1);
                carController.setMotor(1);
            }
            else
            {
                carController.setBoost(0);
            }

            if (_boostClickResetter != null)
            {
                StopCoroutine(_boostClickResetter);
                _boostClickResetter = null;
            }

            _boostClickResetter = StartCoroutine(ResetBoostClickCountAfterDelay());
        }

        private IEnumerator ResetBoostClickCountAfterDelay()
        {
            yield return new WaitForSeconds(0.5f);
            _boostClickCounter = 0;
        }

        public void ShowInput(bool isShown)
        {
            rightCorner.gameObject.SetActive(isShown);
            leftCorner.gameObject.SetActive(isShown);
        }
    }
}