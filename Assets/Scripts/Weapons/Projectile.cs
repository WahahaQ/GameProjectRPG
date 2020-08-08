using UnityEngine;

public class Projectile : MonoBehaviour
{
	public bool playerOwned; //Does the player own this projectile?
	public bool hittable;
	public bool followPlayer;
	public float followSpeed;
	public int damage;
	public Rigidbody2D rig;
	private GameObject player;
	public Vector3 impactNormal;
	public GameObject projectileDeathParticle;

	//Side Stepping
	private bool stepLeft;
	private float stepTimer;

	private void Start()
	{
		if (!followPlayer)
			Destroy(gameObject, 3.0f);

		if (GetComponent<TrailRenderer>())
			GetComponent<TrailRenderer>().sortingLayerName = "Enemy";
	}

	private void Update()
	{
		//If the projectile follows the player, follow them.
		if (followPlayer)
		{
			if (!player)
			{
				if (Game.game.playerShootingBehaviour)
				{
					player = Game.game.playerGameObject;
				}
			}
			else
			{
				transform.position = Vector3.MoveTowards(transform.position, player.transform.position, followSpeed * Time.deltaTime);
			}
		}

		//Side Stepping
		if (name.Contains("Orb"))
		{
			//Used for the king's green orbs. Bounces side to side.
			if (stepTimer <= 0.0f)
			{
				stepTimer = Random.Range(0.5f, 1.0f);
				stepLeft = !stepLeft;
			}

			stepTimer -= Time.deltaTime;

			transform.position += (stepLeft ? transform.right : -transform.right) * 2.0f * Time.deltaTime;
		}
	}

	private void OnTriggerEnter(Collider hit)
	{
		if (hit.CompareTag("Enemy") || hit.CompareTag("Wall"))
		{
			projectileDeathParticle = Instantiate(projectileDeathParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
			Destroy(gameObject);
			return;
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		//If it's the player's projectile then just check for the enemy tag.
		if (playerOwned)
		{
			switch (col.gameObject.tag)
			{
				case "Enemy":

					if (!col.gameObject.name.Contains("King"))
					{
						col.gameObject.GetComponent<EnemyBasicAI>().TakeDamage(damage);
					}
					else
					{
						col.gameObject.GetComponent<FinalBoss>().TakeDamage(damage);
					}

					DestroyProjectile();

					break;
				case "Projectile":

					if (col.gameObject.GetComponent<Projectile>().hittable)
					{
						Destroy(col.gameObject);
						DestroyProjectile();
					}

					break;
				case "Wall":
					DestroyProjectile();
					break;
			}
		}
		else
		{
			if (col.gameObject.CompareTag("PlayerHitbox") || col.gameObject.CompareTag("Player"))
			{
				Game.game.playerHealthController.TakeDamage(damage);
				Destroy(gameObject);
			}
		}
	}

	private void DestroyProjectile()
	{
		DestroyProjectile(gameObject.transform.position);
	}

	private void DestroyProjectile(Vector2 position)
	{
		GameObject pe = Instantiate(projectileDeathParticle, position, Quaternion.identity);
		Destroy(gameObject);
		Destroy(pe, 0.2f);
	}
}
