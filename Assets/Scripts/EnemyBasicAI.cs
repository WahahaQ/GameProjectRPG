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

	private Rigidbody2D enemyRigidbody;
	private SpriteRenderer enemySpriteRenderer;

#pragma warning restore 0649

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

		//If the enemy has a target...
		if (target != null)
		{
			//Look at Target.
			Vector3 dir = transform.position - target.transform.position;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			//If the enemy is further then their attack range, chase the target.
			if (Vector3.Distance(transform.position, target.transform.position) > attackRange)
			{
				ChaseTarget();
			}
			//Otherwise attack.
			else
			{
				if (attackTimer >= attackRate)
				{
					attackTimer = 0.0f;
					Attack();
				}
			}
		}
		//Otherwise make the player the target.
		else
		{
			if (Game.game.playerShootingBehaviour)
			{
				target = Game.game.playerGameObject;
			}
			//If the player doesn't exist, freeze the enemy.
			else
			{
				enemyRigidbody.simulated = false;
			}
		}
	}

	//Called when the player attacks the enemy.
	public void TakeDamage(int dmg)
	{
		if (health - dmg <= 0)
		{
			Die();
		}
		else
		{
			health -= dmg;
			StartCoroutine(DamageFlash());
		}

		Game.game.Shake(0.15f, 0.15f, 30.0f);
	}

	//Move towards the target.
	private void ChaseTarget()
	{
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);
	}

	//If ranged enemy, shoot a projectile. If a melee enemy, hit the target.
	private void Attack()
	{
		if (type == EnemyType.Archer || type == EnemyType.Mage)
		{
			Shoot();
		}
		else if (type == EnemyType.Knight)
		{
			Melee();
		}
	}

	//Spawns and shoots respectable projectile.
	private void Shoot()
	{
		GameObject proj = Instantiate(projectilePrefab, transform.position + (transform.up * 0.7f), transform.rotation);
		Projectile projScript = proj.GetComponent<Projectile>();

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

	//Damages the player, with small knockback.
	private void Melee()
	{
		Game.game.playerHealthController.TakeDamage(damage);
		enemyRigidbody.AddForce((target.transform.position - transform.position).normalized * -3 * Time.deltaTime);
	}

	//Called when taking damage. Flashes sprite red.
	private IEnumerator DamageFlash()
	{
		enemySpriteRenderer.color = Color.red;
		yield return new WaitForSeconds(0.03f);
		enemySpriteRenderer.color = Color.white;
	}

	//Called when health reaches below 0. Destroys them.
	private void Die()
	{
		Game.game.curEnemies.Remove(gameObject);
		GameObject pe = Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
		Destroy(pe, 2);
		Destroy(gameObject);
	}
}

