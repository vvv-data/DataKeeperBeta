using System;
using System.IO;
using System.Windows;
using Shared;

namespace Model
{
    namespace Processing
    {
        namespace DataBase
        {

            public static class FileHandler
            {
                
                static public bool DeleteFile(string fileName)
                {
                    try
                    {
                        File.Delete(fileName);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Message.Error = ex.Message;
                        Message.ShowError();
                        return false;
                    }
                }

                static public string[] DirectoryGetFiles(string path)
                {
                    try
                    {
                        if (Directory.Exists(path))
                            return Directory.GetFiles(path);
                        else
                            return null;
                    }
                    catch (Exception ex)
                    {
                        Message.Error = ex.Message;
                        Message.ShowError();
                        return null;
                    }
                }

                static public bool DirectoryCreate(string path)
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Message.Error = ex.Message;
                        Message.ShowError();
                        return false;
                    }
                }

                static public bool DirectoryExists(string path)
                {
                    try
                    {
                        return Directory.Exists(path);
                    }
                    catch (Exception ex)
                    {
                        Message.Error = ex.Message;
                        Message.ShowError();
                        return false;
                    }
                }

                static public bool FileExists(string fileName)
                {
                    try
                    {
                        return File.Exists(fileName);
                    }
                    catch (Exception ex)
                    {
                        Message.Error = ex.Message;
                        Message.ShowError();
                        return false;
                    }
                }

                /// <summary>
                /// Записывает string data в файл fileName
                /// </summary>
                /// <param name="fileName"></param>
                /// <param name="data"></param>
                /// <returns>возвращает true по окончании</returns>
                static public bool FileWrite(string fileName, string data)
                {
                    try
                    {
                        File.WriteAllText(fileName, data); // создать или перезаписать файл
                        return true;

                    }
                    catch (Exception ex)
                    {
                        Message.Error = ex.Message;
                        Message.ShowError();
                        return false;
                    }
                }


                /// <summary>
                /// Получает данные из файла
                /// </summary>
                /// <param name="fileName"></param>
                /// <returns>Возвращает одну строку</returns>
                static public string FileRead(string fileName)
                {
                    try
                    {
                        if (!File.Exists(fileName))
                            return null;
                        else
                        {
                            return File.ReadAllText(fileName);  // возвращаем одну строку

                        }
                    }
                    catch (Exception ex)
                    {
                        Message.Error = ex.Message;
                        Message.ShowError();
                        return null;
                    }

                }
                               
            }
        }
    }    
}


