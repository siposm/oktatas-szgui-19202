﻿using folderlister.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace folderlister.BusinessLogic
{
    class DirectoryLogic : IDirectoryLogic
    {
        public IEnumerable<DirectoryEntry> CollectDirectoryEntries(string currentDirectory)
        {
            var entries = new List<DirectoryEntry>();
            if (currentDirectory == string.Empty)
            {
                foreach (DriveInfo akt in DriveInfo.GetDrives())
                    entries.Add(new DirectoryEntry(akt.Name, true));
            }
            else
            {
                try
                {
                    foreach (string subDir in Directory.GetDirectories(currentDirectory))
                        entries.Add(new DirectoryEntry(subDir, true));

                    foreach (string file in Directory.GetFiles(currentDirectory))
                        entries.Add(new DirectoryEntry(file, false));
                }
                catch (Exception) { }
            }
            return entries;
        }

        public void SelectEntry(DirectoryEntry entry)
        {
            if (entry == null) return;
            if (entry.IsDir)
            {
                // Window dependency => extract to service call
                MainWindow window = new MainWindow(entry.Name);
                window.Show();
            }
            else
                Process.Start(entry.Name);
        }

        public void LogCurrent(string currentDirectory)
        {
            string filename = "_LOGGED_folders_and_files.txt";

            StreamWriter sw = new StreamWriter(filename);

            foreach (string subDir in Directory.GetDirectories(currentDirectory))
                sw.WriteLine("[DIR]\t" + subDir);

            foreach (string file in Directory.GetFiles(currentDirectory))
                sw.WriteLine("[FILE]\t" + file);

            sw.Close();

            Process.Start(filename);
        }
    }
}
