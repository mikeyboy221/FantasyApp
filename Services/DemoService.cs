namespace FantasyApp.Services;

public class DemoService: IDemoService
{
    public DateTime CurrentDate;

    public DemoService()
    {
        CurrentDate = DateTime.Now;
    }

    public DateTime GetCurrentDate()
    {
        return CurrentDate;
    }

    public void IncrementMonth()
    {
        CurrentDate = CurrentDate.AddMonths(1);
    }

    public void DecrementMonth()
    {
        CurrentDate = CurrentDate.AddMonths(-1);
    }

    public void IncrementDay()
    {
        CurrentDate = CurrentDate.AddDays(1);
    }

    public void DecrementDay()
    {
        CurrentDate = CurrentDate.AddDays(-1);
    }
}

public interface IDemoService
{
    public DateTime GetCurrentDate();

    public void IncrementMonth();
    public void DecrementMonth();

    public void IncrementDay();
    public void DecrementDay();
}