using DataAccessLayer;
using System.Configuration;
using System.IO;
using System.Windows;

namespace FileMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (!File.Exists(ConfigurationManager.AppSettings["DatabasePath"]))
            {
                using var _db = new FileMonitorDbContext(ConfigurationManager.ConnectionStrings[nameof(FileMonitorDbContext)].ConnectionString);
                _db.Database.EnsureCreated();
            }
            base.OnStartup(e);
        }
    }
}
