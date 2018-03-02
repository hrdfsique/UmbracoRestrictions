using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Events;
using Umbraco.Core.Models;

namespace GiuUnit.UmbracoRestrictions.Core
{
    internal class RestrictionBase
    {
        protected RestrictionsConfigRoot _config;
        protected string _ruleName;
        protected IEnumerable<IContent> _restrictedEntities;
        protected Rule _rule;

        public RestrictionBase(RestrictionsConfigRoot config)
        {
            _config = config;
        }

        protected void ReturnOtherThanRestricted(PublishEventArgs<IContent> e)
        {
            var aliases = ReturnRestricted_DoWork();
            if (aliases == null) return;

            //one of the documents we publish has a restriction
            _restrictedEntities = e.PublishedEntities.Where(x => aliases.Contains(x.ContentType.Alias));

            if (!_restrictedEntities.Any())
                return;
        }

        protected void ReturnOtherThanRestricted(SaveEventArgs<IContent> e)
        {
            var aliases = ReturnRestricted_DoWork();
            if (aliases == null) return;

            //one of the documents we publish has a restriction
            _restrictedEntities = e.SavedEntities.Where(x => aliases.Contains(x.ContentType.Alias));

            if (_restrictedEntities == null || !_restrictedEntities.Any())
                return;
        }

        private IEnumerable<string> ReturnRestricted_DoWork()
        {
            _rule = _config.RulesNode.RuleList.FirstOrDefault(x => x.Name.ToLower().Equals(_ruleName.ToLower()));

            if (_rule == null)
                return null;

            var aliases = _rule.AddList.Select(x => x.DocTypeAlias);

            return (!aliases.Any())? null : aliases;
      
        }


    }
}
