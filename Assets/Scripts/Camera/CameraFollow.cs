using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform followTransform;		// An object that the camera will follow

	private void FixedUpdate()
	{
		this.transform.position = new Vector3(followTransform.position.x, followTransform.position.y, transform.position.z);
	}
}
