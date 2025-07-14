namespace MenuFilesGen.Models
{
    /// <summary> Общие данные по палитрам инструментов  </summary>
    public class PanelDefinition
    {
        /// <summary> Родительская панель. Если null, то панель не является подчиненной </summary>
        public PanelDefinition ParentPanel { get; set; }
        /// <summary> Имя панели </summary>
        public string Name { get; set; }
    }
}
