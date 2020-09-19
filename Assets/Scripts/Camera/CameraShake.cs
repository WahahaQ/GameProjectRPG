using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraShake : MonoBehaviour
{
	private Transform cameraTransform;
	private Vector3 cameraOrigin;

	// Properties of the camera shake effect
	private float shakeTimeRemaining, shakeFadeTime;
	private float shakePower, shakeRotation;
	public float rotationMultiplier = 50f;

	public void Start()
	{
		InitializeCameraInstance();
	}

	public void ShakeCamera()
	{
		if (cameraTransform == null)
		{
			InitializeCameraInstance();
		}

		if (shakeTimeRemaining > 0)
		{
			shakeTimeRemaining -= Time.deltaTime;
			float xAmount = Random.Range(-1f, 1f) * shakePower;
			float yAmount = Random.Range(-1f, 1f) * shakePower;

			cameraTransform.transform.position += new Vector3(xAmount, yAmount, 0f);
			shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
			shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
		}

		// Shake camera via changing its rotation
		cameraTransform.rotation = Quaternion.Euler(0f, 0f, shakeRotation * Random.Range(-1f, 1f));
		cameraTransform.transform.position = cameraOrigin;
	}

	public void StartShake(float lenght, float power)
	{
		shakeTimeRemaining = lenght;
		shakePower = power;
		shakeFadeTime = power / lenght;
		shakeRotation = power * rotationMultiplier;
	}

	private void InitializeCameraInstance()
	{
		cameraTransform = GetComponent<Camera>().transform;
		cameraOrigin = cameraTransform.transform.position;
	}
}
