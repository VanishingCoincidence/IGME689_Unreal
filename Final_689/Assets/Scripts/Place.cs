using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using UnityEngine;

public class Place : MonoBehaviour
{
    public PlaceData placeData;
    public ArcGISLocationComponent arcgisLocation;
    
    private Renderer renderer;
    private Color32 color0Default = new Color32(59, 59, 59, 255);
    private Color32 color1Default = new Color32(255, 220, 209, 255);
    private Color32 color2Default = new Color32(255, 168, 181, 255);
    private Color32 color3Default = new Color32(233, 108, 162, 255);
    private Color32 color4Default = new Color32(220, 89, 135, 255);
    private Color32 color5Default = new Color32(179, 33, 52, 255);
    
    private Color32 color0Hover = new Color32(59, 59, 59, 100);
    private Color32 color1Hover = new Color32(255, 220, 209, 100);
    private Color32 color2Hover = new Color32(255, 168, 181, 100);
    private Color32 color3Hover = new Color32(233, 108, 162, 100);
    private Color32 color4Hover = new Color32(220, 89, 135, 100);
    private Color32 color5Hover = new Color32(179, 33, 52, 100);

    void Awake()
    {
        arcgisLocation = GetComponent<ArcGISLocationComponent>();
        renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        arcgisLocation.Position = new ArcGISPoint(placeData.Longitude, placeData.Latitude, 0, ArcGISSpatialReference.WGS84());
        renderer.material.color = FindDefaultColor();
    }

    private void OnMouseEnter()
    {
        renderer.material.color = FindHoverColor();
    }

    private void OnMouseExit()
    {
        renderer.material.color = FindDefaultColor();
    }

    public void UpdateColor()
    {
        renderer.material.color = FindDefaultColor();
    }

    private Color32 FindDefaultColor()
    {
        if (placeData.CurrentCases == 0)
        {
            return color0Default;
        }
        else if (placeData.CurrentCases == 1)
        {
            return color1Default;
        }
        else if (placeData.CurrentCases == 2)
        {
            return color2Default;
        }
        else if (placeData.CurrentCases == 3)
        {
            return color3Default;
        }
        else if (placeData.CurrentCases == 4)
        {
            return color4Default;
        }
        else
        {
            return color5Default;
        }
    }
    
    private Color32 FindHoverColor()
    {
        if (placeData.CurrentCases == 0)
        {
            return color0Hover;
        }
        else if (placeData.CurrentCases == 1)
        {
            return color1Hover;
        }
        else if (placeData.CurrentCases == 2)
        {
            return color2Hover;
        }
        else if (placeData.CurrentCases == 3)
        {
            return color3Hover;
        }
        else if (placeData.CurrentCases == 4)
        {
            return color4Hover;
        }
        else
        {
            return color5Hover;
        }
    }
}
