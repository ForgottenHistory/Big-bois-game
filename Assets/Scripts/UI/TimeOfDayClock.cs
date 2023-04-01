using System;
using UnityEngine;
using TMPro;

public class TimeOfDayClock : MonoBehaviour, IInitialize
{
    public TextMeshProUGUI clockText;
    public Transform sun;
    GameManager gameManager;

    public bool isActive { get; set; } = false;

    public void Initialize()
    {
        gameManager = GameManager.Instance;
        isActive = true;
    }

    public void Deinitialize()
    {
        isActive = false;
    }

    private void Update()
    {
        if (isActive == false)
            return;

        float timeOfDay = gameManager.GetTimeOfDay;
        TimeSpan time = TimeSpan.FromMinutes(timeOfDay + 300); // Add 5 hours in minutes
        string timeString = string.Format("{0:00}:{1:00} {2}",
                                          ((time.Hours + 11) % 12) + 1,
                                          time.Minutes,
                                          time.Hours < 12 ? "AM" : "PM");
        clockText.SetText(timeString);

        // Rotate sun based on time of day
        float rotationX = ((timeOfDay / 1440f) * 360f) - 25f; // Convert time of day to rotation angle around X axis
        float rotationY = ((timeOfDay / 1440f) * 180f) - 90f; // Convert time of day to rotation angle around Y axis
        sun.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}