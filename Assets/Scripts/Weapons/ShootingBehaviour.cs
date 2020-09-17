using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

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
		if (Game.game.pauseMenu.isActive)
		{
			return;
		}

		attackTimer += Time.deltaTime;

		Inputs();
	}

	private void Inputs()
	{
		// Get mouse coordinates, relative to the point from screen space
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition = new Vector3(mousePosition.x, mousePosition.y, .0f);

		// Look at Mouse / Joystick
		Vector3 dir = transform.position - mousePosition;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		if (Input.GetMouseButtonDown(0) && (ejectionPort.position - mousePosition).magnitude > (ejectionPort.position - transform.position).magnitude)
		{
			// Shoot
			if (attackTimer >= attackRate)
			{
				attackTimer = 0.0f;
				Shoot();
				DropBulletCase();
			}
		}
	}

	private void Shoot()
	{
		// Spawn muzzle flash for a moment, then destroy
		GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
		Destroy(muzzleFlash, .05f);

		Vector3 currentMousePosition = mousePosition;

		GameObject projectile = Instantiate(bulletPrefab, transform.position, transform.rotation.normalized);
		Projectile projectileScript = projectile.GetComponent<Projectile>();

		// Set projectile's damage and shoot it forward
		projectileScript.damage = damage;
		projectileScript.projectileRigidbody.velocity = (mousePosition - transform.position).normalized * bulletSpeed;
		
		// Look at Mouse / Joystick
		Vector3 dir = transform.position - mousePosition;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
		projectileScript.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
