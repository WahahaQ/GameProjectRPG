using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
#pragma warning disable 0649

	[Space]
	[SerializeField]
	private OwnerType ownerType;

#pragma warning restore 0649

	private enum OwnerType
	{
		Player,
		Enemy
	}

	private void FixedUpdate()
	{
		switch (ownerType)
		{
			case OwnerType.Player:
				// Get the difference between mouse coordinates and transform position
				Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
				difference.Normalize();

				float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

				if (rotationZ < -90 || rotationZ > 90)
				{
					// Rotate transform
					if (Game.game.playerGameObject.transform.eulerAngles.y == 0)
					{
						transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
					}
					else if (Game.game.playerGameObject.transform.eulerAngles.y == 180)
					{
						transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
					}
				}
				break;
			case OwnerType.Enemy:
				GameObject target = Game.game.playerGameObject;
				Vector3 direction = target.transform.position - transform.position;	// Get target direction

				// Set transform rotation 
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		break;
		}
	}
}
