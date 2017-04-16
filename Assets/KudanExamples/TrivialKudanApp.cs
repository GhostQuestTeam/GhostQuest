using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kudan.AR;

public class TrivialKudanApp : MonoBehaviour {

    public KudanTracker _kudanTracker;  // The tracker to be referenced in the inspector. This is the Kudan Camera object.
    public TrackingMethodMarker _markerTracking;    // The reference to the marker tracking method that lets the tracker know which method it is using
    public TrackingMethodMarkerless _markerlessTracking;	// The reference to the markerless tracking method that lets the tracker know which method it is using

    void Start()
    {
        _kudanTracker.ChangeTrackingMethod(_markerlessTracking);
    }

    public void StartMarkerlessTracking(Vector3 pos, Quaternion orient)
    {
        _kudanTracker.ChangeTrackingMethod(_markerlessTracking);
        //_kudanTracker.ArbiTrackStart(pos, orient);              // Starts markerless tracking based upon the given floor position and orientations

        // from the floor placer.
        Vector3 floorPosition;          // The current position in 3D space of the floor
        Quaternion floorOrientation;    // The current orientation of the floor in 3D space, relative to the device

        _kudanTracker.FloorPlaceGetPose(out floorPosition, out floorOrientation);   // Gets the position and orientation of the floor and assigns the referenced Vector3 and Quaternion those values
        _kudanTracker.ArbiTrackStart(floorPosition, floorOrientation);
    }

    public void StopMarkerlessTracking()
    {
        _kudanTracker.ArbiTrackStop();
    }

}
