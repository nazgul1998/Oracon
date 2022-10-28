using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Oracon.Models.Entidades
{
    public class StringExtensions
    {
         readonly Regex YoutubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:(.*)v(/|=)|(.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);
         readonly Regex VimeoVideoRegex = new Regex(@"vimeo\.com/(?:.*#|.*/videos/)?([0-9]+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        
        public string UrlToEmbedCode(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var youtubeMatch = YoutubeVideoRegex.Match(url);

                if (youtubeMatch.Success)
                {
                    return getYoutubeEmbedCode(youtubeMatch.Groups[youtubeMatch.Groups.Count - 1].Value);
                }

                var vimeoMatch = VimeoVideoRegex.Match(url);
                if (vimeoMatch.Success)
                {
                    return getVimeoEmbedCode(vimeoMatch.Groups[1].Value);
                }
            }

            return null;
        }

        string youtubeEmbedFormat = "<iframe type=\"text/html\" class=\"embed-responsive-item\" src=\"https://www.youtube.com/embed/{0}\"></iframe>";

        private string getYoutubeEmbedCode(string youtubeId)
        {
            return string.Format(youtubeEmbedFormat, youtubeId);
        }

        string vimeoEmbedFormat = "<iframe src=\"https://player.vimeo.com/video/{0}\" class=\"embed-responsive-item\" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>";
        private string getVimeoEmbedCode(string vimeoId)
        {
            return string.Format(vimeoEmbedFormat, vimeoId);
        }
    }
}
