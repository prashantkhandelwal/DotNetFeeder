using Microsoft.AspNet.SignalR;

/// <summary>
/// Summary description for FeedNotificationHub
/// </summary>
public class FeedNotificationHub:Hub
{
    public void FeedJobNotification(string status)
    {
        Clients.All.FeedJobNotification(status);
    }
}