using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
   public Slider slider;

   public void SetMaxValue(int health) {
        slider.maxValue = health;
        slider.value = health;
   }

   public void SetValue(int health) {
    slider.value = health;
   }
}
