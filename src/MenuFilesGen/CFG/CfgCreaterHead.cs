namespace MenuFilesGen.Repositories
{
    public partial class CfgCreater
    {
        /// <summary>
        /// Шапка конфига
        /// </summary>
        void cfgHeader()
        {
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

        }
    }
}
