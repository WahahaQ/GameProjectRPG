using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
	[SerializeField]
	private float movementSpeed = 5f;

	//[System.NonSerialized]
	public Directions playerFacing;

	private Rigidbody2D playerRigidbody;
	private Vector2 movementVector;

	public enum Directions
	{
		EAST,
		NORTH,
		NORTH_EAST,
		NORTH_WEST,
		SOUTH,
		SOUTH_EAST,
		SOUTH_WEST,
		WEST
	};


	private void Start()
	{
		// Get player rigidbody component
		playerRigidbody = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		// Input
		movementVector.x = Input.GetAxisRaw(GameConstants.AXIS_HORIZONTAL);
		movementVector.y = Input.GetAxisRaw(GameConstants.AXIS_VERTICAL);

		// Set direction before normalization
		UpdatePlayerDirection(movementVector);
		movementVector = movementVector.normalized;
	}

	void FixedUpdate()
	{
		// Move player to certain position
		playerRigidbody.MovePosition(playerRigidbody.position + movementVector * movementSpeed * Time.fixedDeltaTime);
	}

	void UpdatePlayerDirection(Vector2 movementVector)
	{
		/* 
			This method handles incoming player input
			
			Directions:
			  ○ If (x=-1; y=-1)	- South_West
			  ○ If (x=-1; y=0)	- West
			  ○ If (x=-1; y=1)	- North West

			  ○ If (x=0; y=-1)	- South
			  ○ If (x=0; y=1)	- North

			  ○ If (x=1; y=-1)	- South_East
			  ○ If (x=1; y=0)	- East
			  ○ If (x=1; y=1)	- North_East
		*/

		//switch (movementVector.x)
		//{
		//	case -1:
		//		playerFacing =
		//			movementVector.y == 0 ? Directions.WEST :
		//			movementVector.y > 0 ? Directions.NORTH_WEST : Directions.SOUTH_WEST;
		//		break;
		//	case 1:
		//		playerFacing =
		//			movementVector.y == 0 ? Directions.EAST :
		//			movementVector.y > 0 ? Directions.NORTH_EAST : Directions.SOUTH_EAST;
		//		break;
		//	case 0:

		//		if(movementVector.y == 0)
		//		{
		//			break;
		//		}

		//		playerFacing = movementVector.y > 0 ? Directions.NORTH : playerFacing = Directions.SOUTH;
		//		break;
		//}

		switch (movementVector.x)
		{
			case -1:
				if (movementVector.y == 0)
				{
					playerFacing = Directions.WEST;
					transform.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f);
				}
				else if (movementVector.y > 0)
				{
					playerFacing = Directions.NORTH_WEST;
					transform.GetComponent<SpriteRenderer>().color = new Color(0f, 255f, 0f);
				}
				else
				{
					playerFacing = Directions.SOUTH_WEST;
					transform.GetComponent<SpriteRenderer>().color = new Color(0, 0f, 255f);
				}
				break;
			case 1:
				if (movementVector.y == 0)
				{
					playerFacing = Directions.EAST;
					transform.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 0f);
				}
				else if (movementVector.y > 0)
				{
					playerFacing = Directions.NORTH_EAST;
					transform.GetComponent<SpriteRenderer>().color = new Color(0f, 255f, 255f);
				}
				else
				{
					playerFacing = Directions.SOUTH_EAST;
					transform.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 255f);
				}
				break;
			case 0:

				if (movementVector.y == 0)
				{
					break;
				}

				if (movementVector.y > 0)
				{
					playerFacing = Directions.NORTH;
					transform.GetComponent<SpriteRenderer>().color = new Color(125f, 125f, 0f);
				}
				else
				{
					playerFacing = Directions.SOUTH;
					transform.GetComponent<SpriteRenderer>().color = new Color(0f, 125f, 125f);
				}
				break;
		}
	}
}
