using UnityEngine;

namespace Cubie.Game
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] float cameraScrollSpeed = 0.01f;
		[SerializeField] float cameraDragMultiplier = 1f;
		[SerializeField] float maxCameraShift = 100f;

		private Vector3 dragOrigin;

		private void Update()
		{
			MoveCamera();
		}

		private void MoveCamera()
		{
			float xpos = Input.mousePosition.x;
			float ypos = Input.mousePosition.y;
			Vector3 movement = new Vector3(0, 0, 0);

			if (Input.GetKey("a"))
			{
				movement.x += cameraScrollSpeed;
			}
			else if (Input.GetKey("d"))
			{
				movement.x -= cameraScrollSpeed;
			}
			else if (Input.GetKey("s"))
			{
				movement.y += cameraScrollSpeed;

			}
			else if (Input.GetKey("w"))
			{
				movement.y -= cameraScrollSpeed;
			}

			if (Input.GetMouseButtonDown(0))
			{
				dragOrigin = Input.mousePosition;
			}

			if (Input.GetMouseButton(0))
			{
				Vector3 pos = dragOrigin - Input.mousePosition;
				movement = new Vector3(pos.x * cameraDragMultiplier, pos.y * cameraDragMultiplier, 0);
				dragOrigin = Input.mousePosition;
				movement = Camera.main.ScreenToViewportPoint(movement);
			}

			movement = Camera.main.transform.TransformDirection(movement);

			//calculate desired camera position based on received input
			Vector3 origin = Camera.main.transform.position;
			Vector3 destination = origin;
			destination.x += movement.x;
			destination.y += movement.y;

			if (destination.y > maxCameraShift)
			{
				destination.y = maxCameraShift;
			}
			else if (destination.y < -maxCameraShift)
			{
				destination.y = -maxCameraShift;
			}

			//if a change in position is detected perform the necessary update
			if (destination != origin)
			{
				Camera.main.transform.position = destination;
			}
		}
	}
}
