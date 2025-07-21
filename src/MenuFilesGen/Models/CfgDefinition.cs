using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuFilesGen.Models
{
    public class CfgDefinition
    {
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

        /// <summary> регистрация горячих клавиш</summary>
        public List<string> Accelerators { get; set; }



    }
}
