//------------------------------------------------------------------------------
// <copyright file="GestureDetector.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using UnityEngine;
using System.Collections;

public class GestureEventArgs : EventArgs
{

    
    public bool IsBodyTrackingIdValid { get; private set; }

    public bool IsGestureDetected { get; private set; }

    public float DetectionConfidence { get; private set; }

    public GestureEventArgs(bool isBodyTrackingIdValid, bool isGestureDetected, float detectionConfidence)
    {
        this.IsBodyTrackingIdValid = isBodyTrackingIdValid;
        this.IsGestureDetected = isGestureDetected;
        this.DetectionConfidence = detectionConfidence;
    }
}

/// <summary>
/// Gesture Detector class which listens for VisualGestureBuilderFrame events from the service
/// and calls the OnGestureDetected event handler when a gesture is detected.
/// </summary>
public class GestureDetector : IDisposable
{
    
    /// <summary> Path to the gesture database that was trained with VGB </summary>
    private readonly string liftEarthdb = "LiftEarth.gba";
    private readonly string armSmallLiftdb = "LiftEarth_ArmSmallLift.gba";

    private readonly string militaryPressdb = "MilitaryPress.gbd";
    /// <summary> Name of the discrete gesture in the database that we want to track </summary>
    private readonly string liftEarthGesture = "LiftEarth";
    private readonly string armSmallLiftGesture = "LiftEarth_ArmSmallLift";
    //-----------
    private readonly string MP_false1Gesture = "FalseGesture1";
    //Error 1: Y Shaped Straight Arm
    private readonly string MP_false2Gesture = "FalseGesture2";
    //Error 2: Elbows too low/arm too low

    private readonly string MP_Level1Gesture = "Level1";
    //
    private readonly string MP_Level2Gesture = "Level2";
    //
    private readonly string MP_Level3Gesture = "Level3";
    //


    /// <summary> Gesture frame source which should be tied to a body tracking ID </summary>
    private VisualGestureBuilderFrameSource vgbFrameSource = null;

    /// <summary> Gesture frame reader which will handle gesture events coming from the sensor </summary>
    private VisualGestureBuilderFrameReader vgbFrameReader = null;

    public event EventHandler<GestureEventArgs> OnGestureDetected;

