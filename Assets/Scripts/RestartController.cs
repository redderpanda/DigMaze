using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartController : MonoBehaviour {
    public Text tutorialText;
    public string[] tutorialLines;
    public int currentTutorial = -1;
    public Button needsToBeDestroyed;

    public void Awake()
    {
        //tutorialText.text = "You can move around using WASD or the arrow keys";
        tutorialLines = new string[5]{ "You can move around using WASD or the arrow keys", "You can DIG the GROUND by left-clicking or pressing the SPACE bar and HOLDING down the desired DIRECTION",
    "Your goal is to collect ALL the diamonds on the map and escape through the EXIT", "There are BOMBS so watch out",
    "GOOD LUCK and safe digging" };
    }

    public void restartLevel(string levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
            
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Tutorial()
    {
        if (currentTutorial == 4)
        {
            Destroy(tutorialText);
            Destroy(needsToBeDestroyed);
        }
        else
        {
            currentTutorial++;
            tutorialText.text = tutorialLines[currentTutorial];

        }

    }
}
