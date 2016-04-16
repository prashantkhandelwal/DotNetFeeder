using FluentScheduler;

public class FeedJob: Registry
{
    public FeedJob()
    {
        Schedule<Feeder>().ToRunEvery(30).Minutes();
    }
}