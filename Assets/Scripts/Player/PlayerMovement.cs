using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	private float movementSpeed = 5f;		// Player movement speed

	[System.NonSerialized]
	public Directions playerFacing;			// Represents the current player facing direction

	private SpriteRenderer spriteRenderer;	// Player sprite renderer component
	private Rigidbody2D playerRigidbody;	// Player rigidbody component
	private Animator animatorComponent;		// Player animator component
	private Vector2 movementVector;			// Player movement vector

	private static Vector2 characterScale;  // Character default scale
	

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

		// Get player animator component
		animatorComponent = GetComponent<Animator>();

		// Get player sprite renderer component
		spriteRenderer = GetComponent<SpriteRenderer>();

		// Get the character scale
		characterScale = transform.localScale;
	}

	private void Update()
	{
		// Input
		movementVector.x = Input.GetAxisRaw(GameConstants.AXIS_HORIZONTAL);
		movementVector.y = Input.GetAxisRaw(GameConstants.AXIS_VERTICAL);

		// Set direction before normalization
		UpdatePlayerDirection(movementVector);
		movementVector = movementVector.normalized;
	}

	private void FixedUpdate()
	{
		// Move the player to a certain position
		playerRigidbody.MovePosition(playerRigidbody.position + movementVector * movementSpeed * Time.fixedDeltaTime);
	}

	private void UpdatePlayerDirection(Vector2 movementVector)
	{
		/* 
			This method handles incoming player input
			
			Directions:
			  ○ If (x=-1; y=-1)	- South West	|	↙
			  ○ If (x=-1; y=0)	- West			|	←
			  ○ If (x=-1; y=1)	- North West	|	↖

			  ○ If (x=0; y=-1)	- South			|	↓
			  ○ If (x=0; y=1)	- North			|	↑

			  ○ If (x=1; y=-1)	- South East	|	↘
			  ○ If (x=1; y=0)	- East			|	→
			  ○ If (x=1; y=1)	- North East	|	↗
		*/

		animatorComponent.SetBool("IsRunning", true);

		switch (movementVector.x)
		{
			case -1:
				playerFacing =
					movementVector.y == 0 ? Directions.WEST :
					movementVector.y > 0 ? Directions.NORTH_WEST : Directions.SOUTH_WEST;

				// Flip the character
				spriteRenderer.flipX = true;
				//transform.localScale = new Vector2(characterScale.x * -1, transform.localScale.y);  ;
				break;
			case 0:
				if (movementVector.y == 0)
				{
					animatorComponent.SetBool("IsRunning", false);
				}

				playerFacing = movementVector.y > 0 ? Directions.NORTH : playerFacing = Directions.SOUTH;
				break;
			case 1:
				playerFacing =
					movementVector.y == 0 ? Directions.EAST :
					movementVector.y > 0 ? Directions.NORTH_EAST : Directions.SOUTH_EAST;

				// Flip the character
				spriteRenderer.flipX = false;
				//transform.localScale = new Vector2(characterScale.x, transform.localScale.y); ;
				break;
		}
	}
}
