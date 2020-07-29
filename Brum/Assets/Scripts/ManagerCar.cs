using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace UnityStandardAssets.Vehicles.Car
{
    public class ManagerCar : MonoBehaviour
    {
        #region variables
        [Header("UI Elements")]
        public Text Cash;                                                       //UI text that show money value
        public Slider FuelSlider;                                               //slidet that is imitating fuel bar
        public GameObject ClientPanel;                                          //UI panel that appears when we stop on interbox without passenger
        public Text Payout;                                                     //here player see how much he can get for ride
        public Text Distance;                                                   //here player see how far he have to go
        [Header("CarParametrs")]
        public int rnd;                                                         //random nr. for destination boxes and arrow
        public float MaxSpeedStatic = 25;                                       //max speed of car that cant be change 
        public float MaxSpeed;                                                  //max speed of car
        public static float Fuel = 5;                                           //current fuel
        public float MaxFuel = 5;                                               //max fuel that car can have
        public float speed;                                                     //curent speed of car
        [Header("Variables")]
        public float Money = 100;                                               //money 
        int PayOut;                                                             //How much player ll get for ride
        float distnace;                                                         //distance between points
        public static int FuelLvl;                                              //FuelLvl upgrade value
        int EngineLvl;                                                          //EngineLvl upgarde value
        int PrestigeLvl;                                                        //PrestigeLvl upgrade value
        float time;                                                             //just time ;p
        int LookAt;                                                            //0 = LookAtCurentTarget 1 = LookAtGarage 2 = LookAtGasStation 
        [Header("CarComponents")]
        public GameObject Camera;                                               //MainCamera
        public GameObject Arrow;                                                //Arrow that show where is destination point :3
        [Header("ObjectsOnMap")]
        public GameObject[] InterPoint;                                         //Boxes that player can stop on to pickup passenger
        public GameObject GasStation;                                           //GasStation object
        public GameObject Garage;                                               //And Garage object
        [Header("UIADS")]
        public GameObject FuelAdsPanel;                                         //Panel that popsup when fuel end
        [Header("ADSBools")]
        bool FuelAdd;                                                           //If false FuelAddAds Can be Open                                 
        public static bool DoubledPayOut;                                       //If true player getting double PayOut for Ride(Set to true By AdsManager after watching ad)
        bool passengeronboard;
        #endregion 

        private void Awake()
        {
           
            FuelLvl = PlayerPrefs.GetInt("FuelLvl");                            //Load Fuel Lvl
            EngineLvl = PlayerPrefs.GetInt("EngineLvl");                        //Load Engine Lvl
            PrestigeLvl = PlayerPrefs.GetInt("PrestigeLvl");                    //Load Prestige Lvl
            Money = PlayerPrefs.GetFloat("Money");                              //Load Money Balance
            MaxSpeedStatic =22 +( 5 * EngineLvl);                               //SetMaxSpeedStatic
            MaxFuel += 1.25f * FuelLvl;                                         //Boost max fuel that car cam have
            MaxSpeed = MaxSpeedStatic;                                          //Set MaxSpeed to MaxSpeedStatic;

            InterPoint = GameObject.FindGameObjectsWithTag("InterPoint");       //find all gameobject where player can take passager

            for (int i = 0; i < InterPoint.Length; i++)
            {
                InterPoint[i].SetActive(false);                                 //disable all points
            }

            Decline();                                                          //Set first destination point
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Fuel = 0.2f;
            }
            FuelSlider.maxValue = MaxFuel;                                      //UI fuel element
            FuelSlider.value = Fuel;                                            //UI fuel element
            Cash.text = "" + Money.ToString("F2") + "$";                        //UI Money element
            speed = GetComponent<Rigidbody>().velocity.magnitude * 2.24f;       //calculating curent speed of car
            CarUserControl.Fuel = Fuel;                                         //Set fuel value in CarUserControl
            CarUserControl.MaxSpeed = MaxSpeed;                                 //Set MaxSpeed = MaxSpeed of car in CarUserControl script
            if (CarUserControl.moving == true)                                  //check if car is moving
            {
                Fuel -= 0.05f * Time.deltaTime;                                 //removing fuel 0.05 per second
            }

            if (Fuel < 0)
            {
                Fuel = 0;                                                       //if fuel is lower than 0 fuell is 0
                if (FuelAdd == false)                                           //chec if Add is open 
                {
                    FuelAds();                                                  //Invoke FuelAds
                }

            }
            else if (Fuel > 0)                                                  //if fuel is > 0
            {
                FuelAdd = false;                                                //mark FuelAdd as false so it can be open when fuel gets 0 again
            }
            if (Money <= 0)                                                     //same as fuel
            {
                Money = 0;
            }

            #region LookAt
            if (LookAt == 0)
            {
                Arrow.transform.LookAt(InterPoint[rnd].transform);              //Look at current destination point
            }
            else if (LookAt == 1)
            {
                Arrow.transform.LookAt(Garage.transform);                       //Look at Garage
            }
            else if (LookAt == 2)
            {
                Arrow.transform.LookAt(GasStation.transform);                   //Look at GasStation
            }

            #endregion
            #region AutoSave
            time += Time.deltaTime;                                             //time = curent time
            if (time > 30)                                                      //if time > 30seconds
            {
                PlayerPrefs.SetFloat("Money", Money);                           //save money value
                time = 0;                                                       //and reset time value to 0
            }
            #endregion

        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Gas" && Fuel < MaxFuel && Money > 0)   //if car stops on gameobject with tag fuel and have more money than 0 and fuel lower than max
            {
                Fuel += 0.25f * Time.deltaTime;                                 //taning fuel 
                Money -= 2 * Time.deltaTime;                                    //and paying for it
            }

            if (other.gameObject.tag == "InterPoint" && passengeronboard == false && speed < 10 )
            {
                OpenPassengerPanel();
                other.gameObject.SetActive(false);
            }
            else if (other.gameObject.tag == "InterPoint" && passengeronboard == true && speed < 10)
            {
                other.gameObject.SetActive(false);                              //dezactivate object that player colliding
                if (DoubledPayOut == true)                                      //check if DoublePayOut is true, give player double amount of money, simple right?
                {
                    PayOut = PayOut * 2;
                }
                Money += PayOut;                                                //add balance 
                Decline();                                                      //invoke function

            }

            if (other.gameObject.tag == "Road")
            {
                MaxSpeed = MaxSpeedStatic;                                      //when car is on road his max speed is set to... maxspeed :p
                CarController.m_SteerHelper = 0.75f;                            //And SteerHelper is set to 0.75
                CarController.m_TractionControl = 0.75f;                        //And TractionControl to 0.75 for better driving :3
            }
            else if (other.gameObject.tag == "Ground")
            {
                MaxSpeed = (MaxSpeedStatic * 0.65f);                            //MaxSpeed is lower around 35%
                CarController.m_SteerHelper = 0.40f;                            //And SettterHelper is lower to 0.40f
                CarController.m_TractionControl = 0.40f;                        //And TractionControl is lower to 0.40 so car can sliding on grass 
            }

            if (other.gameObject.tag == "Garage")                               //If car enter to garage zone
            {
                PlayerPrefs.SetFloat("Money", Money);                           //Save Money amount 
                SceneManager.LoadScene(2);                                      //load garage scene
            } 

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Fonar")                            //if car hit object with tag "fonar"
            {
                GameObject Foner = collision.gameObject;                        //Get collision object
                Foner.AddComponent<Rigidbody>();                                //add rigidbody so it can be moved
                Foner.tag = "Untagged";                                         //change tag so script cant be invoke again for this same object
                Destroy(Foner, 10);                                             //destroy object after 10s
                Money -= 200;                                                   //take money from player balance for destroing objects
            }
        }

        void OpenPassengerPanel()
        {
            int BonusCash = 100 * PrestigeLvl;
            ClientPanel.SetActive(true);                                        //Open client panel
            PayOut = Random.Range(50 + BonusCash, 250 + BonusCash );            //Set Payout between 50 and 250 and multipile it by bonuscash;
            rnd = Random.Range(0, InterPoint.Length);                           //get random numer 
            distnace = Vector3.Distance(InterPoint[rnd].transform.position, transform.position) /100;  //calculate distance between car and destination point

            Payout.text = "$ " + PayOut;                                        //Show value on UI
            Distance.text = "Distance " + distnace.ToString("F1") + " Km";      //Show value on UI
        }
        public void Accept()                                                    //if player accepted offert
        {

            passengeronboard = true;                                            //set bool as true
            InterPoint[rnd].SetActive(true);                                    //active destination object
        }

        public void Decline()                                                   //if player not accept offert or arrived to point with passenger
        {
            passengeronboard = false;                                           //set bool to false
            DoubledPayOut = false;                                              //Set doublepayout to false
            rnd = Random.Range(0, InterPoint.Length);                           //get new random number
            InterPoint[rnd].SetActive(true);                                    //and set new destination
        }

        void FuelAds()
        {
            FuelAdsPanel.SetActive(true);                                       //Open FuelAdsPanel
            FuelAdd = true;                                                     //Mark FuelAdd as true so it can be closed by X button
        }
        #region LookAt
        public void LookAt0()                                                   //Voids invoke by buttons
        {
            LookAt = 0;                                                         //And changing variable value
        }
        public void LookAt1()
        {
            LookAt = 1;
        }
        public void LookAt2()
        {
            LookAt = 2;
        }
        #endregion
    }
}
