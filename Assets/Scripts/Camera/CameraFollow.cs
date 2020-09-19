using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
#pragma warning disable 0649

	[SerializeField]
	private GameObject objectToFollow;

#pragma warning restore 0649

	private Vector3 offset;

	private void Start()
	{
		// Set the offset coordinates
		offset = transform.position - objectToFollow.transform.position;
	}

	private void LateUpdate()
	{
		// Call the camera shake method before changing its position
		Game.game.cameraShakeController.ShakeCamera();
		transform.position = objectToFollow.transform.position + offset;	// Change position of the GameObject
	}
}
