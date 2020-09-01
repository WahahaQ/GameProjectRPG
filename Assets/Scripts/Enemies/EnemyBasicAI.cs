using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class EnemyBasicAI : MonoBehaviour
{
	public int health;
	public int damage;
	public float movementSpeed;
	public float attackRate, attackRange;
	public float projectileSpeed;

#pragma warning disable 0649

	[SerializeField]
	protected GameObject projectilePrefab;

	[SerializeField]
	protected GameObject deathParticleEffect;


#pragma warning restore 0649

	protected Animator animatorComponent;
	protected Rigidbody2D enemyRigidbody;
	protected SpriteRenderer enemySpriteRenderer;

	protected bool isDeathAnimationPlaying = false;

	public GameObject target;
	public EnemyType type;
	public enum EnemyType
	{
		Knight,
		Archer,
		Mage
	}

	protected float attackTimer;
	
	private void Start()
	{
		// Get all of the components
		animatorComponent = GetComponent<Animator>();
		enemyRigidbody = GetComponent<Rigidbody2D>();
		enemySpriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected void Update()
	{
		attackTimer += Time.deltaTime;

		if (target != null)
		{
			// Chase the player if he's too far away
			if (Vector3.Distance(transform.position, target.transform.position) > attackRange && !isDeathAnimationPlaying)
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
		// Flash sprite
		enemySpriteRenderer.color = Color.red;
		yield return new WaitForSeconds(0.03f);
		enemySpriteRenderer.color = Color.white;
	}

	public virtual void Die()
	{
		isDeathAnimationPlaying = true;
		enemyRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
		animatorComponent.SetBool("IsGoingToDie", true);
	}

	protected void DestroyGameObject()
	{
		Game.game.curEnemies.Remove(gameObject);
		Destroy(gameObject);
	}

	#endregion

	#region Attack

	protected virtual void ChaseTarget()
	{
		// Move towards the target
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);
	}

	protected virtual void Attack()
	{
		if (isDeathAnimationPlaying)
		{
			return;
		}

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

	protected virtual void Shoot()
	{
		GameObject proj = Instantiate(projectilePrefab, transform.position + (transform.up * 0.7f), transform.rotation);
		Projectile projScript = proj.GetComponent<Projectile>();
		
		// Set projectile's damage and shoot it forward
		projScript.damage = damage;

		if (type != EnemyType.Mage)
		{
			projScript.projectileRigidbody.velocity = (target.transform.position - transform.position).normalized * projectileSpeed;
		}
		else
		{
			projScript.followSpeed = projectileSpeed;
		}
	}

	protected virtual void Melee()
	{
		// Damage the player, with a small knockback
		Game.game.playerHealthController.TakeDamage(damage);
		enemyRigidbody.AddForce((target.transform.position - transform.position).normalized * -3 * Time.deltaTime);
	}

	#endregion
}

