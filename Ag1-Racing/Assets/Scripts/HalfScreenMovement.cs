/* Arnav Correction : 13/03/2022 - on Shashank working file corresponding to Arnav edit 4 apk
file provided by Shashank on : 13/03/2022

4 edits done by Arnav on 09/03/2022 - tested on apks provided by Uniplay on 12/03/2022
5th edit done on 13/03/2022

This is based on EDIT 5 - based on what needed to be corrected - as testing showed

NOTE : this edit requires edited MovementController.cs file also - edited from EDIT 3
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HalfScreenMovement : MonoBehaviour
{
    private float screenCenterX;
    [SerializeField] private MovementController MovementController;

    static float t;
    private bool isReducing = false;

    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;

    private bool BoostInitiated = false;
    private bool BreakPressed = false;

    public float SWIPE_THRESHOLD = 20f;

    #region swipe and hold
    public float fingerInitialPositionX;
    public float fingerMovedPositionX;
    public float fingerHeldPositionX;
    public float fingerEndPositionX;

    public float fingerInitialPositionY;
    public float fingerMovedPositionY;
    public float fingerHeldPositionY;
    public float fingerEndPositionY;

    public bool swipeUpOn;
    public bool swipeDownOn;
    public bool swipeRightOn;
    public bool swipeLeftOn;
    #endregion

    #region arnav
    public float fingerInitialTime;
    public float fingerMovedTime;
    public float fingerHeldTime;
    public float fingerEndTime;

    public bool hasMoved;
    #endregion

    // arnav second edit added
    #region special
    public float fingerSubInitialPositionX;
    public float fingerSubInitialPositionY;
    public float VirtualfingerSubInitialPositionX;
    public float VirtualfingerSubInitialPositionY;
    public float previousPositionX;
    public float previousPositionY;
    public float newPositionX;
    public float newPositionY;
    public float moveDirectionX;
    public float moveDirectionY;
    public float previousmoveDirectionX;
    public float previousmoveDirectionY;
    public float newmoveDirectionX;
    public float newmoveDirectionY;
    public float directionChangeX;
    public float directionChangeY;

    public float normalizedNewDirX;
    public float normalizedNewDirY;
    public float normalizedPrevDirX;
    public float normalizedPrevDirY;

    public float DIRECTION_THRESHOLD = 0f; // need to test and see what would be a proper value for this
    // NOTE that DIRECTION_THRESHOLD was removed in edit 5
    public float tapTime = 0.1f; // added in edit 4 - need to test and see what would be a proper value
    public float tapThresholdTime = 0.2f; // added in edit 4 - need to test and see similarly

    // edit 4 further additions
    public float firstTapPositionX;
    public float firstTapPositionY;
    public float firstTapTime;
    public bool checkForDoubleTap;

    public bool isMoving;
    public bool hasHeld;
    public bool horizontalPredominance;
    public bool verticalPredominance;
    public bool predominanceChange;
    #endregion

    private void Start()
    {

        swipeUpOn = false;
        swipeDownOn = false;
        swipeRightOn = false;
        swipeLeftOn = false;
        // save the horizontal center of the screen
        screenCenterX = Screen.width * 0.5f;
    }

    public IEnumerator FloatReduce()
    {
        for (float i = 1f; i > 0f; i -= 0.1f)
        {
            MovementController.HorMovement = Mathf.Round(i);
            //  Debug.Log(MovementController.HorMovement);


        }

        yield return new WaitForSeconds(0.05f);

    }

    private void Update()
    {
        // edit 4 - checking for double tap
        if (checkForDoubleTap)
        {
            float doubleTapTimeOffset = Time.time - firstTapTime;
            if (doubleTapTimeOffset >= tapThresholdTime)
            {
                StartCoroutine(OnDoubleTapFail());
            }
        }

        if (MovementController.Controller == GameConsts.ControlType.Touch)
        {
            if (Input.touchCount > 0)
            {
                // get the first one
                Touch firstTouch = Input.GetTouch(0);

                // if it began this frame
                if (firstTouch.phase == TouchPhase.Began)
                {
                    if (firstTouch.position.x > screenCenterX)
                    {
                        Debug.Log("Right");
                        MovementController.HorMovement = 1f;
                        isReducing = false;
                        // if the touch position is to the right of center
                        // move right
                    }
                    else if (firstTouch.position.x < screenCenterX)
                    {
                        MovementController.HorMovement = -1f;
                        Debug.Log("Leftt");
                        isReducing = false;
                        // if the touch position is to the left of center
                        // move left
                    }
                }

                else if (firstTouch.phase == TouchPhase.Ended)
                {


                    Debug.Log("End");
                }
            }

            else if (Input.touchCount == 0)
            {
                if (MovementController.HorMovement != 0)
                {
                    if (!isReducing)
                    {
                        StartCoroutine(FloatReduce());
                        isReducing = true;
                    }



                    // MovementController.HorMovement = Mathf.Lerp(MovementController.HorMovement, 0f, Time.deltaTime * 25f);
                }


            }


        }

        else if (MovementController.Controller == GameConsts.ControlType.Motion)
        {

            MovementController.HorMovement = Input.acceleration.x * 1.3f;
        }

        else if (MovementController.Controller == GameConsts.ControlType.Swipe)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUp = touch.position;
                    fingerDown = touch.position;
                }

                //Detects Swipe while finger is still moving
                if (touch.phase == TouchPhase.Moved)
                {
                    if (!detectSwipeOnlyAfterRelease)
                    {
                        fingerDown = touch.position;
                        checkSwipe();
                    }
                }

                //Detects swipe after finger is released
                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

        }

        else if (MovementController.Controller == GameConsts.ControlType.Hybrid)
        {
            foreach (Touch FingerTouch in Input.touches) //get touches
            {
                // for edit 4 - shifted jump functionality to touch phase end near checking for first tap

                //do things when touch has just begun
                if (FingerTouch.phase == TouchPhase.Began)
                {

                    // arnav deleted touch hybrid initiation here - transferred to hold & moved case

                    fingerInitialPositionX = FingerTouch.position.x; //get initial X position of touch
                    fingerInitialPositionY = FingerTouch.position.y; //get initial Y position of touch
                    fingerInitialTime = Time.time; // arnav added
                    Debug.Log("Touch initiated");
                    hasMoved = false; // arnav added

                    // arnav second edit - special measures
                    previousPositionX = fingerInitialPositionX;
                    previousPositionY = fingerInitialPositionY;
                    previousmoveDirectionX = 0;
                    previousmoveDirectionY = 0;
                    directionChangeX = 0;
                    directionChangeY = 0;

                    fingerSubInitialPositionX = fingerInitialPositionX;
                    fingerSubInitialPositionY = fingerInitialPositionY;

                }

                //do things as soon as finger is moved (and make it not repeat theinformation every frame)
                else if (FingerTouch.phase == TouchPhase.Moved) //do things as soon as finger is moved
                {

                    fingerMovedPositionX = FingerTouch.position.x; //get the new X position of touch
                    fingerMovedPositionY = FingerTouch.position.y; //get the new Y position of touch
                    fingerMovedTime = Time.time; // arnav added
                    float horizontalOffset = fingerMovedPositionX - fingerInitialPositionX;
                    float verticalOffset = fingerMovedPositionY - fingerInitialPositionY;
                    float timeOffset = fingerMovedTime - fingerInitialTime;

                    // arnav second edit - special measures
                    newPositionX = fingerMovedPositionX;
                    newPositionY = fingerMovedPositionY;
                    moveDirectionX = newPositionX - previousPositionX;
                    moveDirectionY = newPositionY - previousPositionY;


                    newmoveDirectionX = moveDirectionX;
                    newmoveDirectionY = moveDirectionY;
                    if (moveDirectionX == 0)
                    {
                        normalizedNewDirX = 0;
                    }
                    else
                    {
                        normalizedNewDirX = moveDirectionX / Mathf.Abs(moveDirectionX);
                    }
                    if (moveDirectionY == 0)
                    {
                        normalizedNewDirY = 0;
                    }
                    else
                    {
                        normalizedNewDirY = moveDirectionY / Mathf.Abs(moveDirectionY);
                    }
                    if (previousmoveDirectionX == 0)
                    {
                        normalizedPrevDirX = 0;
                    }
                    else
                    {
                        normalizedPrevDirX = previousmoveDirectionX / Mathf.Abs(previousmoveDirectionX);
                    }
                    if (previousmoveDirectionY == 0)
                    {
                        normalizedPrevDirY = 0;
                    }
                    else
                    {
                        normalizedPrevDirY = previousmoveDirectionY / Mathf.Abs(previousmoveDirectionY);
                    }
                    directionChangeX = normalizedNewDirX - normalizedPrevDirX;
                    directionChangeY = normalizedNewDirY - normalizedPrevDirY;

                    // if direction has changed and appropriate conditions are met then sub initial position
                    if (directionChangeX != 0 || directionChangeY != 0)
                    {
                        /* if (Mathf.Abs(moveDirectionX) > DIRECTION_THRESHOLD || Mathf.Abs(moveDirectionY) > DIRECTION_THRESHOLD)
                        {
                            // first case - new direction is to the right, direction change is predominantly horizontal (x axis)
                            if (directionChangeX > 0 && Mathf.Abs(moveDirectionX) >= Mathf.Abs(moveDirectionY))
                            {
                                VirtualfingerSubInitialPositionX = previousPositionX;
                                VirtualfingerSubInitialPositionY = previousPositionY;
                                // can maybe IN LATER EDITS add settings to change to a right swipe
                            }
                            // second case - new direction is to the left, direction change is predominantly horizontal (x axis)
                            else if (directionChangeX < 0 && Mathf.Abs(moveDirectionX) >= Mathf.Abs(moveDirectionY))
                            {
                                VirtualfingerSubInitialPositionX = previousPositionX;
                                VirtualfingerSubInitialPositionY = previousPositionY;
                                // can maybe IN LATER EDITS add settings to change to a left swipe
                            }
                            // third case - new direction is up, direction change is predominantly vertical (y axis)
                            else if (directionChangeY > 0 && Mathf.Abs(moveDirectionX) < Mathf.Abs(moveDirectionY))
                            {
                                VirtualfingerSubInitialPositionX = previousPositionX;
                                VirtualfingerSubInitialPositionY = previousPositionY;
                                // can maybe IN LATER EDITS add settings to change to am up swipe
                            }
                            // fourth case - new direction is down, direction change is predominantly vertical (y axis)
                            else if (directionChangeY < 0 && Mathf.Abs(moveDirectionX) < Mathf.Abs(moveDirectionY))
                            {
                                VirtualfingerSubInitialPositionX = previousPositionX;
                                VirtualfingerSubInitialPositionY = previousPositionY;
                                // can maybe IN LATER EDITS add settings to change to a down swipe
                            }
                        } */

                        // edit 5 - arnav removed direction threshold functionality totally
                        // now simple general virtual sub position - as below

                        // generally sub position virtually - as per edit 5
                        VirtualfingerSubInitialPositionX = previousPositionX;
                        VirtualfingerSubInitialPositionY = previousPositionY;

                        // NOTE - I was able to correct a critical syntax error here - which could have given lot of pain
                        // correction done - after edit 5 - and after seeing Shashank's code - and then rechecking my edit 5 code
                        // - Arnav

                    }


                    // virtual position only converts to actual position to be used if following smoothing conditions are met
                    float VirtualhorizontalOffset = newPositionX - VirtualfingerSubInitialPositionX;
                    float VirtualverticalOffset = newPositionY - VirtualfingerSubInitialPositionY;
                    if (Mathf.Abs(VirtualhorizontalOffset) > SWIPE_THRESHOLD || Mathf.Abs(VirtualverticalOffset) > SWIPE_THRESHOLD)
                    {
                        fingerSubInitialPositionX = VirtualfingerSubInitialPositionX;
                        fingerSubInitialPositionY = VirtualfingerSubInitialPositionY;
                    } // and so direction changes result in 'swipe-center' position change smoothly

                    // second edit - horizontal / vertical offsets change by sub-position
                    horizontalOffset = newPositionX - fingerSubInitialPositionX;
                    verticalOffset = newPositionY - fingerSubInitialPositionY;
                    // NOTE that above was shifted below for edit 5 - correction

                    // as per edit 4 - only do things if it is not a quick tap
                    if (timeOffset >= tapTime)
                    {

                        // just to clear a subtle loose end
                        checkForDoubleTap = false;

                        // only do hybrid swipe if movement if beyond threshold - arnav modified
                        if (Mathf.Abs(horizontalOffset) > SWIPE_THRESHOLD || Mathf.Abs(verticalOffset) > SWIPE_THRESHOLD)
                        {

                            // edit 3 - no hold down boost or break possible now that swipe movement is activated
                            BoostInitiated = false;
                            BreakPressed = false;
                            MovementController.stopBoosting = true;
                            MovementController.stopBreaking = true;

                            //first case - finger is moved right, movement is predominantly horizontal (x axis)
                            if (fingerMovedPositionX > fingerSubInitialPositionX && Mathf.Abs(fingerMovedPositionX - fingerSubInitialPositionX) >= Mathf.Abs(fingerMovedPositionY - fingerSubInitialPositionY))
                            {
                                //swipe right
                                if (swipeRightOn == false && swipeLeftOn == false && swipeUpOn == false && swipeDownOn == false) //make it so you can't initiate a new swipe after one has already bin initiated
                                {
                                    //initiate stuff on swipe right
                                    swipeRightOn = true;
                                    MovementController.HorMovement = 1;
                                    Debug.Log("Swipe right initiated");
                                }
                                else if (swipeLeftOn)
                                {
                                    swipeLeftOn = false;
                                    swipeRightOn = true;
                                    MovementController.HorMovement = 1;
                                    Debug.Log("Swipe right initiated");

                                }
                                // edit 3 changes
                                else if (swipeUpOn)
                                {
                                    swipeUpOn = false;
                                    swipeRightOn = true;
                                }
                                else if (swipeDownOn)
                                {
                                    swipeDownOn = false;
                                    swipeRightOn = true;
                                }
                            }
                            //second case - finger is moved left, movement is predominantly horizontal (x axis)
                            else if (fingerMovedPositionX < fingerSubInitialPositionX && Mathf.Abs(fingerMovedPositionX - fingerSubInitialPositionX) >= Mathf.Abs(fingerMovedPositionY - fingerSubInitialPositionY))
                            {
                                //swipe left
                                if (swipeRightOn == false && swipeLeftOn == false && swipeUpOn == false && swipeDownOn == false)
                                {
                                    //initiate stuff on swipe left
                                    MovementController.HorMovement = -1;
                                    swipeLeftOn = true;
                                    Debug.Log("Swipe left initiated");
                                }
                                else if (swipeRightOn)
                                {
                                    swipeRightOn = false;
                                    MovementController.HorMovement = -1;
                                    swipeLeftOn = true;

                                }
                                // edit 3 changes
                                else if (swipeUpOn)
                                {
                                    swipeUpOn = false;
                                    swipeLeftOn = true;
                                }
                                else if (swipeDownOn)
                                {
                                    swipeDownOn = false;
                                    swipeLeftOn = true;
                                }
                            }
                            // arnav added horizontal bias in case horizontal and vertical offset are equal
                            // in arnav second edit

                            //third case - finger is moved up, movement predominantly vertical (y axis)
                            else if (fingerMovedPositionY > fingerSubInitialPositionY && Mathf.Abs(fingerMovedPositionX - fingerSubInitialPositionX) < Mathf.Abs(fingerMovedPositionY - fingerSubInitialPositionY))
                            {
                                //swipe up
                                if (swipeRightOn == false && swipeLeftOn == false && swipeUpOn == false && swipeDownOn == false)
                                {
                                    //initiate stuff on swipe up
                                    swipeUpOn = true;
                                    // edit 3 - removed boost not initiated requirement
                                    MovementController.Initboost();
                                    Debug.Log("Swipe up initiated");
                                }
                                // edit 3 changes
                                else if (swipeRightOn)
                                {
                                    swipeRightOn = false;
                                    swipeUpOn = true;
                                    MovementController.Initboost();
                                }
                                else if (swipeLeftOn)
                                {
                                    swipeLeftOn = false;
                                    swipeUpOn = true;
                                    MovementController.Initboost();
                                }
                                else if (swipeDownOn)
                                {
                                    swipeDownOn = false;
                                    swipeUpOn = true;
                                    MovementController.Initboost();
                                }
                            }

                            //fourth case - finger is moved down, movement predominantly vertical (y axis)
                            else if (fingerMovedPositionY < fingerSubInitialPositionY && Mathf.Abs(fingerMovedPositionX - fingerSubInitialPositionX) < Mathf.Abs(fingerMovedPositionY - fingerSubInitialPositionY))
                            {
                                //swipe down
                                if (swipeRightOn == false && swipeLeftOn == false && swipeUpOn == false && swipeDownOn == false)
                                {
                                    //initiate stuff on swipe down
                                    swipeDownOn = true;
                                    // edit 3 - removed break not initiated requirement
                                    MovementController.InitBreak();
                                    Debug.Log("Swipe down initiated");
                                }
                                // edit 3 changes
                                else if (swipeRightOn)
                                {
                                    swipeRightOn = false;
                                    swipeDownOn = true;
                                    MovementController.InitBreak();
                                }
                                else if (swipeLeftOn)
                                {
                                    swipeLeftOn = false;
                                    swipeDownOn = true;
                                    MovementController.InitBreak();
                                }
                                // there was an error here - which needed correction
                                else if (swipeUpOn)
                                {
                                    swipeDownOn = true;
                                    swipeUpOn = false; // corrected in arnav edit 5
                                    MovementController.InitBreak();
                                }
                            }

                            hasMoved = true; // arnav added
                                             // changed position in second edit - this should be wherever actual swipe movement takes place

                        }
                        else
                        {

                            // do hybrid touch with position as initial finger position if hasn't moved, and swipes aren't on
                            // otherwise exactly like in hold - for whichever swipe is held down

                            // hybrid touch
                            if (hasMoved == false && swipeRightOn == false && swipeLeftOn == false && swipeUpOn == false && swipeDownOn == false)
                            {
                                if (fingerInitialPositionX > screenCenterX)
                                {
                                    //  Debug.Log("Right");
                                    MovementController.HorMovement = 1f;
                                    //    isReducing = false;
                                    // if the touch position is to the right of center
                                    // move right
                                }
                                else if (fingerInitialPositionX < screenCenterX)
                                {
                                    MovementController.HorMovement = -1f;
                                    // Debug.Log("Leftt");
                                    // isReducing = false;
                                    // if the touch position is to the left of center
                                    // move left
                                }
                            }

                            // continue swipes as if held in same direction
                            if (swipeRightOn == true)
                            {
                                //swipe right is held
                                MovementController.HorMovement = 1;
                                Debug.Log("Swipe right is held");
                            }
                            else if (swipeLeftOn == true)
                            {
                                MovementController.HorMovement = -1;
                                //swipe left is held
                                Debug.Log("Swipe left is held");
                            }
                            else if (swipeUpOn == true)
                            {
                                // edit 3 - start boost loop
                                BoostInitiated = true;
                                MovementController.stopBoosting = false;
                                MovementController.InitboostLoop();

                                //swipe up is held
                                Debug.Log("Swipe up is held");
                            }
                            else if (swipeDownOn == true)
                            {
                                // edit 3 - start break loop
                                BreakPressed = true;
                                MovementController.stopBreaking = false;
                                MovementController.InitBreakLoop();

                                //swipe down is held
                                Debug.Log("Swipe down is held");
                            }
                        }
                    }

                    // set current as previous for next frame
                    previousPositionX = fingerMovedPositionX;
                    previousPositionY = fingerMovedPositionY;
                    previousmoveDirectionX = moveDirectionX;
                    previousmoveDirectionY = moveDirectionY;
                }
                // above code for touch phase moved modified extensively in arnav second edit

                //do things when touch has ended
                else if (FingerTouch.phase == TouchPhase.Ended)
                {
                    fingerEndPositionX = FingerTouch.position.x; //get the X position at the end, you may not need it unless you make gestures such as right and then left
                    fingerEndPositionY = FingerTouch.position.y; //get the Y position at the end, you may not need it unless you make gestures such as down and then up
                    fingerEndTime = Time.time; // arnav added
                    float timeOffset = fingerEndTime - fingerInitialTime; // added for edit 4

                    MovementController.HorMovement = 0;
                    // edit 3 - added
                    BoostInitiated = false;
                    BreakPressed = false;
                    MovementController.stopBoosting = true;
                    MovementController.stopBreaking = true;

                    // edit 4 - to check for and perform jump

                    // if its a tap
                    if (timeOffset < tapTime)
                    {
                        bool isDoubleTap = false;
                        if (checkForDoubleTap)
                        {
                            float doubleTapTimeOffset = Time.time - firstTapTime;
                            if (doubleTapTimeOffset < tapThresholdTime)
                            {
                                if (Input.touchCount == 1)
                                {
                                    if (FingerTouch.tapCount == 2 || isValidDoubleTap(FingerTouch.position.x, FingerTouch.position.y, firstTapPositionX, firstTapPositionY) == true)
                                    {
                                        MovementController.Jump();
                                        isDoubleTap = true;
                                        checkForDoubleTap = false;
                                    }
                                    else
                                    {
                                        StartCoroutine(OnDoubleTapFail());
                                    }
                                }
                                else
                                {
                                    StartCoroutine(OnDoubleTapFail());
                                }
                            }
                        }

                        // code for triggering above check and recording first tap
                        if (Input.touchCount == 1)
                        {
                            // will always trigger check for double tap except if it was the second tap
                            if (!isDoubleTap)
                            {
                                checkForDoubleTap = true;
                                firstTapTime = fingerEndTime;
                                firstTapPositionX = fingerInitialPositionX;
                                firstTapPositionY = fingerInitialPositionY;
                            }
                        }

                    } // end of edit 4 added code here


                    //now reset all booleans and do stuff at the end of all swipes - like despawning shields on release etc.
                    if (swipeRightOn == true)
                    {
                        swipeRightOn = false;
                        MovementController.HorMovement = 0;
                        Debug.Log("Swipe right released");
                    }
                    else if (swipeLeftOn == true)
                    {
                        MovementController.HorMovement = 0;
                        swipeLeftOn = false;
                        Debug.Log("Swipe left released");
                    }
                    else if (swipeUpOn == true)
                    {
                        BoostInitiated = false;
                        MovementController.stopBoosting = true; // added by Arnav edit 3
                        swipeUpOn = false;
                        Debug.Log("Swipe up released");
                    }
                    else if (swipeDownOn == true)
                    {
                        BreakPressed = false;
                        MovementController.stopBreaking = true; // added by Arnav edit 3
                        swipeDownOn = false;
                        Debug.Log("Swipe down released");
                    }
                }

                //else statement which makes it so you can hold down a swipe and keep things activated etc.
                else
                {
                    //get current position of touch
                    fingerHeldPositionX = FingerTouch.position.x;
                    fingerHeldPositionY = FingerTouch.position.y;
                    fingerHeldTime = Time.time; // arnav added
                    float timeOffset = fingerHeldTime - fingerInitialTime; // added for edit 4

                    // as per edit 4 - only do things if it is not a quick tap
                    if (timeOffset >= tapTime)
                    {

                        // just to clear a subtle loose end
                        checkForDoubleTap = false;

                        if (hasMoved == false && swipeRightOn == false && swipeLeftOn == false && swipeUpOn == false && swipeDownOn == false)
                        {
                            if (FingerTouch.position.x > screenCenterX)
                            {
                                //  Debug.Log("Right");
                                MovementController.HorMovement = 1f;
                                //    isReducing = false;
                                // if the touch position is to the right of center
                                // move right
                            }
                            else if (FingerTouch.position.x < screenCenterX)
                            {
                                MovementController.HorMovement = -1f;
                                // Debug.Log("Leftt");
                                // isReducing = false;
                                // if the touch position is to the left of center
                                // move left
                            }
                        }

                        if (swipeRightOn == true)
                        {
                            //swipe right is held
                            MovementController.HorMovement = 1;
                            Debug.Log("Swipe right is held");
                        }
                        else if (swipeLeftOn == true)
                        {
                            MovementController.HorMovement = -1;
                            //swipe left is held
                            Debug.Log("Swipe left is held");
                        }
                        else if (swipeUpOn == true)
                        {
                            // edit 3 - start boost loop
                            BoostInitiated = true;
                            MovementController.stopBoosting = false;
                            MovementController.InitboostLoop();

                            //swipe up is held
                            Debug.Log("Swipe up is held");
                        }
                        else if (swipeDownOn == true)
                        {
                            // edit 3 - start break loop
                            BreakPressed = true;
                            MovementController.stopBreaking = false;
                            MovementController.InitBreakLoop();

                            //swipe down is held
                            Debug.Log("Swipe down is held");
                        }

                        // sub initial position to make the new held position act like this
                        // as per edit 5

                        // virtually or directly needs to be decided - as per testing MAYBE
                        VirtualfingerSubInitialPositionX = fingerHeldPositionX;
                        VirtualfingerSubInitialPositionY = fingerHeldPositionY;
                        // fingerSubInitialPositionX = fingerHeldPositionX;
                        // fingerSubInitialPositionY = fingerHeldPositionY;

                        // also update previous position - just to close out any loose end
                        previousPositionX = fingerHeldPositionX;
                        previousPositionY = fingerHeldPositionY;
                        // as per edit 5

                    }
                }
            }
        }

        // if there are any touches currently

    }

    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        //No Movement at-all
        else
        {
            //Debug.Log("No Swipe!");
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    void OnSwipeUp()
    {
        Debug.Log("Swipe UP");
    }

    void OnSwipeDown()
    {
        Debug.Log("Swipe Down");
    }

    void OnSwipeLeft()
    {
        Debug.Log("Swipe Left");
        StartCoroutine(MoveSwipeLeft());
    }

    void OnSwipeRight()
    {
        Debug.Log("Swipe Right");
        StartCoroutine(MoveSwipeRight());
    }

    public IEnumerator MoveSwipeRight()
    {
        MovementController.HorMovement = 1f;
        yield return new WaitForSeconds(0.3f);
        MovementController.HorMovement = 0f;
    }

    public IEnumerator MoveSwipeLeft()
    {
        MovementController.HorMovement = -1f;
        yield return new WaitForSeconds(0.3f);
        MovementController.HorMovement = 0f;
    }

    // added by Arnav for edit 4
    public IEnumerator OnDoubleTapFail()
    {
        checkForDoubleTap = false;

        if (firstTapPositionX > screenCenterX)
        {
            //  Debug.Log("Right");
            MovementController.HorMovement = 1f;
            //    isReducing = false;
            // if the touch position is to the right of center
            // move right
        }
        else if (firstTapPositionX < screenCenterX)
        {
            MovementController.HorMovement = -1f;
            // Debug.Log("Leftt");
            // isReducing = false;
            // if the touch position is to the left of center
            // move left
        }

        yield return new WaitForSeconds(0.1f);
        if (swipeRightOn == false && swipeLeftOn == false)
        {
            MovementController.HorMovement = 0;
        }

    }

    // added for edit 4
    bool isValidDoubleTap(float secondTapPosX, float secondTapPosY, float firstTapPosX, float firstTapPosY)
    {
        float horizontalOffset = secondTapPosX - firstTapPosX;
        float verticalOffset = secondTapPosY - firstTapPosY;
        if (Mathf.Abs(horizontalOffset) < SWIPE_THRESHOLD && Mathf.Abs(verticalOffset) < SWIPE_THRESHOLD)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}