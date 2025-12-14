using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.Rendering;

public class PlaceManager : MonoBehaviour
{
    public ArcGISMapComponent arcgisMap;
    public GameObject placePrefab;
    public GameObject riverPrefab;
    public GameObject roadPrefab;
    
    private DataLoader dataLoader;
    public List<Place> places;
    public List<River> rivers;
    public Transform spawnPosition;
    
    //public List<Person> people = new List<Person>();
    //public GameObject personPrefab;

    void Awake()
    {
        arcgisMap = FindFirstObjectByType<ArcGISMapComponent>();
        places = new List<Place>();
        rivers = new List<River>();
        dataLoader = new DataLoader();
        StartCoroutine(dataLoader.GetCityFeatures());
        //StartCoroutine(dataLoader.GetRiverFeatures());
        StartCoroutine(dataLoader.GetRoadFeatures());
    }

    void Start()
    {
        // this would sometimes not load, so this forced it to wait until (hopefully) GetFeatures is done
        StartCoroutine(DelayedAction());
    }
    
    public IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(5f); 

        SpawnPlaces();
        SpawnRoads();
        //SpawnRivers();
        //SpawnConnections();
    }

    private void SpawnPlaces()
    {
        foreach (var county in dataLoader.placeDataList)
        {
            Place place = Instantiate(placePrefab, spawnPosition).GetComponent<Place>();
            place.placeData = county;
            places.Add(place);
        }
    }
    
    public void SpawnPerson(string county)
    {
        
    }
    
    private void SpawnRivers()
    {
        foreach (var river in dataLoader.riverList)
        {
            rivers.Add(river);
            
            foreach (var riverList in river.Coordinates)
            {
                LineRenderer connectionPath = Instantiate(roadPrefab, spawnPosition).GetComponent<LineRenderer>();
                connectionPath.positionCount = river.Coordinates.Count;
                connectionPath.material.SetColor("_Color", Color.blue);
                
                int count = 0;

                foreach(var coord in riverList)
                {
                    var arcPosition = new ArcGISPoint(coord[0], coord[1], 300, ArcGISSpatialReference.WGS84());
                    var position = arcgisMap.GeographicToEngine(arcPosition);
                
                    connectionPath.SetPosition(count, position);

                    count++;
                }
            }
            
        }
    }
    
    private void SpawnRoads()
    {
        foreach (var road in dataLoader.roadList)
        {
            Debug.Log(road.Name);
            
            LineRenderer connectionPath = Instantiate(roadPrefab, spawnPosition).GetComponent<LineRenderer>();
            connectionPath.positionCount = road.Coordinates.Count;
            connectionPath.material.SetColor("_Color", Color.black);
            
            int count = 0;
            
            foreach (var coordinate in road.Coordinates)
            {
                var arcPosition = new ArcGISPoint(coordinate[0], coordinate[1], 300, ArcGISSpatialReference.WGS84());
                var position = arcgisMap.GeographicToEngine(arcPosition);
                
                connectionPath.SetPosition(count, position);
            
                count++;
            }
            
            //for (int i = 0; i < road.Coordinates.Count - 1; i++)
            //{
            //    LineRenderer connectionPath = Instantiate(roadPrefab, spawnPosition).GetComponent<LineRenderer>();
            //    connectionPath.material.SetColor("_Color", Color.black);
            //           
            //    var arcPosition = new ArcGISPoint(road.Coordinates[i][0], road.Coordinates[i][1], 300, ArcGISSpatialReference.WGS84());
            //    var position = arcgisMap.GeographicToEngine(arcPosition);
            //    var arcPosition2 = new ArcGISPoint(road.Coordinates[i + 1][0], road.Coordinates[i + 1][1], 300, ArcGISSpatialReference.WGS84());
            //    var position2 = arcgisMap.GeographicToEngine(arcPosition2);
            //    
            //           
            //    // draw a line between the two counties
            //    connectionPath.positionCount = 2;
            //    connectionPath.SetPosition(0, position);
            //    connectionPath.SetPosition(1, position2);
            //    //Debug.Log(position + " " + position2);
            //}
            
            
            
        }
    }
    
    private void SpawnConnections()
    {
        //// go through each county
        //foreach (var county in dataLoader.placeDataList)
        //{
        //    // go through each county that the county being looked at is connected to
        //    foreach (var connection in county.Connected_Places)
        //    {
        //        LineRenderer connectionPath = Instantiate(linePrefab, spawnPosition).GetComponent<LineRenderer>();
        //        
        //        var arcPosition = new ArcGISPoint(county.Longitude, county.Latitude, 90, ArcGISSpatialReference.WGS84());
        //        var position = arcgisMap.GeographicToEngine(arcPosition);
        //        var arcPosition2 = new ArcGISPoint(connection.Longitude, connection.Latitude, 90, ArcGISSpatialReference.WGS84());
        //        var position2 = arcgisMap.GeographicToEngine(arcPosition2);
        //        
        //        // draw a line between the two counties
        //        connectionPath.positionCount = 2;
        //        connectionPath.SetPosition(0, position);
        //        connectionPath.SetPosition(1, position2);
        //        //Debug.Log(position + " " + position2);
        //    }
        //}
    }
    
}
