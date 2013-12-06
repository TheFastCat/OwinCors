using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.Nancy.Environment
{
    /// <summary>
    /// INancyBootstrapper to be used with SalesApplication.Azure.Website
    /// </summary>
    /// <remarks>Most bootstrapping logic is in base CustomBoostrapper class - all this one does is define a custom favicon</remarks>
    public sealed class WebsiteBootstrapper : CustomBootstrapper
    {
        private byte[] favicon;

        protected override byte[] FavIcon
        {
            get { return this.favicon ?? (this.favicon = LoadFavIcon()); }
        }

        private byte[] LoadFavIcon()
        {
            // load the custom (embedded resource) favicon for the Website from the SalesApplication.UI assembly...
            using (var resourceStream = GetUIAssembly().GetManifestResourceStream("SalesApplication.UI.Images.Nancy.websiteFavicon.png"))
            {
                var tempFavicon = new byte[resourceStream.Length];
                resourceStream.Read(tempFavicon, 0, (int)resourceStream.Length);
                return tempFavicon;
            }
        }
    }
}
