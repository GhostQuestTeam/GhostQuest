using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Mapbox.Map;
using Mapbox.Unity;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;

public class MinimapController : MonoBehaviour, Mapbox.Utils.IObserver<RasterTile>
{

    [SerializeField]
    RawImage _imageContainer;

    Map<RasterTile> _map;

    int _mapstyle = 1;
    string[] _mapboxStyles = new string[]
    {
            "mapbox://styles/mapbox/satellite-v9",
            "mapbox://styles/mapbox/streets-v9",
            "mapbox://styles/mapbox/dark-v9",
            "mapbox://styles/mapbox/light-v9"
    };

    MinimapStateProvider _msp;

    void Awake()
    {
        _msp = gameObject.GetComponent<MinimapStateProvider>();
        _msp.OnMinimapStateChanged += MinimapStateProvider_OnMinimapStateChanged;
    }

    public void MinimapStateProvider_OnMinimapStateChanged(object o, MinimapStateProvider.MinimapStateEventArgs e)
    {
        _map.Center = e.LatLonCenter;
        _map.Zoom = e.Zoom;
        _map.Update();
    }

    void OnDestroy()
    {
        _msp.OnMinimapStateChanged -= MinimapStateProvider_OnMinimapStateChanged;
    }

    void Start()
    {
        _map = new Map<RasterTile>(MapboxAccess.Instance);
        _map.MapId = _mapboxStyles[_mapstyle];
        _map.Subscribe(this);
        //_map.Update();
    }


    /// <summary>
    /// Update the texture with new data.
    /// </summary>
    /// <param name="tile">Tile.</param>
    public void OnNext(RasterTile tile)
    {
        if (tile.CurrentState != Tile.State.Loaded || tile.Error != null)
        {
            return;
        }

        // Can we utility this? Should users have to know source size?
        var texture = new Texture2D(256, 256);
        texture.LoadImage(tile.Data);
        _imageContainer.texture = texture;
    }
}

