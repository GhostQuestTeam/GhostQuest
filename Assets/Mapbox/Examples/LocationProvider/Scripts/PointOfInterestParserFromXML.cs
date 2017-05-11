using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using Mapbox.Utils;

public class PointOfInterestParserFromXML : MonoBehaviour {

    public string RootPath;
    public List<string> FilesToParse = new List<string>();
    public List<Vector2d> ParsedPoints;

    // Use this for initialization
    void Awake () {
        RootPath = Application.dataPath + "/Mapbox/Examples/LocationProvider/";
        FilesToParse.Add(RootPath + "moscow-poi-tourism-attraction.osm.xml");
        FilesToParse.Add(RootPath + "moscow-poi-tourism-museum.osm.xml");
        FilesToParse.Add(RootPath + "moscow-poi-tourism-viewpoint.osm.xml");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<Vector2d> parseXmlFiles()
    {
        ParsedPoints = new List<Vector2d>();
        foreach (string path in FilesToParse)
        {
            parseXmlFile(path);
        }
        return ParsedPoints;
    }

    public void parseXmlFile(string filePath)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);
        XmlNodeList nodes = doc.SelectNodes("*/node");
        foreach(XmlNode node in nodes)
        {
            string strLat = node.Attributes["lat"].Value;
            string strLon = node.Attributes["lon"].Value;
            ParsedPoints.Add( new Vector2d( double.Parse(strLat), double.Parse(strLon) ) );
        }
    } 

}
