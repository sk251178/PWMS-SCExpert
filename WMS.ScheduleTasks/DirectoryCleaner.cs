using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;

namespace WMS.ScheduleTasks
{
    public class DirectoryCleaner
    {
        public DirectoryCleaner()
        {

        }

        //public void ClearDirectory(string FolderName, int DeleteInSubFolders, int DaysOld, string Filter)
        public void ClearDirectory(string FolderName, bool DeleteInSubFolders, int DaysOld, string Filter)
        {
            DateTime TimeStamp;

            try
            {
                TimeStamp = DateTime.Now.AddDays(-1 * Convert.ToDouble(DaysOld));
            }
            catch (Exception )
            {

                return;
            }

            DirectoryInfo SelectedDir = new DirectoryInfo(FolderName);
            if (SelectedDir.Exists == false)
            {

            }
            // Delete from Local Directory
            try
            {
                foreach (FileInfo DirFile in SelectedDir.GetFiles(Filter))
                {
                    if (DirFile.CreationTime <= TimeStamp)
                    {

                    }
                    try
                    {
                        DirFile.Delete();
                    }
                    catch (Exception )
                    {

                    }
                }
            }
            catch (Exception )
            {

            }


            // Delete from Sub Directories
                if (DeleteInSubFolders)
                {
                    System.IO.DirectoryInfo[] DirInfoArr = SelectedDir.GetDirectories();
                    foreach (DirectoryInfo SubDir in DirInfoArr)
                    {
                        foreach (FileInfo DirFile in SubDir.GetFiles(Filter))
                        {
                            if (DirFile.CreationTime <= TimeStamp)
                            {
                                try
                                {
                                    DirFile.Delete();
                                }
                                catch (Exception )
                                {

                                }
                            }
                        }
                    }
                }
        }

    }
}