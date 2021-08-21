using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
namespace UnityStandardAssets.Vehicles.Car
{
    public class AdsManager : MonoBehaviour, IUnityAdsListener
    {
        string GooglePlay_ID = "3726353";
        bool TestMode = false;
        string myPlacementId = "rewardedVideo";
        public int WhichAdd; //0 = AddFuel, 1 = DoublePayOut for ride, 2 = FreeColor
        void Start()
        {
            Advertisement.AddListener(this);
            Advertisement.Initialize(GooglePlay_ID, TestMode);
        }

        public void DisplayIntAds()
        {
            Advertisement.Show();
        }

        public void FuelAdd()
        {
            Advertisement.Show(myPlacementId);
            WhichAdd = 0;
        }
        public void DoubleReward()
        {
            Advertisement.Show(myPlacementId);
            WhichAdd = 1;
        }
        public void UnlockForFree()
        {
            WhichAdd = 2;
            Advertisement.Show(myPlacementId);
            
        }

        // Implement IUnityAdsListener interface methods:
        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            // Define conditional logic for each ad completion status:
            if (showResult == ShowResult.Finished)
            {
                if (WhichAdd == 0)
                {
                    ManagerCar.Fuel = 5 + (5 * (ManagerCar.FuelLvl / 2));
                }
                else if (WhichAdd == 1)
                {
                    ManagerCar.DoubledPayOut = true;
                }
                else if (WhichAdd == 2)
                {
                    Garage.FreeUnlock = true;
                }
            }
            else if (showResult == ShowResult.Skipped)
            {
                // Do not reward the user for skipping the ad.
                Debug.Log("skip");
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.LogWarning("The ad did not finish due to an error.");
            }
        }

        public void OnUnityAdsReady(string placementId)
        {
            // If the ready Placement is rewarded, show the ad:
            if (placementId == myPlacementId)
            {
                // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
            }
        }

        public void OnUnityAdsDidError(string message)
        {
            // Log the error.
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            // Optional actions to take when the end-users triggers an ad.
        }

        // When the object that subscribes to ad events is destroyed, remove the listener:
        public void OnDestroy()
        {
            Advertisement.RemoveListener(this);
        }
    }
}

