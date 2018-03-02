using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;

namespace GiuUnit.UmbracoRestrictions.Core
{
    internal class UnpublishParentWhenNoPublishedChildrenRestriction : RestrictionBase,  IUnpublishRestriction
    {
        public UnpublishParentWhenNoPublishedChildrenRestriction(RestrictionsConfigRoot config) : base(config)
        {
            base._ruleName = "unpublishParentWhenNoPublishedChildren";
        }

        /// <summary>
        /// Unpublish the parent of any nodes of certain type that have no published children.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnUnpublish(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            base.ReturnOtherThanRestricted(e);
            if (base._restrictedEntities == null) return;

            var parents = base._restrictedEntities.Select(x => x.Parent());
            if (!parents.Any()) return;

            foreach (var parent in parents)
            {
                // if parent has any published children leave it.
                if (parent.Children().Where(x => x.Published).Any())
                {
                    return;
                }
                var cs = ApplicationContext.Current.Services.ContentService;
                cs.UnPublish(parent);
            }
        }
    }
}
