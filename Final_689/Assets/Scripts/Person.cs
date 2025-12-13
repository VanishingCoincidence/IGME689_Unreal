using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using UnityEngine;

public class Person : MonoBehaviour
{
    public ArcGISLocationComponent arcgisLocation;

    public Place currentCounty;
    public double longitude;
    public double latitude;
    private double offset = 0.5;
    
    private Renderer renderer;
    private Color32 colorDefault = new Color32(0, 71, 171, 255);
    private Color32 colorHover = new Color32(100, 149, 237, 255);

    void Awake()
    {
        arcgisLocation = GetComponent<ArcGISLocationComponent>();
        renderer = GetComponent<Renderer>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        arcgisLocation.Position = new ArcGISPoint(longitude + offset, latitude, 0, ArcGISSpatialReference.WGS84());
        renderer.material.color = colorDefault;
    }

    public void Move(double lon, double lat)
    {
        arcgisLocation.Position = new ArcGISPoint(lon + offset, lat, 0, ArcGISSpatialReference.WGS84());
    }

    private void OnMouseEnter()
    {
        renderer.material.color = colorHover;
    }

    private void OnMouseExit()
    {
        renderer.material.color = colorDefault;
    }
}
