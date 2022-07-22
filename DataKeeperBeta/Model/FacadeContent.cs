using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;
using Shared;

namespace Model
{
    using DB = Processing.DataBaseConnect;   

    class FacadeContent : Content
    {
        private static FacadeContent facadeContent = null;
        private static bool addDel = false; // флаг делаем тру если произошло добавление или удаление для пересчета

        // основные методы для front

        internal override Dictionary<int, string> GetNotes(int idCatalog)
        {
            Dictionary<int, string> notes = GetListNote(idCatalog);
            
            if (addDel) // нужно обновить количество тк было добавление или удаление
            {
                SetCount(idCatalog, notes.Count); // в файле
                Catalog.catalog[idCatalog].Count = notes.Count; // в данных каталога
                addDel = false;
            }
            
            return notes;
        }

        internal override bool AddNote(int idCatalog, string heading, string description)
        {
            addDel = true;
            int id = GetID(idCatalog);
            bool flag  = false;

            if(id >0 && idCatalog >=0 && idCatalog < Catalog.count)
            {
                if (String.IsNullOrEmpty(heading))
                {
                    Message.Error = "Поле - " + Catalog.catalog[idCatalog].Heading + " Не заполнено.";                   
                }
                else if (heading.Length > Limits.heading)
                {
                    Message.Error = "Поле - " + Catalog.catalog[idCatalog].Heading + " должно быть менее - " + Limits.heading + " символов.";
                }

                if (String.IsNullOrEmpty(description))
                {
                    Message.Error = "Поле - " + Catalog.catalog[idCatalog].Description + " Не заполнено.";
                }
                else if (description.Length > Limits.description)
                {
                    Message.Error = "Поле - " + Catalog.catalog[idCatalog].Description + " должно быть менее - " + Limits.description + " символов.";
                }

                if (SetHeading(id, idCatalog, heading)
                    && SetDescription(id, idCatalog, description)) 
                { 
                    flag = true;
                    Message.Success = "Запись добавлена.";
                }

                else
                {
                    Message.Error = "Не получилось добавить запись, попробуйте повторить.";
                }

            }
            else
            {
                Message.Error = "Раздел указан не верно";
            }
            
            Message.ShowAll();
            return flag;
        }

        internal override bool UpdateNote(int id, int idCatalog, string heading, string description)
        {            
            bool flag = true;
            int idNew = GetID(idCatalog);

            if (idNew <= 0)
            {
                Message.Error = "Ошибка получения ID записи";
                flag = false;
            }

            if (!SetHeading(idNew, idCatalog, heading))
            {
                Message.Error = "Не удалось сохранить - " + Catalog.catalog[idCatalog].Heading;
                flag = false;
            }

            if (!SetDescription(idNew, idCatalog, description))
            {
                Message.Error = "Не удалось сохранить - " + Catalog.catalog[idCatalog].Description;
                flag = false;
            }

            if (GetDescription(idNew, idCatalog) != description) // дополнительная проверка что файл записался
            {
                Message.Error = "Файл записался не корректно";
                flag = false;
            }

            if (!DeleteNote(id, idCatalog))  // удаляем устаревшие файлы
            {
                Message.Error = "Не получилось удалить устаревший вариант записи";
                flag = false;
            }

            if(flag == true)
                Message.Success = "Изменения успешно сохранены";
            
            Message.ShowAll();
            return flag;            
        }

        internal override Note GetNote(int id, int idCatalog)
        {
            Note note = new Note() 
            { 
              Heading = GetHeading(id, idCatalog),
              Description = GetDescription(id, idCatalog),
            };
            return note;
        }

        internal override bool Delete(int id, int idCatalog)
        {
            addDel = true;
            bool flag = false;

            if(DeleteNote(id, idCatalog))
            {
                Message.Success = "Запись успешно удалена!";
                flag = true;
            }
            else
                Message.Error = "Не получилось удалить запись!";

            Message.ShowAll();
            return flag;
        }      

        public static FacadeContent CreateFacadeContent()
        {
            if (facadeContent == null)
                facadeContent = new FacadeContent();

            GetCounts(); // инициализируем массив каталога, количеством записей в разделах


            return facadeContent;
        }

        FacadeContent()
        {
        }

        #region Privat

        // внутренние методы        

        static void GetCounts()
        {
            int[] counts = GetCountAll();
            // инициализируем массив каталога, количеством записей в разделах при выборе раздела
            for (int i = 0; i < Catalog.count; i++)
            {
                Catalog.catalog[i].Count = counts[i];
            }
        }

        static Dictionary<int, string> GetListNote(int catalog)
        {
            return DB.GetNotes(catalog);
        }

        static bool SetHeading(int id, int catalog, string str)
        {
            return DB.SetHeading(id, catalog, str);
        }

        static int GetID(int catalog)
        {
            return DB.GetID(catalog);
        }


        static bool SetDescription(int id, int catalog, string str)
        {
            return DB.SetDescription(id, catalog, str);
        }

        static bool DeleteNote(int id, int catalog)
        {
            return DB.DeleteNote(id, catalog);
        }

        static string GetHeading(int id, int catalog)
        {
            return DB.GetHeading(id, catalog);
        }

        static string GetDescription(int id, int catalog)
        {
            return DB.GetDescription(id, catalog);
        }

        static int[] GetCountAll()
        {
            return DB.GetCounts();
        }

        static int GetCount(int idCatalog)
        {
            return DB.GetCount(idCatalog);
        }

        static bool SetCount(int idCatalog, int count)
        {
            return DB.SetCount(idCatalog, count);
        }

        #endregion
    }

   
}