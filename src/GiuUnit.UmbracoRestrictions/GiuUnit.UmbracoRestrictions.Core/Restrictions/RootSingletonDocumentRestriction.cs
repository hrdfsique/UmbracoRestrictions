using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using System.Linq;
using Umbraco.Core.Services;
using Umbraco.Core;

namespace GiuUnit.UmbracoRestrictions.Core
{
    internal class RootSingletonDocumentRestriction : RestrictionBase
    {

        public RootSingletonDocumentRestriction(RestrictionsConfigRoot config) : base(config)
        {
            base._ruleName = "rootSingletonDocument";
        }


        public void OnSaving(IContentService sender, SaveEventArgs<IContent> e)
        {
            base.ReturnOtherThanRestricted(e);

            foreach (var entity in base._restrictedEntities)
            {
                //element not at the root level
                if (entity.ParentId != -1)
                    continue;


                //Do not used cached content here. If the user deletes and tries again, it won't work.
                //We must look at the database.
                var cs = ApplicationContext.Current.Services.ContentService;
                var siblingsAliases = cs.GetRootContent().Select(x => x.ContentType.Alias).ToList();

                //any siblings with the same doc type ? 
                var dbDocumentsCondition = base._restrictedEntities.Any(x => siblingsAliases.Contains(x.ContentType.Alias));

                //documents with the restriction have been found
                if (dbDocumentsCondition)
                {
                    e.CancelOperation(new EventMessage("Content Restriction", base._rule.ErrorMessage, EventMessageType.Error));
                }
            }
        }
    }
}
