using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
	public EnemyType type;

	public int health;
	public int damage;
	public float moveSpeed;
	public float attackRate;
	private float attackTimer;
	public float attackRange;
	public float projectileSpeed;

	public GameObject target;
	public Rigidbody2D rig;
	public SpriteRenderer sr;
	public GameObject projectilePrefab;

	public GameObject deathParticleEffect;

	void Update ()
	{
		attackTimer += Time.deltaTime;

		//If the enemy has a target...
		if(target != null)
		{
			//Look at Target.
			Vector3 dir = transform.position - target.transform.position;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			//If the enemy is further then their attack range, chase the target.
			if(Vector3.Distance(transform.position, target.transform.position) > attackRange)
			{
				ChaseTarget();
			}
			//Otherwise attack.
			else
			{
				if(attackTimer >= attackRate)
				{
					attackTimer = 0.0f;
					Attack();
				}
			}
		}
		//Otherwise make the player the target.
		else
		{
			if(Game.game.playerShootingBehaviour)
			{
				target = Game.game.playerGameObject;
			}
			//If the player doesn't exist, freeze the enemy.
			else
			{
				rig.simulated = false;
			}
		}
	}

	//Move towards the target.
	void ChaseTarget ()
	{
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
	}

	//If ranged enemy, shoot a projectile. If a melee enemy, hit the target.
	void Attack ()
	{
		if(type == EnemyType.Archer || type == EnemyType.Mage)
		{
			Shoot();
		}
		else if(type == EnemyType.Knight)
		{
			Melee();
		}
	}

	//Spawns and shoots respectable projectile.
	void Shoot ()
	{
		GameObject proj = Instantiate(projectilePrefab, transform.position + (transform.up * 0.7f), transform.rotation);
		Projectile projScript = proj.GetComponent<Projectile>();

		projScript.damage = damage;

		if(type != EnemyType.Mage)
		{
			projScript.rig.velocity = (target.transform.position - transform.position).normalized * projectileSpeed;
		}
		else
		{
			projScript.followSpeed = projectileSpeed;
		}
	}

	//Damages the player, with small knockback.
	void Melee ()
	{
		Game.game.playerHealthController.TakeDamage(damage);
		rig.AddForce((target.transform.position - transform.position).normalized * -3 * Time.deltaTime);
	}

	//Called when the player attacks the enemy.
	public void TakeDamage (int dmg)
	{
		if(health - dmg <= 0)
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

	//Called when taking damage. Flashes sprite red.
	IEnumerator DamageFlash ()
	{
		sr.color = Color.red;
		yield return new WaitForSeconds(0.03f);
		sr.color = Color.white;
	}

	//Called when health reaches below 0. Destroys them.
	void Die ()
	{
		Game.game.curEnemies.Remove(gameObject);
		GameObject pe = Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
		Destroy(pe, 2);
		Destroy(gameObject);
	}
}

public enum EnemyType {Knight, Archer, Mage}