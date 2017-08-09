using System.Collections;
using HauntedCity.Geo;
using Mapbox.Unity.Location;
using ModestTree;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    public Text text;
    public GameObject prefab;
    public Transform container;

    // Use this for initialization
    void Start()
    {
    }

    void OnEnable()
    {

        StartCoroutine(DelayedStart());

    }

    IEnumerator DelayedStart()
    {
        yield return  new WaitForSeconds(2f); 
        var poiRoot = GameObject.Find("POIRoot").transform;
        foreach (Transform child in poiRoot)
        {
            var poi = Instantiate(prefab);
            poi.transform.SetParent(container, false);
            poi.GetComponent<Text>().text = child.GetComponent<PointOfInterestWithLocationProvider>()._metadata.LatLon.ToString();
            
        }
    }

    public void Log(string message)
    {
        text.text = message;
    }
    
    
    

    // Update is called once per frame
    void Update()
    {
    }
}