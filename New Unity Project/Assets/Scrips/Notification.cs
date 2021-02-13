using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class Notification : MonoBehaviour
{
    private AndroidNotificationChannel defaultNotificationChannel;
    private int identifier;
    private AndroidNotification newNotification;

    private void Start()
    {
        defaultNotificationChannel = new AndroidNotificationChannel()
        {
            Id = "default channel",
            Name = "Default Channel",
            Description = "For Generic notifications",
            Importance = Importance.Default,
        };
        AndroidNotificationCenter.RegisterNotificationChannel(defaultNotificationChannel);

        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Test Notification!",
            Text = "This is a test notification!",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = System.DateTime.Now.AddSeconds(10),
        };

        identifier = AndroidNotificationCenter.SendNotification(notification, "default_channel");

        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler = delegate (AndroidNotificationIntentData data)
        {
            var msg = "Notification received : " + data.Id + "\n";
            msg += "\n Notification received: ";
            msg += "\n .Title: " + data.Notification.Title;
            msg += "\n .Body: " + data.Notification.Text;
            msg += "\n .Channel: " + data.Channel;
            Debug.Log(msg);
        };

        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;

        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

        if (notificationIntentData != null)
        {
            Debug.Log("was opened with notification!");
        }

    }
    private void onApplicationPause(bool pause)
    {
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Scheduled)
        {
            //If the player has left the game and the game is not running. Send them a new notification
            AndroidNotification newlotification = new AndroidNotification()
            {
                Title = "Reminder Notification!",
                Text = "You've paused Unity Royale!",
                SmallIcon = "app_icon_small",
                LargeIcon = "app_icon_large",
                FireTime = System.DateTime.Now
            };

            // Replace the currently scheduled notification with a new notification.
            AndroidNotificationCenter.UpdateScheduledNotification(identifier, newNotification, "default_channel");
        }
        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Delivered)
        {
            //Remove the notification from the status bar
            AndroidNotificationCenter.CancelNotification(identifier);
        }
        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Unknown)
        {
            AndroidNotification notification = new AndroidNotification()
            {
                Title = "Test Notification!",
                Text = "This is a test notification!",
                SmallIcon = "app_icon_small",
                LargeIcon = "app_icon_large",
                FireTime = System.DateTime.Now.AddSeconds(10),
            };

            //Try sending it again
            identifier = AndroidNotificationCenter.SendNotification(notification, "default_channel");
        }
    }
}
    


