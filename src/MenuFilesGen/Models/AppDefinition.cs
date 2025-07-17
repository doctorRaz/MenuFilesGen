namespace MenuFilesGen.Models
{
    /// <summary> Общие данные по аддонам инструментов  </summary>
    public class AppDefinition
    {
        /// <summary> Родительская панель. Если null, то панель не является подчиненной </summary>
        public string Parent { get; set; }

        /// <summary> Имя панели </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the addon.
        /// </summary>
        /// <value>
        /// The addon.
        /// </value>
        public List<AddonDefinition> Addon { get; set; }
    }
}
