
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace GiuUnit.UmbracoRestrictions.Core
{
    interface ISavingRestriction
    {
        void OnSaving(IContentService sender, SaveEventArgs<IContent> e);
    }
}
