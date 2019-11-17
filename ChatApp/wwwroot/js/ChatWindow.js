class ChatWindow {
    _messages = [];
    _currentFriendID = 0;

    ChatWindow = null;
    ChatPanel = null;
    btnSend = null;
    txtMessageInput = null;

    SendMessageEvent = null;

    constructor(ChatWindow) {
        this.ChatWindow = ChatWindow;
        this.ChatPanel = $(this.ChatWindow).children("#ChatPanel");
        this.btnSend = $(this.ChatWindow).find("#btnSendMessage");
        this.txtMessageInput = $(this.ChatWindow).find("#txtMessageInput");

        $(this.btnSend).click(this.btnSend_Click.bind(this));
        $(this.txtMessageInput).keydown(this.txtMessageInput_Keydown.bind(this));

        setInterval(function (parent) {
            if (Boolean($(parent.txtMessageInput).val()) === false)
                $(parent.btnSend).removeClass("btn-primary").addClass("btn-disabled");
            else
                $(parent.btnSend).removeClass("btn-disabled").addClass("btn-primary");
        }, 125, this);
    }

    //Properties
    get CurrentFriendID() { return this._currentFriendID; }
    set CurrentFriendID(value) { this._currentFriendID = value; }

    //Events
    txtMessageInput_Keydown(e) {
        if (e.key === "Enter")
            this.btnSend_Click();
    }

    btnSend_Click() {
        var contents = $(this.txtMessageInput).val();
        if (Boolean(contents) === false)
            return;

        $(this.txtMessageInput).val("").focus();

        //Create a temporary message row so there is no delay in displaying a user message while waiting for the server to return an ID
        this.CreateTempMessageRow(contents);

        var msg = { RecipientID: this._currentFriendID, Contents: contents };
        this.SendMessageEvent(msg);
    }

    //Member functions
    CreateTempMessageRow(Message) {
        var row = $(this.ChatWindow).children("#UserMessageTemplate").clone();

        row.removeAttr("style").attr("id", "TempMessageRow").find(".message").text(Message);
        $(this.ChatPanel).append(row);
        this.ScrollToChatWindowBottom();
    }

    RemoveTempMessageRow() {
        $(this.ChatWindow).find("#TempMessageRow").remove();
    }

    ShowInputBar() {
        $(this.ChatWindow).find("#InputRow").removeAttr("style");
    }

    CreateMessageRow(Friend) {
        var row = null;
        if (Friend)
            row = $("#FriendMessageTemplate").clone();
        else
            row = $("#UserMessageTemplate").clone();

        row.removeAttr("style").removeAttr("id");
        return row;
    }

    SortMessagesByDate(MessageArray) {
        MessageArray.sort(function sortFunc(a, b) {
            if (a.timestamp < b.timestamp)
                return -1;
            if (a.timestamp > b.timestamp)
                return 1;

            return 0;
        });

    }

    Render(ScrollToBottom, Prepend) {
        if (this._currentFriendID === 0)
            return;

        this.RemoveTempMessageRow();

        var _currentMessages = _messages.filter(msg => Number(msg.senderID) === Number(this._currentFriendID) || Number(msg.recipientID) === Number(this._currentFriendID));
        this.SortMessagesByDate(_currentMessages);
        //This is needed because the foreach loop cannot reference the ChatWindow as 'this'
        var parent = this;
        //This is used to prepend messages when Prepend==true
        var firstMessage = $("#ChatPanel .messageLine").first();
        var oldScroll = 0;
        if (firstMessage.offset() != null)
            oldScroll = firstMessage.offset().top;
        _currentMessages.forEach(function (msg) {
            if ($(parent.ChatPanel).find(".messageLine[data-id=" + msg.id + "]").length > 0)
                return;

            var row = parent.CreateMessageRow(Number(msg.senderID) === Number(parent.CurrentFriendID));
            row.attr("data-id", msg.id).find(".message").text(msg.contents);

            if (Prepend === true)
                row.insertBefore($(firstMessage));
            else
                $(parent.ChatPanel).append(row);

        });

        this.ShowInputBar();
        if (ScrollToBottom === true)
            this.ScrollToChatWindowBottom();

        //If loading previous messages, use the current offset of the previously oldest message minus its new offset to prevent the panel from scrolling
        //to the new first message
        if (Prepend === true)
            $(this.ChatPanel).animate({ scrollTop: $(firstMessage).offset().top - oldScroll }, 0);
    }



    ScrollToChatWindowBottom() {
        $(this.ChatPanel).animate({ scrollTop: $(".message").last().offset().top }, 0);
    }
}