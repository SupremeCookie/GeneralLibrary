public class Stepper
{
	public const float DEFAULT_SPEED = 1.0f;

	public Stepper(int interval, float speed = DEFAULT_SPEED)
	{
		_speed = speed;
		_interval = interval;
		_currentStep = 0;

		_durationPerSprite = 1.0f / interval;
	}

	public int currentStep => _currentStep;
	public bool hasChangedThisFrame => _hasChanged;

	public float speed => _speed;
	public int interval => _interval;


	private int _currentStep;
	private float _durationPerSprite;
	private float _currentTime;
	private bool _hasChanged = false;
	private float _speed;
	private int _interval;

	public void Update(float seconds)
	{
		_hasChanged = false;

		_currentTime += seconds * _speed;
		if (_currentTime >= 1.0f)
			_hasChanged = true;

		_currentTime %= 1.0f;
		int newStep = UnityEngine.Mathf.FloorToInt(_currentTime / _durationPerSprite);
		if (newStep != _currentStep)
			_hasChanged = true;

		_currentStep = newStep;
	}
}