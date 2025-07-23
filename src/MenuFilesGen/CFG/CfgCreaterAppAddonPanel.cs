using MenuFilesGen.Models;

namespace MenuFilesGen.CFG
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
                string menuAddon = "";
                string addonName = "";
                string menuPanel = "";
                foreach (AddonDefinition Addon in App.Addons)
                {
                    addonName = Addon.Name;


                    if (!string.IsNullOrEmpty(addonName))//аддон есть
                    {
                        if (menuAddon != $"{menuApp}\\{addonName}")
                        {
                            menuAddon = $"{menuApp}\\{addonName}";

                            Cfg.Menu.Add("");
                            if (Addon.IsAddonSeparator)//separator
                            {
                                Cfg.Menu.Add($"{menuApp}\\sep_{addonName}]");
                            }
                            Cfg.Menu.Add($"{menuAddon}]");
                            Cfg.Menu.Add($"Name=s{addonName}");
                        }
                    }
                    else
                    {
                        menuAddon = menuApp;
                    }

                    //++ *******уровень панели *************
                    string panelName = "";
                    foreach (PanelDefinition panel in Addon.Panel)
                    {
                        panelName = panel.Name; //имя панели не может быть пустым


                        //+menu
                        if (menuPanel != $"{menuAddon}\\{panelName}")//пропуск если было
                        {
                            menuPanel = $"{menuAddon}\\{panelName}";
                            if (panel.IsPanelSeparator)
                            {
                                Cfg.Menu.Add($"{menuAddon}\\sep_{panelName}]");
                            }
                            Cfg.Menu.Add($"{menuPanel}]");
                            Cfg.Menu.Add($"Name=s{panelName}");
                        }

                        //+ **** уровень команды ********
                        foreach (CommandDefinition cmd in panel.Command)
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
