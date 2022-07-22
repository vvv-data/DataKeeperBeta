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
    /// Логика взаимодействия для WindowAddNote.xaml
    /// </summary>
    public partial class WindowAddNote : Window
    {
        static WindowAddNote window = null;
        static int IdCatalog { get; set; }
        internal static MediatorContent Mediator { private get; set; }

        static string Heading 
        {
            get
            {
                return window.headingNote.Text;
            }
            set
            {
                window.headingNote.Text = value;
            }
        }

        static string Description
        {
            get
            {
                return window.descriptionNote.Text;
            }
            set
            {
                window.descriptionNote.Text = value;
            }
        }


        public static WindowAddNote CreateWindowAddNote(int idCatalog)
        {
            if(window == null)
                window = new WindowAddNote();

            SetWindow(idCatalog);
            window.ShowDialog();
            //window.DialogResult = true;  

            return window;
        }


        WindowAddNote()
        {
            InitializeComponent();
            // Создание привязок.
            CommandBinding binding;
            binding = new CommandBinding(ApplicationCommands.New);
            binding.Executed += Add_Note;
            this.CommandBindings.Add(binding);
            // Событие, которое вызывается для проверки доступности элемента управления к которому привязана команда.
            binding.CanExecute += Can_Add_Note;
            this.CommandBindings.Add(binding);
        }              


        void Add_Note(object sender, RoutedEventArgs e)
        {
            if(Mediator.AddNote(IdCatalog, Heading, Description)) 
            {
                Heading = null;
                Description = null;
                Mediator.UpdateListNotes(); // обновляем список на главной
            }
        }

        // для возможности перемещения окна мышкой
        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        void Cancel_Click(object sender, RoutedEventArgs e)
        {
            window.DialogResult = false;

            window = null;
        }

        void Can_Add_Note(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Is_Empty();
        }

        bool Is_Empty()
        {
            if(window == null)
                return false;
            if (!String.IsNullOrEmpty(window.headingNote.Text) 
                && !String.IsNullOrEmpty(window.descriptionNote.Text)) 
                return true;
            else 
                return false;           
        }

        static void SetWindow(int idCatalog)
        {
            window.title.Text = "Добавление: " + Catalog.catalog[idCatalog].Category;
            window.heading.Text = Catalog.catalog[idCatalog].Heading;
            window.description.Text = Catalog.catalog[idCatalog].Description;
            IdCatalog = idCatalog;
            window.headingNote.MaxLength = Limits.heading;
            window.descriptionNote.MaxLength = Limits.description;
        }
    }
}
