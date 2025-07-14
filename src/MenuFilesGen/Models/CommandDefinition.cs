using MenuFilesGen.Enums;

namespace MenuFilesGen.Models
{
    /// <summary> Общие данные по команде </summary>
    public class CommandDefinition
    {
        /// <summary> Имя команды, как оно будет показываться в меню </summary>
        public string DispName { get; set; }
        /// <summary> Внутреннее имя команды, как оно определено в dll / nrx / lsp </summary>
        public string InterName { get; set; }
        /// <summary> Описание команды, показываемое в качестве всплывающей подсказки </summary>
        public string StatusText { get; set; }
        /// <summary> Имя иконки </summary>
        public string IconName { get; set; }

        /// <summary> имя панели/подменю </summary>
        public string PanelName { get; set; }
        /// <summary> Имя ресурсной dll. Обязательна, если установлена иконка (?) </summary>
        public string ResourceDllName { get; set; }

        /// <summary> Команду зарегистрировать, но не показывать нигде </summary>
        public bool DontMenu { get; set; }

        /// <summary> не регистрировать команду, пропустить </summary>
        public bool DontTake { get; set; }

        /// <summary>название сплитера ленты </summary>
        public string RibbonSplitButtonName  { get; set; }

        /// <summary> родительское меню </summary>
        public string Root { get; set; }

        /// <summary> Размер кнопки на ленте. None - кнопки не будет </summary>
        public string RibbonSize { get; set; }


        /// <summary> Локальное имя команды </summary>
        public string LocalName { get; set; }

        /// <summary> Реальное имя команды </summary>
        public string RealCommandName { get; set; }

        /// <summary> Ключевое слово </summary>
        public string Keyword { get; set; }

        /// <summary> вес команды)) </summary>
        public int Weight { get; set; }

        /// <summary> Тип команды, контекст выполнения , документ/приложение </summary>
        public int CmdType { get; set; }

   
    }


    /// <summary> Общие данные по команде </summary>
    public class CommandDefinitionKpc
    {
        /// <summary> Имя команды, как оно будет показываться в меню </summary>
        public string MenuCommandName { get; set; }
        /// <summary> Внутреннее имя команды, как оно определено в dll / nrx / lsp </summary>
        public string InternalName { get; set; }
        /// <summary> Описание команды, показываемое в качестве всплывающей подсказки </summary>
        public string Description { get; set; }
        /// <summary> Имя иконки </summary>
        public string IconName { get; set; }
        /// <summary> Имя ресурсной dll. Обязательна, если установлена иконка (?) </summary>
        public string ResourceDllName { get; set; }
        /// <summary> Команду ввести в меню, но не показывать нигде </summary>
        public bool HideCommand { get; set; }
        /// <summary> В каких панелях инструментов будет участвовать команда </summary>
        public IEnumerable<PanelDefinition> Panel { get; set; }
        /// <summary> В каких кусках ленты будет участвовать команда </summary>
        public IEnumerable<RibbonPaletteDefinition> RibbonPanel { get; set; }
        /// <summary> Размер кнопки на ленте. None - кнопки не будет </summary>
        public RibbonButtonSize RibbonSize { get; set; }
    }
}
