using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car;                                                                                           // the car controller we want to use
        public static float Fuel;                                                                                              //Fuel parament is getting from ManagerCarScript
        public static bool moving;                                                                                             //Bool that checking if car is moving
        public static float MaxSpeed;                                                                                          //max speed of car (Getting from ManagerCar script)
        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        void FixedUpdate()
        {

            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");

            if (Fuel > 0)                                                                                                       // check if car have fuel, if yes allow player to play
            {
                CarController.m_Topspeed = MaxSpeed;                                                                            //get the max speed from managercar script and set it in CarController
                if (CrossPlatformInputManager.GetAxis("Vertical") == 1 || CrossPlatformInputManager.GetAxis("Vertical") == -1)  //check if car is moving/driving :p
                {
                    moving = true;
                }
                else
                {
                    moving = false;
                }
            }
            else if (Fuel <= 0)
            {
                CarController.m_Topspeed = 7.5f;
            }
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
                m_Car.Move(h, v, v, 0f);
#endif
            
            
        }
    }
}
