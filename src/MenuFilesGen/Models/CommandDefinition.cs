using MenuFilesGen.Enums;
using MenuFilesGen.Service;

namespace MenuFilesGen.Models
{
    /// <summary> Общие данные по команде </summary>
    public class CommandDefinition
    {
        /// <summary> Имя команды, как оно будет показываться в меню </summary>
        public string DispName { get; set; }

        #region InterName
        /// <summary> Внутреннее имя команды, как оно определено в dll / nrx / lsp </summary>
        public string InterName => InterNames[0].Trim();

          List< string> InterNames => InterNameRaw.RawSplit();
        public string InterNameRaw { get; set; } = "";

        public bool IsCommandSeparator=>InterNames.Count > 1;
        
        #endregion


        /// <summary> Описание команды, показываемое в качестве всплывающей подсказки </summary>
        public string StatusText { get; set; }

        #region PanelName
        

        /// <summary> имя панели/подменю </summary>
        public string PanelName => PanelNames[0].Trim();

        List<string> PanelNames => PanelNameRaw.RawSplit();
        public string PanelNameRaw { get; set; } = "";

        public bool IsPanelSeparator => PanelNames.Count > 1;

        #endregion


        /// <summary> Размер кнопки на ленте. None - кнопки не будет </summary>
        public string RibbonSize { get; set; }

        /// <summary>название сплитера ленты </summary>
        public string RibbonSplitButtonName { get; set; }

        /// <summary> не регистрировать команду </summary>
        public bool DontTake { get; set; }

        /// <summary> Команду зарегистрировать, но не показывать в UI </summary>
        public bool HideCommand { get; set; }

        /// <summary> Имя ресурсной dll. Обязательна, если установлен IconName </summary>
        public string ResourceDllName { get; set; }

        /// <summary> Имя иконки </summary>
        public string IconName { get; set; }

        /// <summary> приложение </summary>
        public string AppName { get; set; }

        /// <summary> Локальное имя команды </summary>
        public string LocalName { get; set; }

        /// <summary> Реальное имя команды </summary>
        public string RealCommandName { get; set; }

        /// <summary> Ключевое слово </summary>
        public string Keyword { get; set; }

        /// <summary> вес команды)) </summary>
        public int Weight { get; set; }

        /// <summary> Тип команды, контекст выполнения , 
        /// <br>документ-1</br>
        /// <br>приложение-0</br> </summary>
        public int CmdType { get; set; }

        /// <summary>подсказки </summary>
        public string ToolTipText { get; set; }

        /// <summary> хоткеи </summary>
        public string Accelerators { get; set; }

        #region AddonName

        /// <summary> аддон </summary>
        public string AddonName => AddonNames[0].Trim();

        List<string> AddonNames => AddonNameRaw.RawSplit();
        public string AddonNameRaw { get; set; } = "";

        public bool IsAddonSeparator => AddonNames.Count > 1;

        #endregion

    }


    /// <summary> Общие данные по команде not Used </summary>
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
