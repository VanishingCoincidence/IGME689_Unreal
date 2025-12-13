using UnityEngine;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class River
{
    public string Name;
    public List<double[]> Coordinates;
    
    public River(string name, List<double[]> coordinates)
    {
        
        Name = name;
        //Longitude = longitude;
        //Latitude = latitude;
        Coordinates = coordinates;
    }
}
