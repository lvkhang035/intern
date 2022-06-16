using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TNFuelSystem : MonoBehaviour
{

    public Slider fuelSlider;
    public float currentFuel = 100f;
    public Text fuellDisplay;
    float countDown;
    public float baseInterval = 4f;
    bool carIsMoving;

    public static TNFuelSystem tfs;
    // Start is called before the first frame update
    void Start()
    {
        tfs = this;
        carIsMoving = true;
        countDown = baseInterval;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (carIsMoving)
        {
            if(countDown > 0)
            {
                countDown -= Time.deltaTime;
            }
            else
            {
                countDown = baseInterval;
                currentFuel -= 1f;
                
            }
                
        }
        fuellDisplay.text = "Fuel: " + currentFuel;
        fuelSlider.value = currentFuel / 100;
    }

    public void setCarMovement(bool yesOrNo)
    {
        carIsMoving = yesOrNo;
    }
}
