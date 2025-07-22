using MenuFilesGen.Models;

namespace MenuFilesGen.Repositories
{
    public partial class CfgCreater
    {
        /// <summary>
        /// привязка команд к меню
        /// </summary>
        void AppAddonPanel()
        {
            #region Формируем меню   
                        
            //++ ********** уровень приложения *********

            foreach (AppDefinition App in groupingAppAddonPanel)//уровень приложения
            {
                string appName = App.Name;

                // Классическое меню шапка

                string menuApp = $"[\\menu\\{appName}";
                Cfg.Menu.Add("");
                Cfg.Menu.Add($"{menuApp}]");
                Cfg.Menu.Add($"Name=s{appName}");

                //++ ***** уровень аддона *****
                foreach (AddonDefinition Addon in App.Addons)
                {
                    List<string> addonNames =Addon.Name.Split('|').ToList();
                    string addonName =addonNames[0];

                    string menuAddon = "";

                    if (!string.IsNullOrEmpty(addonName))//аддон есть
                    {

                        menuAddon = $"{menuApp}\\{addonName}";

                        Cfg.Menu.Add("");

                        if(addonNames.Count() > 1)
                        {
                            Cfg.Menu.Add($"{menuApp}\\sep_{addonName}");
                        }
                        Cfg.Menu.Add($"{menuAddon}]");
                        Cfg.Menu.Add($"Name=s{addonName}");
                    }
                    else
                    {
                        menuAddon = menuApp;
                    }
                    //++ *******уровень панели *************
                    foreach (PanelDefinition Panel in Addon.Panel)
                    {
                        var panelNames = Panel.Name.Split('|').ToList();
                        string panelName = panelNames[0]; //имя панели не может быть пустым
                        string menuPanel = "";


                        //+menu
                        menuPanel = $"{menuAddon}\\{panelName}";
                        if(panelNames.Count() > 1)
                        {
                            Cfg.Menu.Add($"{menuAddon}\\sep_{panelName}");
                        }
                        Cfg.Menu.Add($"{menuPanel}]");
                        Cfg.Menu.Add($"Name=s{panelName}");

                        //+ **** уровень команды ********
                        foreach (CommandDefinition cmd in Panel.Command)
                        {
                            if (cmd.HideCommand) continue;// не добавлять в меню пропуск

                            #region Классическое меню

                            string interName = cmd.InterName;

                            if (cmd.IsCommandSeparator)
                            {
                                Cfg.Menu.Add($"{menuPanel}\\sep_{interName}]");
                            }
                            Cfg.Menu.Add($"{menuPanel}\\s{interName}]");
                            Cfg.Menu.Add($"name=s{cmd.DispName}");
                            Cfg.Menu.Add($"Intername=s{interName}");

                            #endregion
                        }
                    }

                }
            }

            #endregion
        }
    }
}
