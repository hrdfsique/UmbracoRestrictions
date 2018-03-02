using System.Linq;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;

namespace GiuUnit.UmbracoRestrictions.Core
{
    internal class LeastOnceDocumentRootRestriction : RestrictionBase, IUnpublishRestriction
    {

        public LeastOnceDocumentRootRestriction(RestrictionsConfigRoot config) : base(config)
        {
            base._ruleName = "leastOnceDocumentRoot";
        }

        public void OnUnpublish(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            base.ReturnOtherThanRestricted(e);

            foreach (var entity in base._restrictedEntities)
            {
                //element not at the root level
                if (entity.ParentId != -1)
                    continue;

                //umbraco context is already defined as we are in one of the pluggins
                var siblingsAliases = Umbraco.Web.UmbracoContext.Current.ContentCache.GetAtRoot()
                    .Where(x => x.Id != entity.Id)
                    .Select(x => x.DocumentTypeAlias)
                    .ToList();

                //any siblings with the same doc type ? 
                var noOther = !base._restrictedEntities.Any(x => siblingsAliases.Contains(x.ContentType.Alias));

                //documents with the restriction have been found
                if (noOther)
                {
                    e.CancelOperation(new EventMessage("Content Restriction", base._rule.ErrorMessage, EventMessageType.Error));
                }
            }
        }
    }
}
