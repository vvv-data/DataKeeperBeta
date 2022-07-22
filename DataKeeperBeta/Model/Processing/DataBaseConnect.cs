
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using Shared;

namespace Model
{
    namespace Processing
    {
        static class DataBaseConnect
        {
            // наружные для BackConnect

            public static string GetDescription(int id, int catalog)
            {
                string str = null;

                if (Config.Sql.Equals("sql"))
                {
                    str = "sql";
                }
                else if (Config.Sql.Equals("file"))
                {
                    str = GetContentFile(Config.file_Dir + @"\" + catalog.ToString() + @"\all\" + id.ToString());
                }
                return Cryption(id, catalog.ToString() + @"\all", str);
            }

            public static string GetHeading(int id, int catalog)
            {
                string str = null;

                if (Config.Sql.Equals("sql"))
                {
                    str = "sql";
                }
                else if (Config.Sql.Equals("file"))
                {
                    str = GetContentFile(Config.file_Dir + @"\" + catalog.ToString() + @"\" + id.ToString());
                }
                return Cryption(id, catalog.ToString(), str);
            }


            public static bool SetDescription(int id, int catalog, string str)
            {
                str = Cryption(id, catalog.ToString() + @"\all", str);

                if (Config.Sql.Equals("sql"))
                {
                    return false;
                }
                else if (Config.Sql.Equals("file"))
                {
                    return SetContentFile(Config.file_Dir + @"\" + catalog.ToString() + @"\all\" + id.ToString(), str);
                }
                return false;
            }

            public static int GetID(int catalog)
            {
                if (Config.Sql.Equals("sql"))
                {
                    return 0;
                }
                else if (Config.Sql.Equals("file"))
                {
                    return DirectoryGetID(Config.file_Dir + @"\" + catalog.ToString());
                }
                return 0;
            }

            public static Dictionary<int, string> GetNotes(int catalog)
            {
                if (Config.Sql.Equals("sql"))
                {
                    return null;
                }
                else if (Config.Sql.Equals("file"))
                {
                    string[] files = DirectoryGetFiles(Config.file_Dir + @"\" + catalog.ToString());
                    Dictionary<int, string> notes = new Dictionary<int, string>(files.Length);
                    string str;
                    int id;

                    foreach (string fullPath in files)
                    {
                        id = int.Parse(Path.GetFileName(fullPath));

                        // раскодируем строку
                        str = Cryption(id, catalog.ToString(), GetContentFile(fullPath));

                        notes.Add(id, str);                    
                    }
                    return notes;
                }
                return null;
            }

            public static bool SetHeading(int id, int catalog, string str)
            {
                str = Cryption(id, catalog.ToString(), str);

                if (Config.Sql.Equals("sql"))
                {
                    return false;
                }
                else if (Config.Sql.Equals("file"))
                {
                    return SetContentFile(Config.file_Dir + @"\" + catalog.ToString() + @"\" + id.ToString(), str);
                }
                return false;
            }

            public static bool DeleteNote(int id, int catalog)
            {
                if (Config.Sql.Equals("sql"))
                {
                    return false;
                }
                else if (Config.Sql.Equals("file"))
                {

                    DeleteFile(Config.file_Dir + @"\" + catalog.ToString() + @"\" + id.ToString()); // удаляем заголовок
                    DeleteFile(Config.file_Dir + @"\" + catalog.ToString() + @"\all\" + id.ToString()); // удаляем данные
                    return true;
                }
                return false;
            }

            public static int[] GetCounts()
            {
                int[] count = null; 

                if (Config.Sql.Equals("sql"))
                {
                    count = null;
                }
                else if (Config.Sql.Equals("file"))
                {
                    count = new int[Catalog.count];
                    string[] files = DirectoryGetFiles(Config.file_Dir + @"\" + Config.count_Dir + @"\");
                    int i = 0;

                    foreach (string fullPath in files)
                    {
                        count[i] = int.Parse(GetContentFile(fullPath));
                        i++;
                    }
                }
                return count;
            }

            public static int GetCount(int catalog)
            {
                int count = 0;

                if (Config.Sql.Equals("sql"))
                {
                    count = 0;
                }
                else if (Config.Sql.Equals("file"))
                {
                    count = Int32.Parse(GetContentFile(Config.file_Dir + @"\" + Config.count_Dir + @"\" + catalog.ToString()));
                }
                return count;
            }

            public static bool SetCount(int catalog, int count)
            {
                if (Config.Sql.Equals("sql"))
                {
                    return false;
                }
                else if (Config.Sql.Equals("file"))
                {
                    return SetContentFile(Config.file_Dir + @"\" + Config.count_Dir + @"\" + catalog.ToString(), count.ToString());
                }
                return false;
            }

            // внутренние Processing

            static public int DirectoryGetID(string path)
            {
                string[] files = DirectoryGetFiles(path);
                int id = 0, id2 = 0;
                if(files.Length > 0)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        id2 = int.Parse(Path.GetFileName(files[i]));
                        if (id < id2)
                           id = id2;
                    }
                }                
                return ++id;
            }

            static public string[] DirectoryGetFiles(string path)
            {
                return DataBase.FileHandler.DirectoryGetFiles(path);
            }


            public static string Cryption(int id, string catalog, string str)
            {
                if (Int32.Parse(Config.Crypt) > 0)
                    return Crypt.Cryption(id, catalog, str);
                else return str;
            }


            public static bool DirectoryCreate(string dir)
            {
                return DataBase.FileHandler.DirectoryCreate(dir);
            }

            public static bool DirectoryExists(string dir)
            {
                return DataBase.FileHandler.DirectoryExists(dir);
            }

            public static bool FileExists(string fileDir)
            {
                return DataBase.FileHandler.FileExists(fileDir);
            }

            public static string GetContentFile(string fileDir)
            {
                return DataBase.FileHandler.FileRead(fileDir);
            }

            public static bool SetContentFile(string fileDir, string str)
            {
                return DataBase.FileHandler.FileWrite(fileDir, str);
            }

            public static bool DeleteFile(string fileDir)
            {
                return DataBase.FileHandler.DeleteFile(fileDir);
            }

        }
    }

    
}




