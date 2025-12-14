using UnityEngine;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Road
{
    public string Name;
    public List<double[]> Coordinates;
    
    public Road(string name, List<double[]> coordinates)
    {
        
        Name = name;
        //Longitude = longitude;
        //Latitude = latitude;
        Coordinates = coordinates;
    }
}
