using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HMUI.Classes
{
   public class HSProjectManager
    {
       private static HSProjectManager instance;
       private static OleDbConnection m_dbCon;
       private static string m_ProjectPath;
       private static int m_nProjectID;

       public static HSProjectManager Instance
       {
           get
           {
               if (instance == null)
               {
                   instance = new HSProjectManager(m_ProjectPath);
               }
               return instance;
           }
       }

       public static int ProjectID
       {
           get { return m_nProjectID; }
       }
       
        /// <summary>
        /// Returns true when there is a project currently open
        /// </summary>
        public bool IsCurrentProject
        {
            get { return m_dbCon is System.Data.OleDb.OleDbConnection; }
        }

        public System.Data.OleDb.OleDbConnection ProjectDatabaseConnection
        {
            get 
            { 
                return m_dbCon;
            }
            set
            {
                m_dbCon = value;
            }
        }

        public HSProjectManager(string sProjectPath)
        {
            if (!string.IsNullOrEmpty(sProjectPath))
            {
            m_ProjectPath = sProjectPath;
            OpenProjectDatabase(sProjectPath);
            }

            //m_dbCon = null;
        }

        public HSProjectManager()
        {
            //OpenProjectDatabaseDialog();
        }

        public void Reset()
        {
            instance = null;
        }

        public static string ProjectPath
        {
            get
            { 
                return m_ProjectPath; 
            }
            //set
            //{
            //    m_ProjectPath = value;
            //}
        }

        public static string ProjectFolder
        {
            get { return System.IO.Path.GetDirectoryName(m_ProjectPath); }
        }

        public void CreateNewProjectDatabase(string sProjectPath)
        {
            // 1. Get the path of the executing assembly (software).
            // 2. Find the master copy of the habitat database 
            // 3. Make a copy in the project path location.
            // 4. Instantiate a new copy of the project database.

            if (System.IO.File.Exists(sProjectPath))
            {
                MessageBox.Show("The project database path already exists at: " + sProjectPath,"Unable to Create Project", MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }

            string sPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            sPath = System.IO.Path.GetDirectoryName(sPath);
            sPath = System.IO.Path.Combine(sPath,"HabitatModel.accdb");
            //sPath = @"C:\Users\A01674762\Desktop\Professional\CHaMP\HabitatModel\HabitatModel\HMUI\HabitatModel.accdb";

             if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(sProjectPath)))
             {
                 System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sProjectPath));
             }

            System.IO.File.Copy(sPath, sProjectPath);

            if (System.IO.File.Exists(sProjectPath))
            {
                m_ProjectPath = sProjectPath;
                OpenProjectDatabase(sProjectPath);
            }
       }

        public void CloseCurrentProject()
        {
            // Save the dataset changees to the database
            // close the connection

            if (m_dbCon is System.Data.OleDb.OleDbConnection)
                if (m_dbCon.State == System.Data.ConnectionState.Open)
                    m_dbCon.Close();

            m_ProjectPath = string.Empty;
            m_dbCon = null;
        }

        public void OpenProjectDatabase(string sProjectPath)
        {
            if (!System.IO.File.Exists(sProjectPath))
            {
                MessageBox.Show("The master database file does not exist at " + sProjectPath, "Error Opening Project", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sProjectPath;
            System.Diagnostics.Debug.WriteLine("Opening project database at: " + sProjectPath);
            m_dbCon = new OleDbConnection(sConString);
            m_dbCon.Open();
            m_ProjectPath = sProjectPath;

            // Get the project ID as the project is loaded.
            CHaMPWorkbench.Habitat.dsHabitatTableAdapters.ProjectsTableAdapter daProjects = new CHaMPWorkbench.Habitat.dsHabitatTableAdapters.ProjectsTableAdapter();
            daProjects.Connection = m_dbCon;
            CHaMPWorkbench.Habitat.dsHabitat.ProjectsDataTable taProjects = new CHaMPWorkbench.Habitat.dsHabitat.ProjectsDataTable();
            m_nProjectID = 0;
            daProjects.Fill(taProjects);
                if (taProjects.Rows.Count >0)
                m_nProjectID = taProjects.First().ProjectID;

        }

        public void OpenProjectDatabaseDialog()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Habitat Model Database";
            dlg.Filter = "Habitat Model Databases (*.accdb)|*.accdb|All Files (*.*)|*.*";
            dlg.CheckFileExists = true;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OpenProjectDatabase(dlg.FileName);
            }
            //OpenProjectDatabase(dlg.FileName);
            //UpdateLastUsedSettingsDatabase(dlg.FileName);
        }
    }
}
