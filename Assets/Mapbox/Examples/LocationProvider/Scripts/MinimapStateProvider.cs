using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mapbox.Utils;
using Mapbox.Examples.LocationProvider;

public class MinimapStateProvider : MonoBehaviour {

    public GameObject PlayerGameObj;

    public class MinimapStateEventArgs : EventArgs
    {
        public Vector2d LatLonCenter;
        public int Zoom;
    }

    public event EventHandler<MinimapStateEventArgs> OnMinimapStateChanged;

    public double Lat;
    public double Lon;
    public int _zoom;
    public int Zoom
    {
        get { return this._zoom; }
        set { this._zoom = Math.Max(0, Math.Min(20, value)); }
    }

    private void ChangeState(double lat, double lon, int zoom)
    {
        bool isStateReallyChanged = false;
        if(lat != Lat)
        {
            Lat = lat;
            isStateReallyChanged = true;
        }
        if (lon != Lon)
        {
            Lon = lon;
            isStateReallyChanged = true;
        }
        if (zoom != Zoom)
        {
            Zoom = zoom;
            isStateReallyChanged = true;
        }
        if(isStateReallyChanged)
        {
            MinimapStateEventArgs e = new MinimapStateEventArgs();
            e.LatLonCenter = new Vector2d(Lat, Lon);
            e.Zoom = Zoom;
            if(OnMinimapStateChanged != null)
            {
                OnMinimapStateChanged(this, e);
            }
        }

    }//fn

    PositionWithLocationProvider _pwlp;

    void Start () {
        _pwlp = PlayerGameObj.GetComponent<PositionWithLocationProvider>();
        _pwlp.LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
	}

    public void LocationProvider_OnLocationUpdated(object o, Mapbox.Unity.Location.LocationUpdatedEventArgs e)
    {
        ChangeState(e.Location.x, e.Location.y, Zoom);
        _pwlp.LocationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
    }

    public void ZoomPlus()
    {
        ChangeState(Lat, Lon, Zoom + 1);
    }

    public void ZoomMinus()
    {
        ChangeState(Lat, Lon, Zoom - 1);
    }

    private double _moveDelta {
        get {
            return __moveDelta * (20 - Zoom) * 3;
        }
    }
    private double __moveDelta = 0.001;

    public void MoveUp()
    {
        ChangeState(Lat, Lon + _moveDelta, Zoom);
    }

    public void MoveDown()
    {
        ChangeState(Lat, Lon - _moveDelta, Zoom);
    }

    public void MoveLeft()
    {
        ChangeState(Lat - _moveDelta, Lon, Zoom);
    }

    public void MoveRight()
    {
        ChangeState(Lat + _moveDelta, Lon, Zoom);
    }
}
