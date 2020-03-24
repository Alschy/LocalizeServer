using Alschy.LocalizeServer.AspNetCore.Localization.Options;
using Alschy.LocalizeServer.Common.Services;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Alschy.LocalizeServer.AspNetCore.Localization
{
    public class LocalizeServerViewLocalizer : BaseLocalizer, IViewLocalizer
    {
        public LocalizeServerViewLocalizer(ILocalizeService localizeService, LocalizeServerResolveOptions? options = null) : base(localizeService, options)
        {
        }

        public LocalizedHtmlString this[string name] => new LocalizedHtmlString(name, ResolveRequestSync(name));

        public LocalizedHtmlString this[string name, params object[] arguments] => new LocalizedHtmlString(name, ResolveRequestSync(name, arguments[0]));

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public LocalizedString GetString(string name) => new LocalizedString(name, ResolveRequestSync(name));

        public LocalizedString GetString(string name, params object[] arguments) => new LocalizedString(name, ResolveRequestSync(name));

        public IHtmlLocalizer WithCulture(CultureInfo culture)
        {
            return new LocalizeServerViewLocalizer(localizeService, GetNewOptions(culture));
        }
    }
}
