using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour // Script for Player`s inventory
{
    int heart = 0, blue_gem = 0, green_gem = 0;
    public Sprite[] numbers;
    public Sprite is_heart, no_heart, is_blue_gem, no_blue_gem, is_green_gem, no_green_gem, is_key, no_key;
    public Image heart_image, blue_gem_image, green_gem_image, key_image;
    public Player player;
    bool can_use_blue = true, can_use_green = true;

    private void Start() // Load sprites in UI inventory
    {
        if (PlayerPrefs.GetInt("heart") > 0)
        {
            heart = PlayerPrefs.GetInt("heart");
            heart_image.sprite = is_heart;
            heart_image.transform.GetChild(0).GetComponent<Image>().sprite = numbers[heart];
        }

        if (PlayerPrefs.GetInt("green_gem") > 0)
        {
            green_gem = PlayerPrefs.GetInt("green_gem");
            green_gem_image.sprite = is_green_gem;
            green_gem_image.transform.GetChild(0).GetComponent<Image>().sprite = numbers[green_gem];
        }

        if (PlayerPrefs.GetInt("blue_gem") > 0)
        {
            blue_gem = PlayerPrefs.GetInt("blue_gem");
            blue_gem_image.sprite = is_blue_gem;
            blue_gem_image.transform.GetChild(0).GetComponent<Image>().sprite = numbers[blue_gem];
        }
    }

    public void Add_heart() // Adds a heart to in inventory
    {
        heart++;
        heart_image.sprite = is_heart;
        heart_image.transform.GetChild(0).GetComponent<Image>().sprite = numbers[heart];
    }

    public void Add_blue_gem() // Adds a blue gem to in inventory
    {
        blue_gem++;
        blue_gem_image.sprite = is_blue_gem;
        blue_gem_image.transform.GetChild(0).GetComponent<Image>().sprite = numbers[blue_gem];
    }

    public void Add_green_gem() // Adds a green gem to in inventory
    {
        green_gem++;
        green_gem_image.sprite = is_green_gem;
        green_gem_image.transform.GetChild(0).GetComponent<Image>().sprite = numbers[green_gem];
    }

    public void Add_key() // Adds a key to in inventory
    {
        key_image.sprite = is_key;
    }

    public void Use_heart() // Use a heart from inventory
    {
        if (heart > 0)
        {
            heart--;
            player.RecountHp(1);
            heart_image.transform.GetChild(0).GetComponent<Image>().sprite = numbers[heart];
            if (heart == 0)
                heart_image.sprite = no_heart;
        }
    }

    public void Use_blue_gem() // Use a blue gem from inventory
    {
        if (blue_gem > 0 && can_use_blue)
        {
            blue_gem--;
            player.BlueGem();
            blue_gem_image.transform.GetChild(0).GetComponent<Image>().sprite = numbers[blue_gem];
            if (blue_gem == 0)
                blue_gem_image.sprite = no_blue_gem;
        }
        StartCoroutine(Wait_For_Use_Blue());
    }

    public void Use_green_gem() // Use a green gem from inventory
    {
        if (green_gem > 0 && can_use_green)
        {
            green_gem--;
            player.GreenGem();
            green_gem_image.transform.GetChild(0).GetComponent<Image>().sprite = numbers[green_gem];
            if (green_gem == 0)
                green_gem_image.sprite = no_green_gem;
        }
        StartCoroutine(Wait_For_Use_Green());
    }

    public void Recount_Items() // Recount all items in inventory
    {
        PlayerPrefs.SetInt("heart", heart);
        PlayerPrefs.SetInt("blue_gem", blue_gem);
        PlayerPrefs.SetInt("green_gem", green_gem);
    }

    IEnumerator Wait_For_Use_Green() // Waiting time before reuse green gem
    {
        can_use_green = false;
        yield return new WaitForSeconds(5f);
        can_use_green = true;
    }

    IEnumerator Wait_For_Use_Blue() // Waiting time before reuse blue gem
    {
        can_use_blue = false;
        yield return new WaitForSeconds(10f);
        can_use_blue = true;
    }
}
