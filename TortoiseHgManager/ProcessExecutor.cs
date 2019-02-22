using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Management;
using System.ComponentModel;

namespace TortoiseHgManager
{
    /// <summary>
    /// Execution result returned from ProcessExecutor
    /// </summary>
    class ProcessResult : EventArgs
    {
        /// <summary>
        /// Standard output lines.
        /// </summary>
        public string[] Output { get; set; }
        /// <summary>
        /// Exit code return from process.
        /// </summary>
        public int ExitCode { get; set; }
        /// <summary>
        /// This flag is set if any stderr message is received.
        /// </summary>
        public bool ErrorDetected { get; set; }

        public ProcessResult() { Output = new string[] { }; }
    }

    /// <summary>
    /// Execute Console based application without showing the console window.
    /// </summary>
    class ProcessExecutor
    {
        ProcessStartInfo processInfo;
        List<string> outputData;
        private bool errorDetected;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessExecutor(string name = "Unnamed")
        {
            Name = name;
            TraceLogEnabled = false;
        }
        /// <summary>
        /// Instance Name
        /// </summary>
        [Category("General")]
        public string Name { get; set; }
        /// <summary>
        /// Application EXE - Full path.
        /// </summary>
        [Category("General")]
        public string Application { get; set; }
        /// <summary>
        /// Arguments that passed to application.
        /// </summary>
        [Browsable(false)]
        [Category("General")]
        public string Arguments { get; set; }
        /// <summary>
        /// Start location of command prompt.
        /// </summary>
        [Category("General")]
        public string WorkingDirectory { get; set; }
        /// <summary>
        /// Enable / Disable Trace Log
        /// </summary>
        [Browsable(false)]
        [Category("General")]
        public bool TraceLogEnabled { get; set; }
        
        /// <summary>
        /// Get Process Handler
        /// </summary>
        [Browsable(false)]
        [Category("General")]
        public Process ProcessHandler { get; private set; }

        private bool RedirectToFile;
        private string LogFile;

        /// <summary>
        /// Redirect standard output and standrad error to file. By default, standard output and
        /// standard error are stored in ProcessResult which returned at the end of Execute()
        /// </summary>
        /// <param name="logFile">Target file to store output logs.</param>
        public void RedirectOutputToFile(string logFile)
        {
            LogFile = logFile;
            RedirectToFile = true;
        }

        /// <summary>
        /// Execute process and return result in the form of ProcessResult.
        /// </summary>
        /// <remarks>No exception is expected from this application. Execution error will be reflected in ExitCode.</remarks>
        /// <returns></returns>
        public ProcessResult Execute(bool waitForExit = true)
        {
            ProcessResult result = new ProcessResult();
            outputData = new List<string>();
            errorDetected = false;
            try
            {
                processInfo = new ProcessStartInfo();
                processInfo.FileName = Application;
                if (Application.Contains(" ")) processInfo.FileName = "\"" + processInfo.FileName + "\"";
                processInfo.WorkingDirectory = WorkingDirectory;
                processInfo.Arguments = Arguments;
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                processInfo.RedirectStandardOutput = true;
                processInfo.RedirectStandardError = true;

                if(TraceLogEnabled) Trace.WriteLine(Name + ": Executing: " + processInfo.FileName + " " + processInfo.Arguments);
                ProcessHandler = Process.Start(processInfo);
                ProcessHandler.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
                ProcessHandler.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
                ProcessHandler.BeginOutputReadLine();
                ProcessHandler.BeginErrorReadLine();
                if (!waitForExit) return null;

                ProcessHandler.WaitForExit();
                result.ExitCode = ProcessHandler.ExitCode;
            }
            catch (Exception ex)
            {
                result.Output = new string[] { ex.ToString() };
                result.ErrorDetected = true;
                result.ExitCode = -999;
                return result;
            }
            ProcessHandler.OutputDataReceived -= process_OutputDataReceived;
            ProcessHandler.ErrorDataReceived -= process_ErrorDataReceived;

            result.ErrorDetected = errorDetected;
            result.Output = outputData.ToArray();
            return result;
        }

        /// <summary>
        /// Check if the specific tool exists.
        /// </summary>
        /// <returns></returns>
        public void Validate() 
        {
            if (!System.IO.File.Exists(Application))
            {
                throw new System.IO.FileNotFoundException(Application);
            }
        }

        /// <summary>
        /// Get tools version.
        /// </summary>
        /// <param name="command">Command to retrieve version number, e.g. "/?", "--version"</param>
        /// <returns>Version information, null if not success.</returns>
        protected virtual Version Version(string command)
        {
            if(string.IsNullOrEmpty(command)) return null;
            Arguments = command;
            ProcessResult procResult = Execute();
            if (procResult.ExitCode != 0) return null;

            for (int x = 0; x < procResult.Output.Length; x++)
            {
                //Get First non-empty line
                if (string.IsNullOrEmpty(procResult.Output[x])) continue;

                string firstLine = procResult.Output[x];
                string [] firstLineParams = firstLine.Split(' ');

                Version verInfo;
                int tVal;
                for (int n = 0; n < firstLineParams.Length; n++)
                {
                    //Find parameter which begin with number
                    if (int.TryParse(firstLineParams[n][0].ToString(), out tVal))
                    {
                        string verString = firstLineParams[n];
                        
                        //HACK: Trim end character "." for Visual Studio (devenv)
                        while (!int.TryParse(verString.Last().ToString(), out tVal))
                        {
                            verString = verString.Substring(0, verString.Length - 1);
                        }
                        //---- end HACK

                        try
                        {
                            verInfo = new Version(verString);
                            return verInfo;
                        }
                        catch { continue; }
                    }
                }
            }
            return null; //No output returned from command.
        }

        /// <summary>
        /// Stop the current process and all its child process.
        /// </summary>
        public void Abort()
        {
            if (ProcessHandler == null) return;
            if (ProcessHandler.HasExited) return;
            KillProcessAndChildren(ProcessHandler.Id);
        }

        #region [ Private Functions ]

        private static void KillProcessAndChildren(int pid)
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }
        private void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            //ToDo: Review exception : "Source array was not long enough. Check srcIndex and length, and the array's lower bounds."
            if (e.Data != null)
            {
                string data = e.Data;
                errorDetected = true;
                if (RedirectToFile) System.IO.File.AppendAllText(LogFile, data + "\r\n");
                else outputData.Add(data);
                if (TraceLogEnabled) Trace.WriteLine(Name + ": " + data);
            }
        }
        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //ToDo: Review exception : "Source array was not long enough. Check srcIndex and length, and the array's lower bounds."
            if (e.Data != null)
            {
                string data = e.Data;
                if (RedirectToFile) System.IO.File.AppendAllText(LogFile, data + "\r\n");
                else outputData.Add(data);
                if (TraceLogEnabled) Trace.WriteLine(Name + ": " + data);
            }
        }

        #endregion
    }
}
