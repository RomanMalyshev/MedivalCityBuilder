using UnityEngine;

public class DayNight : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("Starting time of day (0-24)")]
    [SerializeField, Range(0f, 24f)] private float _startTime = 8f;
    
    [Header("Duration Settings (in real seconds)")]
    [Tooltip("Real seconds for day time (6:00 - 18:00)")]
    [SerializeField] private float _dayDurationSeconds = 120f;
    
    [Tooltip("Real seconds for night time (18:00 - 6:00)")]
    [SerializeField] private float _nightDurationSeconds = 60f;
    
    [Header("Sun")]
    [SerializeField] private Transform _sun;
    
    [Header("Debug (Read Only)")]
    [SerializeField] private float _timeOfDay;
    [SerializeField] private int _currentHour;
    [SerializeField] private int _currentMinute;
    [SerializeField] private bool _isDay;
    
    // Day constants
    private const float DAY_START = 8f;   // 6:00 - sunrise
    private const float DAY_END = 18f;    // 18:00 - sunset
    private const float DAY_HOURS = 12f;  // hours of daylight
    private const float NIGHT_HOURS = 12f; // hours of night

    public float TimeOfDay => _timeOfDay;
    public int CurrentHour => _currentHour;
    public int CurrentMinute => _currentMinute;
    public bool IsDay => _isDay;
    public bool IsNight => !_isDay;
    public string TimeString => $"{_currentHour:D2}:{_currentMinute:D2}";

    private void Start()
    {
        _timeOfDay = Mathf.Clamp(_startTime, 0f, 24f);
        UpdateTimeValues();
        UpdateSunRotation();
    }

    private void Update()
    {
        UpdateTime();
        UpdateTimeValues();
        UpdateSunRotation();
    }
    
    private void UpdateTime()
    {
        float hoursPerSecond = GetCurrentTimeSpeed();
        _timeOfDay += hoursPerSecond * Time.deltaTime;
        
        if (_timeOfDay >= 24f)
            _timeOfDay -= 24f;
        else if (_timeOfDay < 0f)
            _timeOfDay += 24f;
    }
    
    private float GetCurrentTimeSpeed()
    {
        // During day (6:00 - 18:00): speed = 12 hours / dayDuration seconds
        // During night (18:00 - 6:00): speed = 12 hours / nightDuration seconds
        if (_timeOfDay >= DAY_START && _timeOfDay < DAY_END)
        {
            // Daytime
            return DAY_HOURS / Mathf.Max(_dayDurationSeconds, 0.1f);
        }
        else
        {
            // Nighttime
            return NIGHT_HOURS / Mathf.Max(_nightDurationSeconds, 0.1f);
        }
    }
    
    private void UpdateTimeValues()
    {
        // Update hour and minute
        _currentHour = Mathf.FloorToInt(_timeOfDay) % 24;
        _currentMinute = Mathf.FloorToInt((_timeOfDay - _currentHour) * 60f) % 60;
        
        // Update day/night flag
        _isDay = _timeOfDay >= DAY_START && _timeOfDay < DAY_END;
    }
    
    private void UpdateSunRotation()
    {
        if (_sun == null) return;
        
        // Map time to sun angle
        // At 6:00 (sunrise) - sun at horizon (0°)
        // At 12:00 (noon) - sun at zenith (90°)
        // At 18:00 (sunset) - sun at horizon (180°)
        // At 0:00 (midnight) - sun below (-90° or 270°)
        
        // Convert 24h time to angle (0h = -90°, 6h = 0°, 12h = 90°, 18h = 180°, 24h = 270°)
        float sunAngle = (_timeOfDay - 6f) / 24f * 360f;
        
        _sun.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
    }
    
    public void SetTime(int hour, int minute = 0)
    {
        _timeOfDay = Mathf.Clamp(hour, 0, 23) + Mathf.Clamp(minute, 0, 59) / 60f;
        UpdateTimeValues();
        UpdateSunRotation();
    }
    
    public void SetTime(float time)
    {
        _timeOfDay = Mathf.Repeat(time, 24f);
        UpdateTimeValues();
        UpdateSunRotation();
    }
    
    public void GetTime(out int hours, out int minutes)
    {
        hours = _currentHour;
        minutes = _currentMinute;
    }
}
