using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class FinalBoss : MonoBehaviour
{
	[Header("Properties:")]
	public int health;
	public float movementSpeed;
	public float attackRange;

	// Damage
	[Space]
	public int damageWeapon1;
	public int damageWeapon2;

	// Attack rate
	[Space]
	public float attackRateWeapon1;
	public float attackRateWeapon2;

	// Attack timer
	[Space]
	private float attackTimerWeapon1;
	private float attackTimerWeapon2;

	// Projectile speed
	[Space]
	public float projectileSpeedWeapon1;
	public float projectileSpeedWeapon2;

	// Weapons
	[Space]
	public GameObject projectileWeaponPrefab1;
	public GameObject projectileWeaponPrefab2;


#pragma warning disable 0649

	[Header("Components:")]

	[SerializeField]
	private GameObject deathParticleEffect;

#pragma warning restore 0649

	private Rigidbody2D enemyRigidbody;
	private SpriteRenderer enemySpriteRenderer;
	public GameObject target;
	
	// Upgrades
	private bool hasUpgradedWeapon1;
	private bool hasUpgradedWeapon2;

	// Muzzle points
	[Space]
	public Transform muzzleWeapon1;
	public Transform muzzleWeapon2;

	private void Start()
	{
		// Get all of the components
		enemyRigidbody = GetComponent<Rigidbody2D>();
		enemySpriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		attackTimerWeapon1 += Time.deltaTime;
		attackTimerWeapon2 += Time.deltaTime;

		if (target != null)
		{
			Act();
		}
		else
		{
			if (!Game.game.playerShootingBehaviour && !Game.game.gameDone)
			{
				if (target != null)
				{
					// Set the player as the target
					target = Game.game.playerGameObject;
				}
			}
			else
			{
				// Freeze enemies, if the player doesn't exist
				enemyRigidbody.simulated = false;
			}
		}

		Act(health);
	}

	private void Act()
	{
		// Look at the target
		Vector3 dir = transform.position - target.transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		// Chase the player if he's too far away
		if (Vector3.Distance(transform.position, target.transform.position) > attackRange)
		{
			ChaseTarget();
		}

		// Otherwise attack
		else
		{
			if (attackTimerWeapon1 >= attackRateWeapon1)
			{
				attackTimerWeapon1 = 0.0f;
				ShootArrow();
			}
		}

		if (attackTimerWeapon2 >= attackRateWeapon2)
		{
			attackTimerWeapon2 = 0.0f;
			ShootAndFollow();
		}
	}

	private void Act(int health)
	{
		// If low hp (less than 200) - increase attack rate and proj speed
		if (!hasUpgradedWeapon1)
		{
			if (health <= 200)
			{
				hasUpgradedWeapon1 = true;
				attackRateWeapon1 = 0.5f;
				projectileSpeedWeapon1 *= 1.5f;
			}
		}

		// If low hp (less than 100) - trigger burst attack once
		if (!hasUpgradedWeapon2)
		{
			if (health <= 100)
			{
				hasUpgradedWeapon2 = true;
				StartCoroutine(BurstAttack());
			}
		}
	}

	#region TakeDamage

	public void TakeDamage(int damage)
	{
		if (health - damage <= 0)
		{
			Die();
		}
		else
		{
			health -= damage;
			StartCoroutine(DamageFlash());
		}
	}

	private IEnumerator DamageFlash()
	{
		// Flash sprite (set the sprite color to red for a small duration of time)
		enemySpriteRenderer.color = Color.red;
		yield return new WaitForSeconds(0.03f);
		enemySpriteRenderer.color = Color.white;
	}

	private void Die()
	{
		// Destroy the enemy 
		Game.game.curEnemies.Remove(gameObject);
		GameObject pe = Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
		Destroy(pe, 2);
		Game.game.WinGame();
		Destroy(gameObject);
	}

	#endregion

	#region Attack

	private void ChaseTarget()
	{
		// Move towards the target
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);
	}

	private void ShootAndFollow()
	{
		GameObject proj = Instantiate(projectileWeaponPrefab2, muzzleWeapon2.position, transform.rotation);
		Projectile projScript = proj.GetComponent<Projectile>();

		// Set projectile's damage and make it follow the player
		projScript.damage = damageWeapon2;
		projScript.followSpeed = projectileSpeedWeapon2;
	}

	private IEnumerator BurstAttack()
	{
		// Burst a number of projectiles out
		for (int x = 0; x < 10; x++)
		{
			ShootAndFollow();
			yield return new WaitForSeconds(0.05f);
		}
	}

	private void ShootArrow()
	{
		GameObject proj = Instantiate(projectileWeaponPrefab1, muzzleWeapon1.position, transform.rotation);
		Projectile projScript = proj.GetComponent<Projectile>();

		// Set projectile's damage and shoot it forward
		projScript.damage = damageWeapon1;
		projScript.projectileRigidbody.velocity = (target.transform.position - muzzleWeapon1.position).normalized * projectileSpeedWeapon1;
	}

	#endregion
}
