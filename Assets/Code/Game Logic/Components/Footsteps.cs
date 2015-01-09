using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour 
{
	public float audioTimer;
	public AudioClip[] walkSounds;

	public CharacterController charCont;

	void Start () 
	{
		audioTimer = 0; 
		audio.volume = 0.2F;
	}

	void Update () 
	{
		if(audioTimer > 0)
			audioTimer -= charCont.velocity.magnitude / 6;
		
		if(audioTimer < 0)
			audioTimer = 0;
	}

	void OnControllerColliderHit(ControllerColliderHit col)
	{
		if(audioTimer == 0)
		{
			Debug.Log ("WALKING R GUD");
			if(Input.GetAxis("Horizontal") != 0 && charCont.isGrounded || Input.GetAxis("Vertical") != 0 && charCont.isGrounded)
			{
				int i = Random.Range(0, walkSounds.Length - 1);
				audio.clip = walkSounds[i];
				audio.PlayOneShot(walkSounds[i]);
				audioTimer = 20;
				
			}
		}
	}
}
