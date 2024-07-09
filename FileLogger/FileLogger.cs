namespace Services.Logging
{
    public class FileLogger
    {
        private static string _fileName = "logs.txt";
        private static readonly object LogOutputSync = new object();

        static FileLogger()
        {
            if (File.Exists(_fileName))
                File.Delete(_fileName);
        }

        public static void Write(Logger.LogMessageTypes messageType, string WhoSends, string Message)
        {
            lock (LogOutputSync)
            {
                string format = $"[{DateTime.Now:HH:mm:ss}] - {Message}\n";
                File.AppendAllText(_fileName, format);
                switch (messageType)
                {
                    case Logger.LogMessageTypes.Classic:
                        Logger.Log(WhoSends, Message);
                        break;
                    case Logger.LogMessageTypes.Info:
                        Logger.LogInfo(WhoSends, Message);
                        break;
                    case Logger.LogMessageTypes.Warning:
                        Logger.LogWarning(WhoSends, Message);
                        break;
                    case Logger.LogMessageTypes.Error:
                        Logger.LogError(WhoSends, Message);
                        break;
                    case Logger.LogMessageTypes.Fatal:
                        Logger.LogFatal(WhoSends, Message);
                        break;
                }
            }
        }
    }
}
