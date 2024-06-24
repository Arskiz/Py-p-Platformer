using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    PlayerMotor playerMotor;
    Weapon weapon;
    [SerializeField] float smoothVelocityX;
    [SerializeField] float smoothVelocityY;
    [SerializeField] public float maxSpeed;
    [SerializeField] float cameraYGapAmount;
    CameraInputHandler cameraInputHandler;
    // Start is called before the first frame update
    void Start()
    {
        playerMotor = FindAnyObjectByType<PlayerMotor>();
        weapon = FindAnyObjectByType<Weapon>();
        cameraInputHandler = FindAnyObjectByType<CameraInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraTranslate();
    }

    void CameraTranslate(){
        float smoothDampedPosX = Mathf.SmoothDamp(this.gameObject.transform.position.x, weapon.playerObject.transform.position.x, ref smoothVelocityX, maxSpeed);
        if(this.GetComponent<Camera>().fieldOfView < cameraInputHandler.minZoom)
        {
            float smoothDampedPosY = Mathf.SmoothDamp(this.gameObject.transform.position.y, weapon.playerObject.transform.position.y, ref smoothVelocityY, maxSpeed);
            SetCamPos(smoothDampedPosX, smoothDampedPosY);
        }
        else{
            float smoothDampedPosY = Mathf.SmoothDamp(this.gameObject.transform.position.y, weapon.playerObject.transform.position.y + cameraYGapAmount, ref smoothVelocityY, maxSpeed);
            SetCamPos(smoothDampedPosX, smoothDampedPosY);
            }
    }

    void SetCamPos(float targetPosX, float targetPosY){
        Vector3 newVec = new Vector3(targetPosX, targetPosY, this.gameObject.transform.position.z);
        this.gameObject.transform.position = newVec;
    }
}
