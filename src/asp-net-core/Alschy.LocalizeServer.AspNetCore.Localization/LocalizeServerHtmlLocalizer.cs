using Alschy.LocalizeServer.AspNetCore.Localization.Options;
using Alschy.LocalizeServer.Common.Services;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alschy.LocalizeServer.AspNetCore.Localization
{
    public class LocalizeServerHtmlLocalizer : BaseLocalizer, IHtmlLocalizer
    {
        public LocalizeServerHtmlLocalizer(ILocalizeService localizeService, LocalizeServerResolveOptions? options = null) : base(localizeService, options)
        {
        }

        LocalizedHtmlString IHtmlLocalizer.this[string name] => new LocalizedHtmlString(name, ResolveRequestSync(name));

        LocalizedHtmlString IHtmlLocalizer.this[string name, params object[] arguments] => new LocalizedHtmlString(name, ResolveRequestSync(name, arguments[0]));

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public LocalizedString GetString(string name) => new LocalizedString(name, ResolveRequestSync(name));

        public LocalizedString GetString(string name, params object[] arguments) => new LocalizedString(name, ResolveRequestSync(name, arguments[0]));

        IHtmlLocalizer IHtmlLocalizer.WithCulture(CultureInfo culture)
        {
            return new LocalizeServerHtmlLocalizer(localizeService, GetNewOptions(culture));
        }
    }

    public class LocalizeServerHtmlLocalizer<T> : LocalizeServerViewLocalizer, IHtmlLocalizer<T>
    {
        public LocalizeServerHtmlLocalizer(ILocalizeService localizeService, LocalizeServerResolveOptions? options = null) : base(localizeService, options)
        {
        }
    }
}
