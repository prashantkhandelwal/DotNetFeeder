using FluentScheduler;

public class FeedJob: Registry
{
    public FeedJob()
    {
        Schedule<Feeder>().ToRunNow().AndEvery(30).Minutes();
    }
}