using MenuFilesGen.Models;

namespace MenuFilesGen.Repositories
{
    public partial class CfgCreater
    {
        /// <summary>
        /// привязка панелек к приложениям
        /// </summary>
        void AppPanel()
        {
            #region Привязываем панельки к попап и  view по приложениям 

            #region  ToolbarPopupMenu всплывающее меню панелей
            Cfg.ToolbarPopupMenu = new List<string>()
              {
                  "",
                  $";всплывающее меню панелей" ,
                  $"[\\ToolbarPopupMenu]" ,
                  //$"[\\ToolbarPopupMenu\\{addinNameGlobal}]" ,
                  //$"Name=s{addinNameGlobal}"
              };
            #endregion
            #region toolbarsViewMenu меню вид панелей

            Cfg.ToolbarsViewMenu = new List<string>()
                  {
                      "",
                      $";View меню" ,
                       $"[\\menu\\View]",
                      $"[\\menu\\View\\toolbars]" ,
                      //$"[\\menu\\View\\toolbars\\{addinNameGlobal}]" ,
                      //$"Name=s{addinNameGlobal}"
                  };
            #endregion

            //группировка по App и панелькам
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
                foreach (PanelDefinition panel in app.Panels)
                {
                    //+ ****** поп меню ****************
                    Cfg.ToolbarPopupMenu.Add($"{popupPanelRoot}\\{panel.Intername}]");
                    Cfg.ToolbarPopupMenu.Add($"Name=s{panel.Name}");
                    Cfg.ToolbarPopupMenu.Add($"InterName=s{panel.Intername}");

                    //+ ****** вью меню ****************
                    Cfg.ToolbarsViewMenu.Add($"{viewPanelRoot}\\{panel.Intername}]");
                    Cfg.ToolbarsViewMenu.Add($"Name=s{panel.Name}");
                    Cfg.ToolbarsViewMenu.Add($"InterName=s{panel.Intername}");
                }
            }
            #endregion
        }
    }
}
