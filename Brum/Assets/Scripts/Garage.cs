using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Garage : MonoBehaviour
{
    [Header("UIElements")]
    public GameObject UnlockPanel;                                              //Panel that popsup when player chooce color to unlock
    public Text Balance;                                                        //Text where player can see his amount of money
    public GameObject[] ColorsBlocked;                                          //Buttons of blocked colors with "?" icon
    public GameObject[] ColorsBlockedBackGround;                                //Background of "?" icon
    public Slider FuelSlider;                                                   //>>
    public Slider EngineSlider;                                                 //>>>Sliders that show lvl of upgrade
    public Slider PrestigeSlider;                                               //>>
    public Text FuelCost;                                                       //>>
    public Text EngineCost;                                                     //>>>Text that show actual cost of upgrade
    public Text PrestigeCost;                                                   //>>
    int[] UnlockedColors;                                                       //Array for UnlockedColors
    int FuelLvl;                                                                //variable for get lvl of fuel 
    int EngineLvl;                                                              //variable for get lvl of engine 
    int PrestigeLvl;                                                            //variable for get lvl of prestige
    int FCost;                                                                  //>
    int ECost;                                                                  //>> Variables for calculating cost;
    int PCost;                                                                  //>
    [Header(("CarComponents"))]
    public GameObject Car;                                                      //GameObject with materials to change
    [Header("ForColors")]
    Renderer Rend;                                                              //Renderer component
    Material[] Mat;                                                             //Array of matterials
    float Red;                                                                  //Red value in RGB
    float Green;                                                                //Green value in RGB
    float Blue;                                                                 //Blue value in RGB
    int ColorNr;                                                                //Numer of color that player choose to unlock
    [Header("ForAds")]
    public static bool FreeUnlock;                                              //Check if FreeUnlock is true, if is true player can unlock color for free :D
    float Money;                                                                //Amount of money that player have
    private void Awake()
    {
        
        int[] UnlockedColors = new int[15];                                     //Create array
        for (int i = 0; i < ColorsBlocked.Length; i++)                          //loop that length = amount of colors to unlock
        {
            UnlockedColors[i] = PlayerPrefs.GetInt("UnlockedColors" + i);       //Load Unlocked colors
            if (UnlockedColors[i] == i)                                         //Check if there are unlocked colors
            {
                ColorsBlocked[i].SetActive(false);                              //Disable Blocked icon
                ColorsBlockedBackGround[i].SetActive(false);                    //and blocked background for those colors who are already unlocked
            }
        }
        Money = PlayerPrefs.GetFloat("Money");                                  //Load money amount
        FuelLvl = PlayerPrefs.GetInt("FuelLvl");                                //Load Fuel Lvl
        EngineLvl = PlayerPrefs.GetInt("EngineLvl");                            //Load Engine Lvl
        PrestigeLvl = PlayerPrefs.GetInt("PrestigeLvl");                        //Load Prestige Lvl
        Rend = Car.GetComponent<Renderer>();                                    //Get renderer component from Car GameObject
        Mat = Rend.materials;                                                   //Shortcut for Rend.materials


    }

    #region ChangeColor
    public void GetRed(int R)                                                   //Get R vaule from button
    {
        Red = R;
    }
    public void GetGreen(int G)                                                 //Get G value from button
    {
        Green = G;
    }
    public void GetBlue(int B)                                                  //Get B value from button
    {
        Blue = B;
    }
    public void SetBodyColor()                                                  //Change color of Car
    { 
        Mat[0].color = new Color(Red / 255, Green / 255, Blue / 255);           //Get material from array and set color in RGB
    }
    #endregion
    
    private void Update()
    {
        Balance.text = "$ " + Money;                                            //Showing actuall balance
        #region Calculating cost
        if (FuelLvl == 0)                                                       //If Fuel Lvl is 0
        {
            FCost = 250;                                                        //Cost is 250$
        }
        else
        {
            FCost = 250 * FuelLvl;                                              //Else cost is 250 * FuelLvl (example: 250 * (FuelLvl = 2) == 500$)
        }
        if (EngineLvl == 0)                                                     //And same for rest upgradable components :3
        {
            ECost = 250;                                                        //
        }
        else
        {
            ECost = 250 * EngineLvl;                                            //
        }
        if (PrestigeLvl == 0)
        {
            PCost = 250;                                                        //
        }
        else
        {
            PCost = 250 * PrestigeLvl;                                          //
        }
        #endregion
        FuelSlider.value = FuelLvl;                                             //Set Fuel slider value to FuelLvl so player see what lvl he have 
        EngineSlider.value = EngineLvl;                                         //Same here
        PrestigeSlider.value = PrestigeLvl;                                     //and here :3
        
        if (FuelLvl == 5)                                                       //if FuelLvl = 5
        {                                                                       //
            FuelCost.text = "MaxLvl";                                           //Show "Maxlvl" in text field
        }                                                           
        else                                                                    //else
        {
            FuelCost.text = "$ " + FCost;                                       //Show in text field actuall cost of upgrade
        }
        if (EngineLvl == 5)                                                     //same for rest of upgradable objects :p
        {
            EngineCost.text = "MaxLvl";
        }
        else
        {
            EngineCost.text = "$ " + ECost;
        }
        if (PrestigeLvl == 5)
        {
            PrestigeCost.text = "MaxLvl";
        }
        else
        {
            PrestigeCost.text = "$ " + PCost;
        }
       
    }
    #region UnlockColor
    public void PickColor(int Nr)                                               //Get Nr from button
    {
        ColorNr = Nr;                                                           //Set ColorNr value to Nr that we get from button
        UnlockPanel.SetActive(true);                                            //Open panel to ask player if he want to unlock this color
    }
    public void Unlock()                                                        //Unlocking void activate when player click yes
    {
        Invoke("UnlockFree", 5f);

        if (Money >= 500)                                                       //Check if player have money
        {   
            Money -= 500;                                                       //Get 500$ from balance
            ColorsBlocked[ColorNr].SetActive(false);                            //Disable Blocked icon
            ColorsBlockedBackGround[ColorNr].SetActive(false);                  //and background icon so player can choose color
            UnlockPanel.SetActive(false);                                       //Disable unlockpanel
            PlayerPrefs.SetInt("UnlockedColors" + ColorNr, ColorNr);            //Save UnlockedNr 
        }
    }
    void UnlockFree()
    {

        if (FreeUnlock == true)
        {

            ColorsBlocked[ColorNr].SetActive(false);                            //Disable Blocked icon
            ColorsBlockedBackGround[ColorNr].SetActive(false);                  //and background icon so player can choose color
            UnlockPanel.SetActive(false);                                       //Disable unlockpanel
            PlayerPrefs.SetInt("UnlockedColors" + ColorNr, ColorNr);            //Save UnlockedNr 
            FreeUnlock = false;
        }
    }

    #endregion
    #region Upgrade
    public void UpgradeFuel()                                                   //voids invoked by buttons
    {
        
        if (Money >= FCost && FuelLvl < 5)                                      //If player have enough money and Lvl is lower than 5 he can upgrade
        {
            Money -= FCost;                                                     //Getting money from balance
            FuelLvl += 1;                                                       //and increast lvl
        }
    }
    public void UpgradeEngine()                                                 //Same for rest of upgarde voids :p
    {

        if (Money >= ECost && EngineLvl < 5)
        {
            Money -= ECost;
            EngineLvl += 1;
        }
    }
    public void UpgradePrestige()
    {

        if (Money >= PCost && PrestigeLvl < 5)
        {
            Money -= PCost;
            PrestigeLvl += 1;
        }
    }
    #endregion
    public void Save()                                                          //Invoke by "Done" button
    {
        PlayerPrefs.SetFloat("Red", Red);                                       //Save Red value of RGB
        PlayerPrefs.SetFloat("Green", Green);                                   //Save Green value of RGB
        PlayerPrefs.SetFloat("Blue", Blue);                                     //Save Blue value of RGB
        PlayerPrefs.SetFloat("Money", Money);                                   //Save money balance
        PlayerPrefs.SetInt("FuelLvl", FuelLvl);                                 //Save FuelLvl
        PlayerPrefs.SetInt("EngineLvl", EngineLvl);                             //Save EngineLvl
        PlayerPrefs.SetInt("PrestigeLvl", PrestigeLvl);                         //Save PrestigeLvl
    }
}
