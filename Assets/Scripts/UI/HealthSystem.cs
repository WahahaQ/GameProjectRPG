using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
#pragma warning disable 0649

	[SerializeField]
	private Image[] hearts;

	[Header("Heart sprites variants:")]
	[SerializeField]
	private Sprite heartFull;

	[SerializeField]
	private Sprite heartEmpty;

	[Space]
	[SerializeField]
	private int numberOfHearts;

#pragma warning restore 0649

	private int playerHealth;

	public void DecreaseHealth(int damage)
	{
		// Get the player's current health
		playerHealth = Game.game.playerHealthController.currentHealth - damage;

		for (int i = 0; i < hearts.Length; i++)
		{
			hearts[i].sprite = i < playerHealth ? heartFull : heartEmpty;
			hearts[i].enabled = i < numberOfHearts ? true : false;
		}
	}
}
