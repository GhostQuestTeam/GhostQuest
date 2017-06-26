using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class POIInfoWindowController {


    public string POIInfoWindowPrefabPath { get; set; }
    public GameObject POIInfoWidowParent { get; set; }
    public POIInfoWindowUIWrapper UIWrapper
    {
        get { return _uiWrapper; }
    }
    public Vector3 Position
    {
        get { return _position; }
        set
        {
            _position = value;
            move(_position);
        }
    }
    public UnityAction CloseCallback
    {
        get { return _closeCallback; }
        set { setCloseCallback(value); }
    }
    public UnityAction ToFightCallback
    {
        get { return _toFightCallback; }
        set { setToFightCallback(value); }
    }

    private POIInfoWindowUIWrapper _uiWrapper;
    private GameObject _poiInfoWindowObject;
    private Vector3 _position;
    private UnityAction _closeCallback;
    private UnityAction _toFightCallback;

    private int _nFarPosition = 10000;

    public void Initialize
        (
            string prefabPath = "",
            GameObject parent = null,
            Vector3 position = new Vector3(),
            UnityAction closeCb = null,
            UnityAction toFightCb = null
        )
    {
        POIInfoWindowPrefabPath = prefabPath;
        POIInfoWidowParent = parent;
        Position = position;
        CloseCallback = closeCb;
        ToFightCallback = toFightCb;
        make();
    }

    public bool make()
    {
        if(_poiInfoWindowObject != null)
        {
            GameObject.Destroy(_poiInfoWindowObject);
            _poiInfoWindowObject = null;
        }
            
        _poiInfoWindowObject = GameObject.Instantiate<GameObject>
            (
                Resources.Load<GameObject>(POIInfoWindowPrefabPath),
                new Vector3(_nFarPosition, _nFarPosition),
                Quaternion.identity
            );

        if (POIInfoWidowParent != null)
            _poiInfoWindowObject.transform.SetParent(POIInfoWidowParent.transform);

        hide();
        move(Position);

        _uiWrapper = new POIInfoWindowUIWrapper();
        _uiWrapper.BaseObject = _poiInfoWindowObject;

        setCloseCallback(_closeCallback);
        setToFightCallback(_toFightCallback);

        return true;
    }


    public void applyPOIMetadata(GameSparksPOIsExtraction.ExtractedPointMetadata metadata)
    {
        //КАКОЙ-ТО БАГ - ПРОВЕРКА НА NULL НЕ ПОХОДИТСЯ, НО РАБОТАЕТ 0_0
        //if(_uiWrapper != null)
        //{
            _uiWrapper.PointOwner = metadata.uoid; //ЭМ, ПОКА ЧТО
            _uiWrapper.Info = metadata.LatLon.ToString();
            foreach(string k in metadata.enemies.Keys)
            {
                _uiWrapper.ghosts.add(k, metadata.enemies[k]);
            }
        //}
    }


    public void hide()
    {
        if(_poiInfoWindowObject != null)
            _poiInfoWindowObject.transform.localScale = Vector3.zero;
    }

    public void show()
    {
        if (_poiInfoWindowObject != null)
            _poiInfoWindowObject.transform.localScale = Vector3.one;
    }

    public void move(Vector3 position)
    {
        if (_poiInfoWindowObject != null && position != null)
            _poiInfoWindowObject.transform.localPosition = position;
    }

    public void setCloseCallback(UnityAction cb)
    {
        _closeCallback = cb;
        if(UIWrapper != null)
        {
            UIWrapper.Close.onClick.RemoveAllListeners();
            if (cb != null)
                UIWrapper.Close.onClick.AddListener(cb);
        }
    }

    public void setToFightCallback(UnityAction cb)
    {
        _toFightCallback = cb;
        if (UIWrapper != null)
        {
            UIWrapper.ToFight.onClick.RemoveAllListeners();
            if(cb != null)
                UIWrapper.ToFight.onClick.AddListener(cb);
        }
    }

}
