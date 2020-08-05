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
		offset = transform.position - objectToFollow.transform.position;
	}

	private void LateUpdate()
	{
		transform.position = objectToFollow.transform.position + offset;
	}
}
