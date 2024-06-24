using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CameraInputHandler : MonoBehaviour
{

    [SerializeField] float zoomIncrement;
    float curZoomVelocity;
    [SerializeField] float maxZoomSpeed;
    [SerializeField] public float maxZoom;
    [SerializeField] public float minZoom;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ZoomingHandler();
    }

    void ZoomingHandler()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            if(minZoom < this.GetComponent<Camera>().fieldOfView){
                float newFOV = Mathf.SmoothDamp(this.GetComponent<Camera>().fieldOfView, this.GetComponent<Camera>().fieldOfView - zoomIncrement, ref curZoomVelocity, maxZoomSpeed);
                this.GetComponent<Camera>().fieldOfView = newFOV;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            if(maxZoom > this.GetComponent<Camera>().fieldOfView){
                float newFOV = Mathf.SmoothDamp(this.GetComponent<Camera>().fieldOfView, this.GetComponent<Camera>().fieldOfView + zoomIncrement, ref curZoomVelocity, maxZoomSpeed);
                this.GetComponent<Camera>().fieldOfView = newFOV;
            }
            
        }
    }
}
