using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeTerminalUIController : MonoBehaviour
{
    [Header("Intro Screen")]
    public GameObject IntroPanel;
    public TMP_Text Console;    
    private bool hasTextChanged;

    void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

    void Start()
    {
        //DisplayIntro();
    }

    public void DisplayIntro()
    {
        IntroPanel.SetActive(true);
        StartCoroutine(RevealCharacters());
    }

    public void BringDownScreens()
    {
        IntroPanel.SetActive(false);
    }

    private void OnTextChanged(UnityEngine.Object obj)
    {
        hasTextChanged = true;
    }

    /// <summary>
    /// Method revealing the text one character at a time.
    /// </summary>
    /// <returns></returns>
    IEnumerator RevealCharacters()
    { 

        Console.ForceMeshUpdate();       

        TMP_TextInfo textInfo = Console.textInfo;

        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
        int visibleCount = 0;

       while (visibleCount!=totalVisibleCharacters)
       {
            if (hasTextChanged)
            {
                totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
                hasTextChanged = false;
            }

            if (visibleCount > totalVisibleCharacters)
            {
                yield return new WaitForSeconds(1.0f);
                visibleCount = 0;
            }

            Console.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            visibleCount += 1;

            yield return new WaitForSeconds(0.1f);
       }
    }


    /// <summary>
    /// Method revealing the text one word at a time.
    /// </summary>
    /// <returns></returns>
    IEnumerator RevealWords(string text, float speed)
    {
        Console.ForceMeshUpdate();

        int totalWordCount = Console.textInfo.wordCount;
        int totalVisibleCharacters = Console.textInfo.characterCount; // Get # of Visible Character in text object
        int counter = 0;
        int currentWord = 0;
        int visibleCount = 0;

        while (visibleCount != totalVisibleCharacters)
        {
            currentWord = counter % (totalWordCount + 1);

            // Get last character index for the current word.
            if (currentWord == 0) // Display no words.
                visibleCount = 0;
            else if (currentWord < totalWordCount) // Display all other words with the exception of the last one.
                visibleCount = Console.textInfo.wordInfo[currentWord - 1].lastCharacterIndex + 1;
            else if (currentWord == totalWordCount) // Display last word and all remaining characters.
                visibleCount = totalVisibleCharacters;

            Console.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            // Once the last character has been revealed, wait the set speed and start over.
            if (visibleCount >= totalVisibleCharacters)
            {
                yield return new WaitForSeconds(speed);
            }

            counter += 1;

            yield return new WaitForSeconds(0.1f);
        }
    }
}