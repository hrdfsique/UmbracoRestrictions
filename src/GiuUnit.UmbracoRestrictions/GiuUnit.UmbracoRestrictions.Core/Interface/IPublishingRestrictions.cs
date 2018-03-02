

namespace GiuUnit.UmbracoRestrictions.Core
{
    interface IPublishingRestriction
    {
        void OnPublishing(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e);
    }
}
