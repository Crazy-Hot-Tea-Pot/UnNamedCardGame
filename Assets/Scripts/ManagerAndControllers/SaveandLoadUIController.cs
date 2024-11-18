using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveandLoadUIController : MonoBehaviour
{
    public TextMeshProUGUI DisplayScreen;
    public GameObject InputField;
    // Start is called before the first frame update
    void Start()
    {
        DisplayScreen.text =
            "________________________\r\n" +
            "|                       |\r\n" +
            "|   _________________   |\r\n" +
            "|  |                 |  |\r\n" +
            "|  |    <color=#A20000>Chip Sect</color>    |  |\r\n" +
            "|  |_________________|  |\r\n" +
            "|                       |\r\n" +
            "|     _____________     |\r\n" +
            "|    |             |    |\r\n" +
            "|    |   <size=60%>Data Station</size>   |    |\r\n" +
            "|    |  <size=40%>(left Click) To continue</size>  |    |\r\n" +
            "|    |_____________|    |\r\n" +
            "|_______________________|";

        StartCoroutine(RevealText(DisplayScreen, false, true,0.01f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Display text overtime.
    /// </summary>    
    /// <param name="loopText">If to loop text on display.</param>
    /// <param name="revealByCharacter">If to reveal by character or by word</param>
    /// <param name="revealSpeed">higher the number longer it takes</param>
    /// <returns></returns>
    public IEnumerator RevealText(TMP_Text textComponent, bool loopText, bool revealByCharacter, float revealSpeed)
    {
        textComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = textComponent.textInfo;
        int totalVisibleCharacters = textInfo.characterCount; // Total characters in text

        do
        {
            if (revealByCharacter)
            {
                for (int i = 0; i <= totalVisibleCharacters; i++)
                {
                    textComponent.maxVisibleCharacters = i; // Reveal characters up to the index
                    yield return new WaitForSeconds(revealSpeed); // Use the reveal speed parameter
                }
            }
            else
            {
                string[] words = textComponent.text.Split(' ');
                int wordCount = 0;

                foreach (string word in words)
                {
                    wordCount += word.Length + 1; // Account for spaces
                    textComponent.maxVisibleCharacters = wordCount;
                    yield return new WaitForSeconds(revealSpeed); // Use the reveal speed parameter
                }
            }

            yield return new WaitForSeconds(0.5f); // Pause after full reveal

        } while (loopText);
    }

}
