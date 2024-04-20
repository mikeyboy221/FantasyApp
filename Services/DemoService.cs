namespace FantasyApp.Services;

public class DemoService: IDemoService
{
    private readonly DateTime _currentDate;

    public DemoService()
    {
        _currentDate = new DateTime(2023, 11, 21);
    }

    public DateTime GetCurrentDate()
    {
        return _currentDate;
    }
}

public interface IDemoService
{
    public DateTime GetCurrentDate();
}