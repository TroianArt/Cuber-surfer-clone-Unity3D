using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public static PlayerMovementController _instance;

    [Header("General Variables")]
    public bool canMove = false;
    public float defMoveSpeed;
    public float decreasedMoveSpeed;
    float currentMoveSpeed;
    public float sensitivityMultiplier;
    public float deltaThreshold;
    Vector2 firstTouchPosition;
    float finalTouchX, finalTouchZ;
    Vector2 curTouchPosition;
    public float minXPos;
    public float maxXPos;
    public float mixZposWhenTurn;
    public float maxZposWhenTurn;
    public bool turn=false;
    int currentMultiplierZone = 1;

    [Header("References")]
    public PlayerCubeDetectorController playerCubeDetectorScript;
    public PlayerAnimatorController playerAnimScript;
    public CameraRotaterParent cameraRotaterParent;
    public PlayerVacuumController playerVacuumScript;
    Rigidbody rbPlayer;
    Camera mainCam;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        AttachReferences();
        ResetInputValues();

    } // Start()

    void Update()
    {
        if (canMove)
        {
            //HandleEndlessRun();
            HandleMovementWithSlide();
        }

    } // Update()

    private void FixedUpdate()
    {

        if (canMove)
        {
            HandleEndlessRun();
        }

    } // FixedUpdate()

    public void TriggerGameStarted()
    {
        canMove = true;

    } // TriggerGameStarted()

    void AttachReferences()
    {
        rbPlayer = GetComponent<Rigidbody>();
        mainCam = Camera.main;
        currentMoveSpeed = defMoveSpeed;

    } // AttachReferences()

    void ResetInputValues()
    {
        rbPlayer.velocity = new Vector3(0f, rbPlayer.velocity.y, rbPlayer.velocity.z);
        firstTouchPosition = Vector2.zero;
        finalTouchX = 0f;
        curTouchPosition = Vector2.zero;

    } // ResetInputValues()

    void HandleEndlessRun()
    {
        if (!turn)
        {
            rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, rbPlayer.velocity.y, 600f * Time.fixedDeltaTime);
        }
        else rbPlayer.velocity = new Vector3(-currentMoveSpeed * Time.fixedDeltaTime, rbPlayer.velocity.y, rbPlayer.velocity.z);
    } // HandleEndlessRun()

    void HandleMovementWithSlide()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            curTouchPosition = Input.mousePosition;
            Vector2 touchDelta = (curTouchPosition - firstTouchPosition);

            if (firstTouchPosition == curTouchPosition)
            {
                rbPlayer.velocity = new Vector3(0f, rbPlayer.velocity.y, rbPlayer.velocity.z);
            }

           
            finalTouchX = transform.position.x;

            if (Mathf.Abs(touchDelta.x) >= deltaThreshold)
            {
                finalTouchX = (transform.position.x + (touchDelta.x * sensitivityMultiplier));
            }

            rbPlayer.position = new Vector3(finalTouchX, transform.position.y, transform.position.z);
            rbPlayer.position = new Vector3(Mathf.Clamp(rbPlayer.position.x, minXPos, maxXPos), rbPlayer.position.y, rbPlayer.position.z);

            firstTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            ResetInputValues();
        }

    } // HandleMovementWithSlide()

    public void TriggerLevelFinished()
    {
        canMove = false;

    } // TriggerLevelFinished()

    public void TriggerCubeLostForSpeedDecrease()
    {
        CheckHasCubesForDeath();

        if (DOTween.IsTweening("PlayerSpeedDecreaseTween"))
        {
            DOTween.Kill("PlayerSpeedDecreaseTween", true);
        }

        if (DOTween.IsTweening("PlayerSpeedIncreaseTween"))
        {
            DOTween.Kill("PlayerSpeedIncreaseTween", true);
        }

        //DOTween.To(() => currentMoveSpeed, x => currentMoveSpeed = x, decreasedMoveSpeed, 0.1f).SetId("PlayerSpeedDecreaseTween").OnComplete(() =>
        //{

        //    DOTween.To(() => currentMoveSpeed, x => currentMoveSpeed = x, defMoveSpeed, 0.1f).SetId("PlayerSpeedIncreaseTween");
        //});

    } // TriggerCubeLostForSpeedDecrease()

    void CheckHasCubesForDeath()
    {
        playerCubeDetectorScript.CheckHasCubeLeft();

    } // CheckHasCubesForDeath()

    public void TriggerMovementStopped()
    {
        canMove = false;
        rbPlayer.velocity = Vector3.zero;

    } // TriggerMovementStopped()

    public void TriggerMovementStoppedWithDeath()
    {
        TriggerMovementStopped();
        playerAnimScript.TriggerRagDollDeath();
        CameraController._instance.cameraState = CameraController.CameraStates.OnSuccessFinish;
        AudioManager._instance.MuteSFX(false);

    } // TriggerMovementStoppedWithDeath()

    public void TriggerFinishFloorUpperPart()
    {
        if (playerCubeDetectorScript.CheckHasOneCubeLeftForSuccessFinish())
        {
            TriggerMovementStopped();
            GameManager._instance.TriggerLevelSuccessed(currentMultiplierZone);
            playerAnimScript.TriggerDance();
            cameraRotaterParent.canRotate = true;
            CameraController._instance.TriggerLevelSuccessFinished(cameraRotaterParent.transform);
            VFXManager._instance.StartConfettiLoop(transform);
        }
        else
        {
            if (DOTween.IsTweening("PlayerFinishFloorUpperTween"))
            {
                DOTween.Kill("PlayerFinishFloorUpperTween");
            }

            rbPlayer.DOMoveY(rbPlayer.position.y + 1.15f, 0.1f).SetId("PlayerFinishFloorUpperTween");
        }

    } // TriggerFinishFloorUpperPart()

    public void TurnLeft()
    {

        //var camPos = mainCam.transform.position;
        //var playerPos = this.transform.position;
        //Debug.Log("First:");
        //Debug.Log(playerPos.z);
        //Debug.Log(playerPos.z + (camPos.x - playerPos.x));
        //mainCam.transform.position = new Vector3(playerPos.x+(playerPos.z-camPos.z), camPos.y, playerPos.z + (camPos.x - playerPos.x));
        //this.transform.Rotate(new Vector3(0f, -90f, 0f));
        //mainCam.transform.Rotate(new Vector3(0f, -90f, 0f), Space.World);
        //Debug.Log("SECOND");
        //Debug.Log(mainCam.transform.position.z);
        //turn = true;

    }
    public void TurnRight()
    {
        //var offset = new Vector3(4.5f, 6, -9);
        //StartCoroutine(RotateCamera(offset, 2f));
        this.transform.Rotate(new Vector3(0f, 90f, 0f));
        mainCam.transform.Rotate(new Vector3(0f, 90f, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Killzone"))
        {
            AudioManager._instance.MuteSFX(false);
            Destroy(gameObject);
        }

        if (other.CompareTag("FinishFloor"))
        {
            currentMultiplierZone = other.gameObject.GetComponent<FinishFloorController>().currentMultiplierZone;
        }

        if (other.CompareTag("LatestStopperColl"))
        {
            currentMultiplierZone = 10;

            TriggerMovementStopped();
            GameManager._instance.TriggerLevelSuccessed(currentMultiplierZone);
            playerAnimScript.TriggerDance();
            cameraRotaterParent.canRotate = true;
            CameraController._instance.TriggerLevelSuccessFinished(cameraRotaterParent.transform);
            VFXManager._instance.StartConfettiLoop(transform);

            AudioManager._instance.MuteSFX(false);
        }

        if (other.CompareTag("MagnetCollectable"))
        {
            Destroy(other.gameObject);
            playerVacuumScript.TriggerMagnetActive();
        }
        if (other.CompareTag("LeftTurn"))
        {
            TurnLeft();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("RightTurn"))
        {
            TurnRight();
            Destroy(other.gameObject);
        }

    }
    public void RePosition(Vector3 offset)
    {
        Vector3 desiredPosition = this.transform.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, 1.5f * Time.deltaTime);
        transform.position = smoothPosition;
    }
    private IEnumerator RotateCamera(Vector3 offset, float duration)
    {

        Vector3 desiredPosition = this.transform.position + offset;
        var t = 0f;

        while (t <= 1f)
        {
            Vector3 smoothPosition = Vector3.Lerp(mainCam.transform.position, desiredPosition, 1.5f * Time.deltaTime);
            mainCam.transform.position = smoothPosition;
            yield return null;
        }
        mainCam.transform.position = desiredPosition;
    }
} 
