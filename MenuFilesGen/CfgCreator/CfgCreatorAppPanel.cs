using MenuFilesGen.Models;

namespace MenuFilesGen.CFG
{
    public partial class CfgCreator
    {
        /// <summary>
        /// привязка панелек к приложениям
        /// </summary>
        void AppPanel()
        {
            #region Привязываем панельки к попап и  view по приложениям 


            //по App
            foreach (AppDefinition app in groupingAppPanel)
            {
                //todo загонять панельки под AddonName в ToolBarPopUpMenuб и View
                //цикл по -		groupingAppAddonPanel сперва по аддонам, потом по панелям

                string appName = app.Name;
                //+попап панелей
                string popupPanelRoot = $"[\\ToolBarPopUpMenu\\{appName}";
                Cfg.ToolBarPopUpMenu.Add("");
                Cfg.ToolBarPopUpMenu.Add($"{popupPanelRoot}]");
                Cfg.ToolBarPopUpMenu.Add($"Name=s{appName}");

                //+ меню вид
                string viewPanelRoot = $"[\\menu\\View\\toolbars\\{appName}";
                Cfg.ToolBarsViewMenu.Add("");
                Cfg.ToolBarsViewMenu.Add($"{viewPanelRoot}]");
                Cfg.ToolBarsViewMenu.Add($"Name=s{appName}");

                //по панелькам
                string panelIntername = "";
                foreach (PanelDefinition panel in app.Panels)
                {
                    if (panelIntername == panel.Intername)//уже было
                    {
                        continue;
                    }
                    string panelName = panel.Name;
                    panelIntername = panel.Intername;
                    //+ ****** поп меню ****************
                    if (panel.IsPanelSeparator)
                    {
                        Cfg.ToolBarPopUpMenu.Add($"{popupPanelRoot}\\sep_{panelIntername}]");
                    }
                    Cfg.ToolBarPopUpMenu.Add($"{popupPanelRoot}\\{panelIntername}]");
                    Cfg.ToolBarPopUpMenu.Add($"Name=s{panelName}");
                    Cfg.ToolBarPopUpMenu.Add($"InterName=s{panelIntername}");

                    //+ ****** вью меню ****************
                    if (panel.IsPanelSeparator)
                    {
                        Cfg.ToolBarsViewMenu.Add($"{viewPanelRoot}\\sep_{panelIntername}]");
                    }
                    Cfg.ToolBarsViewMenu.Add($"{viewPanelRoot}\\{panelIntername}]");
                    Cfg.ToolBarsViewMenu.Add($"Name=s{panelName}");
                    Cfg.ToolBarsViewMenu.Add($"InterName=s{panelIntername}");
                }
            }
            #endregion
        }
    }
}
