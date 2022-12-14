using System.IO;

namespace APICatalogo.Logging
{
    public class CustomerLogger : ILogger
    {
        readonly string loggerName;
        readonly CustomLoggerProviderConfiguration loggerConfig;

        public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
        {
            loggerName = name;
            loggerConfig = config;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, 
            TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string mensagem = $"Log level: {logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
            EscreverTextoNoArquivo(mensagem);
        }

        private void EscreverTextoNoArquivo(string message)
        {
            string caminhoLog = @"c:\log\log.txt";
            using (StreamWriter sw = new StreamWriter(caminhoLog, true))
            {
                try
                {
                    sw.WriteLine(message);
                    sw.Close();
                }
                catch(Exception)
                {
                    throw;
                }
            }
        }
    }
}
