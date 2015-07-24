using System;
using System.Windows.Forms;

namespace CoverMyOST.Windows
{
    static internal class Program
    {
        /// <summary>
        ///     Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static private void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainWindow = new MainWindow();
            Application.Run(mainWindow.View);
        }
    }
}