using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Esri.GameEngine.Geometry;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Camera camera;
    public PlaceManager placeManager;
    private Place placeToGo;
    
    [Header("PlayerData")]
    private int currentPlayerPoints = 6;
    private int totalPlayerPoints = 6;
    public string cityString = null;
    public int Wood = 0;
    public int Wheat = 0;
    public int Stone= 0;
    public int Iron = 0;
    public int Clay = 0;
    private List<Boat> boats = new List<Boat>();
    private List<Wagon> wagons = new List<Wagon>();
    private int availableBoats = 0;
    private int availableWagons = 0;

    [Header("Modifiers")] 
    private int travelTimeMod = 0;
    private int moneyPerTurn = 10;
    private int moneyMod = 1;
    private int transportCost = 10;
    private int transportCostMod = 1;
    private int evilMoneyMod = 0;

    [Header("InstructionsCanvas")]
    public Canvas tutorialCanvas;
    public Button startButton;
    
    [Header("BeginningCanvas")]
    public Canvas citySelectCanvas;
    public Button proceedButton;
    public TMP_Dropdown selectCityDropdown;

    [Header("PlayerInfoCanvas")]
    public Canvas playerInfoCanvas;
    public TMP_Text playerNameText;
    public TMP_Text moneyText;
    public TMP_Text wheatText;
    public TMP_Text woodText;
    public TMP_Text stoneText;
    public TMP_Text ironText;
    public TMP_Text clayText;
    public TMP_Text boatText;
    public TMP_Text wagonsText;
    public TMP_Text boatAvailableText;
    public TMP_Text wagonsAvailableText;
    
    [Header("OtherCityCanvas")]
    public Canvas cityInfoCanvas;
    public TMP_Text cityNameText;
    public TMP_Text cityMoneyText;
    public TMP_Text resourceText;
    public TMP_Text travelTimeText;
    public Button sendBoatBtn;
    public Button sendWagonBtn;
    
    public TMP_Text pointsInfo;
    public Button endTurnButton;
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
        
        // tutorial info
        tutorialCanvas.enabled = true;
        startButton.onClick.AddListener(ToSelect);
        
        // city select
        citySelectCanvas.enabled = false;
        selectCityDropdown.onValueChanged.AddListener(SelectCity);
        proceedButton.interactable = false;
        proceedButton.onClick.AddListener(Proceed);
        
        //player info
        playerInfoCanvas.enabled = false;
        
        // other city info
        cityInfoCanvas.enabled = false;
        
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
                    cityInfoCanvas.enabled = true;
                    FixCityInfo(hit.collider.gameObject.GetComponent<Place>());
                }
                else
                {
                    cityInfoCanvas.enabled = false;
                }
            }

        }
    }


    void FixCityInfo(Place place)
    {
        Place playerPlace = null;
        bool isPlayer = false;
        
        foreach (Place p in placeManager.places)
        {
            if (p.placeData.IsPlayer)
            {
                playerPlace = p;
                
                if (playerPlace.placeData.Name.Equals(place.placeData.Name))
                {
                    Debug.Log(p.placeData.Name + " + " +place.placeData.Name);
                    isPlayer = true;
                }
            }
            
        }

        if (isPlayer)
        {
            cityNameText.text = place.placeData.Name;
            cityMoneyText.text = "This is your city";
            resourceText.enabled = false;
            travelTimeText.enabled = false;
            sendWagonBtn.enabled = false;
            sendBoatBtn.enabled = false;
        }
        else
        {
            resourceText.enabled = true;
            travelTimeText.enabled = true;
            sendWagonBtn.enabled = true;
            sendBoatBtn.enabled = true;
            
            cityNameText.text = place.placeData.Name;
            cityMoneyText.text = "Money: " + place.placeData.Money;
            resourceText.text = "Resource: " + place.placeData.HomeResource;
            

            if (playerPlace.placeData.IsConnectedWater(place.placeData))
            {
                sendWagonBtn.interactable = false;
                
                // calculate distance
                double x = place.placeData.Latitude - playerPlace.placeData.Latitude;
                x = Math.Pow(x, 2);
        
                double y = place.placeData.Longitude - playerPlace.placeData.Longitude;
                y = Math.Pow(y, 2);

                double distance = Math.Sqrt(x + y);
                distance = Math.Abs(distance);
                
                int turnsToTravel = (int)(distance / 7) - travelTimeMod;
                
                if (turnsToTravel < 1)
                {
                    turnsToTravel = 1;
                }
                
                travelTimeText.text = "Travel Time: " + turnsToTravel;

                if (BoatAvailable())
                {
                    sendBoatBtn.interactable = true;
                }
                else
                {
                    sendBoatBtn.interactable = false;
                }
            }
            else
            {
                sendBoatBtn.interactable = false;
                
                // calculate distance
                double x = place.placeData.Latitude - playerPlace.placeData.Latitude;
                x = Math.Pow(x, 2);
        
                double y = place.placeData.Longitude - playerPlace.placeData.Longitude;
                y = Math.Pow(y, 2);

                double distance = Math.Sqrt(x + y);
                distance = Math.Abs(distance);

                // wagon debuff
                int turnsToTravel = (int)(distance / 7) - travelTimeMod + 2;
                
                if (turnsToTravel < 1)
                {
                    turnsToTravel = 1;
                }
        
                travelTimeText.text = "Travel Time: " + turnsToTravel;
            
                if (WagonAvailable())
                {
                    sendWagonBtn.interactable = true;
                }
                else
                {
                    sendWagonBtn.interactable = false;
                }
            }
        }
        
        
        
    }

    bool BoatAvailable()
    {
        if (boats.Count > 0)
        {
            foreach (Boat boat in boats)
            {
                if (boat.Destination == null)
                {
                    return true;
                }
            }
        }
        
        return false;
    }
    
    bool WagonAvailable()
    {
        if (wagons.Count > 0)
        {
            foreach (Wagon wagon in wagons)
            {
                if (wagon.Destination == null)
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    void UpdatePlayerInfo()
    {
        foreach (Place place in placeManager.places)
        {
            place.UpdateColor();
            
            if (place.placeData.Name.Equals(cityString))
            {
                playerNameText.text = cityString;
                moneyText.text = "Money: " + place.placeData.Money;
                wheatText.text = "Wheat: " + Wheat;
                woodText.text = "Wood: " + Wood;
                stoneText.text = "Stone: " + Stone;
                ironText.text = "Iron: " + Iron;
                clayText.text = "Clay: " + Clay;
                boatText.text = "Total Boats: " + boats.Count;
                boatAvailableText.text = "Available Boats: " + availableBoats;
                wagonsText.text = "Total Wagons: " + wagons.Count;
                wagonsAvailableText.text = "Available Wagons: " + availableWagons;
                
                
                break;
            }
        }
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
        UpdatePlayerInfo();
        playerInfoCanvas.enabled = true;
    }
    
    void SelectCity(int index)
    {
        cityString = " " + selectCityDropdown.options[index].text;

        foreach (Place place in placeManager.places)
        {
            if (place.placeData.Name.Equals(cityString))
            {
                place.placeData.IsPlayer = true;

                if (place.placeData.HomeResource.Equals("wheat"))
                {
                    Wheat += 10;
                }
                else if (place.placeData.HomeResource.Equals("wood"))
                {
                    Wood += 10;
                }
                else if (place.placeData.HomeResource.Equals("stone"))
                {
                    Stone += 10;
                }
                else if (place.placeData.HomeResource.Equals("iron"))
                {
                    Iron += 10;
                }
                else if (place.placeData.HomeResource.Equals("clay"))
                {
                    Clay += 10;
                }
                
                break;
            }
        }

        if (cityString.Equals("Select a City"))
        {
            proceedButton.interactable = false;
        }
        else
        {
            proceedButton.interactable = true;
        }
        
    }
    
    
}
