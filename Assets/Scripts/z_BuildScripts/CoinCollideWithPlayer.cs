using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollideWithPlayer : MonoBehaviour
{
    private TutorialManagerScript tutorialManagerScript;
    private GameManagerBuildScript gameManagerBuildScript;

    private void Start()
    {
        tutorialManagerScript = FindObjectOfType<TutorialManagerScript>();
        gameManagerBuildScript = FindObjectOfType<GameManagerBuildScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            gameManagerBuildScript.pigAnimator.SetTrigger(
                FinalValues.TAKE_MONEY_TRIGGER_BUILD_SCENE_PIG_ANIMATOR);

            tutorialManagerScript.RemoveCoin();
            FindObjectOfType<AudioManager>().PlayAudio("coin");
            Destroy(gameObject);
        }
    }
}
