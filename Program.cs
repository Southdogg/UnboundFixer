using System;
using System.IO;

namespace UnboundFixer
{
    internal class Program
    {
        #region inDocumentsMoves
        static DirectoryInfo SearchPathInDocuments()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            try
            {
                foreach (var Path in Directory.GetDirectories(documentsPath))
                {
                    DirectoryInfo documentGamePath = new DirectoryInfo(Path);
                    if (documentGamePath.Name == "Need For Speed(TM) Unbound")
                    {
                        return documentGamePath;
                    }
                }
                return new DirectoryInfo(@"D:\"); ;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Отказанно в Доступе");
            }
            return new DirectoryInfo(@"D:\");
        }
        static void MakePatchInDocuments(DirectoryInfo userDocumentGamePath)
        {
            DirectoryInfo[] inGameSettings = userDocumentGamePath.GetDirectories();
            foreach (var file in inGameSettings)
            {
                if (file.Name == "SaveGame" | file.Name == "Screenshots")
                    continue;
                Console.WriteLine(file.Name + " Fixed");
                file.Delete();
            }
        }
        #endregion
        #region SearchGameFolder
        static string SearchGameFolderInDrives(string currentDirectory)
        {
            try
            {
                string[] subDirectories = Directory.GetDirectories(currentDirectory);
                foreach (var subDirectory in subDirectories) 
                {
                    if(Path.GetFileName(subDirectory).Equals("Excalibur"))
                    {
                        Console.WriteLine($"Найденно: {subDirectory}");
                        return subDirectory;
                    }
                    string foundPathInString = SearchGameFolderInDrives(subDirectory);
                    if(!string.IsNullOrEmpty(foundPathInString))
                    {
                        return foundPathInString;
                    }
                }

            }
            catch(UnauthorizedAccessException)
            {
            }
            catch(Exception ex)
            {
                Console.WriteLine("________ERROR________");
            }
            return null;
        }
        static void MakePatchInGameFolder(string userGamePath)
        {
            string[] InGameFolderDirectories = Directory.GetDirectories(userGamePath);
            foreach (var directory in InGameFolderDirectories)
            {
                //Console.WriteLine("\t" + directory);
                if (Path.GetFileName(directory).Equals("shadercache"))
                {
                    DirectoryInfo shadercacheDirectory = new DirectoryInfo(directory);
                    DirectoryInfo parentShadercache = shadercacheDirectory.Parent;
                    shadercacheDirectory.Delete(true);
                    Console.WriteLine("\t" + "\t" + directory + " Delited");
                    Directory.CreateDirectory(parentShadercache.FullName + "/shadercache/");
                    Console.WriteLine("\t" + "\t" + directory + " Created");
                }
            }
        }
        #endregion

        static void Main(string[] args)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            string userGameFolder = null;
            foreach (var drive in drives)
            {
                if (userGameFolder == null)
                {
                    Console.WriteLine($"Поиск в: {drive.Name}");
                    userGameFolder = SearchGameFolderInDrives(drive.Name);
                }
                else
                    break;
            }
            MakePatchInGameFolder(userGameFolder);
            DirectoryInfo documentGameFolder =  SearchPathInDocuments();
            MakePatchInDocuments(documentGameFolder);
            Console.WriteLine("Complite");
            Console.WriteLine("\n..######...#######..##.....##.########.##.....##.########...#######...######....######.." +
                              "\n.##....##.##.....##.##.....##....##....##.....##.##.....##.##.....##.##....##..##....##." +
                              "\n.##.......##.....##.##.....##....##....##.....##.##.....##.##.....##.##........##......." +
                              "\n..######..##.....##.##.....##....##....#########.##.....##.##.....##.##...####.##...####" +
                              "\n.......##.##.....##.##.....##....##....##.....##.##.....##.##.....##.##....##..##....##." +
                              "\n.##....##.##.....##.##.....##....##....##.....##.##.....##.##.....##.##....##..##....##." +
                              "\n..######...#######...#######.....##....##.....##.########...#######...######....######..");
            Console.ReadKey();
        }
    }
}
