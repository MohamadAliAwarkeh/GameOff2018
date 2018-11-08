using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    //Public Variables
    public int playerHealth;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Update()
    {
        if (playerHealth > numOfHearts)
        {
            playerHealth = numOfHearts;
        }

        //Runs through the array to check all the hearts we have
        for (int i = 0; i < hearts.Length; i++)
        {
            //Checking if the players health is less than [i]
            if (i < playerHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            //Checking the number of heart UI is active
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
