using System.Diagnostics;
using System.Text;

namespace Log.Common;

public class LogBase
{
    private const string FileExtension = ".log";
    private static readonly string s_lineIndent = $"{Environment.NewLine}\t\t\t\t\t";
    private static string? s_fileName;

    private static int s_logStackFrame;
    private static int s_callerStackFrame;

    /// <summary>Open mode property used by the file listener.</summary>
    /// <value>
    /// [Default: FileMode.Create]
    /// <a>https://learn.microsoft.com/en-us/dotnet/api/system.io.filemode</a>
    /// </value>
    public static FileMode FileMode { get; set; } = FileMode.Create;

    /// <summary>Property of the header format prepended to log messages.</summary>
    /// <value>
    /// [Default: "{0:yy-MM-dd HH:mm:ss.ffff}\t{1}\t{2}\t{3}\t{4}()\t"]
    /// <ul>
    /// <li>0: Current time</li>
    /// <li>1: Debug or Trace</li>
    /// <li>2: Debug or Trace method name (Print, Assert, ETC)</li>
    /// <li>3: Class name containing the calling method</li>
    /// <li>4: Calling method name</li>
    /// </ul>
    /// </value>
    public static string LogLineHeaderFormat { get; set; } = "{0:yy-MM-dd HH:mm:ss.ffff}\t{1}\t{2}\t{3}\t{4}()\t";

    /// <summary>Add a file listener.</summary>
    /// <param name="fileName">File name</param>
    public static void AddFileListener(string? fileName = null)
    {
        s_fileName = fileName ?? AppDomain.CurrentDomain.BaseDirectory + "\\"
            + AppDomain.CurrentDomain.FriendlyName + FileExtension;
        try
        {
            // Specifies that the operating system should create a new file. This requires Write permission.
            // If the file already exists, an IOException exception is thrown.
            if(FileMode is FileMode.CreateNew && File.Exists(s_fileName)) File.Delete(s_fileName);
            using var file = File.Open(s_fileName, FileMode);
        }
        catch
        {
            Trace.TraceWarning(Texts.FileNotAvailable, s_fileName);
            s_fileName = null;
        }
    }

    /// <summary>Displays a message box, prints logs and exits.</summary>
    /// <param name="format">Message format</param>
    /// <param name="messages">Messages</param>
    protected static void ExFail(string format, params object[] messages)
        => Trace.Fail(PrintLog(format, messages));

    /// <summary>Output logs.</summary>
    /// <param name="format">Message format</param>
    /// <param name="messages">Messages</param>
    protected static void ExPrint(string format, params object[] messages)
        => Trace.WriteLine(PrintLog(format, messages));

    private static string PrintLog(string format, params object[] messages)
    {
        var stackTrace = new StackTrace();
        if(s_logStackFrame == 0)
        {
            for(var i = 1; i < stackTrace.FrameCount; i++)
            {
                var className = stackTrace.GetFrame(i)?.GetMethod()?.DeclaringType?.Name;
                if(className is (nameof(ExTrace)) or (nameof(ExDebug)))
                {
                    s_logStackFrame = i;
                    s_callerStackFrame = i + 1;
                    break;
                }
            }
        }
        var log = stackTrace.GetFrame(s_logStackFrame)?.GetMethod();
        var caller = stackTrace.GetFrame(s_callerStackFrame)?.GetMethod();
        var text = new StringBuilder()
            .AppendFormat(LogLineHeaderFormat, DateTime.Now,
                log?.DeclaringType?.Name, log?.Name, caller?.DeclaringType?.Name, caller?.Name)
            .AppendFormat(format, messages).Replace(Environment.NewLine, s_lineIndent);

        if(s_fileName is not null) File.AppendAllText(s_fileName, text.ToString() + Environment.NewLine);
        return text.ToString();
    }
}