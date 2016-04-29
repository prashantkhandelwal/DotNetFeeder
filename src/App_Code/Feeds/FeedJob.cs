using FluentScheduler;

public class FeedJob: Registry
{
    public FeedJob(GeneralSettings _settings)
    {
        Schedule<Feeder>().ToRunNow().AndEvery(_settings.refreshTime).Minutes();
    }
}