using Api.Models;

namespace Api.Services;

public interface IActivityProcessor
{
    Task<Activity> ProcessActivity(Activity activity);
}