using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Model;
using Shared;

namespace Bridge
{
    abstract class Content
    {
        static public FacadeContent bridge = null;

        public static void CreateBridge()
        {
            bridge = FacadeContent.CreateFacadeContent();
        }

        abstract internal Dictionary<int, string> GetNotes(int idCatalog);
        abstract internal bool AddNote(int idCatalog, string heading, string description);
        abstract internal bool UpdateNote(int id, int idCatalog, string heading, string description);
        abstract internal Note GetNote(int id, int idCatalog);
        abstract internal bool Delete(int id, int idCatalog);

    }
}
