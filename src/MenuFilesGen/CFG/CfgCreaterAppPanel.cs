using MenuFilesGen.Models;

namespace MenuFilesGen.CFG
{
    public partial class CfgCreater
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
                string appName = app.Name;
                //+попап панелей
                string popupPanelRoot = $"[\\ToolbarPopupMenu\\{appName}";
                Cfg.ToolbarPopupMenu.Add("");
                Cfg.ToolbarPopupMenu.Add($"{popupPanelRoot}]");
                Cfg.ToolbarPopupMenu.Add($"Name=s{appName}");

                //+ меню вид
                string viewPanelRoot = $"[\\menu\\View\\toolbars\\{appName}";
                Cfg.ToolbarsViewMenu.Add("");
                Cfg.ToolbarsViewMenu.Add($"{viewPanelRoot}]");
                Cfg.ToolbarsViewMenu.Add($"Name=s{appName}");

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
                        Cfg.ToolbarPopupMenu.Add($"{popupPanelRoot}\\sep_{panelIntername}]");
                    }
                    Cfg.ToolbarPopupMenu.Add($"{popupPanelRoot}\\{panelIntername}]");
                    Cfg.ToolbarPopupMenu.Add($"Name=s{panelName}");
                    Cfg.ToolbarPopupMenu.Add($"InterName=s{panelIntername}");

                    //+ ****** вью меню ****************
                    if (panel.IsPanelSeparator)
                    {
                        Cfg.ToolbarsViewMenu.Add($"{viewPanelRoot}\\sep_{panelIntername}]");
                    }
                    Cfg.ToolbarsViewMenu.Add($"{viewPanelRoot}\\{panelIntername}]");
                    Cfg.ToolbarsViewMenu.Add($"Name=s{panelName}");
                    Cfg.ToolbarsViewMenu.Add($"InterName=s{panelIntername}");
                }
            }
            #endregion
        }
    }
}