    /// <summary>
    /// Initializes a new instance of the GestureDetector class along with the gesture frame source and reader
    /// </summary>
    /// <param name="kinectSensor">Active sensor to initialize the VisualGestureBuilderFrameSource object with</param>
    public GestureDetector(KinectSensor kinectSensor)
    {
        if (kinectSensor == null)
        {
            throw new ArgumentNullException("kinectSensor");
        }

        // create the vgb source. The associated body tracking ID will be set when a valid body frame arrives from the sensor.
        this.vgbFrameSource = VisualGestureBuilderFrameSource.Create(kinectSensor, 0);
        this.vgbFrameSource.TrackingIdLost += this.Source_TrackingIdLost;

        // open the reader for the vgb frames
        this.vgbFrameReader = this.vgbFrameSource.OpenReader();
        if (this.vgbFrameReader != null)
        {
            this.vgbFrameReader.IsPaused = true;
            this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
        }

        // load the 'Seated' gesture from the gesture database
        var databasePath = Path.Combine(Application.streamingAssetsPath, this.militaryPressdb);
        using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(databasePath))
        {
            // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
            // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
            foreach (Gesture gesture in database.AvailableGestures)
            {
                //if (gesture.Name.Equals(this.liftEarthGesture))
                //{
                    this.vgbFrameSource.AddGesture(gesture);
                //}
            }
        }
        //var smallArmLift_dbPath = Path.Combine(Application.streamingAssetsPath, this.armSmallLiftdb);
        //using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(smallArmLift_dbPath))
        //{
        //    // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
        //    // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
        //    foreach (Gesture gesture in database.AvailableGestures)
        //    {
        //        if (gesture.Name.Equals(this.armSmallLiftGesture))
        //        {
        //            this.vgbFrameSource.AddGesture(gesture);
        //        }
        //    }
        //}
    }

    /// <summary>
    /// Gets or sets the body tracking ID associated with the current detector
    /// The tracking ID can change whenever a body comes in/out of scope
    /// </summary>
    public ulong TrackingId
    {
        get
        {
            return this.vgbFrameSource.TrackingId;
        }

        set
        {
            if (this.vgbFrameSource.TrackingId != value)
            {
                this.vgbFrameSource.TrackingId = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether or not the detector is currently paused
    /// If the body tracking ID associated with the detector is not valid, then the detector should be paused
    /// </summary>
    public bool IsPaused
    {
        get
        {
            return this.vgbFrameReader.IsPaused;
        }

        set
        {
            if (this.vgbFrameReader.IsPaused != value)
            {
                this.vgbFrameReader.IsPaused = value;
            }
        }
    }

    /// <summary>
    /// Disposes all unmanaged resources for the class
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader objects
    /// </summary>
    /// <param name="disposing">True if Dispose was called directly, false if the GC handles the disposing</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (this.vgbFrameReader != null)
            {
                this.vgbFrameReader.FrameArrived -= this.Reader_GestureFrameArrived;
                this.vgbFrameReader.Dispose();
                this.vgbFrameReader = null;
            }

            if (this.vgbFrameSource != null)
            {
                this.vgbFrameSource.TrackingIdLost -= this.Source_TrackingIdLost;
                this.vgbFrameSource.Dispose();
                this.vgbFrameSource = null;
            }
        }
    }

    /// <summary>
    /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {
        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {
            if (frame != null)
            {
                // get the discrete gesture results which arrived with the latest frame
                var discreteResults = frame.DiscreteGestureResults;

                if (discreteResults != null)
                {
                    // we only have one gesture in this source object, but you can get multiple gestures
                    foreach (Gesture gesture in this.vgbFrameSource.Gestures)
                    {
                        if (gesture.Name.Equals(this.MP_false1Gesture) && gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);
                            if (result != null)
                            {
                                if (result.Confidence >= .8)
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(true, result.Detected, result.Confidence));
                                    KinectManager.km.FalseGesture1();
                                    Debug.Log("False1: Elbow not bent");
                                }
                                else
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(false, result.Detected, result.Confidence));
                                }
                            }
                        }
                        
                        if (gesture.Name.Equals(this.MP_false2Gesture) && gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);
                            if (result != null)
                            {
                                if (result.Confidence >= .7)
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(true, result.Detected, result.Confidence));
                                    KinectManager.km.FalseGesture2();
                                    Debug.Log("False2: Eblow too low");
                                }
                                else
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(false, result.Detected, result.Confidence));
                                }
                            }
                        }

                        if (gesture.Name.Equals(this.MP_Level1Gesture) && gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);
                            if (result != null)
                            {
                                if (result.Confidence >= .9)
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(true, result.Detected, result.Confidence));
                                    KinectManager.km.MP_Level1();
                                    Debug.Log("30%");
                                }
                                else
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(false, result.Detected, result.Confidence));
                                }
                            }
                        }
                        if (gesture.Name.Equals(this.MP_Level2Gesture) && gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);
                            if (result != null)
                            {
                                if (result.Confidence >= .8)
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(true, result.Detected, result.Confidence));
                                    KinectManager.km.MP_Level2();
                                    Debug.Log("60%");
                                }
                                else
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(false, result.Detected, result.Confidence));
                                }
                            }
                        }

                        if (gesture.Name.Equals(this.MP_Level3Gesture) && gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);
                            if (result != null)
                            {
                                if (result.Confidence >= .8)
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(true, result.Detected, result.Confidence));
                                    KinectManager.km.MP_Level3();
                                    Debug.Log("100%");
                                }
                                else
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(false, result.Detected, result.Confidence));
                                }
                            }
                        }

                    }
                }
            }
        }
    }

    /// <summary>
    /// Handles the TrackingIdLost event for the VisualGestureBuilderSource object
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void Source_TrackingIdLost(object sender, TrackingIdLostEventArgs e)
    {
        if (this.OnGestureDetected != null)
        {
            this.OnGestureDetected(this, new GestureEventArgs(false, false, 0.0f));
        }
    }
}
