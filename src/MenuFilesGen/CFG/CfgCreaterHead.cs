using MenuFilesGen.Models;
using MenuFilesGen.Service;
using System.Text;

namespace MenuFilesGen.Repositories
{
    public partial class CfgCreater
    {         
        /// <summary>
        /// Шапка конфига
        /// </summary>
        void cfgHeader()
        {

            //#region Configman команды
            //Cfg.Configman = new List<string>
            //{
            //    "",
            //    ";Команды",
            //    $"[\\configman]" ,
            //    $"[\\configman\\commands]"
            //};
            //#endregion

            #region Ribbon прописка ленты
            Cfg.Ribbon = new List<string>()
            {
                "",
                ";Лента",
                $"[\\ribbon\\{addonNameGlobal}]" ,
                $"CUIX=s%CFG_PATH%\\{addonNameGlobal}.cuix" ,
                $"visible=f1"
            };
            #endregion

            //#region Toolbars панели
            //Cfg.Toolbars = new List<string>()
            //{
            //    "",
            //    $";Панели" ,
            //    $"[\\toolbars]"
            //};
            //#endregion

            //#region  ToolbarPopupMenu всплывающее меню панелей
            //Cfg.ToolbarPopupMenu = new List<string>()
            //{
            //    "",
            //    $";всплывающее меню панелей" ,
            //    $"[\\ToolbarPopupMenu]" ,
            //    //$"[\\ToolbarPopupMenu\\{addinNameGlobal}]" ,
            //    //$"Name=s{addinNameGlobal}"
            //};
            //#endregion

            //#region Accelerators горячие клавиши
            //Cfg.Accelerators = new List<string>()
            //{
            //    "",
            //    ";горячие клавиши",
            //    $"[\\Accelerators]"
            //};
            //#endregion

            //#region Menu
            ////Cfg.Menu = new List<string>();
            ////{
            ////    "",
            ////    ";Меню",
            ////    $"[\\menu]"
            ////};
            //#endregion

            //#region toolbarsCmd команды вызова панелей
            //Cfg.ToolbarsCmd = new List<string>()
            //{
            //    "",
            //    ";команды вызова панелей",
            //};
            //#endregion

            //#region toolbarsViewMenu меню вид панелей
            //Cfg.ToolbarsViewMenu = new List<string>()
            //{
            //    "",
            //    $";View меню" ,
            //     $"[\\menu\\View]",
            //    $"[\\menu\\View\\toolbars]" ,
            //    //$"[\\menu\\View\\toolbars\\{addinNameGlobal}]" ,
            //    //$"Name=s{addinNameGlobal}"
            //};
            //#endregion
        }



    }
}
