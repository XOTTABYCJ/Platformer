using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour // Script of Main menu
{
    public UnityEngine.UI.Button[] lvls;
    public Text coinText;
    public Player player;
    public Slider musicSlider, soundSlider;
    public Text musicText, soundText;
    public AudioSource musicVolume, soundVolume;

    void Start() // Load the information of completed levels; quantity of heart, blue gem and green gem in inventory; Music and sound settings 
    {
        if (PlayerPrefs.HasKey("Lvl"))
        {
            for(int i = 0; i < lvls.Length; i++)
            {
                if (i <= PlayerPrefs.GetInt("Lvl"))
                    lvls[i].interactable = true;
                else 
                    lvls[i].interactable = false;
            }
        }

        if (!PlayerPrefs.HasKey("heart"))
            PlayerPrefs.SetInt("heart", 0);
        if (!PlayerPrefs.HasKey("blue_gem"))
            PlayerPrefs.SetInt("blue_gem", 0);
        if (!PlayerPrefs.HasKey("green_gem"))
            PlayerPrefs.SetInt("green_gem", 0);

        if (!PlayerPrefs.HasKey("musicVolume"))
            PlayerPrefs.SetInt("musicVolume", 3);
        if (!PlayerPrefs.HasKey("soundVolume"))
            PlayerPrefs.SetInt("soundVolume", 7);

        musicSlider.value = PlayerPrefs.GetInt("musicVolume");
        soundSlider.value = PlayerPrefs.GetInt("soundVolume");
    }

    void Update()
    {
        PlayerPrefs.SetInt("musicVolume", (int)musicSlider.value);
        PlayerPrefs.SetInt("soundVolume", (int)soundSlider.value);
        musicText.text = musicSlider.value.ToString();
        soundText.text = soundSlider.value.ToString();
        musicVolume.volume = ((float)PlayerPrefs.GetInt("musicVolume")) / 9;
        soundVolume.volume = ((float)PlayerPrefs.GetInt("soundVolume")) / 9;


        if (PlayerPrefs.HasKey("coins"))
            coinText.text = PlayerPrefs.GetInt("coins").ToString();
        else
            coinText.text = "0";
    }

    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
        Time.timeScale = 1f;
        player.enabled = true;
    }

    public void Buy_Hearth(int cost) // Buying an item from a store
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("heart", PlayerPrefs.GetInt("heart") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - 5);
        }
    }

    public void Buy_Blue_Gem(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("blue_gem", PlayerPrefs.GetInt("blue_gem") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - 15);
        }
    }

    public void Buy_Green_Gem(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("green_gem", PlayerPrefs.GetInt("green_gem") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - 10);
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
