using System.IO;
using Umbraco.Core;
using Umbraco.Core.Services;

namespace GiuUnit.UmbracoRestrictions.Core
{
    public class MVCStartup : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var appPath = umbracoApplication.Server.MapPath("~/");
            var configFilePath = Path.Combine(appPath, @"config\restrictions.config");

            var config = new RestrictionsConfig(configFilePath);

            ContentService.Saving += new RootSingletonDocumentRestriction(config.ConfigEntries).OnSaving;
            ContentService.Saving += new OnlyChildDocumentRestriction(config.ConfigEntries).OnSaving;
            ContentService.Publishing += new DoNotPublishWithoutChildrenRestriction(config.ConfigEntries).OnPublishing;
            ContentService.UnPublished += new UnpublishParentWhenNoPublishedChildrenRestriction(config.ConfigEntries).OnUnpublish;
            ContentService.UnPublishing += new LeastOnceDocumentRootRestriction(config.ConfigEntries).OnUnpublish;
        }
    }
}
