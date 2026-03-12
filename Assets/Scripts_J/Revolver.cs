using UnityEngine;

public class Revolver : MonoBehaviour
{
    private MagManager magManager;

    private void Start()
    {
        magManager.InitMag(6);
    }


}
