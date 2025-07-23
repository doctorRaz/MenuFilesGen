using MenuFilesGen.Models;
using MenuFilesGen.Service;

namespace MenuFilesGen.CFG
{
    public partial class CfgCreater
    {
        /// <summary>
        /// Заполняем панельки, регистрируем команды
        /// </summary>
        void PanelCmd()
        {
            #region Формируем панельки и прописываем команды
            string panelNameEn = "";
            string panelName = "";
            string panelIntername = "";
            foreach (PanelDefinition panel in groupingPanel)
            {
                if (panelNameEn != panel.NameEn)
                {
                    panelNameEn = panel.NameEn;

                    panelName = panel.Name;
                    panelIntername = panel.Intername;


                    //  регистрация панельки
                    Cfg.Toolbars.Add("");
                    Cfg.Toolbars.Add($"[\\toolbars\\{panelNameEn}]");
                    Cfg.Toolbars.Add($"name=s{panel.Name}");

                    //регистрация ее команды
                    //+регистрируем команду вызова панели
                    Cfg.ToolbarsCmd.Add("");
                    Cfg.ToolbarsCmd.Add($"[\\configman\\commands\\{panelIntername}]");
                    Cfg.ToolbarsCmd.Add($"weight=i10");
                    Cfg.ToolbarsCmd.Add($"cmdtype=i0");
                    Cfg.ToolbarsCmd.Add($"Intername=s{panelIntername}");
                    Cfg.ToolbarsCmd.Add($"StatusText=sОтображение панели {panelName}");
                    Cfg.ToolbarsCmd.Add($"ToolTipText=sОтображение панели {panelName}");
                    Cfg.ToolbarsCmd.Add($"DispName=sОтображение панели {panelName}");
                    Cfg.ToolbarsCmd.Add($"LocalName=s{panel.LocalName}");

                    //добавлять к команде показа панели иконку, по первой команде панели
                    Cfg.ToolbarsCmd.AddRange(Utils.IconDefinition(panel.Command[0]));
                }

                foreach (CommandDefinition cmd in panel.Command)//по описаниям команд
                {
                    #region Регистрация команд

                    Cfg.Configman.Add("");
                    Cfg.Configman.Add($"[\\configman\\commands\\{cmd.InterName}]");
                    Cfg.Configman.Add($"weight=i{cmd.Weight}");
                    Cfg.Configman.Add($"cmdtype=i{cmd.CmdType}");
                    Cfg.Configman.Add($"intername=s{cmd.InterName}");
                    Cfg.Configman.Add($"DispName=s{cmd.DispName}");
                    Cfg.Configman.Add($"StatusText=s{cmd.StatusText}");

                    //необязательные ключи
                    if (!string.IsNullOrEmpty(cmd.ToolTipText)) Cfg.Configman.Add($"ToolTipText=s{cmd.ToolTipText}");
                    if (!string.IsNullOrEmpty(cmd.LocalName)) Cfg.Configman.Add($"LocalName=s{cmd.LocalName}");
                    if (!string.IsNullOrEmpty(cmd.RealCommandName)) Cfg.Configman.Add($"RealCommandName=s{cmd.RealCommandName}");
                    if (!string.IsNullOrEmpty(cmd.Keyword)) Cfg.Configman.Add($"Keyword=s{cmd.Keyword}");

                    //иконка                
                    Cfg.Configman.AddRange(Utils.IconDefinition(cmd));
                    #endregion

                    //хоткеи
                    if (!string.IsNullOrEmpty(cmd.Accelerators))
                    {
                        Cfg.Accelerators.Add($"{cmd.InterName}=s{cmd.Accelerators}");
                    }

                    if (cmd.HideCommand) continue;// не добавлять в меню пропуск

                    #region Панели

                    if (cmd.IsCommandSeparator)
                    {
                        Cfg.Toolbars.Add($"[\\toolbars\\{panel.NameEn}\\sep_{cmd.InterName}]");
                    }
                    Cfg.Toolbars.Add($"[\\toolbars\\{panel.NameEn}\\{cmd.InterName}]");
                    Cfg.Toolbars.Add($"Intername=s{cmd.InterName}");
                    #endregion
                }
            }

            #endregion
        }
    }
}
