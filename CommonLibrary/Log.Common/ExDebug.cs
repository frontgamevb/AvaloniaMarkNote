using System.Diagnostics;

namespace Log.Common;

/// <summary>
/// Extended <c>System.Diagnostics.Debug</c> class.
///
/// As a difference from the existing <c>Debug</c>, additional information useful
/// for debugging is output as a message header
/// </summary>
public class ExDebug : LogBase
{
    /// <summary>
    /// If the condition is <c>false</c>, the message and call stack are displayed in a message box and exits.
    /// /// </summary>
    /// <param name="condition">Condition</param>
    /// <param name="format">Message format</param>
    /// <param name="messages">Messages</param>
    [Conditional("DEBUG")]
    public static void Assert(bool condition, string format = "", params object[] messages)
    {
        if(!condition) ExFail(format, messages);
    }

    /// <summary>the message are displayed in a message box and exits.</summary>
    /// <param name="format">Message format</param>
    /// <param name="messages">Messages</param>
    [Conditional("DEBUG")]
    public static void Fail(string format, params object[] messages) => ExFail(format, messages);

    /// <summary>the message and exception are displayed in a message box and exits.</summary>
    /// <param name="ex">Exception.</param>
    /// <param name="format">Message format</param>
    /// <param name="messages">Messages</param>
    [Conditional("DEBUG")]
    public static void Error(Exception ex, string format = "", params object[] messages)
         => ExFail($"{format}{Environment.NewLine}{ex}", messages);

    /// <summary>Prints logs.</summary>
    /// <param name="format">Message format</param>
    /// <param name="messages">Messages</param>
    [Conditional("DEBUG")]
    public static void Print(string format, params object[] messages) => ExPrint(format, messages);

    /// <summary>Prints logs, if the condition is <c>true</c>.</summary>
    /// <param name="condition">Condition.</param>
    /// <param name="format">Message format</param>
    /// <param name="messages">Messages</param>
    [Conditional("DEBUG")]
    public static void PrintIf(bool condition, string format, params object[] messages)
    {
        if(!condition) ExPrint(format, messages);
    }

    /// <summary>Prints warning logs.</summary>
    /// <param name="format">Message format</param>
    /// <param name="messages">Messages</param>
    [Conditional("DEBUG")]
    public static void Warning(string format, params object[] messages) => ExPrint(format, messages);

    /// <summary>Prints warning logs, if the condition is <c>true</c>.</summary>
    /// <param name="condition">Condition.</param>
    /// <param name="format">Message format</param>
    /// <param name="messages">Messages</param>
    [Conditional("DEBUG")]
    public static void WarningIf(bool condition, string format, params object[] messages)
    {
        if(!condition) ExPrint(format, messages);
    }
}