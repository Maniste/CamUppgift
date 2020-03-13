using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCam : MonoBehaviour {


    [Header("Camera Properties")]
	public float CamDrift;
	public float CamSmoothing;
	public float SensitivityX;
	public float SensitivityY;
    public float CameraRadius;
    public bool HideCursor;

    [Header("Camera Movement")]
    public float DistanceFromPlayer;
    float TargetDistanceFromPlayer;
    public float DistanceFromPlayerY;
    public float distanceFromPlayerMax = 10, distanceFromPlayerMin = 2;
    public bool SetInverted;

    [Header ("Field of view properties")]
    public float FadeSpeed;
    public float ObjectTransparency; //How transparent should they be

    float TemporaryObjectTransparency;
    float FadeTeaTime = 1.0f;
    float TempFadeTeaTimeFadeIn;
    float CurrentX = 0.0f;
    float CurrentY = 0.0f;


	GameObject PlayerToFollow;
	Transform PlayerTransform;
    Vector3 PCamera;
	Vector3 CamOffSet;
	Vector3 Target;
    Collider previousWall;
    Collider CurrentWall;
    Collider[] CurrentWallArray;
    Color AlphaColor;
    Vector2 YMinMax = new Vector2(-50.0f, 60.0f);
	Vector3 RotationSmoothing;
	Vector3 CurrentCamRotation;
	// Use this for initialization
	void Start () {
		PlayerToFollow = GameObject.FindGameObjectWithTag ("Player");
		PlayerTransform = PlayerToFollow.transform;
        TempFadeTeaTimeFadeIn = FadeTeaTime;
	}

    void SetAlhpaToTransparent(RaycastHit hit) {
        AlphaColor = hit.collider.GetComponent<Renderer>().material.color;
        AlphaColor.a = 0.05f;
        AlphaColor.b = 0.7f;
    }
    void FadeOut()
    {
        if (FadeTeaTime > 0.5)
        {
            AlphaColor.a = FadeTeaTime;
            AlphaColor.b = FadeTeaTime;
            FadeTeaTime -= FadeSpeed * Time.deltaTime;
        }
    }
    void FadeIn() {
            AlphaColor.a = FadeTeaTime;
            AlphaColor.b = FadeTeaTime;
            FadeTeaTime += FadeSpeed * Time.deltaTime;
    }

    void CheckIfViewIsBlocked()
    {
        Vector3 StartPoint = transform.position;
        Vector3 Target = PlayerTransform.position;
        Vector3 TargetLocation = Target - StartPoint;
        int CamLayerMask = 1 << 9;
        //CamLayerMask = ~CamLayerMask;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(StartPoint, TargetLocation, DistanceFromPlayer, CamLayerMask);

        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];
            Collider ObjectBeingHitRightnow = hit.transform.GetComponent<Collider>();
            PropProperties PropScript = hit.transform.GetComponent<PropProperties>();
            if (PropScript)
            {
                PropScript.PassValues(FadeSpeed, FadeTeaTime, ObjectTransparency);
                PropScript.BlockingView = true;
                PropScript.IfRayIsStillThereCheck = 0.1f;
            }
          
        }
    }

   /* void CameraRunningIntoStuffFunctionThing() {
        int CameraMask = 1 << 9;
        CameraMask = ~CameraMask;
        RaycastHit colliding;
        Vector3 CameraCenter = transform.position;
        Vector3 CameraLeft = Vector3.left;
        Vector3 CameraRotationUp = Vector3.up;
        Vector3 CameraRotationForward = Vector3.forward;
        Quaternion q = Quaternion.AngleAxis(100 * Time.time, Vector3.up);
        Vector3 d = transform.forward * CameraRadius;

        if (Physics.Raycast(CameraCenter * 2, transform.TransformDirection(Vector3.forward), out colliding, CameraRadius, CameraMask))
        {
            DistanceFromPlayer += -0.1f;
            Debug.DrawRay(CameraCenter, q*d, Color.red);
        }
        else
            Debug.DrawRay(CameraCenter, q * d, Color.green);

    }*/
    // Update is called once per frame
    private void FixedUpdate()
    {
        CheckIfViewIsBlocked();
    }
    void LateUpdate () {
		if (Input.GetButtonDown ("Fire1") && HideCursor == true) {
			HideCursor = false;
		} else if (Input.GetButtonDown ("Fire1") && HideCursor == false) {
			HideCursor = true;
		}
		CurrentX += Input.GetAxis ("Mouse X") * SensitivityX;
		if (SetInverted == true) {
			CurrentY += Input.GetAxis ("Mouse Y") * SensitivityY;
		} else {
			CurrentY -= Input.GetAxis ("Mouse Y")* SensitivityY;
		}
		CurrentY = Mathf.Clamp (CurrentY, YMinMax.x, YMinMax.y);


		CurrentCamRotation = Vector3.SmoothDamp (CurrentCamRotation, new Vector3 (CurrentY, CurrentX), ref RotationSmoothing, CamSmoothing);
		//Vector3 TargetRotation = new Vector3 (CurrentY, CurrentX);
		transform.eulerAngles = CurrentCamRotation;
        transform.position = PlayerTransform.position - (transform.forward * DistanceFromPlayer) + (transform.up * DistanceFromPlayerY);

        //allows the player to scroll in and out
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			DistanceFromPlayer += 1.0f;
            DistanceFromPlayerY += 0.25f;
		} else if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			DistanceFromPlayer -= 1.0f;
            DistanceFromPlayerY -= 0.25f;
        }
		if (DistanceFromPlayer > distanceFromPlayerMax) {
			DistanceFromPlayer = distanceFromPlayerMax;
            DistanceFromPlayerY = 3.0f;
        } else if (DistanceFromPlayer < distanceFromPlayerMin) {
			DistanceFromPlayer = distanceFromPlayerMin;
            DistanceFromPlayerY = 1.0f;
		}

	}
}
