using Alschy.LocalizeServer.AspNetCore.Localization.Extensions;
using Alschy.LocalizeServer.AspNetCore.Localization.Options;
using Alschy.LocalizeServer.Common.Models;
using Alschy.LocalizeServer.Common.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alschy.LocalizeServer.AspNetCore.Localization
{
    public abstract class BaseLocalizer
    {
        protected readonly ILocalizeService localizeService;
        protected readonly LocalizeServerResolveOptions options;

        public BaseLocalizer(ILocalizeService localizeService, LocalizeServerResolveOptions? options = null)
        {
            options ??= new LocalizeServerResolveOptions();
            this.localizeService = localizeService;
            this.options = options;
        }

        protected LocalizeServerResolveOptions GetNewOptions(CultureInfo culture)
        {
            return  new LocalizeServerResolveOptions
            {
                UseApplicationName = options.UseApplicationName,
                DefaultCulture = culture.Name,
                ApplicationName = options.ApplicationName
            };
        }

        protected string ResolveRequestSync(string resourceName, string? culture = null)
        {
            culture ??= options.DefaultCulture;
            string? appName = null;
            if (options.UseApplicationName && !string.IsNullOrEmpty(options.ApplicationName))
            {
                appName = options.ApplicationName;
            }
            var requestModel = new ResourceRequestModel(resourceName, culture, appName);
            var result = localizeService.Localize(requestModel);
            return result.Value;
        }

        protected string ResolveRequestSync(string resourceKey, object param)
        {
            if (param is CultureInfo cultureInfo)
            {
                return ResolveRequestSync(resourceKey, cultureInfo.Name);
            }
            else if (param is string culture)
            {
                if (IsValidCultureString(culture))
                {
                    return ResolveRequestSync(resourceKey, culture);
                }
                else
                {
                    throw new ArgumentException($"Culture '{culture}' is unknown!");
                }
            }
            else
            {
                throw new ArgumentException("Parameter must be a culture object or a string!", nameof(param));
            }
        }

        protected bool IsValidCultureString(string culture)
        {
            if (!culture.Contains('-'))
            {
                return false;
            }
            try
            {
                CultureInfo.CreateSpecificCulture(culture);
                return true;
            }
            catch (CultureNotFoundException)
            {
                return false;
            }
        }
    }
}
