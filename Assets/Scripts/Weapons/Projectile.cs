using UnityEngine;

public class Projectile : MonoBehaviour
{
	public int damage;
	public float followSpeed;
	public bool hittable;
	public bool playerOwned, followPlayer;

	public Vector3 impactNormal;
	public Rigidbody2D projectileRigidbody;

#pragma warning disable 0649

	[SerializeField]
	private GameObject projectileDeathParticle;

#pragma warning restore 0649

	private bool stepLeft;
	private float stepTimer;
	private GameObject playerGameObject;

	private void Start()
	{
		// Get all of the components
		projectileRigidbody = GetComponent<Rigidbody2D>();

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
			if (!playerGameObject)
			{
				if (Game.game.playerShootingBehaviour)
				{
					playerGameObject = Game.game.playerGameObject;
				}
			}
			else
			{
				transform.position = Vector3.MoveTowards(transform.position, playerGameObject.transform.position, followSpeed * Time.deltaTime);
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

					Game.game.cameraShakeController.StartShake(.5f, .02f);
					DestroyProjectile();
					return;
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

			Game.game.cameraShakeController.StartShake(.3f, .01f);
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
