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
        /// <param name="_addOnNameGlobal">The addon name global.</param>
        public CfgDefinition(string _addOnNameGlobal)
        {
            addonNameGlobal = _addOnNameGlobal;
        }


        /// <summary> Меню</summary>
        public List<string> Menu { get; set; } = new List<string>
                                                                {
                                                                    "",
                                                                    ";Меню",
                                                                    $"[\\menu]"
                                                                };

        /// <summary> попап Меню</summary>
        public List<string> ToolBarPopUpMenu { get; set; } = new List<string>
                                                                          {
                                                                              "",
                                                                              $";всплывающее меню панелей" ,
                                                                              $"[\\ToolBarPopUpMenu]" ,
                                                                          };

        /// <summary> Меню панелей</summary>
        public List<string> ToolBarsViewMenu { get; set; } = new List<string>
                                                                            {
                                                                                "",
                                                                                $";View меню" ,
                                                                                $"[\\menu\\View]",
                                                                                $"[\\menu\\View\\toolbars]" ,
                                                                            };


        /// <summary> панели</summary>
        public List<string> ToolBars { get; set; } = new List<string>
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
        public List<string> ToolBarsCmd { get; set; } = new List<string>
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

        public List<string> ViewPopUpMenu { get; set; } = new List<string>
                                                                        {
                                                                            "",
                                                                            ";Меню по ПКМ not used",
                                                                            "[\\ViewPopUpMenu]"
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
