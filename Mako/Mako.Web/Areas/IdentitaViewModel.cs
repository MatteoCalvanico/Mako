﻿using Mako.Web.Infrastructure;

namespace Mako.Web.Areas
{
    public class IdentitaViewModel
    {
        public static string VIEWDATA_IDENTITACORRENTE_KEY = "IdentitaUtenteCorrente";

        public string EmailUtenteCorrente { get; set; }

        public string GravatarUrl
        {
            get
            {
                return EmailUtenteCorrente.ToGravatarUrl(ToGravatarUrlExtension.DefaultGravatar.Identicon, null);
            }
        }
    }
}
