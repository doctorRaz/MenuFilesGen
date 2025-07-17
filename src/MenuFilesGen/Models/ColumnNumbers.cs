namespace MenuFilesGen.Models
{
    /// <summary> Номера колонок для корректного парсинга текстового файла. Сделано классом, чтоб потом можно было, например, создать отдельный
    /// файл, в котором будут прописываться назначения колонок и их номера, а здесь в конструкторе их читать</summary>
    public class ColumnNumbers
    {
        public ColumnNumbers()
        {
            DispNameColumn = 0;
            InternameColumn = 1;
            StatusTextColumn = 2;
            PanelNameColumn = 3;
            RibbonSizeColumn = 4;
            RibbonSplitButtonColumn = 5;
            DontTakeColumn = 6;
            DontMenuColumn = 7;
            ResourseDllNameColumn = 8;
            IconColumn = 9;
            AppNameColumn = 10;
            LocalNameColumn = 11;
            RealCommandNameColumn = 12;
            KeywordColumn = 13;
            WeightColumn = 14;
            CmdTypeColumn = 15;
            ToolTipTextColumn = 16;
            AcceleratorsColumn = 17;
            //todo добавить addinNameGlobal
        }

        /// <summary> Номер колонки для имени команды </summary>
        public int DispNameColumn { get; }

        /// <summary> ВНутренне имя команды </summary>
        public int InternameColumn { get; }

        /// <summary> Описание команды </summary>
        public int StatusTextColumn { get; }

        /// <summary> Размер кнопки команды в ленте </summary>
        public int RibbonSizeColumn { get; }

        /// <summary> Имя панели  </summary>
        public int PanelNameColumn { get; }

        /// <summary> Имя сплитера ленты  </summary>
        public int RibbonSplitButtonColumn { get; }

        /// <summary>Не учитывать команду никак и нигде </summary> 
        public int DontTakeColumn { get; }

        /// <summary>библиотека ресурсов или иконка с относительным путем </summary>
        public int ResourseDllNameColumn { get; }

        /// <summary>
        /// имя ресурсной иконки
        /// </summary>
        public int IconColumn { get; }

        /// <summary> команду регистрировать но не показывать ни в меню ни в ленте ни в панелях </summary>
        public int DontMenuColumn { get; }

        /// <summary>  родитель меню  </summary>
        public int AppNameColumn { get; }

        /// <summary>  локальное имя команды </summary>
        public int LocalNameColumn { get; }

        /// <summary> реальное имя команды </summary>
        public int RealCommandNameColumn { get; }

        /// <summary> ключевое слово </summary>
        public int KeywordColumn { get; }

        /// <summary>вес команды???)) </summary>
        public int WeightColumn { get; }

        /// <summary> подсказки </summary>
        public int ToolTipTextColumn { get; }

        /// <summary>Тип команды, контекст выполнения , документ/приложение</summary>
        public int CmdTypeColumn { get; }

        /// <summary>горячие кнопки</summary>
        public int AcceleratorsColumn { get; }
    }
}
