using UnityEngine.UI;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    public Text liveText;

    // Update is called once per frame
    void Update()
    {
        liveText.text = PlayerStats.Lives.ToString() + " LIVES";
    }
}
