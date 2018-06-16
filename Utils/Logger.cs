using System.IO;

namespace RayRender.Utils
{
    public class Logger
    {
        private static string LogFileName = @"debug.log";

        private static Logger instance;

        private StreamWriter writer;

        private Logger()
        {
            FileMode fileMode = FileMode.CreateNew;

            if (File.Exists(Logger.LogFileName))
            {
                fileMode = FileMode.Truncate;
            }

            FileStream fileStream = new FileStream(Logger.LogFileName, fileMode);
            writer = new StreamWriter(fileStream);
        }
        
        public static void Inicialize()
        {
            Logger.instance = new Logger();
        }

        public static void Debug(string message)
        {
            Logger.instance.writer.WriteLine(message);
            Logger.instance.writer.Flush();
        }

        public static void Debug(string message, params object[] args)
        {
            Logger.Debug(string.Format(message, args));
        }

        public static void Error(string message)
        {
            Logger.instance.writer.WriteLine(message);
            Logger.instance.writer.Flush();
        }
    }
}
