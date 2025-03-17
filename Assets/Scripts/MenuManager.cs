using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;


public class MenuManager : MonoBehaviour
{
    [Header("Input Name")]
    public GameObject inputGO;
    public TMP_InputField nameInput;
    public TMP_Text nameText;

    [Header("Levels")]
    public Level selected;
    public List<Level> levels = new List<Level>();
    public int listValue = 0;
    public Image imageLevel;
    public TMP_Text nameLevel;
    public GameObject rightButton, leftButton;

    [Header("Sons")]
    public AudioClip clickButton;
    public AudioClip positiveButton;
    public AudioSource sfx;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LevelToRight()
    {
        sfx.PlayOneShot(clickButton);
        if (listValue < levels.Count - 1)
        {
            listValue += 1;
            imageLevel.sprite = levels[listValue].levelImage;
            nameLevel.text = levels[listValue].name;

            if (listValue == levels.Count - 1)
            {
                rightButton.SetActive(false);
                leftButton.SetActive(true);
            }
            else
            {
                leftButton.SetActive(true);
            }
        }
    }
    public void LevelToLeft()
    {
        sfx.PlayOneShot(clickButton);
        if (listValue <= levels.Count && listValue != 0)
        {
            listValue -= 1;
            imageLevel.sprite = levels[listValue].levelImage;
            nameLevel.text = levels[listValue].name;
                       
            if(listValue == 0)
            {
                leftButton.SetActive(false);
                rightButton.SetActive(true);
            }
            else
            {
                rightButton.SetActive(true);
            }
        }
    }
    void CheckInput()
    {
        if (PlayerPrefs.GetString("Name") == null || PlayerPrefs.GetString("Name") == "")
        {
            inputGO.SetActive(true);
        }
        else
        {
            inputGO.SetActive(false);
            nameText.text = PlayerPrefs.GetString("Name");
        }
    }
    public void SetName()
    {
        PlayerPrefs.SetString("Name", nameInput.text);
        nameText.text = PlayerPrefs.GetString("Name");
        sfx.PlayOneShot(clickButton);
    }

    public void ToLevel()
    {
        SceneManager.LoadScene(levels[listValue].sceneName);
        sfx.PlayOneShot(positiveButton);
    }
}
