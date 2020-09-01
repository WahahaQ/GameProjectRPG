using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
#pragma warning disable 0649

	[SerializeField]
	private Image[] hearts;

	[SerializeField]
	private Sprite heartFull, heartEmpty;

#pragma warning restore 0649

	public int numberOfHearts;
	private int playerHealth;

	public void DecreaseHealth(int damage)
	{
		playerHealth = Game.game.playerHealthController.currentHealth - damage;

		for (int i = 0; i < hearts.Length; i++)
		{
			hearts[i].sprite = i < playerHealth ? heartFull : heartEmpty;
			hearts[i].enabled = i < numberOfHearts ? true : false;
		}
	}
}
