using PartyGamesSystem.Web.ViewModels;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace PartyGamesSystem.Web.Helpers
{
    public static class HtmlExtensions
    {
        public static IHtmlString ShowDescription(this HtmlHelper html, PartyGameViewModel partyGame)
        {
            if (partyGame.Description.Length <= 200)
            {
                return new HtmlString(partyGame.Description);
            }
            return new HtmlString(string.Format("{0}{1}", partyGame.Description.Substring(0, 200), html.Encode(html.ActionLink("...", "Details", "ManagePartyGames", new { area = string.Empty, id = partyGame.Id }, new { title="Show more"}))));
        }
    }
}