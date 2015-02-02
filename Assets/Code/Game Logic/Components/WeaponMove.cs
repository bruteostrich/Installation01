using UnityEngine;
using System.Collections;

public class WeaponMove : MonoBehaviour 
{
	public GameObject gunHolder;
	
	public Vector3 gunPosOrigin;
	public Vector3 gunPosNew;
	
	public float movement;
	//public float smoothGun;
	//public float tiltAngle;
	
	void Start () 
	{
		gunPosOrigin = gunHolder.transform.localPosition;
	}
	
	void Update () 
	{	
		float moveOnX = - Input.GetAxis("Mouse X") * Time.deltaTime * movement;
		float moveOnY = - Input.GetAxis("Mouse Y") * Time.deltaTime * movement;
		
		//float gunTilt = Input.GetAxis("Horizontal") * tiltAngle;
		//Quaternion target = Quaternion.Euler(0, 0, -gunTilt);
		
		gunPosNew = new Vector3 (gunPosOrigin.x + moveOnX, gunPosOrigin.y + moveOnY, gunPosOrigin.z);
		gunHolder.transform.localPosition = Vector3.Lerp (gunHolder.transform.localPosition, gunPosNew, 5 * Time.deltaTime);
		
		//transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * smoothGun);
		
	}
}