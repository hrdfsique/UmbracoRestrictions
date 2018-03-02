using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;

namespace GiuUnit.UmbracoRestrictions.Core
{
    internal class OnlyChildDocumentRestriction : RestrictionBase, ISavingRestriction
    {
        public OnlyChildDocumentRestriction(RestrictionsConfigRoot config) : base(config)
        {
            base._ruleName = "onlyChildDocument";
        }

        public void OnSaving(IContentService sender, SaveEventArgs<IContent> e)
        {
            base.ReturnOtherThanRestricted(e);
            foreach (var entity in _restrictedEntities)
            {
                //element at the root level, we ignore, rootSingletonDoc restriction already available for that
                if (entity.ParentId == -1)
                    continue;

                //Do not used cached content here. If the user deletes and tries again, it won't work.
                //We must look at the database.
                var cs = ApplicationContext.Current.Services.ContentService;
                var siblingsAliases = cs.GetById(entity.ParentId).Children().Select(x => x.ContentType.Alias);

                //any siblings with the same doc type ? 
                var dbDocumentsCondition = _restrictedEntities.Any(x => siblingsAliases.Contains(x.ContentType.Alias));

                //documents with the restriction have been found
                if (dbDocumentsCondition)
                {
                    e.CancelOperation(new EventMessage("Content Restriction", base._rule.ErrorMessage, EventMessageType.Error));
                }
            }
        }


    }
}
