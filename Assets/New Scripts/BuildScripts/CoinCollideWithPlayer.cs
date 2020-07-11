using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollideWithPlayer : MonoBehaviour
{
    private GameManagerBuildScript gameManagerBuildScript;
    public TutorialManagerScript tutorialManagerScript;

    private void Start()
    {
        gameManagerBuildScript = FindObjectOfType<GameManagerBuildScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == FinalValues.PLAYER)
        {
            gameManagerBuildScript.pigAnimator.SetTrigger(
                FinalValues.TAKE_MONEY_TRIGGER_BUILD_SCENE_PIG_ANIMATOR);

            tutorialManagerScript.RemoveCoinsFromArr();
            FindObjectOfType<AudioManager>().PlayAudio(FinalValues.COIN_AUDIO);
            Destroy(gameObject);
        }
    }
}
