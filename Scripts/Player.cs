using Godot;

public partial class Player : CharacterBody2D
{
	private AnimatedSprite2D _walk;
	private int _currentFrame = 1; //Facing down Idle animation
	private float _animationTimer = 0f;
	private const float ANIMATION_SPEED = 0.333f; // FPS
	private int _frameDirection = 1;
	
	// Start frames for each direction
	private const int DOWN_START = 0;
	private const int LEFT_START = 3;
	private const int UP_START = 6;
	private const int RIGHT_START = 9;
	
	public override void _Ready()
	{
		_walk = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_walk.Frame = _currentFrame;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		MoveAndSlide();
		
		if (direction != Vector2.Zero)
		{
			// Get which starting frame to use
			int directionStart = GetDirectionStart(direction);
			
			_animationTimer += (float)delta;
			
			if (_animationTimer >= ANIMATION_SPEED)
			{
				_animationTimer = 0f;
				
				int relativeFrame = _currentFrame - directionStart;
				
				if (relativeFrame < 0 || relativeFrame > 2)
				{
					_currentFrame = directionStart + 1;
					_frameDirection = 1;
				}
				
				else
				{
					relativeFrame += _frameDirection;
					
					if (relativeFrame >= 2)
					{
						relativeFrame = 2;
						_frameDirection = -1;
					}
					
					else if (relativeFrame <= 0)
					{
						relativeFrame = 0;
						_frameDirection = 1;
					}
					
					_currentFrame = directionStart + relativeFrame;
				}
				
				_walk.Frame = _currentFrame;
			}
		}
		
		else
		{
			int lastDirectionStart = (_currentFrame / 3) * 3;
			_currentFrame = lastDirectionStart + 1;
			_walk.Frame = _currentFrame;
			_animationTimer = 0f;
		}
	}
	
	private int GetDirectionStart(Vector2 direction)
	{
		if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
		{
			// Determine if left or right is pressed
			return direction.X < 0 ? LEFT_START : RIGHT_START;
		}
		
		else
		{
			// Determine if up or down is pressed
			return direction.Y < 0 ? UP_START : DOWN_START;
		}
	}
}
