using BenefactAPI.DataAccess;
using Replicate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenefactAPI.RPCInterfaces {
    [ReplicateType]
    public class TrelloCard {
        public string Id;
        public bool Closed;
        public List<string> IdLabels;
        public List<TrelloAttachment> Attachments;
        public string IdList;
        public string Desc;
        public string Name;

        [ReplicateIgnore]
        public CardData Card;
    }
    [ReplicateType]
    public class TrelloAttachment {
        public string Name;
        public string Url;
        public List<TrelloAttachmentPreview> Previews;
    }
    [ReplicateType]
    public class TrelloAttachmentPreview {
        public string Url;
        public int Width;
        public int Height;
    }
    [ReplicateType]
    public class TrelloList {
        public string Id;
        public string Name;

        [ReplicateIgnore]
        public ColumnData Column;
    }
    [ReplicateType]
    public class TrelloLabel {
        public string Id;
        public string Name;
        public string Color;

        [ReplicateIgnore]
        public TagData Tag;
    }
    [ReplicateType]
    public class TrelloBoard {
        public string Id;
        public string Name;
        public List<TrelloLabel> Labels;
        public List<TrelloCard> Cards;
        public List<TrelloList> Lists;

        [ReplicateIgnore]
        public BoardData Board;
    }
}
