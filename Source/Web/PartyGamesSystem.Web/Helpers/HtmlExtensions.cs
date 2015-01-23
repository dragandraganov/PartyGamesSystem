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
            return new HtmlString(string.Format("{0}{1}", partyGame.Description.Substring(0, 200), html.Encode(html.ActionLink("...", "Details", "ManagePartyGames", new { area = string.Empty, id = partyGame.Id }, new { title = "Show more", @class = "bolded" }))));
        }

        public static IHtmlString ShowNecessaryItemsInIndexView(this HtmlHelper html, PartyGameViewModel partyGame)
        {
            if (partyGame.NecessaryItems==null || partyGame.NecessaryItems==string.Empty)
            {
                return new HtmlString("Not listed");
            }
            if (partyGame.NecessaryItems.Length <= 50)
            {
                return new HtmlString(partyGame.NecessaryItems);
            }
            return new HtmlString(string.Format("{0}{1}", partyGame.NecessaryItems.Substring(0, 50), html.Encode(html.ActionLink("...", "Details", "ManagePartyGames", new { area = string.Empty, id = partyGame.Id }, new { title = "Show more", @class = "bolded" }))));
        }

        public static IHtmlString ShowPlayingPeopleInfo(this HtmlHelper html, PartyGameViewModel partyGame)
        {
            if (partyGame.MinPlayingPeople == null || partyGame.MinPlayingPeople == 0)
            {
                if (partyGame.MaxPlayingPeople == null || partyGame.MaxPlayingPeople == 0)
                {
                    return new HtmlString("Not mentioned");
                }

                else
                {
                    return new HtmlString(string.Format("0 - {0} people", partyGame.MaxPlayingPeople));
                }
            }

            else
            {
                return new HtmlString(string.Format("{0} - {1} people", partyGame.MinPlayingPeople, partyGame.MaxPlayingPeople));
            }
        }
    }
}