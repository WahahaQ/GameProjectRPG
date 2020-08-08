using UnityEngine;

[RequireComponent(typeof(Transform))]
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
		// Change camera position
		transform.position = objectToFollow.transform.position + offset;
	}
}
