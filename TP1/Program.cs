using System;
using System.Windows.Forms;

namespace TP1
{
    /// <summary>
    /// Point d'entr�e de l'application. Initialise et lance le formulaire principal du menu.
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // D�marrer avec le menu principal
            Application.Run(new MenuForm());
        }
    }
}
