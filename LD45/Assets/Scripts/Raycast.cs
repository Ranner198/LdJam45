using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;
using UnityEngine;
using TMPro;

public class Raycast : MonoBehaviour
{
    public LayerMask lm;
    public Camera main;

    public TextMeshProUGUI headsupDisplay;
    public GameObject parentTransform;

    public FirstPersonController fpsController;

    public Vector3 playersPreviousPosition;
    public Quaternion playersPreviousQuaternion;
    public Quaternion playerCameraRotation;

    private Transform newPosition;
    private bool usingInteractiable = false;

    public TMP_InputField commandLine;

    void Update()
    {
        if (Physics.Raycast(main.transform.position, transform.forward, out RaycastHit hit, 3, lm) && !usingInteractiable)
        {
            headsupDisplay.text = "Press F2 to interact.";
            newPosition = hit.transform;
        }
        else if (!usingInteractiable)
        {
            headsupDisplay.text = "";
            newPosition = null;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (newPosition != null || usingInteractiable)
            {
                if (newPosition != null && newPosition.gameObject.GetComponent<DirectionIndex>() != null)
                {
                    UserDirection.instance.Updateindex(newPosition.gameObject.GetComponent<DirectionIndex>().index);
                }
                    
                StopAllCoroutines();

                usingInteractiable = !usingInteractiable;

                if (usingInteractiable)
                {
                    playersPreviousPosition = parentTransform.transform.position;
                    playersPreviousQuaternion = parentTransform.transform.rotation;
                    playerCameraRotation = main.transform.rotation;
                }
                if (usingInteractiable)
                {
                    headsupDisplay.text = "Press F2 to quit";
                    StartCoroutine(LerpToPosition(newPosition));
                }
                else
                {
                    StartCoroutine(LerpToPosition(playersPreviousPosition, playersPreviousQuaternion, playerCameraRotation, done => {
                        fpsController.enabled = true;
                        fpsController.m_MouseLook.XSensitivity = 2;
                        fpsController.m_MouseLook.YSensitivity = 2;
                        fpsController.m_MouseLook.SetCursorLock(true);
                        fpsController.m_MouseLook.UpdateCursorLock();
                        fpsController.Teleporting = false;
                    }));
                }
            }
        }
    }

    IEnumerator LerpToPosition(Transform newTransform)
    {
        float amt = 0;        

        fpsController.enabled = false;

        if (usingInteractiable)
        {
            fpsController.m_MouseLook.SetCursorLock(false);
            fpsController.m_MouseLook.UpdateCursorLock();
            yield return new WaitForEndOfFrame();
            fpsController.enabled = false;
        }

        commandLine.ActivateInputField(); //Re-focus on the input field
        commandLine.Select();//Re-focus on the input field

        while (amt < 1)
        {
            amt += Time.deltaTime / 2;
            parentTransform.transform.position = Vector3.Lerp(parentTransform.transform.position, newTransform.transform.position, amt);
            parentTransform.transform.rotation = Quaternion.Lerp(parentTransform.transform.rotation, newTransform.transform.rotation, amt);
            main.transform.rotation = Quaternion.Lerp(main.transform.rotation, newTransform.transform.rotation, amt);
            yield return new WaitForEndOfFrame();
        }

        parentTransform.transform.position = newTransform.transform.position;
        parentTransform.transform.rotation = newTransform.transform.rotation;
        main.transform.rotation = newTransform.transform.rotation;
    }

    IEnumerator LerpToPosition(Vector3 pos, Quaternion rot, Quaternion camRot, System.Action<bool> done)
    {
        float amt = 0;

        commandLine.DeactivateInputField();
        commandLine.ReleaseSelection();

        fpsController.m_MouseLook.XSensitivity = 0;
        fpsController.m_MouseLook.YSensitivity = 0;
        fpsController.Teleporting = true;

        Vector3 startPos = parentTransform.transform.position;
        Quaternion startRot = parentTransform.transform.rotation;
        Quaternion startCamRot = main.transform.rotation;

        while (amt <= .9f)
        {            
            parentTransform.transform.position = Vector3.Lerp(startPos, pos, amt);
            parentTransform.transform.rotation = Quaternion.Lerp(startRot, rot, amt);
            main.transform.rotation = Quaternion.Lerp(startCamRot, playerCameraRotation, amt);
            amt += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();

        parentTransform.transform.position = pos;
        parentTransform.transform.rotation = rot;
        main.transform.rotation = playerCameraRotation;

        yield return new WaitForSecondsRealtime(.2f);

        done(true);
    }
}

