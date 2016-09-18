using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GamePlayPanel : MonoBehaviour {

    #region Public_Variables

    #endregion

    #region Private_Variables
    private Image playerHealthBar;
    #endregion

    // Use this for initialization
    void Awake () {
        playerHealthBar = this.transform.Find("Health_Bar/Filler").GetComponent<Image>();
	}

    internal void ReducePlayerHealth(int playerHealth) {
        float originalValue = playerHealthBar.fillAmount;
        float newValue = (float)playerHealth / 100;
        
        LTDescr scaleBar = LeanTween.value(this.gameObject, originalValue, newValue, 0.5f);
        scaleBar.setEase(LeanTweenType.easeOutCirc);
        scaleBar.setOnUpdate((float value) => {
            playerHealthBar.fillAmount = value;
        });       
    }

	public void OnPlayerMoveLeft(string eventType){
		Debug.Log ("Left Button Pressed");
		if(eventType.Equals("Pressed")){
		//	Debug.Log ("Left Button Pressed");
			PlayerMovement.instance.SimulateMovementLeft = true;
		}
		else if(eventType.Equals("Released")){
			PlayerMovement.instance.SimulateMovementLeft = false;
		}
	}

	public void OnPlayerMoveRight(string eventType){
		if(eventType.Equals("Pressed")){
			PlayerMovement.instance.SimulateMovementRight = true;
		}
		else if(eventType.Equals("Released")){			
			PlayerMovement.instance.SimulateMovementRight = false;
		//	Debug.Log ("Player Move Right   "+PlayerMovement.instance.SimulateMovementRight   );
		}
	}


	public void OnPlayerJumpAction(){		
		PlayerMovement.instance.Jump ();
	}

	public void OnPlayerShootAction(string actionType){
		if (actionType.Equals("true")) {
			PlayerFightingSkills.instance.PlayerFightAction (true);
		} else {
			PlayerFightingSkills.instance.PlayerFightAction (false);
		}
	}
}
