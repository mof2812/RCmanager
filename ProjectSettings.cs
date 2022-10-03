using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace ProjectSettings
{

    public partial class ProjectSettings : Component
    {
        public PROJECT_SETTINGS_T settings = new PROJECT_SETTINGS_T();
        private string InitialPath;
        private string DefaultFileName;
        #region Properties
        [
        Category("File"),
        Description("Default data filename"),
        DefaultValue("")
        ]
        public string DefaultDataFileName
        {
            get
            {
                return this.DefaultFileName;
            }
            set
            {
                DefaultFileName = value;
            }
        }
        #endregion
        public ProjectSettings(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        public bool Init(string Filename)
        {
            bool Error;

            Error = false;

            return Error;

            //this.FileName = Filename.Length > 0 ? Filename : this.DefaultFileName;

            //if (File.Exists(FileName))
            //{
            //    // Read data
            //    Load();
            //}
            //else
            //{
            //    // Set default
            //    SetDefault();

            //    Default = true;
            //}
            //return Default;
        }
        public DialogResult Open(ref string Path)
        {
            DialogResult Result;
            OpenFileDialog Dlg = new OpenFileDialog()
            {
                Multiselect = false,
                InitialDirectory = Path,
                Filter = "RCmanager-Projects (*.rcm)|*.rcm",
                Title = "Projekt auswählen"
            };

            Result = Dlg.ShowDialog();

            if (Result == DialogResult.OK)
            {
                Load(InitialPath = Dlg.FileName);
            }

            Path = Dlg.FileName;

            return Result;
        }
        public bool Save(string FileName)
        {
            FileStream Stream = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            BinaryFormatter binform = new BinaryFormatter();
            binform.Serialize(Stream, settings);

            Stream.Close();

            return false;
        }
        public DialogResult Save()
        {
            DialogResult Result;

            Result = DialogResult.OK;

            FileStream Stream = new FileStream(InitialPath, FileMode.Create, FileAccess.Write);
            BinaryFormatter binform = new BinaryFormatter();
            binform.Serialize(Stream, settings);

            Stream.Close();

            return Result;
        }
        public DialogResult SaveAs(ref string Path)
        {
            DialogResult Result;
            SaveFileDialog Dlg = new SaveFileDialog()
            {
                InitialDirectory = Path,
                Filter = "RCmanager-Projects (*.rcm)|*.rcm",
                Title = "Projekt sichern unter"
            };

            Result = Dlg.ShowDialog();

            if (Result == DialogResult.OK)
            {
                Save(InitialPath = Dlg.FileName);
            }

            Path = Dlg.FileName;

            return Result;
        }
        private bool Load(string FileName)
        {
            FileStream Stream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryFormatter binform = new BinaryFormatter();

            settings = (PROJECT_SETTINGS_T)binform.Deserialize(Stream);

            Stream.Close();

            return false;
        }
    }
}
