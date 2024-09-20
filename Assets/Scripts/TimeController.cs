using System;
using UnityEngine;

public class TimeController
{
    private DateTime _timeUtc;
    private float _offset;

    public TimeController()
    {
        _timeUtc = DateTime.UtcNow;
    }

    public void SetTimeUtc(DateTime dateTime)
    {
        _timeUtc = dateTime;

        _offset = Time.realtimeSinceStartup;
    }
    
    public DateTime LocalTime()
    {
        return _timeUtc.ToLocalTime().AddSeconds(Time.realtimeSinceStartup - _offset);
    }
}
