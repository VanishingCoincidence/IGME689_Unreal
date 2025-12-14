using UnityEngine;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class River
{
    public string Name;
    public List<List<double[]>> CoordinatesList;
    //public List<double[]> Coordinates;
    //public bool isLineString = true;
    
    public River(string name, List<List<double[]>> coordinatesList)
    {
        
        Name = name;
        //Longitude = longitude;
        //Latitude = latitude;
        CoordinatesList = coordinatesList;
    }
}
