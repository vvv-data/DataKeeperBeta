using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Shared;

namespace View
{
    /// <summary>
    /// Логика взаимодействия для WindowPasswordAdd.xaml
    /// </summary>
    public partial class WindowPassword : Window
    {
        static WindowPassword windowPassword = null;
        internal static MediatorAuthorization Mediator { private get; set; }

        // static string password;

        static string Password
        {
           get 
            {
                if (windowPassword.showPassword.IsChecked.Value)
                    return windowPassword.pwdTextBox.Text;
                else 
                    return windowPassword.pwdPasswordBox.Password;
            }
           set 
            { 
                windowPassword.pwdPasswordBox.Password = value;
                windowPassword.pwdTextBox.Text = value;
            }
        }

        public static WindowPassword CreateWindowPassword()
        {

            if (Mediator.CheckConfig())
            {
                if (windowPassword == null)
                {
                    windowPassword = new WindowPassword();
                    Mediator.PassExists(); // вызываем чтобы записать предупреждение в Message.MessageWarning
                }

                SetWindow();
                windowPassword.Show();

                Mediator.PassExists();

                Message.ShowWarning(); // выдаст предупреждение если пароль еще не придуман


                return windowPassword;
            }
            else
            {
                return null;
            }
            
        }
               
        WindowPassword()
        {
            InitializeComponent();

            // Создание привязок для кнопки ОК.
            CommandBinding binding;
            binding = new CommandBinding(ApplicationCommands.New);
            binding.Executed += Add_Password;
            this.CommandBindings.Add(binding);
            // Событие, которое вызывается для проверки доступности элемента управления к которому привязана команда.
            binding.CanExecute += Can_Add_Pass;
            this.CommandBindings.Add(binding);
        }

        public static void Heading(string txt)
        {
            windowPassword.heading.Text = txt;
        }


        private void Add_Password(object sender, RoutedEventArgs e)
        {
           string pass = Password; // это нужно т.к. ref нельзя со свойством

            if (Mediator.Login(ref pass))
            {
                MediatorAuthorization.CreateMediatorContent();
                windowPassword.Close();
                Mediator = null;
            }
            Password = pass; // обнулит поле пароля при его удачном добавлении, для повторного ввода

            //this.DialogResult = true;       
        }

        // для возможности перемещения окна мышкой
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked.Value)
            {
                pwdTextBox.Text = pwdPasswordBox.Password; // скопируем в TextBox из PasswordBox
                pwdTextBox.Visibility = Visibility.Visible; // TextBox - отобразить
                pwdPasswordBox.Visibility = Visibility.Hidden; // PasswordBox - скрыть
            }
            else
            {
                pwdPasswordBox.Password = pwdTextBox.Text; // скопируем в PasswordBox из TextBox 
                pwdTextBox.Visibility = Visibility.Hidden; // TextBox - скрыть
                pwdPasswordBox.Visibility = Visibility.Visible; // PasswordBox - отобразить
            }
        }

        void Can_Add_Pass(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Is_Empty();
        }

        bool Is_Empty()
        {
            if (windowPassword == null)
                return false;

            if (showPassword.IsChecked.Value)
            {
                if (String.IsNullOrEmpty(windowPassword.pwdTextBox.Text))
                    return false;
            }
            else
            {
                if (String.IsNullOrEmpty(windowPassword.pwdPasswordBox.Password))
                    return false;
            }
            
             return true;
        }
        static void SetWindow()
        {
            windowPassword.pwdTextBox.MaxLength = Limits.passwordMax;
            windowPassword.pwdPasswordBox.MaxLength = Limits.passwordMax;          
        }

    }
}
