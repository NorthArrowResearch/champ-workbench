using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CHaMPWorkbench.Habitat;

namespace HMDesktop.Classes
{
    public class HSProjectManager
    {
        private static HSProjectManager instance;
        private static dsHabitat m_ProjectDS;
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
            get { return m_ProjectDS is dsHabitat; }
        }

        public dsHabitat ProjectDatabase
        {
            get
            {
                return m_ProjectDS;
            }
            set
            {
                m_ProjectDS = value;
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

        public void Save()
        {
            if (m_ProjectDS is dsHabitat)
            {
                m_ProjectDS.AcceptChanges();
                m_ProjectDS.WriteXml(m_ProjectPath);
            }
        }

        public void CloseCurrentProject()
        {
            // Save the dataset changees to the database
            Save();
            m_ProjectPath = string.Empty;
            m_ProjectDS = null;
        }

        public void OpenProjectDatabase(string sProjectPath)
        {
            m_ProjectDS = new dsHabitat();
            m_ProjectDS.ReadXml(sProjectPath);
            m_ProjectDS.AcceptChanges();
            m_ProjectPath = sProjectPath;

            // Get the project ID as the project is loaded.
            if (m_ProjectDS.Projects.Count > 0)
            {
                dsHabitat.ProjectsRow rProject = m_ProjectDS.Projects[0];
                if (rProject is dsHabitat.ProjectsRow)
                {
                    m_nProjectID = rProject.ProjectID;
                }
            }
        }

        public void OpenProjectDatabaseDialog()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Habitat Model Database";
            dlg.Filter = "Habitat Model Databases (*.xml)|*.xml|All Files (*.*)|*.*";
            dlg.CheckFileExists = true;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OpenProjectDatabase(dlg.FileName);
            }
        }
    }
}
