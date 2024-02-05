﻿using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.ModelAccess;
using Assets.Codebase.Models.Gameplay.Data;
using Assets.Codebase.Views.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DavidJalbert.TinyCarControllerAdvance
{
    public class TCCAStandardInput : MonoBehaviour
    {
        public enum InputType
        {
            None, Axis, RawAxis, Key, Button
        }

        [System.Serializable]
        public struct InputValue
        {
            [Tooltip("Type of input.")]
            public InputType type;
            [Tooltip("Name of the input entry.")]
            public string name;
            [Tooltip("Returns the negative value when using an axis type.")]
            public bool invert;
        }

        public TCCAPlayer carController;

        [Header("Input")]
        [Tooltip("Whether to let this script control the vehicle.")]
        public bool enableInput = true;
        [Tooltip("Input type to check to make the vehicle move forward.")]
        public InputValue forwardInput = new InputValue() { type = InputType.RawAxis, name = "Vertical", invert = false };
        [Tooltip("Input type to check to make the vehicle move backward.")]
        public InputValue reverseInput = new InputValue() { type = InputType.RawAxis, name = "Vertical", invert = true };
        [Tooltip("Input type to check to make the vehicle turn right.")]
        public InputValue steerRightInput = new InputValue() { type = InputType.RawAxis, name = "Horizontal", invert = false };
        [Tooltip("Input type to check to make the vehicle turn left.")]
        public InputValue steerLeftInput = new InputValue() { type = InputType.RawAxis, name = "Horizontal", invert = true };
        [Tooltip("Input type to check to apply the handbrake.")]
        public InputValue handbrakeInput = new InputValue() { type = InputType.Key, name = "space", invert = false };
        [Tooltip("Input type to check to give the vehicle a speed boost.")]
        public InputValue boostInput = new InputValue() { type = InputType.Key, name = "left shift", invert = false };
        [Tooltip("Input type to reset the vehicle to its original position.")]
        public InputValue respawnInput = new InputValue() { type = InputType.Key, name = "r", invert = false };
        [Tooltip("Input type to pause game.")]
        public InputValue pauseInput = new InputValue() { type = InputType.Key, name = "escape", invert = false };

        private float respawnPreviousValue = 0;
        private float pausePreviousValue = 0;

        void Update()
        {
            if (enableInput && carController != null)
            {
                float motorDelta = getInput(forwardInput) - getInput(reverseInput);
                float steeringDelta = getInput(steerRightInput) - getInput(steerLeftInput);
                bool handbrake = getInput(handbrakeInput) >= 0.5f;
                float boostDelta = getInput(boostInput);
                float respawnValue = getInput(respawnInput);
                float pauseValue = getInput(pauseInput);
                bool respawn = respawnValue >= 0.5f && respawnPreviousValue < 0.5f;
                bool pause = pauseValue >= 0.5f && pausePreviousValue < 0.5f;
                respawnPreviousValue = respawnValue;
                pausePreviousValue = pauseValue;

                if (pause)
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

                if (respawn)
                {
                    carController.immobilize();
                    carController.setPosition(carController.getRespawnPosition() + Vector3.up);
                    carController.setRotation(carController.getRespawnRotation());

                    foreach (TrailRenderer t in carController.GetComponentsInChildren<TrailRenderer>())
                    {
                        t.Clear();
                    }
                    return;
                }

                carController.setMotor(motorDelta);
                carController.setSteering(steeringDelta);
                carController.setHandbrake(handbrake);
                carController.setBoost(boostDelta);
            }
        }

        public float getInput(InputValue v)
        {
            float value = 0;
            switch (v.type)
            {
                case InputType.Axis: value = Input.GetAxis(v.name); break;
                case InputType.RawAxis: value = Input.GetAxisRaw(v.name); break;
                case InputType.Key: value = Input.GetKey(v.name) ? 1 : 0; break;
                case InputType.Button: value = Input.GetButton(v.name) ? 1 : 0; break;
            }
            if (v.invert) value *= -1;
            return Mathf.Clamp01(value);
        }
    }
}