using System.Collections;
using UnityEngine;

public class King : MonoBehaviour
{
	public int health;
	public int damage1;
	public int damage2;
	public float moveSpeed;
	public float attack1Rate;
	public float attack2Rate;
	private float attack1Timer;
	private float attack2Timer;
	public float attackRange;
	public float projectile1Speed;
	public float projectile2Speed;

	public GameObject target;
	public Rigidbody2D rig;
	public SpriteRenderer sr;
	public GameObject projectile1Prefab;
	public GameObject projectile2Prefab;

	private bool hasUpgraded1;
	private bool hasUpgraded2;

	public Transform muzzle1;
	public Transform muzzle2;

	public GameObject deathParticleEffect;


	private void Update()
	{
		attack1Timer += Time.deltaTime;
		attack2Timer += Time.deltaTime;

		//If the king has a target...
		if (target != null)
		{
			//Look at Target
			Vector3 dir = transform.position - target.transform.position;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			//Check to see if to follow or attack target
			if (Vector3.Distance(transform.position, target.transform.position) > attackRange)
			{
				ChaseTarget();
			}
			else
			{
				if (attack1Timer >= attack1Rate)
				{
					attack1Timer = 0.0f;
					ShootArrow();
				}
			}

			if (attack2Timer >= attack2Rate)
			{
				attack2Timer = 0.0f;
				ShootFollow();
			}
		}
		//Otherwise find the player and set that as the target.
		else
		{
			if (!Game.game.playerShootingBehaviour && !Game.game.gameDone)
			{
				if (target != null)
					target = Game.game.playerGameObject;
			}
			//If the player doesn't exist, then freeze the king.
			else
			{
				rig.simulated = false;
			}
		}

		//If the king's health is less than 200, increase attack rate and proj speed a single time.
		if (!hasUpgraded1)
		{
			if (health <= 200)
			{
				hasUpgraded1 = true;
				attack2Rate = 0.5f;
				projectile1Speed *= 1.5f;
			}
		}

		//If the king's health is less than 200, trigger burst attack once.
		if (!hasUpgraded2)
		{
			if (health <= 100)
			{
				hasUpgraded2 = true;
				StartCoroutine(BurstAttack());
			}
		}
	}

	//Called when the player's projectile hits the king. Takes damage.
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

	//Bursts a number of projectiles out.
	private IEnumerator BurstAttack()
	{
		for (int x = 0; x < 10; x++)
		{
			ShootFollow();
			yield return new WaitForSeconds(0.05f);
		}
	}

	//Moves towards the target.
	void ChaseTarget()
	{
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
	}

	//Spawns a projectile and shoots it forward.
	private void ShootArrow()
	{
		GameObject proj = Instantiate(projectile1Prefab, muzzle1.position, transform.rotation);
		Projectile projScript = proj.GetComponent<Projectile>();

		projScript.damage = damage1;

		projScript.rig.velocity = (target.transform.position - muzzle1.position).normalized * projectile1Speed;
	}

	//Spawns a projectile and makes it follow the player.
	private void ShootFollow()
	{
		GameObject proj = Instantiate(projectile2Prefab, muzzle2.position, transform.rotation);
		Projectile projScript = proj.GetComponent<Projectile>();

		projScript.damage = damage2;
		projScript.followSpeed = projectile2Speed;
	}

	//Called when the king takes damage. Flashes the sprite red quickly.
	private IEnumerator DamageFlash()
	{
		sr.color = Color.red;
		yield return new WaitForSeconds(0.03f);
		sr.color = Color.white;
	}

	//Called when the health reaches 0. Destroys the king and wins the game for the player.
	private void Die()
	{
		Game.game.curEnemies.Remove(gameObject);
		GameObject pe = Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
		Destroy(pe, 2);
		Game.game.WinGame();
		Destroy(gameObject);
	}
}
