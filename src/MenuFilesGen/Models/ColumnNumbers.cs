namespace MenuFilesGen.Models
{
    /// <summary> Номера колонок для корректного парсинга текстового файла. Сделано классом, чтоб потом можно было, например, создать отдельный
    /// файл, в котором будут прописываться назначения колонок и их номера, а здесь в конструкторе их читать</summary>
    public class ColumnNumbers
    {
        public ColumnNumbers()
        {
            InterNameColumn = 0;
            DispNameColumn = 1;
            PanelNameColumn = 2;
            RibbonSizeColumn = 3;
            RibbonSplitButtonColumn = 4;
            ResourseDllColumn = 5;
            IconColumn = 6;
            AppNameColumn = 7;
            AddonNameColumn = 8;
            LocalNameColumn = 9;
            RealCommandNameColumn = 10;
            KeywordColumn = 11;
            WeightColumn = 12;
            CmdTypeColumn = 13;
            StatusTextColumn = 14;
            ToolTipTextColumn = 15;
            AcceleratorsColumn = 16;
            DontTakeColumn = 17;
            HideCommandColumn = 18;
            IsVirtualPanelColumn = 19;



        }

        /// <summary> Номер колонки для имени команды </summary>
        public int DispNameColumn { get; }

        /// <summary> ВНутренне имя команды </summary>
        public int InterNameColumn { get; }

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
        public int ResourseDllColumn { get; }

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
        public int IsVirtualPanelColumn { get; }
    }
}
