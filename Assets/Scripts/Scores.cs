using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scores : MonoBehaviour
{
    public Text DreadfulDart;
    public Text GuidedGhoul;
    public Text WistfulWanderer;
    // Start is called before the first frame update
    void Start()
    {
        DreadfulDart.text = "0";
        GuidedGhoul.text = "0";
        WistfulWanderer.text = "0";
    }

    public void updateScores(Dictionary<string, int> newScores) {
        DreadfulDart.text = newScores["DreadfulDart"].ToString();
        GuidedGhoul.text = newScores["GuidedGhoul"].ToString();
        WistfulWanderer.text = newScores["WistfulWanderer"].ToString();
    }
}
