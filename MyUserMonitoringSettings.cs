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

namespace MyUserMonitoringSettings
{

    public partial class MyUserMonitoringSettings : Component
    {
        public MY_USER_MONITORING_DATA_T settings = new MY_USER_MONITORING_DATA_T();
        private string FileName;
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
        public MyUserMonitoringSettings(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        public bool Init(string Filename)
        {
            bool Default;

            Default = false;

            this.FileName = Filename.Length > 0 ? Filename : this.DefaultFileName;

            if (File.Exists(FileName))
            {
                // Read data
                Load();
            }
            else
            {
                // Set default
                SetDefault();

                Default = true;
            }
            return Default;
        }
        public bool Save()
        {
            FileStream Stream = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            BinaryFormatter binform = new BinaryFormatter();
            binform.Serialize(Stream, settings);

            Stream.Close();

            return false;
        }
        private bool Load()
        {
            FileStream Stream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryFormatter binform = new BinaryFormatter();

            settings = (MY_USER_MONITORING_DATA_T)binform.Deserialize(Stream);

            Stream.Close();

            return false;
        }
    }
}
