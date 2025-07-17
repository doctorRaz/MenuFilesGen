namespace MenuFilesGen.Models
{
    /// <summary> Общие данные по палитрам инструментов  </summary>
    public class AddonDefinition
    {
        /// <summary> Родительская панель. Если null, то панель не является подчиненной </summary>
        public string Parent { get; set; }
        /// <summary> Имя панели </summary>
        public string Name { get; set; }

        public List<PanelDefinition> Panel { get; set; }
    }
}
