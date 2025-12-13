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
    public List<River> riverList;

    public string urlRiver =
        "https://services.arcgis.com/ue9rwulIoeLEI9bj/arcgis/rest/services/US_Major_Rivers/FeatureServer/0/query?f=geojson&where=1=1&outfields=*";
    
    public string urlRoad =
        "https://services7.arcgis.com/jF2q3LPxL7PETdYk/arcgis/rest/services/US_Primary_Roads/FeatureServer/0/query?f=geojson&where=1=1&outfields=*";
    
    public string urlCity =
        "https://services2.arcgis.com/RQcpPaCpMAXzUI5g/arcgis/rest/services/USA_Major_Cities/FeatureServer/0/query?f=geojson&where=1=1&outfields=*";
    
    public DataLoader()
    {
        placeDataList = new List<PlaceData>();
    }
    
    public IEnumerator GetCityFeatures()
    {
        // To learn more about the Feature Layer rest API and all the things that are possible checkout
        // https://developers.arcgis.com/rest/services-reference/enterprise/query-feature-service-layer-.htm

        UnityWebRequest RequestCity = UnityWebRequest.Get(urlCity);
        yield return RequestCity.SendWebRequest();
        
        if (RequestCity.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(RequestCity.error);
        }
        else
        {
            LoadCityData(RequestCity.downloadHandler.text);
        }
    }
    
    public IEnumerator GetRiverFeatures()
    {
        // To learn more about the Feature Layer rest API and all the things that are possible checkout
        // https://developers.arcgis.com/rest/services-reference/enterprise/query-feature-service-layer-.htm

        UnityWebRequest RequestRiver = UnityWebRequest.Get(urlRiver);
        yield return RequestRiver.SendWebRequest();
        
        if (RequestRiver.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(RequestRiver.error);
        }
        else
        {
            LoadRiverData(RequestRiver.downloadHandler.text);
        }
    }
    
    public IEnumerator GetRoadFeatures()
    {
        // To learn more about the Feature Layer rest API and all the things that are possible checkout
        // https://developers.arcgis.com/rest/services-reference/enterprise/query-feature-service-layer-.htm

        UnityWebRequest RequestRoad = UnityWebRequest.Get(urlRoad);
        yield return RequestRoad.SendWebRequest();
        
        if (RequestRoad.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(RequestRoad.error);
        }
        else
        {
            LoadRoadData(RequestRoad.downloadHandler.text); 
        }
    }

    public void LoadCityData(string response)
    {
        //UnityWebRequest request = UnityWebRequest.Get(url);

        JObject jObject = JObject.Parse(response);
        JToken[] jFeatures = jObject.SelectToken("features").ToArray();

        foreach (var feature in jFeatures)
        {
            // Get coordinates in the Feature Service
            var coordinates = feature.SelectToken("geometry").SelectToken("coordinates").ToArray();
            var properties = feature.SelectToken("properties").ToArray();

            string name = null;
            int population = 0;
            double popPerMile = 0f;

            foreach (var value in properties)
            {
                var key = value.ToString();
                //Debug.Log(key);
                var props = key.Split(':');

                if (props[0] == "\"Name\"")
                {
                    // remove quotations
                    name = new string ((from c in props[1] where char.IsWhiteSpace(c) || char.IsLetter(c) select c).ToArray());
                }
                if (props[0] == "\"F2020_Total_Population\"")
                {
                    population = int.Parse(props[1]); 
                }
                if (props[0] == "\"People_per_square_mile\"")
                {
                    popPerMile = float.Parse(props[1]);  
                }

            }

            //coordinate.ToArray();
            //Debug.Log("coordinate: " + coordinates[1] + " " + coordinates[0]);
            double x = Convert.ToDouble(coordinates[1]);
            double y = Convert.ToDouble(coordinates[0]);

            if (name != null)
            {
                placeDataList.Add(new PlaceData(name,y, x, population, popPerMile));
            }
            
        }

        //foreach (var place1 in placeDataList)
        //{
        //    foreach (var place2 in placeDataList)
        //    {
        //        // don't connect with itself nor connect with something it's already connected to
        //        if (place1.County != place2.County && !place1.Connected_Places.Contains(place2))
        //        {
        //            place1.ConnectPlaces(place2);
        //        }
        //    }
        //}
        
    }
    
     public void LoadRiverData(string response)
    {
        //UnityWebRequest request = UnityWebRequest.Get(url);

        JObject jObject = JObject.Parse(response);
        JToken[] jFeatures = jObject.SelectToken("features").ToArray();

        foreach (var feature in jFeatures)
        {
            // Get coordinates in the Feature Service
            var coordinates = feature.SelectToken("geometry").SelectToken("coordinates").ToArray();
            var properties = feature.SelectToken("properties").ToArray();

            string name = null;
            List<double[]> coordinatesList = new List<double[]>();

            foreach (var value in properties)
            {
                var key = value.ToString();
                var props = key.Split(':');

                if (props[0] == "\"PNAME\"")
                {
                    // remove quotations
                    name = new string ((from c in props[1] where char.IsWhiteSpace(c) || char.IsLetter(c) select c).ToArray());
                }
            }
            
            foreach (var coordinate in coordinates)
            {
                double[] coordToAdd = new double[2];
                coordToAdd[0] = Convert.ToDouble(coordinate[0]);
                coordToAdd[1] = Convert.ToDouble(coordinate[1]);
                coordinatesList.Add(coordToAdd);
            }
            

            if (name != null)
            {
                riverList.Add(new River(name, coordinatesList));
            }
            
        }
        
    }
     
      public void LoadRoadData(string response)
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
            //double x = Convert.ToDouble(coordinates[1]);
            //double y = Convert.ToDouble(coordinates[0]);
            //
            //if (state != null)
            //{
            //    placeDataList.Add(new PlaceData(state, county, y,x,confirmed, deaths, incidentRate));
            //}
            
        }
        
    }
    
}
