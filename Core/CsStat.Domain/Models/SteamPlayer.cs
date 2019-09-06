using System.Collections.Generic;
using CsStat.Domain.Entities;
using Newtonsoft.Json;

namespace CsStat.Domain.Models
{
    public class SteamPlayer
    {
        [JsonProperty("response")]
        public Response Data { get; set; }
    }
    public class Response
    {
        [JsonProperty("players")]
        public List<Info> PlayersInfo { get; set; }
    }

    public class Info
    {
        [JsonProperty("steamid")]
        public string SteamId { get; set; }

        [JsonProperty("communityvisibilitystate")]
        public long CommunityVisibilityState { get; set; }

        [JsonProperty("profilestate")]
        public long ProfileState { get; set; }

        [JsonProperty("personaname")]
        public string PersonaName { get; set; }

        [JsonProperty("lastlogoff")]
        public long LastLogoff { get; set; }

        [JsonProperty("profileurl")]
        public string ProfileUrl { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("avatarmedium")]
        public string AvatarMedium { get; set; }

        [JsonProperty("avatarfull")]
        public string AvatarFull { get; set; }

        [JsonProperty("personastate")]
        public long PersonaState { get; set; }

        [JsonProperty("primaryclanid")]
        public string PrimaryClanId { get; set; }

        [JsonProperty("timecreated")]
        public long TimeCreated { get; set; }

        [JsonProperty("personastateflags")]
        public long PersonaStateFlags { get; set; }

        [JsonProperty("realname", NullValueHandling = NullValueHandling.Ignore)]
        public string RealName { get; set; }

        [JsonProperty("loccountrycode", NullValueHandling = NullValueHandling.Ignore)]
        public string LocCountryCode { get; set; }

        [JsonProperty("locstatecode", NullValueHandling = NullValueHandling.Ignore)]
        public long? LocStateCode { get; set; }
    }

}