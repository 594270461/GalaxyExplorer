﻿// Copyright Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using UnityEngine;
using Windows.Graphics.Display;
using Windows.UI.Xaml;

namespace GalaxyExplorer
{
    class XamlInputHandling
    {
        public void PointerOrSingleFingerReleased(double x, double y)
        {
            float unityX, unityY;

            ConvertToUnityCoordinates(x, y, out unityX, out unityY);

            UnityPlayer.AppCallbacks.Instance.InvokeOnAppThread(() =>
                {
                    if (InputRouter.Instance != null)
                    {
                        InputRouter.Instance.XamlMousePosition.x = (float)unityX;
                        InputRouter.Instance.XamlMousePosition.y = (float)unityY;

                        InputRouter.Instance.InternalHandleOnTapped();
                    }
                },
                waitUntilDone: false);
        }

        public void PointerMoved(double x, double y)
        {
            float unityX, unityY;

            ConvertToUnityCoordinates(x, y, out unityX, out unityY);

            UnityPlayer.AppCallbacks.Instance.InvokeOnAppThread(() =>
                {
                    if (InputRouter.Instance != null)
                    {
                        InputRouter.Instance.XamlMousePosition.x = (float)unityX;
                        InputRouter.Instance.XamlMousePosition.y = (float)unityY;
                    }
                },
                waitUntilDone: false);
        }

        public void ZoomHappened(double scaleDelta)
        {
            UnityPlayer.AppCallbacks.Instance.InvokeOnAppThread(() =>
                {
                    if (InputRouter.Instance != null)
                    {
                        InputRouter.Instance.UpdateZoomFromXaml(scaleDelta);
                    }
                }, waitUntilDone: false);
        }

        public void RotationHappened(double rotationDelta)
        {
            UnityPlayer.AppCallbacks.Instance.InvokeOnAppThread(() =>
            {
                if (InputRouter.Instance != null)
                {
                    InputRouter.Instance.UpdateRotationFromXaml(rotationDelta);
                }
            }, waitUntilDone: false);
        }

        public void TranslateHappened(Vector2 translateDelta)
        {
            UnityPlayer.AppCallbacks.Instance.InvokeOnAppThread(() =>
            {
                if (InputRouter.Instance != null)
                {
                    InputRouter.Instance.UpdateCameraFromXaml(translateDelta);
                }
            }, waitUntilDone: false);
        }

        /// <summary>
        /// Unity and XAML have different coordinate systems in two different ways that need to be accounted for.
        ///    1. Y == 0 for the top of the window in XAML and Y == 0 for the bottom of the window in Unity
        ///    2. Unity is using raw pixel values while XAML is using DPI aware logical pixels
        /// </summary>
        private void ConvertToUnityCoordinates(double xamlX, double xamlY, out float unityX, out float unityY)
        {
            double rawPixelsPerViewPixel = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            // Adjust DPI aware xamlX to raw pixel value
            unityX = (float)(xamlX * rawPixelsPerViewPixel);
           
            // Adjust DPI aware xamlY to raw pixel value and change the origin
            unityY = (float)((Window.Current.Bounds.Height - xamlY) * rawPixelsPerViewPixel);
        }
    }
}
