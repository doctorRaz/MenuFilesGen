namespace MenuFilesGen.Models
{
    /// <summary>
    /// секции файла CFG
    /// </summary>
    public class CfgDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CfgDefinition"/> class.
        /// </summary>
        /// <param name="_addonNameGlobal">The addon name global.</param>
        public CfgDefinition(string _addonNameGlobal)
        {
            addonNameGlobal = _addonNameGlobal;
        }


        /// <summary> Меню</summary>
        public List<string> Menu { get; set; } = new List<string>
                                                                {
                                                                    "",
                                                                    ";Меню",
                                                                    $"[\\menu]"
                                                                };

        /// <summary> попап Меню</summary>
        public List<string> ToolbarPopupMenu { get; set; } = new List<string>
                                                                          {
                                                                              "",
                                                                              $";всплывающее меню панелей" ,
                                                                              $"[\\ToolbarPopupMenu]" ,
                                                                          };

        /// <summary> Меню панелей</summary>
        public List<string> ToolbarsViewMenu { get; set; } = new List<string>
                                                                            {
                                                                                "",
                                                                                $";View меню" ,
                                                                                $"[\\menu\\View]",
                                                                                $"[\\menu\\View\\toolbars]" ,
                                                                            };


        /// <summary> панели</summary>
        public List<string> Toolbars { get; set; } = new List<string>
                                                                    {
                                                                        $"",
                                                                        $"; Панели",
                                                                        $"[\\toolbars]"
                                                                    };

        /// <summary>описания команд </summary>
        public List<string> Configman { get; set; } = new List<string>
                                                                    {
                                                                        "",
                                                                        ";Команды",
                                                                        "[\\configman]",
                                                                        $"[\\configman\\commands]"
                                                                    };

        /// <summary> команды показа панелей</summary>
        public List<string> ToolbarsCmd { get; set; } = new List<string>
                                                                        {
                                                                            "",
                                                                            ";команды вызова панелей"
                                                                        };

        /// <summary> регистрация горячих клавиш</summary>
        public List<string> Accelerators { get; set; } = new List<string>
                                                                        {
                                                                            "",
                                                                            ";горячие клавиши",
                                                                            "[\\Accelerators]"
                                                                        };

        public List<string> ViewPopupMenu { get; set; } = new List<string>
                                                                        {
                                                                            "",
                                                                            ";Меню по ПКМ not used",
                                                                            "[\\ViewPopupMenu]"
                                                                        };


        /// <summary>регистрация ленты, только чтение</summary>
        public List<string> Ribbon => new List<string>()
                                    {
                                        "",
                                        ";Лента",
                                        $"[\\ribbon\\{addonNameGlobal}]" ,
                                        $"CUIX=s%CFG_PATH%\\{addonNameGlobal}.cuix" ,
                                        $"visible=f1"
                                    };
        string addonNameGlobal;
    }
}
