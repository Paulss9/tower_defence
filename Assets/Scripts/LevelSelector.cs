using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;

    public Button[] levelButtons; 

    void Start()
    {
        //this is how we emulate player progress on levelReached
        int levelReached = PlayerPrefs.GetInt ("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
            {
                levelButtons[i].interactable = false;
            }
        }
    }

    public void Select (string levelName)
    {
        fader.FadeTo(levelName);
    }
}
