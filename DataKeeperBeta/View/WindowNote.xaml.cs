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
    /// Логика взаимодействия для WindowContent.xaml
    /// </summary>
    public partial class WindowNote : Window
    {
        internal static MediatorContent Mediator { private get; set; }
        static WindowNote window = null;
        static int IdCatalog { get; set; }
        static int Id { get; set; }
        static string Selected { get; set; } 
        static Brush brushCanEdit = new SolidColorBrush(Color.FromRgb(58, 58, 58));
        static Brush brushNotCanEdit = new SolidColorBrush(Color.FromRgb(210, 210, 210));
        static Note note;
      //  static string HeadingNote { get; set; }
      //  static string DescriptionNote { get; set; }
        static bool canEdit = false;

        // static string Selected { get; set; }




        //void Copy_Text_Click(object sender, RoutedEventArgs e)
        //{
        //    Selected = window.heading.SelectedText;
        //}

        private void txt_TextChanged(object sender, RoutedEventArgs e)
        {
            // Текст в поле ввода был изменен.
            //isDirty = true;
        }

        static public WindowNote CreateWindowNote(int idCatalog, int id)
        {
            if(window == null)
                window = new WindowNote(idCatalog, id);

            SetWindow();
            window.ShowDialog();

            return window;
        }

        public WindowNote(int idCatalog, int id)
        {
            InitializeComponent();
          
            IdCatalog = idCatalog;
            Id = id;
            Selected = null;

            // Создание привязок для кнопки копировать
            CommandBinding binding;
            binding = new CommandBinding(ApplicationCommands.Copy);
            binding.Executed += Copy_Text;
            this.CommandBindings.Add(binding);
            // Событие, которое вызывается для проверки доступности элемента управления к которому привязана команда.
            binding.CanExecute += Can_Copy;
            this.CommandBindings.Add(binding);

            // Создание привязок для кнопки сохранить
            CommandBinding bindingNew;
            bindingNew = new CommandBinding(ApplicationCommands.New);
            bindingNew.Executed += Save_Text;
            this.CommandBindings.Add(bindingNew);
            // Событие, которое вызывается для проверки доступности элемента управления к которому привязана команда.
            bindingNew.CanExecute += Can_Save_Text;
            this.CommandBindings.Add(bindingNew);
        }

        private void Save_Text(object sender, RoutedEventArgs e)
        {
            note.Heading = this.headingNote.Text;
            note.Description = this.descriptionNote.Text;

           if(Mediator.UpdateNote(Id, IdCatalog, note.Heading, note.Description))
            {
                Mediator.UpdateListNotes(); // обновляем список на главной
                Not_Edit(sender, e); // выключаем редактирование
            }
        }

        private void Delete_Note(object sender, RoutedEventArgs e)
        {
            Message.YesNo = "Вы действительно хотите удалить эту запись?";

            if (Message.ShowYesNo()) // запрашиваем дополнительное подтверждение
            {
                if (Mediator.Delete(Id, IdCatalog))
                {
                    Mediator.UpdateListNotes(); // обновляем список на главной
                    Cancel_Click(sender, e); // закрываем текущее окно
                }               
            }                       
        }

        void Can_Edit(object sender, RoutedEventArgs e)
        {
            canEdit = true;
            this.edit.IsEnabled = false;
            this.borderHeading.BorderBrush = brushCanEdit;
            this.borderDescription.BorderBrush = brushCanEdit;
            this.headingNote.IsReadOnly = false;
            this.descriptionNote.IsReadOnly = false;
            this.cancelEdit.IsEnabled = true;
            note.Heading = this.headingNote.Text; // записываем начальное значение для сравнения были ли изменения
            note.Description = this.descriptionNote.Text;
        }

        void Not_Edit(object sender, RoutedEventArgs e)
        {
            canEdit = false;
            this.edit.IsEnabled = true;
            this.borderHeading.BorderBrush = brushNotCanEdit;
            this.borderDescription.BorderBrush = brushNotCanEdit;
            this.headingNote.IsReadOnly = true;
            this.descriptionNote.IsReadOnly = true;
            this.cancelEdit.IsEnabled = false;
            this.headingNote.Text = note.Heading; // записываем начальное значение тк отмена редактирования
            this.descriptionNote.Text = note.Description;
        }

        void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            window = null;
        }

        static void SetWindow()
        {
            window.title.Text += " " + Catalog.catalog[IdCatalog].Category;
            window.heading.Text = Catalog.catalog[IdCatalog].Heading;
            window.description.Text = Catalog.catalog[IdCatalog].Description;
            window.headingNote.MaxLength = Limits.heading;
            window.descriptionNote.MaxLength = Limits.description;
            // записывать после установки лимитов, а то может не влезть
            note = Mediator.GetNote(Id, IdCatalog);
            window.headingNote.Text = note.Heading;
            window.descriptionNote.Text = note.Description;
        }

        private void Selected_Text(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Selected = textBox.SelectedText;
        }        
        
        private void Can_Save_Text(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;

            if (canEdit)
            {
                if (this.headingNote.Text != note.Heading || 
                this.descriptionNote.Text != note.Description) // сравниваем изменен ли текст?
                {
                    e.CanExecute = true;
                }
            }
        }       

        private void Copy_Text(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(Selected);     
        }

        void Can_Copy(object sender, CanExecuteRoutedEventArgs e)
        {
           if(Selected != null)
                 e.CanExecute = true;
           else
                e.CanExecute = false;
        }

        // для возможности перемещения окна мышкой
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }       
    }
}
