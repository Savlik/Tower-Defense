using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TD
{

    static class Program
    {
   
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form_TD form = new Form_TD();
            Images.init();
            Logger.init();

            Application.Run(form);

        }
    }
}
