using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {

	public float minSwipeDist = 50f;
	public float maxSwipeTime = 1f;
    public GameObject targetEntity;

	float actionStartTime = 0f;
    bool swiping = false;
	Vector2 actionStartPos;
	Vector2 moveStartPos;
    TraitMove m_move;
    TraitJump m_jump;

	private void Start()
	{
        if (targetEntity == null)
        {
            throw new UnityException("Target Entity on TouchController must be set.");
        }

        m_move = targetEntity.GetComponent<TraitMove>();
        m_jump = targetEntity.GetComponent<TraitJump>();

        if (m_move == null)
        {
            throw new UnityException("Target Entity on TouchController must have a Trait Movement component.");
        }
        if (m_jump == null)
        {
            throw new UnityException("Target Entity on TouchController must have a Trait Jump component.");
        }
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Input.touchCount > 0) {
			foreach (Touch touch in Input.touches) {
				if(touch.position.x > Screen.width * 0.75f) {
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            actionStartPos = touch.position;
                            actionStartTime = Time.time;
                            break;
                        case TouchPhase.Moved:
                            ActionSwipe(touch.position, actionStartPos);
							break;
                        case TouchPhase.Ended:
                            EndActionTouch();
                            break;
					}
				} else {
					switch (touch.phase) {
						case TouchPhase.Began:
							moveStartPos = touch.position;
							break;
						case TouchPhase.Moved:
							MoveSwipe(touch.position, moveStartPos);
							break;
                        case TouchPhase.Ended:
                            EndMoveTouch();
                            break;
					}
				}
			}
		}
	}


    void ActionSwipe(Vector2 touchPos, Vector2 startPos) {
		float swipeDirection = Mathf.Sign(touchPos.y - startPos.y);
        float swipeTime = Time.time - actionStartTime;
        float swipeDist = Mathf.Abs(touchPos.y - actionStartPos.y);
        if ((swipeTime < maxSwipeTime) && (swipeDist > minSwipeDist))
        {
            swiping = true;
            if ((swipeDirection > 0))
            {
                //Debug.Log("Jumping");
                //m_jump.StartJump();
            }
            else
            {
                //Debug.Log("Interacting");
            }
        }

	}

	void MoveSwipe(Vector2 touchPos, Vector2 startPos) {
		float swipeDist = Mathf.Abs(touchPos.x - startPos.x);
		//Debug.Log("Swipe Dist: " + swipeDist);
		if (swipeDist > minSwipeDist) {
            //int swipeDirection = (int) Mathf.Sign(touchPos.x - startPos.x);
			//Debug.Log ("Moving: " + swipeDirection);
            //m_move.dir = swipeDirection;
		} else {
			//Debug.Log ("Moving: Stopped");
            //m_move.dir = 0;
		}
	}

	void Attack() {
		//Debug.Log ("Attacking");
	}

    void EndActionTouch() {
        if (!swiping)
        {
            Attack();
        }
        swiping = false;
        //m_jump.CancelJump();
    }

    void EndMoveTouch() {
        //m_move.dir = 0;
    }

	// void OnGUI() {
	// 	foreach (Touch touch in Input.touches) {
	// 		string message = "";
	// 		message += "ID: " + touch.fingerId + "\n";
	// 		message += "Phase: " + touch.phase.ToString() + "\n";
	// 		message += "TapCount: " + touch.tapCount + "\n";
	// 		message += "Pos X: " + touch.position.x + "\n";
	// 		message += "Pos Y: " + touch.position.y + "\n";

	// 		int num = touch.fingerId;
	// 		GUI.Label(new Rect(0+130 * num, 0, 120, 100), message);
	// 	}

	// }
}
