using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace lab13
{
    static class MESLog
    {
        public static int Count()//количество записей в файле
        {
            int Count = 0;
            StreamReader reading = new StreamReader(@"D:\proga\MESlogfile.txt");
            string str = reading.ReadLine();
            while (str != null)
            {
                Count++;
                str = reading.ReadLine();
            }
            return Count;

        }
        public static void AddMessage(string message) // Добавить запись в файл
        {
            StreamWriter write = new StreamWriter(@"D:\proga\MESlogfile.txt", true);
            write.WriteLine(message + ' ' + DateTime.Now);
            write.Close();
        }
        public static void PrintAll() // Вывести всю информацию
        {
            StreamReader reading = new StreamReader(@"D:\proga\MESlogfile.txt");
            Console.Write(reading.ReadToEnd());
        }
        public static void Search() // Поиск записи
        {
            Console.WriteLine("Поиск по: 1-времени, 2-дате, 3-сообщению");
            string search = Console.ReadLine();
            switch (search)
            {
                case "1":
                    {
                        Console.WriteLine("Введите конец времен. интервала ( в формате Hour:Minutes) - ");
                        string timeMax = Console.ReadLine();
                        int HourMax = Convert.ToInt32(timeMax.Remove(2, 3));
                        int MinutesMax = Convert.ToInt32(timeMax.Remove(0, 3));
                        Console.WriteLine("Введите начало времен. интервала ( в формате Hour:Minutes ) - ");
                        string timeMin = Console.ReadLine();
                        int HourMin = Convert.ToInt32(timeMin.Remove(2, 3));
                        int MinutesMin = Convert.ToInt32(timeMin.Remove(0, 3));
                        StreamReader reading = new StreamReader(@"D:\proga\MESlogfile.txt");
                        for (int i = 0; i < Count(); i++)
                        {
                            string str = reading.ReadLine();
                            int index = str.IndexOf(":");
                            string HourS = str.Substring(index - 2, 2);
                            string MinutesS = str.Substring(index + 1, 2);
                            int Hour = Convert.ToInt32(HourS);
                            int Minutes = Convert.ToInt32(MinutesS);
                            if (Hour >= HourMin && Hour <= HourMax)
                            {
                                if (Minutes >= MinutesMin && Minutes <= MinutesMax || MinutesMin == 0 && MinutesMax == 0)
                                    Console.WriteLine(str);
                            }
                        }
                        reading.Close();
                    }
                    break;
                case "2":
                    {
                        Console.WriteLine("Введите дату ( в формате Day.Month.Year ) - ");
                        string data = Console.ReadLine();
                        StreamReader reading = new StreamReader(@"D:\proga\MESlogfile.txt");
                        for (int i = 0; i < Count(); i++)
                        {
                            string str = reading.ReadLine();
                            if (str.Contains(data))
                            {
                                Console.WriteLine(str);
                            }
                        }
                        reading.Close();
                    }
                    break;
                case "3":
                    {
                        Console.WriteLine("Введите сообщение - ");
                        string message = Console.ReadLine();
                        StreamReader reading = new StreamReader(@"D:\proga\MESlogfile.txt");
                        for (int i = 0; i < Count(); i++)
                        {
                            string str = reading.ReadLine();
                            if (str.Contains(message))
                            {
                                Console.WriteLine(str);
                            }
                        }
                        reading.Close();
                    }
                    break;
                default:
                    Console.WriteLine("Выбрано неверное число");
                    break;
            }
        }
        public static void Delete() // Оставить записи за текущий час
        {
            DateTime date = new DateTime();
            date = DateTime.Now;
            string tim = Convert.ToString(date);
            int indeX = tim.IndexOf(":");
            int HourMin = Convert.ToInt32(tim.Substring(indeX - 2, 2));
            int HourMax = HourMin + 1;
            StreamReader reading = new StreamReader(@"D:\proga\MESlogfile.txt");
            FileInfo fileInf = new FileInfo(@"D:\proga\MESlogfileTime.txt");
            StreamWriter sw = File.CreateText(fileInf.FullName);
            for (int i = 0; i < Count(); i++)
            {
                string str = reading.ReadLine();
                int index = str.IndexOf(":");
                string HourS = str.Substring(index - 2, 2);
                int Hour = Convert.ToInt32(HourS);
                if (Hour >= HourMin && Hour <= HourMax)
                {
                    sw.WriteLine(str);
                }
            }
            sw.Close();
            reading.Close();
        }
        static class MESDiskInfo
        {
            public static DriveInfo[] drives = DriveInfo.GetDrives();
            public static void PrintFreePlace(string nameDisk) // Cвободном месте на диске
            {
                AddMessage("Метод для вывода свободного места на диске - ");
                foreach (DriveInfo drive in drives)
                {
                    if (drive.Name.Contains(nameDisk) && drive.IsReady)
                        Console.WriteLine($"Свободное пространство на диске {drive.Name} - {drive.TotalFreeSpace / Math.Pow(10, 9)} GB");

                }
            }
            public static void FileSystem(string nameDisk) // Ин-фо о файлововой системе
            {
                AddMessage("Метод для вывода информации о файловой системе - ");
                foreach (DriveInfo drive in drives)
                {
                    if (drive.Name.Contains(nameDisk) && drive.IsReady)
                        Console.WriteLine($"Файловая система на диске {drive.Name} - {drive.DriveFormat}");
                }
            }
            public static void InfoDrivers() // Информация о диске
            {
                AddMessage("Метод для вывода информации о диске - ");
                foreach (DriveInfo drive in drives)
                {
                    Console.Write($"Имя диска: {drive.Name} ");
                    if (drive.IsReady)
                        Console.Write($"Объем диска: {drive.TotalSize / Math.Pow(10, 9)} GB\nСвободное пространство: {drive.TotalFreeSpace / Math.Pow(10, 9)} GB\nМетка тома: {drive.VolumeLabel}\n");
                    else
                        Console.WriteLine();
                }
            }
        }
        static class MESFileInfo
        {
            public static void Path(string path)// Путь к файлу
            {
                AddMessage("Метод для вывода пути к файлу - ");
                FileInfo fileInf = new FileInfo(path);
                if (fileInf.Exists)
                    Console.WriteLine($"Имя файла - {fileInf.Name}. Путь - {fileInf.FullName}");
                else
                {
                    Console.WriteLine("Файла с таким именем не существует!");
                }
            }

            public static void SRN(string path) // Размер, расширение, имя
            {
                AddMessage("Метод для вывода размера, разрешения, имя файла - ");
                FileInfo fileInf = new FileInfo(path);
                if (fileInf.Exists)
                    Console.WriteLine($"Имя файла - {fileInf.Name}. Размер - {fileInf.Length}. Расширение - {fileInf.Extension}");
                else
                {
                    Console.WriteLine("Файла с таким именем не существует!");
                }
            }

            public static void TimeOfCreation(string path) // Время создания
            {
                AddMessage("Метод для вывода времени создания файла - ");
                FileInfo fileInf = new FileInfo(path);
                if (fileInf.Exists)
                    Console.WriteLine($"Имя файла - {fileInf.Name}. Время создания - {fileInf.CreationTime}");
                else
                {
                    Console.WriteLine("Файла с таким именем не существует!");
                }
            }
        }
        static class MESDirInfo
        {
            public static void NumberOfFiles(string path) // Количество файлов
            {
                AddMessage("Метод для вывода количества файлов в директории- ");
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (dirInfo.Exists)
                    Console.WriteLine($"Имя директории - {dirInfo.Name}. Количество файлов - {dirInfo.GetFiles().Length}");
                else
                {
                    Console.WriteLine("Директории с таким именем не существует!");
                }
            }

            public static void TimeOfCreation(string path) // Время создания
            {
                AddMessage("Метод для вывода время создания директории - ");
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (dirInfo.Exists)
                    Console.WriteLine($"Имя директории - {dirInfo.Name}. Время создания - {dirInfo.CreationTime}");
                else
                {
                    Console.WriteLine("Директории с таким именем не существует!");
                }
            }

            public static void NumberOfDir(string path) // Количесвто поддиректорий
            {
                AddMessage("Метод для вывода количества поддиректорий - ");
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (dirInfo.Exists)
                    Console.WriteLine($"Имя директории - {dirInfo.Name}. Количество поддиректории - {dirInfo.GetDirectories().Length}");
                else
                {
                    Console.WriteLine("Директории с таким именем не существует!");
                }
            }

            public static void Parent(string path) // Список родительских поддиректорий
            {
                AddMessage("Метод для вывода списка подительских поддиректорий - ");
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (dirInfo.Exists)
                {
                    Console.Write($"Имя директории - {dirInfo.Name}. Родителские поддерикторий: ");
                    while (dirInfo != null)
                    {
                        Console.Write($"{dirInfo = dirInfo.Parent} ");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Директории с таким именем не существует!");
                }
            }
        }
        static class MESFileManager
        {
            public static DriveInfo[] drives = DriveInfo.GetDrives();
            public static void Method1(string nameDisk)
            {
                AddMessage("Вызван первый метод FileMenager - ");
                foreach (DriveInfo drive in drives)
                {
                    if (drive.Name.Contains(nameDisk) && drive.IsReady)
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(drive.Name);
                        if (dirInfo.Exists)
                        {
                            foreach (var item in dirInfo.GetFiles())
                            {
                                Console.WriteLine($"Имя файла - {item}");
                            }
                            foreach (var item in dirInfo.GetDirectories())
                            {
                                Console.WriteLine($"Имя папки - {item}");
                            }
                            DirectoryInfo dirInfo2 = new DirectoryInfo(@"D:\proga\OOP");
                            dirInfo2.CreateSubdirectory("MESInspect");
                            FileInfo fileInf = new FileInfo(@"D:\proga\OOP\MESInspect\mesdirinfo.txt");
                            StreamWriter sw = File.CreateText(fileInf.FullName);
                            sw.WriteLine("ИНФОРМАЦИЯ");
                            sw.Close();
                            fileInf.CopyTo(@"D:\proga\OOP\MESInspect\newfile.txt", true);
                            fileInf.Delete();
                        }
                        else
                        {
                            Console.WriteLine("Директории с таким именем не существует!");
                        }
                    }
                }
            }

            public static void Method2(string path)
            {
                AddMessage("Вызван второй метод FileMenager - ");
                DirectoryInfo dirInfo = new DirectoryInfo(@"D:\proga\OOP");
                dirInfo.CreateSubdirectory("MESFiles");
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                DirectoryInfo dirInfo2 = new DirectoryInfo(path);
                FileInfo[] fileInf = dirInfo2.GetFiles("*.txt", SearchOption.AllDirectories);
                foreach (var item in fileInf)
                {
                    item.CopyTo($@"D:\proga\OOP\MESFiles\{item.Name}", true);
                }
                DirectoryInfo dir = new DirectoryInfo(@"D:\proga\OOP\MESInspect\");
                dir.CreateSubdirectory("MESFiles");
                if (!dir.Exists)
                {
                    dir.Create();
                }
                DirectoryInfo dir2 = new DirectoryInfo(@"D:\proga\OOP\MESFiles\");
                FileInfo[] fileInf2 = dir2.GetFiles();
                foreach (var item in fileInf2)
                {
                    item.CopyTo($@"D:\proga\OOP\MESInspect\MESFiles\{item.Name}", true);
                }
                dir2.Delete(true);
            }
        }
        class Program
        {
            static void Main(string[] args)
            {
                MESDiskInfo.PrintFreePlace("C");
                MESDiskInfo.FileSystem("D");
                MESDiskInfo.InfoDrivers();
                MESFileInfo.Path(@"D:\proga\OOP\file1.txt");
                MESFileInfo.Path(@"D:\proga\OOP\file2.txt");
                MESFileInfo.SRN(@"D:\proga\OOP\file3.txt");
                MESFileInfo.TimeOfCreation(@"D:\proga\OOP\file4.txt");
                MESDirInfo.NumberOfFiles(@"D:\proga\OOP");
                MESDirInfo.TimeOfCreation(@"D:\proga\OOP");
                MESDirInfo.NumberOfDir(@"D:\proga\OOP");
                MESDirInfo.Parent(@"D:\proga\OOP");
                MESFileManager.Method1("D");
                MESFileManager.Method2(@"D:\proga\OOP");
                Console.WriteLine("Количество записей: " + Count());
                Search();
                Search();
                Search();
                Delete();
                Console.ReadKey();
            }
        }
    }
    
}
