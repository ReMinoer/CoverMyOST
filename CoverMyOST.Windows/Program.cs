using System;
using System.Windows.Forms;
using NLog;

namespace CoverMyOST.Windows
{
    static internal class Program
    {
        static private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static private void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                var mainWindow = new MainWindow();
                Application.Run(mainWindow.View);
            }
            catch (Exception e)
            {
                Logger.Fatal(e);
                throw;
            }
        }
    }
}