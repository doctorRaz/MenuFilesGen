namespace MenuFilesGen.Models
{
    /// <summary> Номера колонок для корректного парсинга текстового файла. Сделано классом, чтоб потом можно было, например, создать отдельный
    /// файл, в котором будут прописываться назначения колонок и их номера, а здесь в конструкторе их читать</summary>
    public class ColumnNumbers
    {
        public ColumnNumbers()
        {
            CommandNameColumn = 0;
            CommandInternaleNameColumn = 1;
            CommandDescriptionColumn = 2;
            PanelNameColumn = 3;
            RibbonNameColumn = 4;
            RibbonSizeColumn = 5;
            ResourseDllNameColumn = 11;
            IconColumn = 12;
        }
        /// <summary> Номер колонки для имени команды </summary>
        public int CommandNameColumn { get; }
        /// <summary> ВНутренне имя команды </summary>
        public int CommandInternaleNameColumn { get; }
        /// <summary> Описание команды </summary>
        public int CommandDescriptionColumn { get; }
        /// <summary> Размер кнопки команды в ленте </summary>
        public int RibbonSizeColumn { get; }
        /// <summary>
        /// Имя панели
        /// </summary>
        public int PanelNameColumn { get; }
        public int RibbonNameColumn { get; }
        public int IconColumn { get; }
        public int ResourseDllNameColumn { get; }
    }
}
