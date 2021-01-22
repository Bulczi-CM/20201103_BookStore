using NUnit.Framework;
using Serilog;
using Serilog.Formatting.Compact;
using System;

namespace BookStore.Tests
{
    [TestFixture]
    class LoggingPlayground
    {

        [Test]
        public void BasicLogging()
        {
            var logConfiguration = new LoggerConfiguration();
            logConfiguration.WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information);
            Serilog.Core.Logger logger = logConfiguration.CreateLogger();

            logger.Debug("to się nie zaloguje, bo minimalnym poziomem jest Information");
            logger.Error("błąd!");

            try
            {
                throw new Exception("can't find the author!");
            }
            catch (Exception e)
            {
                logger.Error(e, "operacja zwróciła wyjątek");
            }
        }

        [Test]
        public void JsonLogging()
        {   
            var logConfiguration = new LoggerConfiguration();
            logConfiguration.WriteTo.Console(new CompactJsonFormatter());
            Serilog.Core.Logger logger = logConfiguration.CreateLogger();

            int ticksElapsed = 123456;
            Log.Information($"String interpolation: Dodawanie autora zajęło {ticksElapsed} sekund."); // źle! Serilog nie rozpozna parametru
            Log.Information("Poprawne formatowanie: Dodawanie autora zajęło {ticksElapsed} sekund.", ticksElapsed); 

            try
            {
                throw new Exception("can't find the author!");
            }
            catch (Exception e)
            {
                logger.Error(e, "operacja zwróciła wyjątek");
            }
                
            var złożonyObiekt = new
            {
                PierwszePole = 123,
                DrugiePole = "21314235"
            };
            logger.Information("Mój złożony obiekt:{obiekt}", złożonyObiekt); // źle! brak małpy - obiekt będzie stringiem zawierającym JSON, a nie JSON-em
            logger.Information("Mój złożony obiekt:{@obiekt}", złożonyObiekt); 

            var liczby = new[] {1, 2, 3};
            logger.Information("Liczby:{@liczby}", liczby); 
        }

        [Test]
        public void LoggingToFile()
        {
            //string relativePath = Path.Combine(Environment.CurrentDirectory, "log.txt");
            string absolutePath = "C:/logi/log.txt"; 

            var logConfiguration = new LoggerConfiguration();
            logConfiguration
                .WriteTo.File(absolutePath, Serilog.Events.LogEventLevel.Debug)
                .WriteTo.Console(new CompactJsonFormatter());

            Serilog.Core.Logger logger = logConfiguration.CreateLogger();

            logger.Debug("to się nie zaloguje, bo minimalnym poziomem jest Information");
            logger.Error("błąd!");

            try
            {
                throw new Exception("can't find the author!");
            }
            catch (Exception e)
            {
                logger.Error(e, "operacja zwróciła wyjątek");
            }
        }

    }
}
