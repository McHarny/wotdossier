namespace WotDossier.Domain.Settings
{
    public class AppSettingsBase
    {
        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        public string Server { get; set; } = "ru";

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string Language { get; set; } = "ru-RU";

        /// <summary>
        /// Gets or sets the theme.
        /// </summary>
        public DossierTheme Theme { get; set; } = DossierTheme.Black;
    }
}