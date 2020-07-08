using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	private float movementSpeed = 5f;		// Player movement speed

	[System.NonSerialized]
	public Directions playerFacing;			// Represents the current player facing direction

	private Rigidbody2D playerRigidbody;	// Player rigidbody component
	private Vector2 movementVector;			// Player movement vector

	private static Vector2 characterScale;	// Character default scale

	public enum Directions
	{
		NORTH,			// N	|	↑
		NORTH_EAST,		// NE	|	↗
		EAST,			// E	|	→
		SOUTH_EAST,		// SE	|	↘
		SOUTH,			// S	|	↓
		SOUTH_WEST,		// SW	|	↙
		WEST,			// W	|	←
		NORTH_WEST		// NW	|	↖
	};


	private void Start()
	{
		// Get player rigidbody component
		playerRigidbody = GetComponent<Rigidbody2D>();

		// Get characher scale
		characterScale = transform.localScale;
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
			  ○ If (x=-1; y=-1)	- South_West	|	↙
			  ○ If (x=-1; y=0)	- West			|	←
			  ○ If (x=-1; y=1)	- North West	|	↖

			  ○ If (x=0; y=-1)	- South			|	↓
			  ○ If (x=0; y=1)	- North			|	↑

			  ○ If (x=1; y=-1)	- South_East	|	↘
			  ○ If (x=1; y=0)	- East			|	→
			  ○ If (x=1; y=1)	- North_East	|	↗
		*/

		switch (movementVector.x)
		{
			case -1:
				playerFacing =
					movementVector.y == 0 ? Directions.WEST :
					movementVector.y > 0 ? Directions.NORTH_WEST : Directions.SOUTH_WEST;
				
				// Flip the character
				transform.localScale = new Vector2(characterScale.x * -1, transform.localScale.y);  ;
				break;
			case 0:
				if (movementVector.y == 0)	break;
				playerFacing = movementVector.y > 0 ? Directions.NORTH : playerFacing = Directions.SOUTH;
				break;
			case 1:
				playerFacing =
					movementVector.y == 0 ? Directions.EAST :
					movementVector.y > 0 ? Directions.NORTH_EAST : Directions.SOUTH_EAST;

				// Flip the character
				transform.localScale = new Vector2(characterScale.x, transform.localScale.y); ;
				break;
		}
	}
}
