using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;

    public void setSongLength(float len){
        slider.maxValue = len;
    }

    public void setDuration(float len){
        slider.value = len;
    }
}
