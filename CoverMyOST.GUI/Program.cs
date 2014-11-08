using System;
using System.Windows.Forms;

namespace CoverMyOST.GUI
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
            Application.Run(new MainView());
        }
    }
}