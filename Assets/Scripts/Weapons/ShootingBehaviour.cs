using UnityEngine;

public class ShootingBehaviour : MonoBehaviour
{
#pragma warning disable 0649

	[SerializeField]
	private GameObject bulletPrefab, muzzleFlashPrefab;

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
		//Using KEYBOARD & MOUSE as well as GAMEPAD inputs.
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);

		//Shooting
		if (Input.GetMouseButtonDown(0))
		{
			if (attackTimer >= attackRate)
			{
				attackTimer = 0.0f;
				Shoot();
			}
		}

		//Look at Mouse / Joystick
		Vector3 dir = transform.position - mousePosition;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	//Spawns a projectile and shoots it forward.
	private void Shoot()
	{
		GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
		Destroy(muzzleFlash, .05f);

		GameObject projectile = Instantiate(bulletPrefab, transform.position + (transform.up * 0.7f), transform.rotation);
		Projectile projectileScript = projectile.GetComponent<Projectile>();

		projectileScript.damage = damage;
		projectileScript.rig.velocity = (mousePosition - transform.position).normalized * bulletSpeed;
	}
}
