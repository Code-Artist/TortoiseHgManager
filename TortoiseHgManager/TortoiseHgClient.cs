using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.ComponentModel;

namespace TortoiseHgManager
{
    class TortoiseHgRepository
    {
        [ReadOnly(true)]
        public string Group { get; set; }
        [ReadOnly(true)]
        public string ShortName { get; set; }
        [ReadOnly(true)]
        public string Path { get; set; }
        [ReadOnly(true)]
        public bool Error { get; set; } = false;
        [ReadOnly(true)]
        public string ErrorString { get; set; } = string.Empty;
        [ReadOnly(true)]
        public bool Selected { get; set; } = true;
    }

    class TortoiseHgClient : ProcessExecutor
    {
        public List<TortoiseHgRepository> Repositories { get; private set; }

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
            Repositories = new List<TortoiseHgRepository>();
        }

        #region [Repositories Manager]

        public void LoadRepositories()
        {
            Repositories.Clear();
            string HgRepRoot = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string RepoRegFile = Path.Combine(HgRepRoot, ".\\TortoiseHg\\thg-reporegistry.xml");

            string group = string.Empty;
            using (XmlReader reader = XmlReader.Create(RepoRegFile))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.EndElement) continue;
                    if (reader.Name == "repo")
                    {
                        string rootName = reader.GetAttribute("root")?.Replace('/', '\\');
                        Repositories.Add(new TortoiseHgRepository()
                        {
                            Group = group,
                            ShortName = reader.GetAttribute("shortname"),
                            Path = rootName
                        });
                    }
                    else if ((reader.Name == "allgroup") || (reader.Name == "group"))
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                            group = reader.GetAttribute("name");
                        else if (reader.NodeType == XmlNodeType.EndElement)
                            group = string.Empty;
                    }
                }//while
            }//using
            Trace.WriteLineIf(TraceLogEnabled, Repositories.Count().ToString() + " repositories loaded.");
        }

        public void ClearRepositoriesError()
        {
            foreach (TortoiseHgRepository repo in Repositories)
            {
                repo.Error = false;
                repo.ErrorString = "";
            }
        }

        public IList<TortoiseHgRepository> GetSelectedRepositories()
        {
            return Repositories.Where(x => x.Selected == true).ToList();
        }

        public IList<TortoiseHgRepository> GetErrorRepositories()
        {
            return Repositories.Where(x => x.Error == true).ToList();
        }

        public IList<TortoiseHgRepository> GetRepositories(string group)
        {
            return Repositories.Where(x => x.Group == group).ToList();
        }

        #endregion

        #region [Mercurial Function]

        private void RaiseDirectoryNotFoundException(string repositoryPath)
        {
            string errMsg = "ERROR: Repository not exists: " + repositoryPath;
            Trace.WriteLine(errMsg);
            throw new DirectoryNotFoundException(errMsg);
        }

        private void RaiseTortoiseHgException(string operation, string repositoryPath, string messages)
        {
            string errMsg = "ERROR: Operation Failed: " + Path.GetFileName(repositoryPath) + " " + messages;
            Trace.WriteLine(errMsg);
            throw new TortoiseHgException(errMsg);
        }

        public void VerifyRepository(string repositoryPath)
        {
            Trace.WriteLineIf(TraceLogEnabled, "");
            Trace.WriteLineIf(TraceLogEnabled, "Verifying " + repositoryPath + "...");
            repositoryPath = Path.GetFullPath(repositoryPath);
            if (!Directory.Exists(repositoryPath)) RaiseDirectoryNotFoundException(repositoryPath);

            Arguments = "--repository \"" + repositoryPath + "\" verify -v";
            ProcessResult result = Execute();

            if (result.ExitCode != 0) RaiseTortoiseHgException("Verification", repositoryPath, String.Join("\r\n", result.Output));
            else Trace.WriteLineIf(TraceLogEnabled, repositoryPath + " verified.");
        }

        public void UpdateRepository(string repositoryPath)
        {
            Trace.WriteLineIf(TraceLogEnabled, "");
            Trace.WriteLineIf(TraceLogEnabled, "Updating " + repositoryPath + "...");
            repositoryPath = Path.GetFullPath(repositoryPath);
            if (!Directory.Exists(repositoryPath)) RaiseDirectoryNotFoundException(repositoryPath);

            string workingPath = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(repositoryPath);

            Arguments = "update --clean --rev tip --verbose";
            ProcessResult result = Execute();
            Directory.SetCurrentDirectory(workingPath);

            if (result.ExitCode != 0) RaiseTortoiseHgException("Update", repositoryPath, String.Join("\\r\n", result.Output));
            Trace.WriteLineIf(TraceLogEnabled, repositoryPath + " OK.");
        }

        private void VerifyExtensionActivated(string repositoryPath, string extension)
        {
            string mercurialINIPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "mercurial.ini");
            if (!File.Exists(mercurialINIPath)) throw new FileNotFoundException("File Not Found!", mercurialINIPath);

            string[] lines = File.ReadAllLines(mercurialINIPath);
            foreach (string line in lines)
            {
                if (line.StartsWith(extension)) return;
            }
            RaiseTortoiseHgException("Fix Repository", repositoryPath, "Extension [" + extension + "] not activated!");
        }

        public void FixRepository(string repositoryPath)
        {
            VerifyExtensionActivated(repositoryPath, "convert");

            Trace.WriteLineIf(TraceLogEnabled, "");
            Trace.WriteLineIf(TraceLogEnabled, "Fixing " + repositoryPath + "...");
            repositoryPath = Path.GetFullPath(repositoryPath);
            if (!Directory.Exists(repositoryPath)) RaiseDirectoryNotFoundException(repositoryPath);

            string tempRepo = repositoryPath + "$";
            if (Directory.Exists(tempRepo)) DeleteFolder(tempRepo);
            Directory.CreateDirectory(tempRepo);

            Arguments = "convert --source-type hg --config convert.hg.ignoreerrors=True \"" + repositoryPath + "\" \"" + tempRepo + "\"";
            ProcessResult result = Execute();

            if (result.ExitCode != 0) RaiseTortoiseHgException("Fix Repository", repositoryPath, string.Join("\r\n", result.Output));

            string oldHg = Path.Combine(repositoryPath, ".hg");
            string newHg = Path.Combine(tempRepo, ".hg");
            string oldHgBackup = Path.Combine(tempRepo, "$.hg");

            //Backup old .hg folder, update with new .hg folder.
            CopyFolder(oldHg, oldHgBackup);
            DeleteFolder(oldHg);
            CopyFolder(newHg, oldHg);
            DeleteFolder(tempRepo);

            Trace.WriteLineIf(TraceLogEnabled, repositoryPath + " Fixed.");
        }

        public void PullIncomingChanges(string repositoryPath)
        {
            Trace.WriteLineIf(TraceLogEnabled, "");
            Trace.WriteLineIf(TraceLogEnabled, "Pulling to " + repositoryPath);
            repositoryPath = Path.GetFullPath(repositoryPath);
            if (!Directory.Exists(repositoryPath)) RaiseDirectoryNotFoundException(repositoryPath);

            Arguments = "--repository \"" + repositoryPath + "\" pull --verbose --force default";
            ProcessResult result = Execute();

            if (result.ExitCode != 0) RaiseTortoiseHgException("Pull Changes", repositoryPath, String.Join("\r\n", result.Output));
            Trace.WriteLineIf(TraceLogEnabled, "Pull completed.");
        }

        public void PushOutgoingChanges(string repositoryPath)
        {
            Trace.WriteLineIf(TraceLogEnabled, "");
            Trace.WriteLineIf(TraceLogEnabled, "Pushing from " + repositoryPath);
            repositoryPath = Path.GetFullPath(repositoryPath);
            if (!Directory.Exists(repositoryPath)) RaiseDirectoryNotFoundException(repositoryPath);

            Arguments = "--repository \"" + repositoryPath + "\" push --verbose --force default";
            ProcessResult result = Execute();

            if (result.ExitCode != 0) RaiseTortoiseHgException("Push Changes", repositoryPath, String.Join("\r\n", result.Output));
            Trace.WriteLineIf(TraceLogEnabled, "Push completed.");
        }

        public void CheckModifiedRepository(string repositoryPath)
        {
            Trace.WriteLineIf(TraceLogEnabled, "Checking " + repositoryPath + "...");
            Arguments = "--repository \"" + repositoryPath + "\" status";
            ProcessResult result = Execute();

            if (result.Output.Length != 0)
            {
                foreach (string line in result.Output)
                {
                    if (!line.StartsWith("?"))
                    {
                        RaiseTortoiseHgException("Check For Changes", repositoryPath, "Repository contains uncommitted changes.");
                    }
                }
            }
            Trace.WriteLineIf(TraceLogEnabled, repositoryPath + " have no changes.");
        }

        #endregion  
    }
}
