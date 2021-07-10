using Serilog;
using System;
using System.Windows.Threading;
using System.Windows;

namespace net_speed_indicator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ILogger Logger { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .File("logs/log_.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    fileSizeLimitBytes: 2091008, 
                    rollOnFileSizeLimit: true,
                    buffered: true
                ).CreateLogger();
            Logger = Log.Logger.ForContext<App>();
            Logger.Verbose("Application started");

            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Dispatcher.UnhandledException += Current_DispatcherUnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Logger.Error("Exception thrown on: {0}", sender.GetType().Name, e.ExceptionObject.ToString());
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Logger.Error("Exception thrown on: {0} \n StackTrace: {1}", sender.GetType().Name, e.Exception.StackTrace);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Logger.Verbose("Application closed");
            Log.CloseAndFlush();
        }
    }
}
