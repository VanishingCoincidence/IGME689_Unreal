using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEditor.Rendering.HighDefinition;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.Splines;


public class DataLoader
{
    public List<PlaceData> placeDataList;

    public string url =
        "https://services2.arcgis.com/RQcpPaCpMAXzUI5g/arcgis/rest/services/COVID_19_Cases_Major/FeatureServer/0/query?f=geojson&where=1=1&outfields=*";
    
    public DataLoader()
    {
        placeDataList = new List<PlaceData>();
    }
    
    public IEnumerator GetFeatures()
    {
        // To learn more about the Feature Layer rest API and all the things that are possible checkout
        // https://developers.arcgis.com/rest/services-reference/enterprise/query-feature-service-layer-.htm

        UnityWebRequest Request = UnityWebRequest.Get(url);
        yield return Request.SendWebRequest();
        
        if (Request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(Request.error);
        }
        else
        {
            LoadData(Request.downloadHandler.text);
        }
    }

    public void LoadData(string response)
    {
        //UnityWebRequest request = UnityWebRequest.Get(url);

        JObject jObject = JObject.Parse(response);
        JToken[] jFeatures = jObject.SelectToken("features").ToArray();

        foreach (var feature in jFeatures)
        {
            // Get coordinates in the Feature Service
            var coordinates = feature.SelectToken("geometry").SelectToken("coordinates").ToArray();
            var properties = feature.SelectToken("properties").ToArray();

            string state = null;
            string county = null;
            float confirmed = 0f;
            float deaths = 0f;
            float incidentRate = 0f;

            foreach (var value in properties)
            {
                var key = value.ToString();
                //Debug.Log(key);
                var props = key.Split(':');

                if (props[0] == "\"Province_State\"")
                {
                    // remove quotations
                    state = new string ((from c in props[1] where char.IsWhiteSpace(c) || char.IsLetter(c) select c).ToArray());
                }
                if (props[0] == "\"Admin2\"")
                {
                    // remove quotations
                    county = new string ((from c in props[1] where char.IsWhiteSpace(c) || char.IsLetter(c) select c).ToArray());
                }
                if (props[0] == "\"Confirmed\"")
                {
                    confirmed = float.Parse(props[1]);  
                }
                if (props[0] == "\"Deaths\"")
                {
                    deaths = float.Parse(props[1]);  
                }
                if (props[0] == "\"Incident_Rate\"")
                {
                    incidentRate = float.Parse(props[1]);  
                }

            }

            //coordinate.ToArray();
            //Debug.Log("coordinate: " + coordinates[1] + " " + coordinates[0]);
            double x = Convert.ToDouble(coordinates[1]);
            double y = Convert.ToDouble(coordinates[0]);

            if (state != null)
            {
                placeDataList.Add(new PlaceData(state, county, y,x,confirmed, deaths, incidentRate));
            }
            
        }

        foreach (var place1 in placeDataList)
        {
            foreach (var place2 in placeDataList)
            {
                // don't connect with itself nor connect with something it's already connected to
                if (place1.County != place2.County && !place1.Connected_Places.Contains(place2))
                {
                    place1.ConnectPlaces(place2);
                }
            }
        }
        
    }
    
}
