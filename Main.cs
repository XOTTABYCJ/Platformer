using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Player player;
    public Text coinText;
    public Image[] hearts;
    public Sprite isLife, nonLife;
    public GameObject PauseScreen;
    public GameObject WinScreen;
    public GameObject LoseScreen;
    float timer = 0f;
    public Text timeText;
    public TimeWork timeWork;
    public float countdown;
    public GameObject inventoryPan;
    public SoundEffector soundEffector;
    public AudioSource musicSource, soundSource;

    public void ReloadLevel()
    {
        Time.timeScale = 1f; 
        player.enabled = true;
        PauseScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void Start() // Load musin and sound
    {
        musicSource.volume = ((float)PlayerPrefs.GetInt("musicVolume")) / 9;
        soundSource.volume = ((float)PlayerPrefs.GetInt("soundVolume")) / 9;

        if ((int)timeWork == 2)
            timer = countdown;
    }


    public void Update() // Timer and recount heals in an UI
    {
        coinText.text = player.GetCoins().ToString();

        for(int i = 0; i < hearts.Length; i++)
        {
            if (player.GetHp() > i)
                hearts[i].sprite = isLife;
            else
                hearts[i].sprite = nonLife;
        }

        if ((int)timeWork == 1)
        {
            timer += Time.deltaTime;
            timeText.text = timer.ToString("F2").Replace(",", ":");
        }
        else if ((int)timeWork == 2)
        {
            timer -= Time.deltaTime;
            timeText.text = ((int)timer / 60).ToString() + ":" + ((int)timer - ((int)timer / 60) * 60).ToString("D2");
            if (timer <= 0)
                Lose();
        }


        else
            timeText.gameObject.SetActive(false);
    }

    public void PauseOn()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        PauseScreen.SetActive(true);
    }


    public void PauseOff()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        PauseScreen.SetActive(false);
    }


    public void Win()
    {
        soundEffector.PlayWinSound();
        Time.timeScale = 0f;
        player.enabled = false;
        WinScreen.SetActive(true);

        if (!PlayerPrefs.HasKey("Lvl") || PlayerPrefs.GetInt("Lvl") < SceneManager.GetActiveScene().buildIndex)
            PlayerPrefs.SetInt("Lvl", SceneManager.GetActiveScene().buildIndex);

        if (PlayerPrefs.HasKey("coins"))
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + player.GetCoins());
        else
            PlayerPrefs.SetInt("coins", player.GetCoins());

        inventoryPan.SetActive(false);
        GetComponent<Inventory>().Recount_Items();
    }

    public void Lose()
    {
        soundEffector.PlayLoseSound();
        Time.timeScale = 0f;
        player.enabled = false;
        LoseScreen.SetActive(true);

        inventoryPan.SetActive(false);
        GetComponent<Inventory>().Recount_Items();
    }


    public void MenuLevel()
    {
        SceneManager.LoadScene("Menu");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
        player.enabled = true;
        PauseScreen.SetActive(false);
    }
}


public enum TimeWork // Choosing the timer type 
{
    None, Stopwatch, Time

}
