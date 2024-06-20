using System.IO;
#if UNITY_ANDROID
using Unity.SharpZipLib.GZip;
using Unity.SharpZipLib.Tar;
public class Utility_SharpZipCommands
{
    //// Calling example
    //CreateTarGZ(@"c:\temp\gzip-test.tar.gz", @"c:\data");
    // Package to be downloaded using the package manager: com.unity.sharp-zip-lib

    //USE THIS:
    public static void CreateTarGZ_FromDirectory(string tgzFilename, string sourceDirectory)
    {

        Stream outStream = File.Create(tgzFilename);
        Stream gzoStream = new GZipOutputStream(outStream);
        TarArchive tarArchive = TarArchive.CreateOutputTarArchive(gzoStream);

        // Note that the RootPath is currently case sensitive and must be forward slashes e.g. "c:/temp"
        // and must not end with a slash, otherwise cuts off first char of filename
        // This is scheduled for fix in next release
        tarArchive.RootPath = sourceDirectory.Replace('\\', '/');
        if (tarArchive.RootPath.EndsWith("/"))
            tarArchive.RootPath = tarArchive.RootPath.Remove(tarArchive.RootPath.Length - 1);

        AddDirectoryFilesToTar(tarArchive, sourceDirectory, true);

        tarArchive.Close();
    }

    public static void AddDirectoryFilesToTar(TarArchive tarArchive, string sourceDirectory, bool recurse)
    {

        // Optionally, write an entry for the directory itself.
        // Specify false for recursion here if we will add the directory's files individually.
        //
        TarEntry tarEntry = TarEntry.CreateEntryFromFile(sourceDirectory);
        tarArchive.WriteEntry(tarEntry, false);

        // Write each file to the tar.
        //
        string[] filenames = Directory.GetFiles(sourceDirectory);
        foreach (string filename in filenames)
        {
            tarEntry = TarEntry.CreateEntryFromFile(filename);
            tarArchive.WriteEntry(tarEntry, true);
        }

        if (recurse)
        {
            string[] directories = Directory.GetDirectories(sourceDirectory);
            foreach (string directory in directories)
                AddDirectoryFilesToTar(tarArchive, directory, recurse);
        }
    }

    public static void ExtractTGZ(string gzArchiveName, string destFolder)
    {
        Stream inStream = File.OpenRead(gzArchiveName);
        Stream gzipStream = new GZipInputStream(inStream);

        TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
        tarArchive.ExtractContents(destFolder);
        tarArchive.Close();

        gzipStream.Close();
        inStream.Close();

    }

}    // Calling example
#endif
