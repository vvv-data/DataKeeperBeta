using System.Collections.Specialized;
using System.Collections;
using System;
using Shared;

namespace Model
{
    namespace Processing
    {
        static class Config
        {
            public const string file_Dir = "database";
            public const string config_Dir = "config";
            public const string file_Sub_Dir = "all";
            public const string count_Dir = "count";

            public const string file_Sql = file_Dir + @"\" + config_Dir + @"\Sql";
            public const string file_Sql_Data = file_Dir + @"\" + config_Dir + @"\SqlData";
            public const string file_version = file_Dir + @"\" + config_Dir + @"\Version";
            public const string file_Pass = file_Dir + @"\" + config_Dir + @"\Pass";
            public const string file_ID = file_Dir + @"\" + config_Dir + @"\ID";
            public const string file_Crypt = file_Dir + @"\" + config_Dir + @"\Crypt";

            static string sql;
            static string version;
            static string sqlData;
            static string crypt;

            static public string Sql { get { return sql; } }
            static public string Version { get { return version; } }
            static public string SqlData { get { return sqlData; } }
            static public string Crypt { get { return crypt; } }

            static string message = "Выберите где хранить данные: в файлах введите - 1; в базе данных введите - 2.";


            static public bool CheckConfig()
            {
                while (true)
                {
                    // сначала проверяем и заполняем конфиг
                    if (ConfigExists())
                    {
                        // файл конфиг уже заполнен, загружаем его данные
                        return GetConfig();
                        break;
                    }
                    else if (AddConfig()) // заполняем файл конфиг, пока не заполним
                    {
                        return GetConfig();
                        break;
                    }
                }
                return false;
            }

            static public bool GetConfig()
            {
                // получаем записи из фалов config
                sql = DataBaseConnect.GetContentFile(file_Sql);
                version = DataBaseConnect.GetContentFile(file_version);
                sqlData = DataBaseConnect.GetContentFile(file_Sql_Data);
                crypt = DataBaseConnect.GetContentFile(file_Crypt);

                if (!String.IsNullOrEmpty(sql) && !String.IsNullOrEmpty(version) && !String.IsNullOrEmpty(crypt))
                    return true;
                else
                    return false;
            }


            // проверяет есть ли записи в config
            static public bool ConfigExists()
            {
                // проверяем дирректорию config
                if (!DataBaseConnect.DirectoryExists(file_Dir + @"\" + config_Dir))
                    return false;

                // проверяем файлы config
                if (!DataBaseConnect.FileExists(file_Sql)
                    || !DataBaseConnect.FileExists(file_Sql_Data)
                    || !DataBaseConnect.FileExists(file_version)
                    || !DataBaseConnect.FileExists(file_ID)
                    || !DataBaseConnect.FileExists(file_Crypt))
                    return false;

                return true;
            }


            // добавление config в первый раз
            static public bool AddConfig()
            {
                // TODO: снять коментарии с строки ниже для выбора места хранения данных 
                //  Message.MessageWarning = message; msg = Message.MessageWarning;
                string config; 

                string sqlData = ""; 
                int con = 1; // это реализовать для подключения к базе данных               

                if (con == 1 || con == 2)
                {
                    bool flag = false;

                    if (con == 1)
                    {
                        flag = true;
                        config = "file";
                    }
                    else
                    {
                        config = "sql";
                        sqlData = "Пароль и хост";
                    }

                        // создаем все необходимые директории
                    if (!CreateDirectory(flag))
                    {
                        Message.Error = "Не получается создать дирректорию";
                        Message.ShowError();
                        return false;
                    }
                                       
                    if (!DataBaseConnect.SetContentFile(file_version, "1")) // Версия базы данных в файлах, при первом добавлении 1 версия
                        return false;
                    if (!DataBaseConnect.SetContentFile(file_Sql, config)) // где хранить в файлах или sql
                        return false;
                    if (!DataBaseConnect.SetContentFile(file_Sql_Data, sqlData)) // пароли к базе данных sql
                        return false;
                    if (!DataBaseConnect.SetContentFile(file_ID, Catalog.catalog.Length.ToString())) // ID начинается с количества разделов, тк первые файлы они для разделов
                        return false;
                    if (!DataBaseConnect.SetContentFile(file_Crypt, "1")) // По умолчанию данные будут шифроваться, но потом можно реализовать на выбор
                        return false;
                    
                    // усли база в файлах то создаем файлы count?  записываем в них 0
                    if(con == 1)
                    {
                        for(int i = 0; i < Catalog.count; i++)
                        {
                            if (!DataBaseConnect.SetContentFile(file_Dir + @"\" + count_Dir + @"\" + i, "0"))
                                return false;
                        }
                    }
                    
                    
                    return true;
                }
                else
                {
                    Message.Error = "Вы ввели не верные данные";
                    Message.ShowError();
                    return false;
                }

            }

            // создаем дирректории базы из файлов, если file = false - то только одну дир. и конфиг, тк sql 
            static bool CreateDirectory(bool file = false)
            {
                if (!DataBaseConnect.DirectoryExists(file_Dir))
                {
                    try
                    {                 
                        // создаем директорию конфига
                        if (!DataBaseConnect.DirectoryCreate(file_Dir + @"\" + config_Dir))
                            return false;                

                        if (file)
                        {
                            // создаем директорию count
                            if (!DataBaseConnect.DirectoryCreate(file_Dir + @"\" + count_Dir))
                                return false;

                            int lastDir = 0;

                            for (int i = 0; i < Catalog.catalog.Length; i++)
                            {
                                lastDir++;
                                if (!DataBaseConnect.DirectoryCreate(file_Dir + @"\" + i.ToString() + @"\" + file_Sub_Dir))
                                    return false;
                            }

                            // проверяем создана ли последняя дирректория
                            if (DataBaseConnect.DirectoryExists(file_Dir + @"\" + (lastDir - 1).ToString() + @"\" + file_Sub_Dir))
                                return true;
                        }
                        else
                        {
                            // проверяем создана ли дирректория конфига
                            if (DataBaseConnect.DirectoryExists(file_Dir + @"\" + config_Dir))
                                return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.Error = ex.Message;
                        Message.ShowError();
                    }
                }
                else return true;

                return false;
            }


        }
    }    
}

