using System;
using System.Windows.Forms;
using Woz.BadlyDrawnRogue.Views;
using Woz.Core;
using Woz.RogueEngine.Entities;

namespace Woz.BadlyDrawnRogue
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Singleton<IEntityFactory>.Instance = DataLoader
                .LoadEntityFactory("Definitions/EntityDefinitions.xml");

            Application.Run(new MainForm());
        }
    }
}
