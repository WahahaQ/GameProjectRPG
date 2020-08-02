using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HealthController : MonoBehaviour
{
	[SerializeField]
	private Animator animatorComponent;

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
			Game.game.Shake(0.1f, 0.1f, 50.0f);
			Game.game.ui.ShakeSlider(0.2f, 0.05f, 30.0f);
			Game.game.ui.StartCoroutine("HealthDown", currentHealth);

			//StartCoroutine(WaitForAnimation(animatorComponent));
		}
	}

	//private IEnumerator WaitForAnimation(Animator playerAnimatorComponent)
	//{
	//	yield return WaitForAnimation(playerAnimatorComponent.);
	//}

	private void Die()
	{
		currentHealth = 0;
		Game.game.ui.StartCoroutine("HealthDown", currentHealth);

		animatorComponent.SetTrigger("OnDeathTrigger");
		//StartCoroutine(WaitForAnimation(animatorComponent));

		Game.game.EndGame();
		Destroy(gameObject);
	}
}
