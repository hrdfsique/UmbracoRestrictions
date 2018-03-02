using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;

namespace GiuUnit.UmbracoRestrictions.Core
{
    internal class DoNotPublishWithoutChildrenRestriction: RestrictionBase, IPublishingRestriction
    {
        public DoNotPublishWithoutChildrenRestriction(RestrictionsConfigRoot config): base(config)
        {
            base._ruleName = "doNotPublishWithoutChildren";
        }

        public void OnPublishing(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            base.ReturnOtherThanRestricted(e);

            foreach (var entity in base._restrictedEntities)
            {
                if (entity.Children().Any())
                {
                    return;
                }

                e.CancelOperation(new EventMessage("Content Restriction", base._rule.ErrorMessage, EventMessageType.Error));
            }

        }
    }
}
