public class ActionCallbackWithPriority
{
    /// <summary>
    /// Чем выше Priority, тем раньше вызовется действие
    /// </summary>
    public readonly int Priority;
    public readonly object Callback;

    public ActionCallbackWithPriority (int priority, object callback)
    {
        Priority = priority;
        Callback = callback;
    }
}
