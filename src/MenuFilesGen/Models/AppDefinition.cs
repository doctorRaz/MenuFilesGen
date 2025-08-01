﻿namespace MenuFilesGen.Models
{
    /// <summary> Общие данные по аддонам инструментов  </summary>
    public class AppDefinition
    {
        /// <summary> Родительская панель. Если null, то панель не является подчиненной </summary>
        //public string Parent { get; set; }

        /// <summary> Имя панели </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the addon.
        /// </summary>
        /// <value>
        /// The addon.
        /// </value>
        public List<AddonDefinition> Addons { get; set; }

        /// <summary>
        /// Gets or sets the panels.
        /// </summary>
        /// <value>
        /// The panels.
        /// </value>
        public List<PanelDefinition> Panels { get; set; }
    }
}
