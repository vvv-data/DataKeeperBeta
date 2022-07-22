using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shared
{
    static class Message
    {

        static public string Warning { get; set; }
        static public string Error { get; set; }
        static public string Success { get; set; }
        static public string YesNo { get; set; }

        static public void ShowError()
        {
            if (Error != null)
            {
                MessageBox.Show(Error, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Error = null;
            }                
        }

        static public void ShowSuccess()
        {
            if (Success != null)
            {
                MessageBox.Show(Success, "OK!", MessageBoxButton.OK);
                Success = null;
            } 
                                
        }

        static public void ShowWarning()
        {
            if (Warning != null) 
            {
                MessageBox.Show(Warning, "Warning!", MessageBoxButton.OK);
                Warning = null;
            } 
        }

        static public bool ShowYesNo()
        {
            bool flag = false;
            MessageBoxResult result;

            if (YesNo != null)
            {
                result = MessageBox.Show(YesNo, "Подтверждение!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    flag = true;
                }                
                YesNo = null;
            }
            return flag;
        }

        static public void ShowAll()
        {
            ShowError();
            ShowSuccess();
            ShowWarning();
        }
    }
}
