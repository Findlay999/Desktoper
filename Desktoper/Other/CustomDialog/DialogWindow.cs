using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;
using Desktoper.View;
using System.Threading.Tasks;

namespace Desktoper.Other.CustomDialog
{
    class DialogWindow
    {
        public static Window Owner;

        public static bool ShowDialog(string Text)
        {
            DesktoperDialog DWindow = new DesktoperDialog();
            DWindow.IsDialog = true;
            DWindow.TX.Text = Text;
            DWindow.Owner = Owner;
            return DWindow.ShowDialog() == true ? true : false;         
        }

        public static void Show(string Text)
        {
            DesktoperDialog DWindow = new DesktoperDialog();
            DWindow.IsDialog = false;
            DWindow.TX.Text = Text;
            DWindow.Owner = Owner;
            DWindow.ShowDialog();
        }
    }
}
