using System;
using ITProcesses.Models;

namespace ITProcesses.SupportMethods;

public class Math
{
    public bool CountingTaskHour(Tasks tasks)
    {
        var totalHourPerUser = tasks.CountHour / tasks.UsersTasks.Count;

        var hourPerUser = totalHourPerUser / (tasks.DateEndTimestamp.Day - tasks.DateStartTimestamp.Day + 1);

        return hourPerUser < 8;
    }
}