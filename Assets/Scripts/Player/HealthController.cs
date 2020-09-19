using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HealthController : MonoBehaviour
{
	[Header("Health:")]
	public int maxHealth;
	public int currentHealth;

	private Animator animatorComponent;

	private void Start()
	{
		// Get all of the components
		animatorComponent = GetComponent<Animator>();
	}

	public void TakeDamage(int damage)
	{
		Game.game.healthSystemUI.DecreaseHealth(damage);

		if (currentHealth - damage <= 0)
		{
			Die();
		}
		else
		{
			currentHealth -= damage;

			if (GameUtilities.CheckAnimatorParameter(animatorComponent, "OnTakeHitTrigger"))
			{
				animatorComponent.SetTrigger("OnTakeHitTrigger");
			}

			StartCoroutine(WaitForAnimation());
		}
	}

	private IEnumerator WaitForAnimation()
	{
		yield return new WaitForSeconds(2);
	}

	private void Die()
	{
		currentHealth = 0;
		animatorComponent.SetTrigger("OnDeathTrigger");
		Game.game.EndGame();
		gameObject.SetActive(false);
	}
}
