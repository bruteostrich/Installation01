using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour 
{
	public static Weapon instance; 
	public WeaponStats weapon;
	public WeaponType weaponType;

	public bool fullReload; 
	public bool isReloading;
	public bool isMelee;

	public GameObject[] hitParticles;
	public GameObject[] hitHoles; 

	public Transform cameraHolder;
	public LayerMask hitlayers; 

	[HideInInspector]
	public int ammoHolder;

	void Start () 
	{
		instance = this; 
		fullReload = false;
		isReloading = false;
		isMelee = false; 

		weapon.fireRateCooler = 0; 

		weapon.bulletsPerMag = weapon.bulletsPerMagStart; 
		weapon.spareBullets = weapon.spareBulletsStart;
	}

	void Update () 
	{
		if(weapon.fireRateCooler > 0)
			weapon.fireRateCooler -= Time.deltaTime;

		if(weapon.fireRateCooler < 0)
			weapon.fireRateCooler = 0;

		if(weapon.hasAmmoCounter)
		{
			if(weapon.bulletsPerMag > 9)
				weapon.ammoCounter.text = weapon.bulletsPerMag.ToString ();
			else if(weapon.bulletsPerMag > 0)
				weapon.ammoCounter.text = "0" + weapon.bulletsPerMag.ToString ();
			else
				weapon.ammoCounter.text = "00";
		}

		if(Input.GetMouseButton(0) && weapon.bulletsPerMag > 0 && weapon.fireRateCooler == 0 && !isReloading && !isMelee && weaponType == WeaponType.Auto)
			Fire(); 

		if(Input.GetMouseButtonDown(0) && weapon.bulletsPerMag > 0 && weapon.fireRateCooler == 0 && !isReloading && !isMelee && weaponType == WeaponType.Threeround)
		{
			Invoke("Fire", 0.08f);
			Invoke("Fire", 0.16f);
			Invoke("Fire", 0.24f);
		}

		if(Input.GetKeyDown(KeyCode.F) && !isReloading && !isMelee)
			Melee ();

		if(Input.GetKeyDown(KeyCode.R) && weapon.bulletsPerMag != 0 && isReloading == false && weapon.bulletsPerMag != weapon.bulletsPerMagStart && weapon.spareBullets != 0)
		{
			StartCoroutine(Reload());
			ammoHolder = weapon.bulletsPerMag;
			fullReload = false;
			isReloading = true;
		}
	}

	void Melee()
	{
		isMelee = true; 
		RaycastHit hit;

		animation.Rewind("Melee");
		animation.Play("Melee");

		isReloading = false;
		StartCoroutine (MeleeTimer ());

		weapon.cameraKickback += new Vector3(weapon.cameraRotation.x, Random.Range(-0.2f, 0.2f));
		audio.PlayOneShot (weapon.melee);

		if(Physics.Raycast(weapon.bulletSpawn.position, weapon.bulletSpawn.forward, out hit, 1.4f, hitlayers.value))
		{
			Instantiate(hitParticles[0],hit.point,Quaternion.FromToRotation(Vector3.up, hit.normal));
			Instantiate(hitHoles[0], hit.point  + hit.normal * 0.04f, Quaternion.FromToRotation(-hit.normal, -Vector3.forward));
			audio.PlayOneShot(weapon.meleeHit);

	                if (hit.collider.transform.root.tag == "Player")
	                {
		                PhotonView photonview = hit.collider.transform.root.GetComponent<PhotonView>();
		                if (photonview.isMine)
		                    return;
	
	        		photonview.RPC ("GetHit", PhotonTargets.AllBufferedViaServer, 50);
	                }
		}
	}

	IEnumerator MeleeTimer()
	{
		yield return new WaitForSeconds (1);
		isMelee = false; 
	}

	void FixedUpdate()
	{
		CameraKick ();
	}

	void CameraKick()
	{
		weapon.cameraKickback = Vector3.Lerp(weapon.cameraKickback, Vector3.zero, 0.4f);
		weapon.cameraKickbackOne = Vector3.Lerp(weapon.cameraKickbackOne, weapon.cameraKickback, 0.4f);

		cameraHolder.localEulerAngles = weapon.cameraKickbackOne * 40.2f;	
	}

	void Fire()
	{
		Vector3 direction  = weapon.bulletSpawn.forward;
		direction.x += Random.Range(-weapon.spread, weapon.spread);
		direction.y += Random.Range(-weapon.spread, weapon.spread);
		direction.z += Random.Range(-weapon.spread, weapon.spread);

		weapon.fireRateCooler = weapon.fireRate;
		weapon.bulletsPerMag--;
		audio.PlayOneShot (weapon.fire);

		weapon.cameraKickback += new Vector3(weapon.cameraRotation.x, Random.Range(-weapon.cameraRotation.y, weapon.cameraRotation.y));
		Instantiate(weapon.muzzleflash, weapon.muzzleFlashSpawn.position, weapon.muzzleFlashSpawn.rotation); 

		RaycastHit hit;
		
		if(Physics.Raycast(weapon.bulletSpawn.position, direction, out hit, 800, hitlayers.value))
		{
			Instantiate(hitParticles[0],hit.point,Quaternion.FromToRotation(Vector3.up, hit.normal));
			Instantiate(hitHoles[0], hit.point  + hit.normal * 0.04f, Quaternion.FromToRotation(-hit.normal, -Vector3.forward));

	                if (hit.collider.transform.root.tag == "Player")
	                {
		                PhotonView photonview = hit.collider.transform.root.GetComponent<PhotonView>();
		                if (photonview.isMine)
		                    return;
		
		                photonview.RPC("GetHit", PhotonTargets.AllBufferedViaServer, weapon.damage);
	                }
		}

		if(weapon.bulletsPerMag > 0)
		{
			animation.Rewind ("Fire");
			animation.Play ("Fire");
		}

		if(weapon.bulletsPerMag <= 0 && weapon.spareBullets > 0)
		{
			fullReload = true;
			isReloading = true; 
			weapon.bulletsPerMag = 0; 
			StartCoroutine(Reload ());
		}
	}

	public IEnumerator Reload()
	{
		audio.PlayOneShot(weapon.reload);
		animation.Rewind ("Reload");
		animation.Play ("Reload");

		yield return new WaitForSeconds(weapon.reloadTime);
		isReloading = false; 

		if(weapon.spareBullets != 0 && fullReload == false && weapon.spareBullets > (weapon.bulletsPerMagStart - ammoHolder))
		{
			weapon.bulletsPerMag = weapon.bulletsPerMagStart;
			weapon.spareBullets -= (weapon.bulletsPerMagStart - ammoHolder);
		}
		else if (weapon.spareBullets != 0 && fullReload == true && weapon.spareBullets >= (weapon.bulletsPerMagStart - ammoHolder))
		{
			weapon.bulletsPerMag = weapon.bulletsPerMagStart;
			weapon.spareBullets -= weapon.bulletsPerMagStart;
		}
		else if(weapon.spareBullets < (weapon.bulletsPerMagStart - ammoHolder))
		{
			weapon.bulletsPerMag += weapon.spareBullets;
			weapon.spareBullets = 0;
		}
	}

	public IEnumerator DrawGun()
	{
		audio.PlayOneShot(weapon.draw);
		if(weapon.bulletsPerMag == 0 && isReloading == false && weapon.spareBullets != 0)
		{
			StartCoroutine(Reload());
			ammoHolder = weapon.bulletsPerMag;
			fullReload = true;
			isReloading = true;
		}

		isMelee = true;
		yield return new WaitForSeconds(1);
		isMelee = false; 
	}
	
	void OnGUI()
	{
		GUI.Box (new Rect (10, 10, 100, 23), weapon.bulletsPerMag.ToString () + " / " + weapon.spareBullets.ToString());
	}
}

[System.Serializable]
public class WeaponStats
{
	public string weaponName; 

	public float fireRate;
	public float spread; 
	[HideInInspector]
	public float fireRateCooler;
	public float damage; 
	[HideInInspector]
	public int bulletsPerMag;
	public int bulletsPerMagStart;
	[HideInInspector]
	public int spareBullets;
	public int spareBulletsStart;
	public float reloadTime;

	public Vector3 cameraRotation;
	public Vector3 cameraKickback;
	public Vector3 cameraKickbackOne;

	public Transform bulletSpawn; 
	public Transform muzzleFlashSpawn; 

	public GameObject muzzleflash; 
	public bool hasAmmoCounter;

	public TextMesh ammoCounter; 

	public AudioClip fire;
	public AudioClip reload;
	public AudioClip draw;
	public AudioClip melee; 
	public AudioClip meleeHit;
}

public enum WeaponType
{
	Auto,
	Threeround
}
