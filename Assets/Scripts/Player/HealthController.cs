using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HealthController : MonoBehaviour
{
#pragma warning disable 0649

	[SerializeField]
	private Animator animatorComponent;

#pragma warning restore 0649

	public int currentHealth, maxHealth;
	
	public void TakeDamage(int damage)
	{
		if (currentHealth - damage <= 0)
		{
			Die();
		}
		else
		{
			currentHealth -= damage;
			//Game.game.Shake(0.1f, 0.1f, 50.0f);
			//Game.game.userInterface.ShakeSlider(0.2f, 0.05f, 30.0f);
			Game.game.userInterface.StartCoroutine("HealthDown", currentHealth);
			animatorComponent.SetTrigger("OnTakeHitTrigger");
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
		Game.game.userInterface.StartCoroutine("HealthDown", currentHealth);

		animatorComponent.SetTrigger("OnDeathTrigger");
		StartCoroutine(WaitForAnimation());

		Game.game.EndGame();
		Destroy(gameObject);
	}
}
