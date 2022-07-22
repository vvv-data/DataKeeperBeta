using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using System.Windows;
using Bridge;
using System.Collections.Specialized;

namespace View
{
    internal class MediatorContent : Content
    {
        static MediatorContent mediator = null;
        public static MainWindow mainWindow = null;
        static WindowAddNote addWindow = null;


        internal override bool AddNote(int idCatalog, string heading, string description)
        {
            return bridge.AddNote(idCatalog, heading, description);
        }

        internal override bool Delete(int id, int idCatalog)
        {
            return bridge.Delete(id, idCatalog);
        }

        internal override Note GetNote(int id, int idCatalog)
        {
            return bridge.GetNote(id, idCatalog);
        }

        internal override Dictionary<int, string> GetNotes(int idCatalog)
        {
            return bridge.GetNotes(idCatalog);
        }

        internal override bool UpdateNote(int id, int idCatalog, string heading, string description)
        {
            return bridge.UpdateNote(id, idCatalog, heading, description);
        }

        public void UpdateListNotes()
        {
            mainWindow.UpdateListNotes();
        }

        public static WindowAddNote CreateWindowAddNote(int idCatalog)
        {
            addWindow = WindowAddNote.CreateWindowAddNote(idCatalog);
            return addWindow;
        }

        public static WindowNote CreateWindowNote(int idCatalog, int id)
        {
            return WindowNote.CreateWindowNote(idCatalog, id);
        }

        internal static MediatorContent CreateMediator()
        {
            if (mediator == null)
                mediator = new MediatorContent();

            WindowAddNote.Mediator = mediator;
            MainWindow.Mediator = mediator;
            WindowNote.Mediator = mediator;

            return mediator;
        }

        MediatorContent()
        {
            CreateBridge();
            mainWindow = MainWindow.CreateMainWindow();
        }
    }
}
