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

        $(this.txtMessageInput).val("").focus();


        //Create a temporary message row so there is no delay in displaying a user message while waiting for the server to return an ID
        this.CreateTempMessageRow(contents);

        var msg = { FriendID: this._currentFriendID, Message: contents };
        this.SendMessageEvent(msg);
    }

    //Member functions
    CreateTempMessageRow(Message) {
        var row = $(this.ChatWindow).children("#UserMessageTemplate").clone();

        row.removeAttr("style").attr("id", "TempMessageRow").find(".message").text(Message);
        $(this.ChatPanel).append(row);
        this.ScrollToChatWindowBottom();
    }

    UpdateTempMessageRow(MessageID) {
        $(this.ChatWindow).find("#TempMessageRow").attr("data-id", MessageID).removeAttr("id");
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

    Render(ScrollToBottom) {
        if (this._currentFriendID === 0)
            return;

        var _currentMessages = _messages.filter(msg => Number(msg.senderID) === Number(this._currentFriendID) || Number(msg.recipientID) === Number(this._currentFriendID));
        this.SortMessagesByDate(_currentMessages);
        //This is needed because the foreach loop cannot reference the ChatWindow as 'this'
        var parent = this;
        _currentMessages.forEach(function (msg) {
            if ($(parent.ChatPanel).find(".messageLine[data-id=" + msg.id + "]").length > 0)
                return;

            var row = parent.CreateMessageRow(Number(msg.senderID) === Number(parent.CurrentFriendID));
            row.attr("data-id", msg.id).find(".message").text(msg.contents);
            $(parent.ChatPanel).append(row);
        });

        this.ShowInputBar();
        if (ScrollToBottom === true)
            this.ScrollToChatWindowBottom();
    }



    ScrollToChatWindowBottom() {
        $(this.ChatPanel).animate({ scrollTop: $(".message").last().offset().top }, 0);
    }
}