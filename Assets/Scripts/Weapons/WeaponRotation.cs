using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
#pragma warning disable 0649

	[SerializeField]
	private GameObject playerGameObject;

#pragma warning restore 0649

	private void FixedUpdate()
	{
		// Get the difference between mouse coordinates and transform position
		Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		difference.Normalize();

		float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
				
		if (rotationZ < -90 || rotationZ > 90)
		{
			// Rotate transform
			if (playerGameObject.transform.eulerAngles.y == 0)
			{
				transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
			}
			else if (playerGameObject.transform.eulerAngles.y == 180)
			{
				transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
			}
		}
	}
}
