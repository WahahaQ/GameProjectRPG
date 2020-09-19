using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
#pragma warning disable 0649

	[SerializeField]
	private float movementSpeed = 5f;

#pragma warning restore 0649

	[System.NonSerialized]
	public Directions playerFacing;

	private SpriteRenderer spriteRenderer;
	private Rigidbody2D playerRigidbody;

	private Animator animatorComponent;
	private Vector2 movementVector;

	public enum Directions
	{
		NORTH,          // N	|	↑
		NORTH_EAST,     // NE	|	↗
		EAST,           // E	|	→
		SOUTH_EAST,     // SE	|	↘
		SOUTH,          // S	|	↓
		SOUTH_WEST,     // SW	|	↙
		WEST,           // W	|	←
		NORTH_WEST      // NW	|	↖
	};


	private void Start()
	{
		// Get all of the components
		playerRigidbody = GetComponent<Rigidbody2D>();
		animatorComponent = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (!Game.game.pauseMenu.isActive)
		{
			// Input
			movementVector.x = Input.GetAxisRaw(GameConstants.AXIS_HORIZONTAL);
			movementVector.y = Input.GetAxisRaw(GameConstants.AXIS_VERTICAL);

			// Set direction before normalization
			UpdatePlayerDirection(movementVector);
			movementVector = movementVector.normalized;
		}
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

		if (GameUtilities.CheckAnimatorParameter(animatorComponent, "IsRunning"))
		{
			animatorComponent.SetBool("IsRunning", true);
		}

		switch (movementVector.x)
		{
			case -1:
				playerFacing =
					movementVector.y == 0 ? Directions.WEST :
					movementVector.y > 0 ? Directions.NORTH_WEST : Directions.SOUTH_WEST;

				// Flip the character
				spriteRenderer.flipX = true;
				break;
			case 0:
				if (movementVector.y == 0)
				{
					if (GameUtilities.CheckAnimatorParameter(animatorComponent, "IsRunning"))
					{
						animatorComponent.SetBool("IsRunning", false);
					}
				}

				playerFacing = movementVector.y > 0 ? Directions.NORTH : playerFacing = Directions.SOUTH;
				break;
			case 1:
				playerFacing =
					movementVector.y == 0 ? Directions.EAST :
					movementVector.y > 0 ? Directions.NORTH_EAST : Directions.SOUTH_EAST;

				// Flip the character
				spriteRenderer.flipX = false;
				break;
		}
	}
}
