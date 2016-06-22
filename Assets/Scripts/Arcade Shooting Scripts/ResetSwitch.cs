using UnityEngine;
using System.Collections;

public class ResetSwitch : MonoBehaviour {

    private GamePlaying gamePlayingScript;
    private StartGameSwitch gameSwitchScript;

    void Awake()
    {
        gamePlayingScript = GetComponent<GamePlaying>();
        gameSwitchScript = GetComponent<StartGameSwitch>();
    }


    //If game is playing, start button mechanic should not be on and vice versa
    void Update()
    {
        gameSwitchScript.enabled = !gamePlayingScript.enabled;
        gamePlayingScript.enabled = !gameSwitchScript.enabled;
    }
}
