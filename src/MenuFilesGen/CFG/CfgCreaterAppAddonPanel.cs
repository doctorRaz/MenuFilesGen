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
                    string addonName = Addon.Name;

                    string menuAddon = "";

                    if (!string.IsNullOrEmpty(addonName))//аддон есть
                    {
                        menuAddon = $"{menuApp}\\{addonName}";

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
                        string panelName = Panel.Name; //имя панели не может быть пустым
                        string menuPanel = "";


                        //+menu
                        menuPanel = $"{menuAddon}\\{panelName}";
                        Cfg.Menu.Add($"{menuPanel}]");
                        Cfg.Menu.Add($"Name=s{panelName}");

                        //+ **** уровень команды ********
                        foreach (CommandDefinition cmd in Panel.Command)
                        {
                            if (cmd.HideCommand) continue;// не добавлять в меню пропуск

                            #region Классическое меню

                            Cfg.Menu.Add($"{menuPanel}\\s{cmd.InterName}]");
                            Cfg.Menu.Add($"name=s{cmd.DispName}");
                            Cfg.Menu.Add($"Intername=s{cmd.InterName}");

                            #endregion
                        }
                    }

                }
            }

            #endregion
        }
    }
}
