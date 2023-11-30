using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToodedAB
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
            Application.Run(new ToodedAB());
        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
        }
    }
}
