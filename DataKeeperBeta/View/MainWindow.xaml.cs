using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Shared;

namespace View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static MainWindow mainWindow = null;
        internal static MediatorContent Mediator { private get; set; }
        static int IdCatalog { get; set; }
        Button[] menu = new Button[Catalog.count]; // массив кнопок меню
        static Brush brushEnabled = new SolidColorBrush(Color.FromRgb(58, 58, 58)); // цвет для рамки включенной сортировки
        static ISorting sorting = new SortingByDateDesc(); // сортировка по умолчанию
        static Dictionary<int, string> listNotes = null;

        public static MainWindow CreateMainWindow()
        {
            if(mainWindow == null)
                mainWindow = new MainWindow();

            mainWindow.BuildingMenu();

            mainWindow.Show();

            return mainWindow;
        }

        MainWindow()
        {
            InitializeComponent();

            //  Mouse.OverrideCursor = Cursors.Wait;            
        }

        private void BuildingMenu()
        {
            mainWindow.wrap.Children.Clear();

            for (int i = 0; i < Catalog.count; ++i)
            {                
                menu[i] = new Button()
                {
                    Content = Catalog.catalog[i].Category + " - " + Catalog.catalog[i].Count,
                    Tag = i,
                    Name = "section_" + i.ToString()
                    // Margin = new Thickness(10, 10, 10, 10),
                };
                menu[i].Click += new RoutedEventHandler(SelectedSection);
                mainWindow.wrap.Children.Add(menu[i]);
            }
        }

        private void Add_Note(object sender, RoutedEventArgs e)
        {
            if (IdCatalog >= 0 && IdCatalog < Catalog.count)
            {
                MediatorContent.CreateWindowAddNote(IdCatalog);
            }
                                              
            else
            {
                Message.Error = "Перед добавлением записи необходимо выбрать раздел";
            }
        }


        private void Viewing_Note(object sender, RoutedEventArgs e)
        {
            MediatorContent.CreateWindowNote(IdCatalog, (int)(sender as Button).Tag);
        }          
       

        static void SelectedSection(object sender, RoutedEventArgs e)
        {
            IdCatalog = (int)(sender as Button).Tag;

            mainWindow.add.Tag = IdCatalog;
            mainWindow.sectionName.Text = Catalog.catalog[IdCatalog].Category;

            mainWindow.BorderSort.BorderBrush = brushEnabled; // меняем цвет рамки сортировки

            listNotes = Mediator.GetNotes(IdCatalog); // получем список из базы
            mainWindow.BuildingListNotes();            
        }
        
        public void BuildingListNotes()
        {
            this.body.Children.Clear();
            Button note;

            //foreach (var heading in sorting.Sort(listNotes))
            foreach (var heading in sorting.Sort(listNotes))
            {
                note = new Button()
                {
                    Content = heading.Value,
                    Tag = heading.Key,
                };
                note.Click += new RoutedEventHandler(Viewing_Note);
                mainWindow.body.Children.Add(note);  
            }
        }

        public void UpdateListNotes()
        {
            listNotes = Mediator.GetNotes(IdCatalog); // получем измененный список из базы

            BuildingListNotes();  // выводим измененный список

            UpdateCountSection(); // меняем цифру у кнопки меню

        }

        public void UpdateCountSection() 
        {
            // обновляем количество на кнопке меню
            mainWindow.menu[IdCatalog].Content = Catalog.catalog[IdCatalog].Category + " - " + Catalog.catalog[IdCatalog].Count;            
        }

        private void shutdown_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем текущее приложение.
            Application.Current.Shutdown();
        }

        public void SetSectionName(string str)
        {
            mainWindow.sectionName.Text = str;           
        }

        private void Selected_Sorted(object sender, RoutedEventArgs e)
        {
           // var selectedTag = mainWindow.SortList.SelectedValue;
            if(mainWindow != null)
            {
                string sort = ((ComboBoxItem)mainWindow.SortList.SelectedItem).Tag.ToString();

                switch (sort)
                {
                    case "0":
                        sorting = new SortingByDateDesc(); 
                        break;
                    case "1":
                        sorting = new SortingByDateAsc();
                        break;
                    case "2":
                        sorting = new SortingByNameAsc();
                        break;
                    case "3":
                        sorting = new SortingByNameDesc();
                        break;

                }
                BuildingListNotes();
            }            
        }
    }
}
