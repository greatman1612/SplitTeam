using DevExpress.LookAndFeel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SplitTeam
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmMain());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                DevExpress.Skins.SkinManager.EnableFormSkins();
                DevExpress.UserSkins.BonusSkins.Register();

                UserLookAndFeel.Default.SetSkinStyle("iMaginary");
                //Money Twins
                //Pumpkin
                //iMaginary
                //UserLookAndFeel.Default.SetSkinStyle("Blue");
                frmMain frmMainFRM = new frmMain();
                
                Application.Run(frmMainFRM);
            }
            catch (Exception ex)
            {
                throw ex;
                //string a = ex.Message;
            }
        }
    }
}
