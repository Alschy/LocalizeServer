using Alschy.LocalizeServer.AspNetCore.Localization.Extensions;
using Alschy.LocalizeServer.AspNetCore.Localization.Options;
using Alschy.LocalizeServer.Common.Models;
using Alschy.LocalizeServer.Common.Services;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Alschy.LocalizeServer.AspNetCore.Localization
{
    public class LocalizeServerStringLocalizer : BaseLocalizer, IStringLocalizer
    {
        public LocalizeServerStringLocalizer(ILocalizeService localizeService, LocalizeServerResolveOptions? options = null) : base(localizeService, options)
        { }

        public LocalizedString this[string name] => new LocalizedString(name, ResolveRequestSync(name));

        public LocalizedString this[string name, params object[] arguments] => new LocalizedString(name, ResolveRequestSync(name, arguments[0]));

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new LocalizeServerStringLocalizer(localizeService, GetNewOptions(culture));
        }
    }

    public class LocalizeServerStringLocalizer<T> : LocalizeServerStringLocalizer, IStringLocalizer<T>
    {
        public LocalizeServerStringLocalizer(ILocalizeService localizeService, LocalizeServerResolveOptions? options = null) : base(localizeService, options)
        { }
    }
}
