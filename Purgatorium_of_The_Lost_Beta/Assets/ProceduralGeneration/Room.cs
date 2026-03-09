using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    Cinemachine.CinemachineBrain brainRef;
    const int defaultCameraOnlinePriority = 12;
    const int defaultCameraOfflinePriority = 10;
    [SerializeField] Cinemachine.CinemachineVirtualCamera cam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnEnterRoom();
        }
    }

    Cinemachine.CinemachineBrain GetBrain()
    {
        if (brainRef == null)
        {
            brainRef = FindObjectOfType<Cinemachine.CinemachineBrain>();
        }
        return brainRef;
    }

    void OnEnterRoom()
    {
        Cinemachine.CinemachineVirtualCamera currentActiveCam = (Cinemachine.CinemachineVirtualCamera)GetBrain().ActiveVirtualCamera;
        if (currentActiveCam != null && currentActiveCam != cam)
        {
            if (currentActiveCam.GetComponentInParent<Room>() != null)
            {
                currentActiveCam.Priority = defaultCameraOfflinePriority;
            }
        }
        cam.Priority = defaultCameraOnlinePriority;
    }


}
