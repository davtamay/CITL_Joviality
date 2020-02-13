using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScreenToShow : MonoBehaviour
{
    [SerializeField]Game_Settings game_settings;

    public void Start()
    {
        switch (game_settings.currentInteractionMode)
        {
            case Interaction_Mode.gazeInteraction:
                transform.GetChild(0).gameObject.SetActive(true);
                break;


            case Interaction_Mode.controllerInteraction:
                transform.GetChild(1).gameObject.SetActive(true);
                break;


        }
    }
}
