using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class EnemyBasicAI : MonoBehaviour
{
	public int health;
	public int damage;
	public float movementSpeed;
	public float attackRate, attackRange;
	public float projectileSpeed;

#pragma warning disable 0649

	[SerializeField]
	private GameObject projectilePrefab;

	[SerializeField]
	private GameObject deathParticleEffect;

#pragma warning restore 0649

	private Rigidbody2D enemyRigidbody;
	private SpriteRenderer enemySpriteRenderer;
	public GameObject target;

	public EnemyType type;
	public enum EnemyType
	{
		Knight,
		Archer,
		Mage
	}

	private float attackTimer;
	
	private void Start()
	{
		// Get enemy rigidbody component
		enemyRigidbody = GetComponent<Rigidbody2D>();

		// Get enemy sprite renderer component
		enemySpriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		attackTimer += Time.deltaTime;

		if (target != null)
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
				if (attackTimer >= attackRate)
				{
					attackTimer = 0.0f;
					Attack();
				}
			}
		}
		else
		{
			if (Game.game.playerShootingBehaviour)
			{
				// Set the player as the target
				target = Game.game.playerGameObject;
			}
			else
			{
				// Freeze enemies, if the player doesn't exist
				enemyRigidbody.simulated = false;
			}
		}
	}

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

		//Game.game.Shake(0.15f, 0.15f, 30.0f);	// Call the screen shake effect
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
		Destroy(gameObject);
	}

	private void ChaseTarget()
	{
		// Move towards the target
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);
	}

	private void Attack()
	{
		/*
			If ranged enemy - shoot a projectile.
			Otherwise - hit the target with the melee attack.
		*/
			
		switch (type)
		{
			case EnemyType.Archer:
			case EnemyType.Mage:
				Shoot();
				break;
			case EnemyType.Knight:
				Melee();
				break;
		}
	}

	private void Shoot()
	{
		GameObject proj = Instantiate(projectilePrefab, transform.position + (transform.up * 0.7f), transform.rotation);
		Projectile projScript = proj.GetComponent<Projectile>();
		
		// Set projectile's damage and shoot it forward
		projScript.damage = damage;

		if (type != EnemyType.Mage)
		{
			projScript.rig.velocity = (target.transform.position - transform.position).normalized * projectileSpeed;
		}
		else
		{
			projScript.followSpeed = projectileSpeed;
		}
	}

	private void Melee()
	{
		// Damage the player, with a small knockback
		Game.game.playerHealthController.TakeDamage(damage);
		enemyRigidbody.AddForce((target.transform.position - transform.position).normalized * -3 * Time.deltaTime);
	}
}

