using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace TortoiseHgManager
{
    class TortoiseHgClient : ProcessExecutor
    {
        public List<string> Repositories { get; private set; }

        private void DeleteFolder(string targetPath)
        {
            foreach (string file in Directory.GetFiles(targetPath, "*", SearchOption.AllDirectories)) File.Delete(file);
            Directory.Delete(targetPath, true);
        }

        private static void CopyFolder(string sourcePath, string destinationPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

            //Copy all the files
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath));
        }

        public TortoiseHgClient()
        {
            TraceLogEnabled = true;
            Name = "TortoiseHg Client";
            Application = "C:\\Program Files\\TortoiseHg\\hg.exe";
            Repositories = new List<string>();
        }

        public void LoadRepositories()
        {
            Repositories.Clear();
            string HgRepRoot = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string RepoRegFile = Path.Combine(HgRepRoot, ".\\TortoiseHg\\thg-reporegistry.xml");

            using (XmlReader reader = XmlReader.Create(RepoRegFile))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "repo")
                        {
                            string rootName = reader.GetAttribute("root").Replace('/', '\\');
                            Repositories.Add(rootName);
                        }
                    }
                }//while
            }//using
            Trace.WriteLine(Repositories.Count().ToString() + " repositories loaded.");
        }

        public string VerifyRepository(string repositoryPath)
        {
            Trace.WriteLine("");
            Trace.WriteLine("Verifying " + repositoryPath + "...");
            repositoryPath = Path.GetFullPath(repositoryPath);
            if (!Directory.Exists(repositoryPath))
            {
                Trace.WriteLine("Repository not exists: " + repositoryPath);
                return "NOT FOUND";
            }

            Arguments = "--repository \"" + repositoryPath + "\" verify -v";
            ProcessResult result = Execute();

            if (result.ExitCode != 0)
            {
                Trace.WriteLine("Verification Failed: " + repositoryPath + "\n" + "ERROR: " + string.Join("\r\n", result.Output));
                return "ERROR";
            }

            Trace.WriteLine(repositoryPath + " verified.");
            return "OK";
        }

        public string UpdateRepository(string repositoryPath)
        {
            Trace.WriteLine("");
            Trace.WriteLine("Updating " + repositoryPath + "...");
            repositoryPath = Path.GetFullPath(repositoryPath);
            if (!Directory.Exists(repositoryPath))
            {
                Trace.WriteLine("Repository not exists: " + repositoryPath);
                return "NOT FOUND";
            }

            string workingPath = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(repositoryPath);

            Arguments = "update --clean --rev tip --verbose";
            ProcessResult result = Execute();
            Directory.SetCurrentDirectory(workingPath);

            if (result.ExitCode != 0)
            {
                Trace.WriteLine("Update Failed: " + repositoryPath + "\n" + "ERROR: " + string.Join("\r\n", result.Output));
                return "ERROR";
            }

            Trace.WriteLine(repositoryPath + " OK.");
            return "OK";
        }

        public string FixRepository(string repositoryPath)
        {
            Trace.WriteLine("");
            Trace.WriteLine("Fixing " + repositoryPath + "...");
            repositoryPath = Path.GetFullPath(repositoryPath);
            if (!Directory.Exists(repositoryPath))
            {
                Trace.WriteLine("Repository not exists: " + repositoryPath);
                return "NOT FOUND";
            }

            string tempRepo = repositoryPath + "$";
            if (Directory.Exists(tempRepo)) DeleteFolder(tempRepo);
            Directory.CreateDirectory(tempRepo);

            Arguments = "convert --source-type hg --config convert.hg.ignoreerrors=True \"" + repositoryPath + "\" \"" + tempRepo + "\"";
            ProcessResult result = Execute();

            if (result.ExitCode != 0)
            {
                Trace.WriteLine("Failed to fix repository: " + repositoryPath + "\n" + "ERROR: " + string.Join("\r\n", result.Output));
                return "ERROR";
            }

            string oldHg = Path.Combine(repositoryPath, ".hg");
            string newHg = Path.Combine(tempRepo, ".hg");
            string oldHgBackup = Path.Combine(tempRepo, "$.hg");

            //Backup old .hg folder, update with new .hg folder.
            CopyFolder(oldHg, oldHgBackup);
            DeleteFolder(oldHg);
            CopyFolder(newHg, oldHg);
            DeleteFolder(tempRepo);

            Trace.WriteLine(repositoryPath + " Fixed.");
            return UpdateRepository(repositoryPath);
        }    
    
        public string PullIncomingChanges(string repositoryPath)
        {
            Trace.WriteLine("");
            Trace.WriteLine("Pulling to " + repositoryPath);
            repositoryPath = Path.GetFullPath(repositoryPath);
            if (!Directory.Exists(repositoryPath))
            {
                Trace.WriteLine("Repository not exists: " + repositoryPath);
                return "NOT FOUND";
            }

            Arguments = "--repository \"" + repositoryPath + "\" pull --verbose --force default";
            ProcessResult result = Execute();

            if (result.ExitCode != 0)
            {
                Trace.WriteLine("Failed to Pull changes to " + repositoryPath + "ERROR: " + string.Join("\r\n", result.Output));
                return "ERROR";
            }

            Trace.WriteLine("Pull completed.");
            return "OK";
        }

        public string PushOutgoingChanges(string repositoryPath)
        {
            Trace.WriteLine("");
            Trace.WriteLine("Pushing from " + repositoryPath);
            repositoryPath = Path.GetFullPath(repositoryPath);
            if (!Directory.Exists(repositoryPath))
            {
                Trace.WriteLine("Repository not exists: " + repositoryPath);
                return "NOT FOUND";
            }

            Arguments = "--repository \"" + repositoryPath + "\" push --verbose --force default";
            ProcessResult result = Execute();

            if (result.ExitCode != 0)
            {
                Trace.WriteLine("Failed to Push changes from " + repositoryPath + "ERROR: " + string.Join("\r\n", result.Output));
                return "ERROR";
            }

            Trace.WriteLine("Push completed.");
            return "OK";
        }
    }
}
