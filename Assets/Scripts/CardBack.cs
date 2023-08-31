using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBack : MonoBehaviour
{
    public GameObject cardBack;

    public void UpdateCard(bool updown)
    {
        cardBack.SetActive(updown);
    }
}
