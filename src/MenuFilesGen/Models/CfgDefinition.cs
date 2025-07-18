using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuFilesGen.Models
{
    public class CfgDefinition
    {
        public CfgDefinition(string addinNameGlobal)
        {
            #region Configman команды
            Configman = new List<string>
            {
                ";Команды",
                $"[\\configman]" ,
                $"[\\configman\\commands]"
            };
            #endregion

            #region Ribbon прописка ленты
            Ribbon = new List<string>()
            {
                ";Лента",
                $"[\\ribbon\\{addinNameGlobal}]" ,
                $"CUIX=s%CFG_PATH%\\{addinNameGlobal}.cuix" ,
                $"visible=f1"
            };
            #endregion

            #region Toolbars панели
            Toolbars = new List<string>()
            {
                $";Панели" ,
                $"[\\toolbars]"
            };
            #endregion

            #region  ToolbarPopupMenu всплывающее меню панелей
            ToolbarPopupMenu = new List<string>()
            {
                $";всплывающее меню панелей" ,
                $"[\\ToolbarPopupMenu]" ,
                $"[\\ToolbarPopupMenu\\{addinNameGlobal}]" ,
                $"Name=s{addinNameGlobal}"
            };
            #endregion

            #region Accelerators горячие клавиши
            Accelerators = new List<string>()
            {
                ";горячие клавиши",
                $"[\\Accelerators]"
            };
            #endregion

            #region Menu
            Menu = new List<string>()
            {
                ";Меню",
                $"[\\menu]"
            };
            #endregion

            #region toolbarsCmd команды вызова панелей
            ToolbarsCmd = new List<string>()
            {
                ";команды вызова панелей",
            };
            #endregion

            #region toolbarsViewMenu меню вид панелей
            ToolbarsViewMenu = new List<string>()
            {
                $";View меню" ,
                $"[\\menu\\View\\toolbars\\{addinNameGlobal}]" ,
                $"Name=s{addinNameGlobal}"
            };
            #endregion

        }
        /// <summary> Меню</summary>
        public List<string> Menu { get; set; }

        /// <summary> попап Меню</summary>
        public List<string> ToolbarPopupMenu { get; set; }

        /// <summary> Меню панелей</summary>
        public List<string> ToolbarsViewMenu { get; set; }

        /// <summary> панели</summary>
        public List<string> Toolbars { get; set; }

        /// <summary>описания команд </summary>
        public List<string> Configman { get; set; }

        /// <summary> команды показа панелей</summary>
        public List<string> ToolbarsCmd { get; set; }

        /// <summary>регистрация ленты</summary>
        public List<string> Ribbon { get; set; }

        /// <summary> регистрация горячих коавиш</summary>
        public List<string> Accelerators { get; set; }



    }
}
