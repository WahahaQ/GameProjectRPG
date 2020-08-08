using System.Collections;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour
{
#pragma warning disable 0649

	[SerializeField]
	private GameObject bulletPrefab, muzzleFlashPrefab;

	[SerializeField]
	private GameObject bulletCase;

	[SerializeField]
	private Animator bulletCaseAnimator;

	[SerializeField]
	private Transform ejectionPort;

#pragma warning restore 0649

	public int damage;
	public float attackRate, bulletSpeed;

	private Vector3 mousePosition;
	private float attackTimer;

	private void Update()
	{
		attackTimer += Time.deltaTime;

		Inputs();
	}

	private void Inputs()
	{
		// Get mouse coordinates, relative to the point from screen space
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition = new Vector3(mousePosition.x, mousePosition.y, .0f);
				
		if (Input.GetMouseButtonDown(0))
		{
			// Shoot
			if (attackTimer >= attackRate)
			{
				attackTimer = 0.0f;
				Shoot();
				DropBulletCase();
			}			
		}

		// Look at Mouse / Joystick
		Vector3 dir = transform.position - mousePosition;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	private void Shoot()
	{
		// Spawn muzzle flash for a moment, then destroy
		GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
		Destroy(muzzleFlash, .05f);

		GameObject projectile = Instantiate(bulletPrefab, transform.position + (transform.up * 0.7f), transform.rotation);
		Projectile projectileScript = projectile.GetComponent<Projectile>();

		// Set projectile's damage and shoot it forward
		projectileScript.damage = damage;
		projectileScript.rig.velocity = (mousePosition - transform.position).normalized * bulletSpeed;
	}

	private void DropBulletCase()
	{
		float x = transform.rotation.x;
		float y = transform.rotation.y;
		float z = Random.rotation.z;
		float w = transform.rotation.w;
		Quaternion rotation = new Quaternion(x, y, z, w);

		GameObject droppedBulletCase = Instantiate(bulletCase, ejectionPort.position, rotation);
		Destroy(droppedBulletCase, 4f);
	}
}
