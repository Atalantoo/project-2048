﻿/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using UnityEngine;
using System;

namespace Commons.Inputs
{
    public enum State
    {
        SwipeNotStarted,
        SwipeStarted
    }

    public class MouseSwipeDetector : InputDetector
    {
        private const int MARGIN_IN_DEGREE = 5;

        private State state = State.SwipeNotStarted;
        private Vector2 startPoint;
        private DateTime timeSwipeStarted;
        private TimeSpan maxSwipeDuration = TimeSpan.FromSeconds(1);
        private TimeSpan minSwipeDuration = TimeSpan.FromMilliseconds(100);

        void Update()
        {
            if (Right == null || Left == null || Up == null || Down == null)
                return;

            if (state == State.SwipeNotStarted)
            {
                if (Input.GetMouseButtonDown(0) 
                    && gameObject.IsInSameArea(Input.mousePosition))
                {
                    timeSwipeStarted = DateTime.Now;
                    state = State.SwipeStarted;
                    startPoint = Input.mousePosition;
                }
            }
            else if (state == State.SwipeStarted)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    TimeSpan timeDifference = DateTime.Now - timeSwipeStarted;
                    if (timeDifference <= maxSwipeDuration && timeDifference >= minSwipeDuration)
                    {
                        Vector2 mousePosition = Input.mousePosition;
                        Vector2 differenceVector = mousePosition - startPoint;
                        float angle = Vector2.Angle(differenceVector, Vector2.right);
                        Vector3 cross = Vector3.Cross(differenceVector, Vector2.right);

                        if (cross.z > 0)
                            angle = 360 - angle;

                        state = State.SwipeNotStarted;

                        if (angle > 0 + MARGIN_IN_DEGREE && angle <= 90 - MARGIN_IN_DEGREE)
                            Up();
                        else if (angle > 90 + MARGIN_IN_DEGREE && angle <= 180 - MARGIN_IN_DEGREE)
                            Left();
                        else if (angle > 180 + MARGIN_IN_DEGREE && angle <= 270 - MARGIN_IN_DEGREE)
                            Down();
                        else if (angle > 270 + MARGIN_IN_DEGREE && angle <= 360 - MARGIN_IN_DEGREE)
                            Right();
                    }

                }
            }
        }

    }
}