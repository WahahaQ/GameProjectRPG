using UnityEngine;

public class CameraShake : MonoBehaviour
{
#pragma warning disable 0649

	[SerializeField]
	private Transform cameraTransform;

#pragma warning restore 0649

	private float shakeTimeRemaining;
	private float shakePower;
	private float shakeFadeTime;
	private float shakeRotation;
	public float rotationMultiplier = 50f;

	private Vector3 cameraOrigin;

	private void LateUpdate()
	{
		if (cameraTransform == null)
		{
			cameraTransform = cameraTransform == null ? Camera.main.transform : cameraTransform;
			cameraOrigin = cameraTransform.transform.position;
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
}
