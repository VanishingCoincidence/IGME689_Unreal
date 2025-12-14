using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Esri.GameEngine.Geometry;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("PlayerData")]
    private int currentPlayerPoints = 6;
    private int totalPlayerPoints = 6;
    public int Wood = 0;
    public int Wheat = 0;
    public int Stone= 0;
    public int Iron = 0;
    public int Clay = 0;
    
    private List<Person> people = new List<Person>();
    public Transform spawnPosition;
    public GameObject personPrefab;
    
    public Camera camera;
    public PlaceManager placeManager;

    public Canvas placeCanvas;
    public TMP_Text placeInfo;
    public TMP_Text currentCaseInfo;
    public TMP_Text connectedInfo;
    
    public Canvas personCanvas;
    public TMP_Text personPlaceInfo;
    public TMP_Text personCurrentCaseInfo;
    public TMP_Text personCanTravelInfo;
    public Button cureButton;
    public Button researchButton;
    public Button gainToolsButton;
    public Button recruitButton;

    [Header("InstructionsCanvas")]
    public Canvas tutorialCanvas;
    public Button startButton;
    
    [Header("BeginningCanvas")]
    public Canvas citySelectCanvas;
    public Button proceedButton;
    public TMP_Dropdown selectCityDropdown;
    
    public TMP_Text pointsInfo;
    public TMP_Text totalCaseInfo;
    public Button endTurnButton;
    private int cureMod = 1;
    private Place currentPlace = null;

    public Canvas legendCanvas;
    public Button legendButton;

    public Image winImage;
    public Image loseImage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //placeCanvas.enabled = false;
        //loseImage.enabled = false;
        //winImage.enabled = false;
        //legendCanvas.enabled = false;
        //personCanvas.enabled = false;
        
        // tutorial info
        tutorialCanvas.enabled = true;
        startButton.onClick.AddListener(ToSelect);
        
        // city select
        citySelectCanvas.enabled = false;
        selectCityDropdown.onValueChanged.AddListener(SelectCity);
        proceedButton.interactable = false;
        proceedButton.onClick.AddListener(Proceed);
        
        //StartCoroutine(DelayedAction());
    }
    
    public IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(1.3f); 

        //UpdateTotalCases();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (currentPlayerPoints <= 0)
        {
            //EndTurn();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            
            Ray ray = camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            
            bool isHit = Physics.Raycast(ray, out hit);

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (isHit && hit.collider.gameObject.GetComponent<Place>() != null)
                {
                    //placeCanvas.enabled = true;
                    FixPlaceInfo(hit.collider.gameObject.GetComponent<Place>());
                }
                else if (isHit && hit.collider.gameObject.GetComponent<Person>() != null)
                {
                    //personCanvas.enabled = true;
                }
                else
                {
                    //placeCanvas.enabled = false;
                    //personCanvas.enabled = false;
                }
            }

        }
    }


    void FixPlaceInfo(Place place)
    {
        //foreach (Place p in placeManager.places)
        //{
        //    if (p.placeData.County == place.placeData.County)
        //    {
        //        string placesCanTravel = "Places can travel: ";
        //
        //        foreach (PlaceData placeData in p.placeData.Connected_Places)
        //        {
        //            placesCanTravel += " " + placeData.County + ", " + placeData.State + " |";
        //        }
        //        
        //        connectedInfo.text = placesCanTravel;
        //        placeInfo.text = p.placeData.County + ", " + p.placeData.State;
        //        currentCaseInfo.text = "Cases: " + p.placeData.CurrentCases;
        //        break;
        //    }
        //}
    }

    void UpdatePoints()
    {
        pointsInfo.text = "Points: " + currentPlayerPoints;
    }

    void ToSelect()
    {
        tutorialCanvas.enabled = false;
        citySelectCanvas.enabled = true;
    }
    void Proceed()
    {
        citySelectCanvas.enabled = false;
    }
    
    void SelectCity(int index)
    {
        string selectedCity = " " + selectCityDropdown.options[index].text;

        foreach (Place place in placeManager.places)
        {
            if (place.placeData.Name.Equals(selectedCity))
            {
                Debug.Log(selectedCity);
                place.placeData.IsPLayer = true;
                break;
            }
        }

        if (selectedCity.Equals("Select a City"))
        {
            proceedButton.interactable = false;
        }
        else
        {
            proceedButton.interactable = true;
        }
        
    }
    
    
}
