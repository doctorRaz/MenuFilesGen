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
            HideCommandColumn = 7;
            ResourseDllNameColumn = 8;
            IconColumn = 9;
            AppNameColumn = 10;
            AddonNameColumn = 11;
            LocalNameColumn = 12;
            RealCommandNameColumn = 13;
            KeywordColumn = 14;
            WeightColumn = 15;
            CmdTypeColumn = 16;
            ToolTipTextColumn = 17;
            AcceleratorsColumn = 18;


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
        public int HideCommandColumn { get; }

        /// <summary> имя приложения </summary>
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

        /// <summary>имя аддона</summary>
        public int AddonNameColumn { get; }
    }
}
